using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class OfferRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public OfferRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_Offer_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Offer_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);

            // Crear propiedad relacionada
            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var repo = new OfferRepository(context);

            var offer = new Offer
            {
                ClientId = "client1",
                PropertyId = property.Id,
                Amount = 95000,
                Status = Status.Pending
            };

            var result = await repo.AddAsync(offer);

            result.Should().NotBeNull();
            result!.Value.Id.Should().BeGreaterThan(0);
            result.Value.Amount.Should().Be(95000);
            result.Value.PropertyId.Should().Be(property.Id);
            result.Value.Status.Should().Be(Status.Pending);
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_Offer()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new OfferRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_Offer_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var offer = new Offer
            {
                ClientId = "client1",
                PropertyId = property.Id,
                Amount = 95000,
                Status = Status.Pending
            };
            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            var repo = new OfferRepository(context);
            var result = await repo.GetById(offer.Id);

            result.Should().NotBeNull();
            result!.Value.Amount.Should().Be(95000);
            result.Value.PropertyId.Should().Be(property.Id);
            result.Value.Status.Should().Be(Status.Pending);
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new OfferRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_Offer()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var offer = new Offer
            {
                ClientId = "client1",
                PropertyId = property.Id,
                Amount = 95000,
                Status = Status.Pending
            };
            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            var repo = new OfferRepository(context);
            offer.Status = Status.Accepted;
            var updated = await repo.UpdateAsync(offer.Id, offer);

            updated.Should().NotBeNull();
            updated!.Value.Status.Should().Be(Status.Accepted);
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new OfferRepository(context);

            var offer = new Offer
            {
                Id = 999,
                ClientId = "client1",
                PropertyId = 1,
                Amount = 95000,
                Status = Status.Pending
            };

            var result = await repo.UpdateAsync(offer.Id, offer);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Offer()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var offer = new Offer
            {
                ClientId = "client1",
                PropertyId = property.Id,
                Amount = 95000,
                Status = Status.Pending
            };
            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            var repo = new OfferRepository(context);
            await repo.DeleteAsync(offer.Id);

            var entity = await repo.GetById(offer.Id);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new OfferRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_Offers()
        {
            using var context = new RealStateContext(_dbOptions);

            var property = new Property { Code = "TEST123", Price = 100000, AgentId = "agent1", SaleTypeId = 1, PropertyTypeId = 1 };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            context.Offers.AddRange(
                new Offer { ClientId = "client1", PropertyId = property.Id, Amount = 95000, Status = Status.Pending },
                new Offer { ClientId = "client2", PropertyId = property.Id, Amount = 97000, Status = Status.Rejected }
            );
            await context.SaveChangesAsync();

            var repo = new OfferRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_Offers()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new OfferRepository(context);

            var result = await repo.GetAllAsync();

            result.Should().BeEmpty();
        }
    }
}
