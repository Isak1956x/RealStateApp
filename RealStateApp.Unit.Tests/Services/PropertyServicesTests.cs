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
    public class PropertyServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public PropertyServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_PropertyService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PropertyMappingProfile>();
                cfg.AddProfile<PropertyTypeMappingProfile>();
                cfg.AddProfile<SaleTypeMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private PropertyService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new PropertyRepository(context);
            return new PropertyService(repo, _mapper);
        }

        private Property CreateProperty(
            string description = "Test Property",
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
        public async Task AddAsync_Should_Add_Property()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var propertyType = new PropertyType { Name = "Apartment" };
            var saleType = new SaleType { Name = "Sale" };
            context.PropertyTypes.Add(propertyType);
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var property = CreateProperty(propertyTypeId: propertyType.Id, saleTypeId: saleType.Id);
            var dto = _mapper.Map<PropertyDto>(property);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetById_Should_Return_Property_With_PropertyType_And_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var propertyType = new PropertyType { Name = "Apartment" };
            var saleType = new SaleType { Name = "Sale" };
            context.PropertyTypes.Add(propertyType);
            context.SaleTypes.Add(saleType);

            var property = CreateProperty(propertyTypeId: propertyType.Id, saleTypeId: saleType.Id);
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var result = await service.GetById(property.Id);

            result.Should().NotBeNull();
            result!.Description.Should().Be("Test Property");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Property()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var propertyType = new PropertyType { Name = "Apartment" };
            var saleType = new SaleType { Name = "Sale" };
            context.PropertyTypes.Add(propertyType);
            context.SaleTypes.Add(saleType);

            var property = CreateProperty(propertyTypeId: propertyType.Id, saleTypeId: saleType.Id);
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<PropertyDto>(property);
            dto.Description = "Updated Property";

            var updated = await service.UpdateAsync(dto.Id, dto);

            updated.Should().NotBeNull();
            updated!.Description.Should().Be("Updated Property");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Property()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(property.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_Properties()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property1 = CreateProperty();
            var property2 = CreateProperty(description: "Second Property", code: "CODE002");
            context.Properties.AddRange(property1, property2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
