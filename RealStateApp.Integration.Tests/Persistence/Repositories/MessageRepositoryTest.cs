using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Integration.Tests.Persistence.Repositories
{
    public class MessageRepositoryTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;

        public MessageRepositoryTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_Message_{Guid.NewGuid()}")
                .Options;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Message_To_Database()
        {
            using var context = new RealStateContext(_dbOptions);

            // Crear chat relacionado
            var chat = new Chat { CustomerId = "cust1", AgentId = "agent1", PropertyId = 1 };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var repo = new MessageRepository(context);

            var message = new Message
            {
                ChatID = chat.ChatId,
                Content = "Hello",
                SenderID = "cust1",
                Date = DateTime.UtcNow
            };

            var result = await repo.AddAsync(message);

            result.Should().NotBeNull();
            result!.Value.Id.Should().BeGreaterThan(0);
            result.Value.Content.Should().Be("Hello");
            result.Value.ChatID.Should().Be(chat.ChatId);
        }

        [Fact]
        public async Task AddAsync_Should_Not_Add_Null_Message()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new MessageRepository(context);

            Func<Task> act = async () => await repo.AddAsync(null!);

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task GetById_Should_Return_Message_When_Exists()
        {
            using var context = new RealStateContext(_dbOptions);

            var chat = new Chat { CustomerId = "cust1", AgentId = "agent1", PropertyId = 1 };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var message = new Message
            {
                ChatID = chat.ChatId,
                Content = "Hello",
                SenderID = "cust1",
                Date = DateTime.UtcNow
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            var repo = new MessageRepository(context);
            var result = await repo.GetById(message.Id);

            result.Should().NotBeNull();
            result!.Value.Content.Should().Be("Hello");
            result.Value.ChatID.Should().Be(chat.ChatId);
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new MessageRepository(context);

            var result = await repo.GetById(999);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateAsync_Should_Modify_Existing_Message()
        {
            using var context = new RealStateContext(_dbOptions);

            var chat = new Chat { CustomerId = "cust1", AgentId = "agent1", PropertyId = 1 };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var message = new Message
            {
                ChatID = chat.ChatId,
                Content = "Hello",
                SenderID = "cust1",
                Date = DateTime.UtcNow
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            var repo = new MessageRepository(context);
            message.Content = "Updated Content";

            var updated = await repo.UpdateAsync(message.Id, message);

            updated.Should().NotBeNull();
            updated!.Value.Content.Should().Be("Updated Content");
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_Failure_When_NotExists()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new MessageRepository(context);

            var message = new Message
            {
                Id = 999,
                ChatID = 1,
                Content = "Fake",
                SenderID = "fake",
                Date = DateTime.UtcNow
            };

            var result = await repo.UpdateAsync(message.Id, message);

            result.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Message()
        {
            using var context = new RealStateContext(_dbOptions);

            var chat = new Chat { CustomerId = "cust1", AgentId = "agent1", PropertyId = 1 };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var message = new Message
            {
                ChatID = chat.ChatId,
                Content = "Hello",
                SenderID = "cust1",
                Date = DateTime.UtcNow
            };
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            var repo = new MessageRepository(context);
            await repo.DeleteAsync(message.Id);

            var entity = await repo.GetById(message.Id);
            entity.IsFailure.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_Not_Throw_When_Id_NotFound()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new MessageRepository(context);

            Func<Task> act = async () => await repo.DeleteAsync(999);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAllList_Should_Return_All_Messages()
        {
            using var context = new RealStateContext(_dbOptions);

            var chat = new Chat { CustomerId = "cust1", AgentId = "agent1", PropertyId = 1 };
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            context.Messages.AddRange(
                new Message { ChatID = chat.ChatId, Content = "Hello", SenderID = "cust1", Date = DateTime.UtcNow },
                new Message { ChatID = chat.ChatId, Content = "Hi", SenderID = "agent1", Date = DateTime.UtcNow }
            );
            await context.SaveChangesAsync();

            var repo = new MessageRepository(context);
            var result = await repo.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetAllList_Should_Return_Empty_When_No_Messages()
        {
            using var context = new RealStateContext(_dbOptions);
            var repo = new MessageRepository(context);

            var result = await repo.GetAllAsync();

            result.Should().BeEmpty();
        }
    }
}
