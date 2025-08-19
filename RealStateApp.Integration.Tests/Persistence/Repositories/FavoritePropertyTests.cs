using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class FavoritePropertyRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public FavoritePropertyRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_FavoriteProperty_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_FavoriteProperty_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new FavoritePropertyRepository(context);

            var favorite = new FavoriteProperty
            {
                UserId = "user123",
                PropertyId = 0,
                Property = null
            };

            var result = await repo.AddAsync(favorite);

            result.Should().NotBeNull();
            result!.Value.FavoritePropertyId.Should().BeGreaterThan(0);
            result.Value.UserId.Should().Be("user123");
            result.Value.PropertyId.Should().Be(0);
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_FavoriteProperty()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new FavoritePropertyRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_FavoriteProperty_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);
            var favorite = new FavoriteProperty
            {
                UserId = "user123",
                PropertyId = 0
            };
            context.FavoriteProperties.Add(favorite);
            await context.SaveChangesAsync();

            var repo = new FavoritePropertyRepository(context);
            var result = await repo.GetById(favorite.FavoritePropertyId);

            result.Should().NotBeNull();
            result!.Value.UserId.Should().Be("user123");
            result.Value.PropertyId.Should().Be(0);
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new FavoritePropertyRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_FavoriteProperty()
        {
            using var context = new RealStateContext(_dbOptions);
            var favorite = new FavoriteProperty
            {
                UserId = "user123",
                PropertyId = 0
            };
            context.FavoriteProperties.Add(favorite);
            await context.SaveChangesAsync();

            var repo = new FavoritePropertyRepository(context);
            favorite.UserId = "updatedUser";

            var updated = await repo.UpdateAsync(favorite.FavoritePropertyId, favorite);

            updated.Should().NotBeNull();
            updated!.Value.UserId.Should().Be("updatedUser");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new FavoritePropertyRepository(context);

            var favorite = new FavoriteProperty
            {
                FavoritePropertyId = 999,
                UserId = "fake",
                PropertyId = 0
            };

            var result = await repo.UpdateAsync(favorite.FavoritePropertyId, favorite);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_FavoriteProperty()
        {
            using var context = new RealStateContext(_dbOptions);
            var favorite = new FavoriteProperty
            {
                UserId = "user123",
                PropertyId = 0
            };
            context.FavoriteProperties.Add(favorite);
            await context.SaveChangesAsync();

            var repo = new FavoritePropertyRepository(context);
            await repo.DeleteAsync(favorite.FavoritePropertyId);

            var entity = await repo.GetById(favorite.FavoritePropertyId);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new FavoritePropertyRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_FavoriteProperties()
        {
            using var context = new RealStateContext(_dbOptions);
            context.FavoriteProperties.AddRange(
                new FavoriteProperty { UserId = "user1", PropertyId = 1 },
                new FavoriteProperty { UserId = "user2", PropertyId = 2 }
            );
            await context.SaveChangesAsync();

            var repo = new FavoritePropertyRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_FavoriteProperties()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new FavoritePropertyRepository(context);

            var result = await repo.GetAllAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllListWithInclude_Should_Include_Property()
        {
            using var context = new RealStateContext(_dbOptions);
            context.Properties.Add(new Property
            {
                Id = 1,
                Description = "Test Property",
                Price = 1000,
                AgentId = "agent1",
                Code = "P1",
                Bathrooms = 1,
                SizeInMeters = 50,
                Bedrooms = 1,
                IsAvailable = true,
                PropertyTypeId = 1,
                SaleTypeId = 1
            });

            context.FavoriteProperties.Add(new FavoriteProperty
            {
                UserId = "user1",
                PropertyId = 1
            });
            await context.SaveChangesAsync();

            var repo = new FavoritePropertyRepository(context);
            var result = await repo.GetAllListWithInclude([ "Property" ]);

            result.Should().NotBeEmpty();
            result[0].Property.Should().NotBeNull();
        }
    }
}
