namespace RealStateApp.Core.Application.Interfaces
{
    public interface IGenericService<DtoModel> where DtoModel : class
    {
        Task<DtoModel?> AddAsync(DtoModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<DtoModel>> GetAll();
        Task<List<DtoModel>> GetAllListWithInclude(List<string> properties);
        Task<DtoModel?> GetById(int id);
        Task<DtoModel?> UpdateAsync(DtoModel dto);
        Task<DtoModel?> UpdateAsync(int Id, DtoModel dto);

    }
}