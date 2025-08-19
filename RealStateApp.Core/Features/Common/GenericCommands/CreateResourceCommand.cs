using AutoMapper;
using MediatR;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Common.GenericCommands
{
    public class CreateResourceCommand<TDto> : BaseResourceCommand, IRequest<TDto>
    {
    }

    public abstract class CreateResourceCommandHandler<TEntity, TDto> : IRequestHandler<CreateResourceCommand<TDto>, TDto>  where TEntity : class
    {
        private readonly IRepositoryBase<TEntity, int> _repository;
        private readonly IMapper _mapper;

        public CreateResourceCommandHandler(IRepositoryBase<TEntity, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
                

        public async Task<TDto> Handle(CreateResourceCommand<TDto> request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name cannot be null or empty");
            }
            try
            {
                TEntity entity = _mapper.Map<TEntity>(request as BaseResourceCommand);
                var returnEntity = await _repository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return default;
                }

                return _mapper.Map<TDto>(returnEntity.Value);
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
