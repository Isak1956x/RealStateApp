using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class SaleTypeRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public SaleTypeRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_SaleType_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_SaleType_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new SaleTypeRepository(context);

            var saleType = new SaleType
            {
                Name = "Rent",
                Description = "Rental property"
            };

            var result = await repo.AddAsync(saleType);

            result.Should().NotBeNull();
            result!.Value.Id.Should().BeGreaterThan(0);
            result.Value.Name.Should().Be("Rent");
            result.Value.Description.Should().Be("Rental property");
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new SaleTypeRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_SaleType_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);
            var saleType = new SaleType
            {
                Name = "Rent",
                Description = "Rental property"
            };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var repo = new SaleTypeRepository(context);
            var result = await repo.GetById(saleType.Id);

            result.Should().NotBeNull();
            result!.Value.Name.Should().Be("Rent");
            result.Value.Description.Should().Be("Rental property");
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new SaleTypeRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var saleType = new SaleType
            {
                Name = "Rent",
                Description = "Rental property"
            };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var repo = new SaleTypeRepository(context);
            saleType.Name = "Sale";

            var updated = await repo.UpdateAsync(saleType.Id, saleType);

            updated.Should().NotBeNull();
            updated!.Value.Name.Should().Be("Sale");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new SaleTypeRepository(context);

            var saleType = new SaleType
            {
                Id = 999,
                Name = "Fake",
                Description = null
            };

            var result = await repo.UpdateAsync(saleType.Id, saleType);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var saleType = new SaleType
            {
                Name = "Rent",
                Description = "Rental property"
            };
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var repo = new SaleTypeRepository(context);
            await repo.DeleteAsync(saleType.Id);

            var entity = await repo.GetById(saleType.Id);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new SaleTypeRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_SaleTypes()
        {
            using var context = new RealStateContext(_dbOptions);
            context.SaleTypes.AddRange(
                new SaleType { Name = "Rent" },
                new SaleType { Name = "Sale" }
            );
            await context.SaveChangesAsync();

            var repo = new SaleTypeRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_SaleTypes()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new SaleTypeRepository(context);

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

            context.SaleTypes.Add(new SaleType
            {
                Name = "Rent"
            });
            await context.SaveChangesAsync();

            var repo = new SaleTypeRepository(context);
            var result = await repo.GetAllListWithInclude(["Properties" ]);

            result.Should().NotBeEmpty();
            result[0].Properties.Should().NotBeNull();
        }
    }
}
