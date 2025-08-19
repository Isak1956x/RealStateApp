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
    public class FavoritePropertyServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public FavoritePropertyServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_FavoritePropertyService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FavoritePropertyMappingProfile>();
                cfg.AddProfile<PropertyMappingProfile>(); // Para poder mapear Property
            });
            _mapper = config.CreateMapper();
        }

        private FavoritePropertyService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new FavoritePropertyRepository(context);
            return new FavoritePropertyService(repo, _mapper);
        }

        private FavoriteProperty CreateFavoriteProperty(
            string userId = "user1",
            int propertyId = 1)
        {
            return new FavoriteProperty
            {
                UserId = userId,
                PropertyId = propertyId
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_FavoriteProperty()
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

            var favorite = CreateFavoriteProperty(propertyId: property.Id);
            var dto = _mapper.Map<FavoritePropertyDto>(favorite);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.FavoritePropertyId.Should().BeGreaterThan(0);
        }

    
        [Fact]
        public async Task UpdateAsync_Should_Update_FavoriteProperty()
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

            var favorite = CreateFavoriteProperty(propertyId: property.Id);
            context.FavoriteProperties.Add(favorite);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<FavoritePropertyDto>(favorite);
            dto.UserId = "updatedUser";

            var updated = await service.UpdateAsync(dto.FavoritePropertyId, dto);

            updated.Should().NotBeNull();
            updated!.UserId.Should().Be("updatedUser");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_FavoriteProperty()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var favorite = CreateFavoriteProperty();
            context.FavoriteProperties.Add(favorite);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(favorite.FavoritePropertyId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_FavoriteProperties()
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

            var fav1 = CreateFavoriteProperty(propertyId: property.Id);
            var fav2 = CreateFavoriteProperty(propertyId: property.Id, userId: "user2");
            context.FavoriteProperties.AddRange(fav1, fav2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
