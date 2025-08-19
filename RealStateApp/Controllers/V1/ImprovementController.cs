using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Presentation.API.Controllers.V1.Generic;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Presentation.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementDto))]
    [SwaggerTag("Matenimiento de Mejoras")]
    public class ImprovementController : GenericWritteControllerController<Core.Application.DTOs.ImprovementDto, int>
    {   
        public ImprovementController(MediatR.IMediator mediator) : base(mediator)
        {
        }
    }
}
