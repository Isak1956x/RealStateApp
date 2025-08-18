using AutoMapper;
using MediatR;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Common.GenericCommands
{
    public class UpdateResourceCommand<TDto> : BaseResourceCommand, IRequest<TDto>
    {
        public int Id { get; set; }
    }
    public class UpdateResourceCommandHandler<TDto, TEntity> : IRequestHandler<UpdateResourceCommand<TDto>, TDto>
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
            var entity = _mapper.Map<TEntity>(request);
            await _repository.UpdateAsync(entity);
            return _mapper.Map<TDto>(entity);
        }
    }
}
