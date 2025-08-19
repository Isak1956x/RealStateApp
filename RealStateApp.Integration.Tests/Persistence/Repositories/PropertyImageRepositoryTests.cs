using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class PropertyImageRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public PropertyImageRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_PropertyImage_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_PropertyImage_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);

            // Crear propiedad relacionada
            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var repo = new PropertyImageRepository(context);

            var image = new PropertyImage
            {
                Url = "https://example.com/image1.jpg",
                PropertyId = property.Id
            };

            var result = await repo.AddAsync(image);

            result.Should().NotBeNull();
            result!.Value.Id.Should().BeGreaterThan(0);
            result.Value.Url.Should().Be("https://example.com/image1.jpg");
            result.Value.PropertyId.Should().Be(property.Id);
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_PropertyImage()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyImageRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_PropertyImage_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var image = new PropertyImage
            {
                Url = "https://example.com/image1.jpg",
                PropertyId = property.Id
            };
            context.PropertyImages.Add(image);
            await context.SaveChangesAsync();

            var repo = new PropertyImageRepository(context);
            var result = await repo.GetById(image.Id);

            result.Should().NotBeNull();
            result!.Value.Url.Should().Be("https://example.com/image1.jpg");
            result.Value.PropertyId.Should().Be(property.Id);
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyImageRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_PropertyImage()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var image = new PropertyImage
            {
                Url = "https://example.com/image1.jpg",
                PropertyId = property.Id
            };
            context.PropertyImages.Add(image);
            await context.SaveChangesAsync();

            var repo = new PropertyImageRepository(context);
            image.Url = "https://example.com/image1-updated.jpg";

            var updated = await repo.UpdateAsync(image.Id, image);

            updated.Should().NotBeNull();
            updated!.Value.Url.Should().Be("https://example.com/image1-updated.jpg");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyImageRepository(context);

            var image = new PropertyImage
            {
                Id = 999,
                Url = "https://example.com/fake.jpg",
                PropertyId = 1
            };

            var result = await repo.UpdateAsync(image.Id, image);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_PropertyImage()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var image = new PropertyImage
            {
                Url = "https://example.com/image1.jpg",
                PropertyId = property.Id
            };
            context.PropertyImages.Add(image);
            await context.SaveChangesAsync();

            var repo = new PropertyImageRepository(context);
            await repo.DeleteAsync(image.Id);

            var entity = await repo.GetById(image.Id);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyImageRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_PropertyImages()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            context.PropertyImages.AddRange(
                new PropertyImage { Url = "https://example.com/image1.jpg", PropertyId = property.Id },
                new PropertyImage { Url = "https://example.com/image2.jpg", PropertyId = property.Id }
            );
            await context.SaveChangesAsync();

            var repo = new PropertyImageRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_PropertyImages()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new PropertyImageRepository(context);

            var result = await repo.GetAllAsync();

            result.Should().BeEmpty();
        }
    }
}
