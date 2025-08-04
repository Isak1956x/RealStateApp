namespace RealStateApp.Core.Application.Wrappers
{
    public class PaginatedResponse<TData> where TData : class
    {
        public required IEnumerable<TData> Data { get; set; }
        public required Pagination Pagination { get; set; }

        public PaginatedResponse<TMap> Map<TMap>(Func<TData, TMap> mapFunction) where TMap : class
        {
            var data = Data.Select(mapFunction);
            return new PaginatedResponse<TMap>
            {
                Data = data,
                Pagination = this.Pagination
            };
        }


    }
}
