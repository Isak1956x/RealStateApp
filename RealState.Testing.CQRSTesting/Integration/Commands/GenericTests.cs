using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using RealState.Infraestructure.Persistence.Context;
using RealState.Infraestructure.Persistence.Repositories;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using RealStateApp.Core.Application.Features.Common.GennericQueries;
using RealStateApp.Core.Application.Features.PropertyTypes.Commands;
using RealStateApp.Core.Application.Features.PropertyTypes.Queries;
using RealStateApp.Core.Application.Mappings.CQRS;
using RealStateApp.Core.Application.Mappings.DtosAndViewModels;
using RealStateApp.Core.Application.Mappings.EntitiesAndDtos;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace RealState.Testing.CQRSTesting.Integration.Commands
{
    public  class GenericTests 
    {
        private readonly DbContextOptions<RealStateContext> _options;
        private readonly IMapper _mapper;

        public GenericTests()
        {
            _options = new DbContextOptionsBuilder<RealStateContext>()
                .UseInMemoryDatabase(databaseName: $"RealStateTestDatabase {Guid.NewGuid()}")
                .Options;
            var mappingConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PropertyTypeCommandsMappingProfile>();
                //cfg.AddProfile<Proper>();
                cfg.AddProfile<PropertyTypeMappingProfile>();
            });

            _mapper = mappingConfig.CreateMapper();
        }


        [Fact]
        public async Task Execute_CreateCommand_Should_Add_Resource_To_Database()
        {
            //Arrange
            using var context = new RealStateContext(_options);
            var repository = new PropertyTypeRepository(context);

            CreateResourceCommandHandler<PropertyType, PropertyTypeDto> createCommandHandler = 
                new CreatePropertyTypeCommand(repository, _mapper); // Assuming you have a valid mapper instance

            //Act
            var command = new CreateResourceCommand<PropertyTypeDto>()
            {
                Name = "Placeholder",
                Description = "Placeholder"
            };
            var res = await createCommandHandler.Handle(command, CancellationToken.None);


            //Assert
            res.Should().NotBeNull();
            res.Should().BeOfType<PropertyTypeDto>();
            res.Name.Should().Be("Placeholder");
            res.Description.Should().Be("Placeholder");
            res.Id.Should().BeGreaterThan(0);
            var resources = await context.PropertyTypes.ToListAsync();
            resources.Should().ContainSingle();
        }

        [Fact]
        public async Task Execute_CreateCommand_When_Name_is_null_Should_Throw_Exception()
        {
            //Arrange
            using var context = new RealStateContext(_options);
            var repository = new PropertyTypeRepository(context);

            CreateResourceCommandHandler<PropertyType, PropertyTypeDto> createCommandHandler =
                new CreatePropertyTypeCommand(repository, _mapper); // Assuming you have a valid mapper instance

            //Act
            var command = new CreateResourceCommand<PropertyTypeDto>()
            {
                Name = null,
                Description = "Placeholder"
            };
            //var res = await createCommandHandler.Handle(command, CancellationToken.None);
            Func<Task> act = async () => await createCommandHandler.Handle(command, CancellationToken.None);
            //Assert
            act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Name cannot be null or empty");
        }

        [Fact]
        public async Task Execute_UpdateCommand_Should_Update_Resource_Data()
        {
            //Arrange
            using var context = new RealStateContext(_options);
            var repository = new PropertyTypeRepository(context);
            await context.PropertyTypes.AddAsync(new PropertyType
            {
                Name = "Placeholder",
                Description = "Placeholder"
            });
            await context.SaveChangesAsync();
            var id = (await context.PropertyTypes.FirstAsync()).Id;


            UpdateResourceCommandHandler<PropertyTypeDto, PropertyType> handler =
                new UpdatePropertyTypeCommandHandler(repository, _mapper); // Assuming you have a valid mapper instance

            //Act
            var command = new UpdateResourceCommand<PropertyTypeDto>()
            {
                Id = id,
                Name = "Placeholder New",
                Description = "Placeholder New"
            };
            //var res = await createCommandHandler.Handle(command, CancellationToken.None);
            var res = await handler.Handle(command, CancellationToken.None);   
            //Assert
            res.Description.Should().Be("Placeholder New");
            res.Name.Should().Be("Placeholder New");
            res.Id.Should().Be(id);
            var resources = await context.PropertyTypes.ToListAsync();
            resources.Should().ContainSingle();
            resources.First().Name.Should().Be("Placeholder New");
        }

        [Fact]
        public async Task Execute_UpdateCommand_If_Resource_Doesnt_Exit_Should_Return_null()
        {
            //Arrange
            using var context = new RealStateContext(_options);
            var repository = new PropertyTypeRepository(context);
            await context.PropertyTypes.AddAsync(new PropertyType
            {
                Name = "Placeholder",
                Description = "Placeholder"
            });
            await context.SaveChangesAsync();
            var id = (await context.PropertyTypes.FirstAsync()).Id;


            UpdateResourceCommandHandler<PropertyTypeDto, PropertyType> handler =
                new UpdatePropertyTypeCommandHandler(repository, _mapper); // Assuming you have a valid mapper instance

            //Act
            var command = new UpdateResourceCommand<PropertyTypeDto>()
            {
                Id = 0,
                Name = "Placeholder New",
                Description = "Placeholder New"
            };
            //var res = await createCommandHandler.Handle(command, CancellationToken.None);
            var res = await handler.Handle(command, CancellationToken.None);
            //Assert
            res.Should().BeNull();
        }

        [Fact]
        public async Task Execute_Delete_Should_Remove_Element_From_Database()
        {
            //Arrange
            using var context = new RealStateContext(_options);
            var repository = new PropertyTypeRepository(context);
            await context.PropertyTypes.AddAsync(new PropertyType
            {
                Name = "Placeholder",
                Description = "Placeholder"
            });
            await context.SaveChangesAsync();
            var id = (await context.PropertyTypes.FirstAsync()).Id;


            DeleteResourceCommandHandler<PropertyType, PropertyTypeDto> handler =
                new DeletePropertyTypeCommandHandler(repository); // Assuming you have a valid mapper instance

            //Act
            var command = new DeleteResourceCommand<PropertyTypeDto>()
            {
                Id = id
            };
            //var res = await createCommandHandler.Handle(command, CancellationToken.None);
            var res = await handler.Handle(command, CancellationToken.None);
            //Assert
            res.Should().BeTrue();
            var resources = await context.PropertyTypes.ToListAsync();  
            resources.Should().BeEmpty();
        }

        [Fact]
        public async Task Execute_GetById_Should_Bring_Element_From_Database()
        {
            //Arrange
            using var context = new RealStateContext(_options);
            var repository = new PropertyTypeRepository(context);
            await context.PropertyTypes.AddAsync(new PropertyType
            {
                Name = "Placeholder",
                Description = "Placeholder"
            });
            await context.SaveChangesAsync();
            var id = (await context.PropertyTypes.FirstAsync()).Id;


            GetByIdQueryHandler<int,PropertyType, PropertyTypeDto> handler =
                new GetPropertyTypeByIdQueryHandler(repository, _mapper); // Assuming you have a valid mapper instance

            //Act
            var command = new GetByIdQuery<int, PropertyTypeDto>
            {
                Id = id
            };  
            //var res = await createCommandHandler.Handle(command, CancellationToken.None);
            var res = await handler.Handle(command, CancellationToken.None);
            //Assert
            res.Should().NotBeNull();
            res.Name.Should().Be("Placeholder");
            res.Description.Should().Be("Placeholder");
        }

        [Fact]
        public async Task Execute_GetById_Should_Bring_Null_When_Element_Id_Doesnt_Exist_In_BD()
        {
            //Arrange
            using var context = new RealStateContext(_options);
            var repository = new PropertyTypeRepository(context);
            await context.PropertyTypes.AddAsync(new PropertyType
            {
                Name = "Placeholder",
                Description = "Placeholder"
            });
            await context.SaveChangesAsync();
            var id = (await context.PropertyTypes.FirstAsync()).Id;


            GetByIdQueryHandler<int, PropertyType, PropertyTypeDto> handler =
                new GetPropertyTypeByIdQueryHandler(repository, _mapper); // Assuming you have a valid mapper instance

            //Act
            var command = new GetByIdQuery<int, PropertyTypeDto>
            {
                Id = 0
            };
            //var res = await createCommandHandler.Handle(command, CancellationToken.None);
            var res = await handler.Handle(command, CancellationToken.None);
            //Assert
            res.Should().BeNull();
            var resource =  context.PropertyTypes.FirstOrDefault(r => r.Id == 0);
            resource.Should().BeNull();

        }




    }
}
