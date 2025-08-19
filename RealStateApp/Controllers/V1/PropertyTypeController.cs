using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Presentation.API.Controllers.V1.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Presentation.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeDto))]
    [SwaggerTag("Matenimiento de Tipos de propiedades")]
    public class PropertyTypeController : GenericWritteControllerController<PropertyTypeDto, int>
    {
        public PropertyTypeController(IMediator mediator) : base(mediator)
        {
        }
    }
}
