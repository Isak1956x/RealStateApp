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
    public class PropertyImageServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public PropertyImageServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_PropertyImageService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PropertyImageMappingProfile>();
                cfg.AddProfile<PropertyMappingProfile>(); // Incluimos propiedad si es necesario
            });
            _mapper = config.CreateMapper();
        }

        private PropertyImageService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new PropertyImageRepository(context);
            return new PropertyImageService(repo, _mapper);
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

        private PropertyImage CreatePropertyImage(Property property, string url = "https://example.com/image.jpg")
        {
            return new PropertyImage
            {
                Url = url,
                Property = property,
                PropertyId = property.Id
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_PropertyImage()
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


            var propertyImage = new PropertyImage
            {
                PropertyId = property.Id,
                Url = "https://example.com/image.jpg"
            };
            var dto = _mapper.Map<PropertyImageDto>(propertyImage);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
        }
 


        [Fact]
        public async Task UpdateAsync_Should_Update_PropertyImage()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var image = CreatePropertyImage(property);
            context.PropertyImages.Add(image);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<PropertyImageDto>(image);
            dto.Url = "https://example.com/new-image.jpg";

            var updated = await service.UpdateAsync(dto.Id, dto);

            updated.Should().NotBeNull();
            updated!.Url.Should().Be("https://example.com/new-image.jpg");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_PropertyImage()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var image = CreatePropertyImage(property);
            context.PropertyImages.Add(image);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(image.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_PropertyImages()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var property = CreateProperty();
            context.Properties.Add(property);
            await context.SaveChangesAsync();

            var image1 = CreatePropertyImage(property);
            var image2 = CreatePropertyImage(property, url: "https://example.com/image2.jpg");
            context.PropertyImages.AddRange(image1, image2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
