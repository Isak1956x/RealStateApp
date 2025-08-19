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
    public class SaleTypeServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public SaleTypeServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_SaleTypeService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SaleTypeMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private SaleTypeService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new SaleTypeRepository(context);
            return new SaleTypeService(repo, _mapper);
        }

        private SaleType CreateSaleType(string name = "Sale", string? description = null)
        {
            return new SaleType
            {
                Name = name,
                Description = description
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var saleType = CreateSaleType();
            var dto = _mapper.Map<SaleTypeDto>(saleType);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
            result.Name.Should().Be("Sale");
        }

        [Fact]
        public async Task GetById_Should_Return_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var saleType = CreateSaleType();
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var result = await service.GetById(saleType.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Sale");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var saleType = CreateSaleType();
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<SaleTypeDto>(saleType);
            dto.Name = "Updated Sale";

            var updated = await service.UpdateAsync(dto.Id, dto);

            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Updated Sale");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_SaleType()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var saleType = CreateSaleType();
            context.SaleTypes.Add(saleType);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(saleType.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_SaleTypes()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var st1 = CreateSaleType("Sale");
            var st2 = CreateSaleType("Rent");
            context.SaleTypes.AddRange(st1, st2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
