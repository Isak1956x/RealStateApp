using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Features.Common.GenericCommands;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Presentation.API.Controllers.V1.Generic
{

    public abstract class GenericWritteControllerController<TDto, TId> : GenericReadController<TDto, TId>
    {
        public GenericWritteControllerController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Create",
            Description = "Permite crear un nuevo recurso apartir de un Json"
        )]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TDto>> Create([FromBody] CreateResourceCommand<TDto> command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Created();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Update",
            Description = "Permite actualizar un  recurso apartir de un Json"
        )]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TDto>> Update([FromBody] UpdateResourceCommand<TDto> command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Delete",
            Description = "Permite borrar un  recurso apartir de su id"
        )]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteResourceCommand<TDto> { Id = id };
            var result = await _mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
