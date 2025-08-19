using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Presentation.API.Controllers.V1.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Presentation.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaleTypeDto))]
    [SwaggerTag("Matenimiento de tipos de venta")]
    public class SaleTypesController : GenericWritteControllerController<SaleTypeDto, int>
    {
        public SaleTypesController(IMediator mediator) : base(mediator)
        {
        }
    }
}
