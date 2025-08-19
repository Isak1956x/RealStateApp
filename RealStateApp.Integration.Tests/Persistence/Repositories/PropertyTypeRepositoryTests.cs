using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class PropertyTypeRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public PropertyTypeRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_PropertyType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_PropertyType_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);

            var propertyType = new PropertyType
            {
                Name = "Apartment",
                Description = "Test description"
            };

            var result = await repo.AddAsync(propertyType);

            result.Should().NotBeNull();
            result!.Value.Id.Should().BeGreaterThan(0);
            result.Value.Name.Should().Be("Apartment");
            result.Value.Description.Should().Be("Test description");
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_PropertyType()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_PropertyType_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);
            var propertyType = new PropertyType
            {
                Name = "Apartment",
                Description = "Test description"
            };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var repo = new PropertyTypeRepository(context);
            var result = await repo.GetById(propertyType.Id);

            result.Should().NotBeNull();
            result!.Value.Name.Should().Be("Apartment");
            result.Value.Description.Should().Be("Test description");
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_PropertyType()
        {
            using var context = new RealStateContext(_dbOptions);
            var propertyType = new PropertyType
            {
                Name = "Apartment",
                Description = "Test description"
            };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var repo = new PropertyTypeRepository(context);
            propertyType.Name = "Updated Apartment";

            var updated = await repo.UpdateAsync(propertyType.Id, propertyType);

            updated.Should().NotBeNull();
            updated!.Value.Name.Should().Be("Updated Apartment");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);

            var propertyType = new PropertyType
            {
                Id = 999,
                Name = "Fake",
                Description = null
            };

            var result = await repo.UpdateAsync(propertyType.Id, propertyType);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_PropertyType()
        {
            using var context = new RealStateContext(_dbOptions);
            var propertyType = new PropertyType
            {
                Name = "Apartment",
                Description = "Test description"
            };
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var repo = new PropertyTypeRepository(context);
            await repo.DeleteAsync(propertyType.Id);

            var entity = await repo.GetById(propertyType.Id);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_PropertyTypes()
        {
            using var context = new RealStateContext(_dbOptions);
            context.PropertyTypes.AddRange(
                new PropertyType { Name = "Apartment" },
                new PropertyType { Name = "House" }
            );
            await context.SaveChangesAsync();

            var repo = new PropertyTypeRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_PropertyTypes()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);

            var result = await repo.GetAllAsync();

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllListWithInclude_Should_Include_Properties()
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

            context.PropertyTypes.Add(new PropertyType
            {
                Name = "Apartment"
            });
            await context.SaveChangesAsync();

            var repo = new PropertyTypeRepository(context);
            var result = await repo.GetAllListWithInclude(["Properties" ]);

            result.Should().NotBeEmpty();
            result[0].Properties.Should().NotBeNull();
        }
    }
}
