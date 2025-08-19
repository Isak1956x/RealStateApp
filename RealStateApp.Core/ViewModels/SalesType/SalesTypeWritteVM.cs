using RealStateApp.Core.Application.ViewModels.Base;

namespace RealStateApp.Core.Application.ViewModels.SalesType
{
    public class SalesTypeWritteVM : BaseWritteVM<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
