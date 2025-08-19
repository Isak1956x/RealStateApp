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
    public class ImprovementServiceTests
    {
        private readonly DbContextOptions<RealStateContext> _dbOptions;
        private readonly IMapper _mapper;

        public ImprovementServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase($"TestDb_ImprovementService_{Guid.NewGuid()}")
                .Options;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ImprovementMappingProfile>();
            });
            _mapper = config.CreateMapper();
        }

        private ImprovementService CreateService()
        {
            var context = new RealStateContext(_dbOptions);
            var repo = new ImprovementRepository(context);
            return new ImprovementService(repo, _mapper);
        }

        private Improvement CreateImprovement(
            string name = "Default Improvement",
            string? description = null)
        {
            return new Improvement
            {
                Name = name,
                Description = description
            };
        }

        [Fact]
        public async Task AddAsync_Should_Add_Improvement()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var improvement = CreateImprovement();
            var dto = _mapper.Map<ImprovementDto>(improvement);

            var result = await service.AddAsync(dto);

            result.Should().NotBeNull();
            result!.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetById_Should_Return_Improvement()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var improvement = CreateImprovement();
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var result = await service.GetById(improvement.Id);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Default Improvement");
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Improvement()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var improvement = CreateImprovement();
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var dto = _mapper.Map<ImprovementDto>(improvement);
            dto.Name = "Updated Improvement";

            var updated = await service.UpdateAsync(dto.Id, dto);

            updated.Should().NotBeNull();
            updated!.Name.Should().Be("Updated Improvement");
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Improvement()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var improvement = CreateImprovement();
            context.Improvements.Add(improvement);
            await context.SaveChangesAsync();

            var result = await service.DeleteAsync(improvement.Id);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAll_Should_Return_List_Of_Improvements()
        {
            using var context = new RealStateContext(_dbOptions);
            var service = CreateService();

            var imp1 = CreateImprovement();
            var imp2 = CreateImprovement(name: "Second Improvement");
            context.Improvements.AddRange(imp1, imp2);
            await context.SaveChangesAsync();

            var result = await service.GetAll();

            result.Should().HaveCount(2);
        }
    }
}
