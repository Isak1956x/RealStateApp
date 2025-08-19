using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class ChatRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public ChatRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                 .UseInMemoryDatabase(databaseName: $"TestDb_Chat_{Guid.NewGuid()}")
                 .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Chat_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ChatRepository(context);

            var chat = new Chat
            {
                PropertyId = 0,
                CustomerId = "customer123",
                AgentId = "agent123",
                Property = null, // puede ser nulo
                Messages = null // puede ser nulo
            };

            var result = await repo.AddAsync(chat);

            result.Should().NotBeNull();
            result!.Value.ChatId.Should().BeGreaterThan(0);
            result.Value.CustomerId.Should().Be("customer123");
            result.Value.AgentId.Should().Be("agent123");
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_Chat()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ChatRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_Chat_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);
            var chat = new Chat
            {
                PropertyId = 0,
                CustomerId = "customer123",
                AgentId = "agent123",
            };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var repo = new ChatRepository(context);
            var result = await repo.GetById(chat.ChatId);

            result.Should().NotBeNull();
            result!.Value.CustomerId.Should().Be("customer123");
            result.Value.AgentId.Should().Be("agent123");
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ChatRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_Chat()
        {
            using var context = new RealStateContext(_dbOptions);
            var chat = new Chat
            {
                PropertyId = 0,
                CustomerId = "customer123",
                AgentId = "agent123",
            };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var repo = new ChatRepository(context);
            chat.CustomerId = "updatedCustomer";

            var updated = await repo.UpdateAsync(chat.ChatId, chat);

            updated.Should().NotBeNull();
            updated!.Value.CustomerId.Should().Be("updatedCustomer");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_Chat_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ChatRepository(context);

            var chat = new Chat
            {
                ChatId = 999,
                PropertyId = 0,
                CustomerId = "fake",
                AgentId = "fake"
            };

            var result = await repo.UpdateAsync(chat.ChatId, chat);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Chat()
        {
            using var context = new RealStateContext(_dbOptions);
            var chat = new Chat
            {
                PropertyId = 0,
                CustomerId = "customer123",
                AgentId = "agent123",
            };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var repo = new ChatRepository(context);
            await repo.DeleteAsync(chat.ChatId);

            var entity = await repo.GetById(chat.ChatId);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ChatRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_Chats()
        {
            using var context = new RealStateContext(_dbOptions);
            context.Chats.AddRange(
                new Chat { PropertyId = 0, CustomerId = "c1", AgentId = "a1" },
                new Chat { PropertyId = 0, CustomerId = "c2", AgentId = "a2" }
            );
            await context.SaveChangesAsync();

            var repo = new ChatRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_Chats()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new ChatRepository(context);

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
            context.Chats.Add(new Chat
            {
                PropertyId = 1,
                CustomerId = "c1",
                AgentId = "a1"
            });
            await context.SaveChangesAsync();

            var repo = new ChatRepository(context);
            var result = await repo.GetAllListWithInclude(["Property"]);

            result.Should().NotBeEmpty();
            result[0].Property.Should().NotBeNull();
        }
    }
}
