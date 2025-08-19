using AutoMapper;
using MediatR;
using RealStateApp.Core.Domain.Interfaces;

namespace RealStateApp.Core.Application.Features.Common.GennericQueries
{
    public class GetAllListQuery<Tid, Tdto> : IRequest<IEnumerable<Tdto>>
    {
    }

    public abstract class GetAllListQueryHandler<Tid, TEntity, Tdto> : IRequestHandler<GetAllListQuery<Tid, Tdto>, IEnumerable<Tdto>>
        where TEntity : class
        where Tdto : class
    {
        private readonly IRepositoryBase<TEntity, Tid> _repository;
        private readonly IMapper _mapper;
        public GetAllListQueryHandler(IRepositoryBase<TEntity, Tid> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Tdto>> Handle(GetAllListQuery<Tid, Tdto> request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<Tdto>>(entities);
        }
    }
}
