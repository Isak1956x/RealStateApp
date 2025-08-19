using AutoMapper;
using MediatR;
using RealStateApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Common.GennericQueries
{
    public class GetByIdQuery<Tid, Tdto> : IRequest<Tdto> 
    {
        public Tid Id { get; set; } 
    }

    public abstract class GetByIdQueryHandler<Tid,TEntity,TDto> : IRequestHandler<GetByIdQuery<Tid, TDto>, TDto>
        where TEntity : class
        where TDto : class
    {
        private readonly IRepositoryBase<TEntity, Tid> _repository;
        private readonly IMapper _mapper;
        public GetByIdQueryHandler(IRepositoryBase<TEntity, Tid> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TDto> Handle(GetByIdQuery<Tid, TDto> request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetById(request.Id);
            return _mapper.Map<TDto>(entity.Value);
        }
    }
}
