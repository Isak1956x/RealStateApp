using MediatR;
using RealStateApp.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealStateApp.Core.Application.Features.Common.GenericCommands
{
    public class DeleteResourceCommand<TDto> : IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class DeleteResourceCommandHandler<TEntity, TDto> : IRequestHandler<DeleteResourceCommand<TDto>, bool> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity, int> _repository;

        public DeleteResourceCommandHandler(IRepositoryBase<TEntity, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteResourceCommand<TDto> request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _repository.GetById(request.Id);
                if (res.IsFailure)
                {
                    return false;
                }
                TEntity entity = res.Value;
                await _repository.DeleteAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }


}
