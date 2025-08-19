using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Unit.Tests.Services
{
    public class MessageServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public MessageServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_MessageService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MessageMappingProfile>();
                cfg.AddProfile<ChatMappingProfile>(); // Para incluir la relación con Chat
            });
            _mapper = config.CreateMapper();
        }

        private MessageService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new MessageRepository(context);
            return new MessageService(repo, _mapper);
        }

        private Chat CreateChat(
            string customerId = "customer1",
            string agentId = "agent1")
        {
            return new Chat
            {
                CustomerId = customerId,
                AgentId = agentId,
                Messages = new List<Message>()
            };
        }

        private Message CreateMessage(
            Chat chat,
            string content = "Hello",
            string senderId = "customer1",
            DateTime? date = null)
        {
            return new Message
            {
                Chat = chat,
                ChatID = chat.ChatId,
                Content = content,
                SenderID = senderId,
                Date = date ?? DateTime.UtcNow
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_Message()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

        
            var chat = new Chat
            {
                CustomerId = "customer1",
                AgentId = "agent1",
                Messages = new List<Message>()
            };
            context.Chats.Add(chat);
            await context.SaveChangesAsync(); 

        
            var message = new Message
            {
                ChatID = chat.ChatId,
                Content = "Hello",
                SenderID = "customer1",
                Date = DateTime.UtcNow
            };

            var dto = _mapper.Map<MessageDto>(message);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            result.ChatID.Should().Be(chat.ChatId);
        }


        [Fact]
        public async Task UpdateAsync_Should_Update_Message()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var chat = CreateChat();
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var message = CreateMessage(chat);
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<MessageDto>(message);
            dto.Content = "Updated Content";

            var updated = await service.UpdateAsync(dto.Id, dto);

            updated.Should().NotBeNull();
            updated!.Content.Should().Be("Updated Content");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Message()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var chat = CreateChat();
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var message = CreateMessage(chat);
            context.Messages.Add(message);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(message.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_Messages()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var chat = CreateChat();
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var msg1 = CreateMessage(chat, content: "First");
            var msg2 = CreateMessage(chat, content: "Second");
            context.Messages.AddRange(msg1, msg2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
