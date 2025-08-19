using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class ImprovementRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public ImprovementRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_Improvement_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Improvement_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ImprovementRepository(context);

            var improvement = new Improvement
            {
                Name = "New Roof",
                Description = "Roof replacement"
            };

            var result = await repo.AddAsync(improvement);

            result.Should().NotBeNull();
            result!.Value.Id.Should().BeGreaterThan(0);
            result.Value.Name.Should().Be("New Roof");
            result.Value.Description.Should().Be("Roof replacement");
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_Improvement()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ImprovementRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_Improvement_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);
            var improvement = new Improvement
            {
                Name = "New Roof",
                Description = "Roof replacement"
            };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var repo = new ImprovementRepository(context);
            var result = await repo.GetById(improvement.Id);

            result.Should().NotBeNull();
            result!.Value.Name.Should().Be("New Roof");
            result.Value.Description.Should().Be("Roof replacement");
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ImprovementRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_Improvement()
        {
            using var context = new RealStateContext(_dbOptions);
            var improvement = new Improvement
            {
                Name = "New Roof",
                Description = "Roof replacement"
            };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var repo = new ImprovementRepository(context);
            improvement.Description = "Updated description";

            var updated = await repo.UpdateAsync(improvement.Id, improvement);

            updated.Should().NotBeNull();
            updated!.Value.Description.Should().Be("Updated description");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ImprovementRepository(context);

            var improvement = new Improvement
            {
                Id = 999,
                Name = "Fake",
                Description = "Fake"
            };

            var result = await repo.UpdateAsync(improvement.Id, improvement);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Improvement()
        {
            using var context = new RealStateContext(_dbOptions);
            var improvement = new Improvement
            {
                Name = "New Roof",
                Description = "Roof replacement"
            };
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var repo = new ImprovementRepository(context);
            await repo.DeleteAsync(improvement.Id);

            var entity = await repo.GetById(improvement.Id);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ImprovementRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_Improvements()
        {
            using var context = new RealStateContext(_dbOptions);
            context.Improvements.AddRange(
                new Improvement { Name = "Roof", Description = "Roof replacement" },
                new Improvement { Name = "Paint", Description = "Exterior paint" }
            );
            await context.SaveChangesAsync();

            var repo = new ImprovementRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_Improvements()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ImprovementRepository(context);

            var result = await repo.GetAllAsync();

            result.Should().BeEmpty();
        }
    }
}
