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
    public class PropertyTypeServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public PropertyTypeServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_PropertyTypeService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PropertyTypeMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private PropertyTypeService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new PropertyTypeRepository(context);
            return new PropertyTypeService(repo, _mapper);
        }

        private PropertyType CreatePropertyType(string name = "Apartment", string? description = null)
        {
            return new PropertyType
            {
                Name = name,
                Description = description
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_PropertyType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var propertyType = CreatePropertyType();
            var dto = _mapper.Map<PropertyTypeDto>(propertyType);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("Apartment");
        }

        [Fact]
        public async Task GetById_Should_Return_PropertyType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var propertyType = CreatePropertyType();
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var result = await service.GetById(propertyType.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Apartment");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_PropertyType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var propertyType = CreatePropertyType();
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<PropertyTypeDto>(propertyType);
            dto.Name = "Updated Apartment";

            var updated = await service.UpdateAsync(dto.Id, dto);

            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Updated Apartment");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_PropertyType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var propertyType = CreatePropertyType();
            context.PropertyTypes.Add(propertyType);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(propertyType.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_PropertyTypes()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var pt1 = CreatePropertyType("Apartment");
            var pt2 = CreatePropertyType("House");
            context.PropertyTypes.AddRange(pt1, pt2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
