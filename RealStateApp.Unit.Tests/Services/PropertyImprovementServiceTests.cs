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
    public class PropertyImprovementServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public PropertyImprovementServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_PropertyImprovementService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PropertyImprovementMappingProfile>();
                cfg.AddProfile<PropertyMappingProfile>();
                cfg.AddProfile<ImprovementMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private PropertyImprovementService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new PropertyImprovementRepository(context);
            return new PropertyImprovementService(repo, _mapper);
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

        private Improvement CreateImprovement(string name = "New Improvement")
        {
            return new Improvement
            {
                Name = name,
                Description = "Test description"
            };
        }

        private PropertyImprovement CreatePropertyImprovement(Property property, Improvement improvement)
        {
            return new PropertyImprovement
            {
                Property = property,
                PropertyId = property.Id,
                Improvement = improvement,
                ImprovementId = improvement.Id
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_PropertyImprovement()
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

         
            var improvement = new Improvement
            {
                Name = "Pool"
            };
            context.Improvements.Add(improvement);

            await context.SaveChangesAsync(); 

        
            var propertyImprovement = new PropertyImprovement
            {
                PropertyId = property.Id,
                ImprovementId = improvement.Id
            };
            var dto = _mapper.Map<PropertyImprovementDto>(propertyImprovement);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.PropertyId.Should().Be(property.Id);
            result.ImprovementId.Should().Be(improvement.Id);
        }


        [Fact]
        public async Task GetAll_Should_Return_List_Of_PropertyImprovements()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            var improvement1 = CreateImprovement("Imp1");
            var improvement2 = CreateImprovement("Imp2");

            context.Properties.Add(property);
            context.Improvements.AddRange(improvement1, improvement2);
            await context.SaveChangesAsync();

            var pi1 = CreatePropertyImprovement(property, improvement1);
            var pi2 = CreatePropertyImprovement(property, improvement2);
            context.PropertyImprovements.AddRange(pi1, pi2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
