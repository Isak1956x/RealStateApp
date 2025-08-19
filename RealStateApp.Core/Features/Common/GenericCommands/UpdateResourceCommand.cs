using AutoMapper;
using MediatR;
using RealStateApp.Core.Domain.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace RealStateApp.Core.Application.Features.Common.GenericCommands
{
    public class UpdateResourceCommand<TDto> : BaseResourceCommand, IRequest<TDto>
    {
        [SwaggerParameter("Identifier of the parameter to update")]
        public int Id { get; set; }
    }
    public abstract class UpdateResourceCommandHandler<TDto, TEntity> : IRequestHandler<UpdateResourceCommand<TDto>, TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IRepositoryBase<TEntity, int> _repository;
        private readonly IMapper _mapper;
        public UpdateResourceCommandHandler(IRepositoryBase<TEntity, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TDto> Handle(UpdateResourceCommand<TDto> request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetById(request.Id);
            if (result.IsFailure)
                return null;

            var existingEntity = result.Value;
            _mapper.Map(request, existingEntity);

            var res = await _repository.UpdateAsync(existingEntity);
            if (res.IsFailure)
            {
                return null;
            }

            return _mapper.Map<TDto>(res.Value);
        }
    }
}
