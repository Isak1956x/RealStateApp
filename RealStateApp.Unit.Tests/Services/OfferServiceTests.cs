using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Unit.Tests.Services
{
    public class OfferServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public OfferServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_OfferService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OfferMappingProfile>();
                cfg.AddProfile<PropertyMappingProfile>(); // Para incluir la propiedad si es necesario
            });
            _mapper = config.CreateMapper();
        }

        private OfferService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new OfferRepository(context);
            return new OfferService(repo, _mapper);
        }

        private Property CreateProperty()
        {
            return new Property
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
        }

        private Offer CreateOffer(Property property, string clientId = "client1", decimal amount = 120000, Status status = Status.Pending)
        {
            return new Offer
            {
                Property = property,
                PropertyId = property.Id,
                ClientId = clientId,
                Amount = amount,
                Status = status,
                Date = DateTime.UtcNow
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_Offer()
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

            var offer = new Offer
            {
                ClientId = "client1",
                PropertyId = property.Id,
                Amount = 120000,
                Status = Status.Pending,
                Date = DateTime.UtcNow
            };

            var dto = _mapper.Map<OfferDto>(offer);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            result.PropertyId.Should().Be(property.Id);
        }

  

        [Fact]
        public async Task UpdateAsync_Should_Update_Offer()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var offer = CreateOffer(property);
            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<OfferDto>(offer);
            dto.Amount = 150000;
            dto.Status = Status.Accepted;

            var updated = await service.UpdateAsync(dto.Id, dto);

            updated.Should().NotBeNull();
            updated!.Amount.Should().Be(150000);
            updated.Status.Should().Be(Status.Accepted);
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Offer()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var offer = CreateOffer(property);
            context.Offers.Add(offer);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(offer.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_Offers()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var offer1 = CreateOffer(property, clientId: "client1");
            var offer2 = CreateOffer(property, clientId: "client2", amount: 130000);
            context.Offers.AddRange(offer1, offer2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
