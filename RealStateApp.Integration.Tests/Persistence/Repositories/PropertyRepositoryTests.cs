using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class PropertyRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public PropertyRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                 .UseInMemoryDatabase(databaseName: $"TestDb_Property_{Guid.NewGuid()}")
                 .Options;
        }

        private Property CreateProperty(
            string description = "Test property",
            decimal price = 100000,
            string agentId = "agent1",
            string code = "CODE123",
            int bathrooms = 2,
            int size = 100,
            int bedrooms = 3,
            bool isAvailable = true,
            int propertyTypeId = 1,
            int saleTypeId = 1)
        {
            return new Property
            {
                Description = description,
                Price = price,
                AgentId = agentId,
                Code = code,
                Bathrooms = bathrooms,
                SizeInMeters = size,
                Bedrooms = bedrooms,
                IsAvailable = isAvailable,
                PropertyTypeId = propertyTypeId,
                SaleTypeId = saleTypeId
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_Property_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyRepository(context);

            var property = CreateProperty();
            var result = await repo.AddAsync(property);

            result.Should().NotBeNull();
            result!.Value.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetById_Should_Return_Property_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);
            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var repo = new PropertyRepository(context);
            var result = await repo.GetById(property.Id);

            result.Should().NotBeNull();
            result!.Value.Description.Should().Be("Test property");
            result.Value.Price.Should().Be(100000);
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_Property()
        {
            using var context = new RealStateContext(_dbOptions);
            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var repo = new PropertyRepository(context);
            property.Description = "Updated property";

            var updated = await repo.UpdateAsync(property.Id, property);

            updated.Should().NotBeNull();
            updated!.Value.Description.Should().Be("Updated property");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Property()
        {
            using var context = new RealStateContext(_dbOptions);
            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var repo = new PropertyRepository(context);
            await repo.DeleteAsync(property.Id);

            var entity = await repo.GetById(property.Id);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_Properties()
        {
            using var context = new RealStateContext(_dbOptions);
            context.Properties.AddRange(CreateProperty(), CreateProperty(description: "Second property", code: "CODE456"));
            await context.SaveChangesAsync();

            var repo = new PropertyRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }
    }
}
