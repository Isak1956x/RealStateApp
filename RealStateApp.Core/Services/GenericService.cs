

using AutoMapper;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Domain.Interfaces;


namespace RealStateApp.Core.Application.Services
{
    public class GenericService<Entity, DtoModel> : IGenericService<DtoModel> where Entity : class
        where DtoModel : class
    {
        private readonly IRepositoryBase<Entity, int> _repository;

        private readonly IMapper _mapper;

        public GenericService(IRepositoryBase<Entity, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public virtual async Task<DtoModel?> AddAsync(DtoModel dto)
        {
            try
            {
                Entity entity = _mapper.Map<Entity>(dto);
                var returnEntity = await _repository.AddAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<DtoModel>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public virtual async Task<DtoModel?> UpdateAsync(DtoModel dto)
        {
            try
            {
                Entity entity = _mapper.Map<Entity>(dto);
                var returnEntity = await _repository.UpdateAsync(entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<DtoModel>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<bool> DeleteAsync(DtoModel dto)
        {
            try
            {
                Entity entity = _mapper.Map<Entity>(dto);
                await _repository.DeleteAsync(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public virtual async Task<DtoModel?> GetById(int id)
        {
            try
            {
                var entity = await _repository.GetById(id);
                if (entity == null)
                {
                    return null;
                }

                DtoModel dto = _mapper.Map<DtoModel>(entity);
                return dto;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public virtual async Task<List<DtoModel>> GetAll()
        {
            try
            {
                var listEntities = await _repository.GetAllAsync();
                var listEntityDtos = _mapper.Map<List<DtoModel>>(listEntities);

                return listEntityDtos;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await _repository.DeleteAsync(id);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<List<DtoModel>> GetAllListWithInclude(List<string> properties)
        {
            try
            {
                var listEntities = await _repository.GetAllListWithInclude(properties);
                var listEntityDtos = _mapper.Map<List<DtoModel>>(listEntities);

                return listEntityDtos;
            }
            catch (Exception)
            {

                return [];
            }

        }

        public async Task<DtoModel?> UpdateAsync( int id, DtoModel dto)
        {
            try
            {
                Entity entity = _mapper.Map<Entity>(dto);
                var returnEntity = await _repository.UpdateAsync(id, entity);
                if (returnEntity == null)
                {
                    return null;
                }

                return _mapper.Map<DtoModel>(returnEntity);
            }
            catch (Exception)
            {
                return null;
            }
        }
  
    }
}
