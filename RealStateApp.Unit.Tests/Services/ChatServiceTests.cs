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
    public class ChatServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public ChatServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_ChatService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ChatMappingProfile>();
                cfg.AddProfile<PropertyMappingProfile>(); // Para incluir la propiedad
            });
            _mapper = config.CreateMapper();
        }

        private ChatService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new ChatRepository(context);
            return new ChatService(repo, _mapper);
        }

        private Chat CreateChat(
            int propertyId = 1,
            string customerId = "customer1",
            string agentId = "agent1")
        {
            return new Chat
            {
                PropertyId = propertyId,
                CustomerId = customerId,
                AgentId = agentId,
                Messages = new List<Message>()
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_Chat()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = new Property
            {
                Description = "Test Property",
                Price = 100000,
                AgentId = "agent1",
                Code = "CODE001",
                Bathrooms = 2,
                SizeInMeters = 100,
                Bedrooms = 3,
                IsAvailable = true,
                PropertyTypeId = 1,
                SaleTypeId = 1
            };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var chat = CreateChat(propertyId: property.Id);
            var dto = _mapper.Map<ChatDto>(chat);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.ChatId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetById_Should_Return_Chat_With_Property_And_Messages()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = new Property
            {
                Description = "Test Property",
                Price = 100000,
                AgentId = "agent1",
                Code = "CODE001",
                Bathrooms = 2,
                SizeInMeters = 100,
                Bedrooms = 3,
                IsAvailable = true,
                PropertyTypeId = 1,
                SaleTypeId = 1
            };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var chat = CreateChat(propertyId: property.Id);
            chat.Messages!.Add(new Message { Content = "Hello", SenderID = "customer1", Date = DateTime.UtcNow });
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var result = await service.GetById(chat.ChatId);

            result.Should().NotBeNull();
            result!.CustomerId.Should().Be("customer1");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Chat()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = new Property
            {
                Description = "Test Property",
                Price = 100000,
                AgentId = "agent1",
                Code = "CODE001",
                Bathrooms = 2,
                SizeInMeters = 100,
                Bedrooms = 3,
                IsAvailable = true,
                PropertyTypeId = 1,
                SaleTypeId = 1
            };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var chat = CreateChat(propertyId: property.Id);
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<ChatDto>(chat);
            dto.CustomerId = "updatedCustomer";

            var updated = await service.UpdateAsync(dto.ChatId, dto);

            updated.Should().NotBeNull();
            updated!.CustomerId.Should().Be("updatedCustomer");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Chat()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var chat = CreateChat();
            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(chat.ChatId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_Chats()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = new Property
            {
                Description = "Test Property",
                Price = 100000,
                AgentId = "agent1",
                Code = "CODE001",
                Bathrooms = 2,
                SizeInMeters = 100,
                Bedrooms = 3,
                IsAvailable = true,
                PropertyTypeId = 1,
                SaleTypeId = 1
            };
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var chat1 = CreateChat(propertyId: property.Id);
            var chat2 = CreateChat(propertyId: property.Id, customerId: "customer2");
            context.Chats.AddRange(chat1, chat2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
