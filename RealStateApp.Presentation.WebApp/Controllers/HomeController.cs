using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Presentation.WebApp.Models;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {

 private readonly IPropertyService _propertyService;
 private readonly IPropertyTypeService _propertyTypeServices;
 private readonly IMapper _mapper;

        public HomeController(IPropertyService propertyService, IPropertyTypeService propertyTypeServices, IMapper mapper)
        {
            _propertyService = propertyService;
            _propertyTypeServices = propertyTypeServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(HomeViewModel? filters)
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var propertyTypes = await _propertyTypeServices.GetAll();
            var propertiesVm = _mapper.Map<List<PropertyViewModel>>(properties)
                .Where(p => p.IsAvailable)
                .OrderByDescending(p => p.CreateAt).ToList();
           if(filters != null && propertiesVm != null)
            {
                if (filters.Code != null)
                {
                    var codeUpper = filters.Code.ToUpper();
                    propertiesVm = propertiesVm
                        .Where(p => p.Code.Contains(codeUpper))
                        .ToList();
                }
                else
                {
                    if (filters.PropertyTypeId.HasValue)
                        propertiesVm = propertiesVm
                            .Where(p => p.PropertyTypeId == filters.PropertyTypeId)
                            .ToList();

                    if (filters.MinPrice.HasValue)
                        propertiesVm = propertiesVm
                            .Where(p => p.Price >= filters.MinPrice)
                            .ToList();

                    if (filters.MaxPrice.HasValue)
                        propertiesVm = propertiesVm
                            .Where(p => p.Price <= filters.MaxPrice)
                            .ToList();

                    if (filters.Bedrooms.HasValue)
                        propertiesVm = propertiesVm
                            .Where(p => p.Bedrooms == filters.Bedrooms)
                            .ToList();

                    if (filters.Bathrooms.HasValue)
                        propertiesVm = propertiesVm
                            .Where(p => p.Bathrooms == filters.Bathrooms)
                            .ToList();
                }
            }
            var propertyTypesVm = _mapper.Map<List<PropertyTypeViewModel>>(propertyTypes);
            var homeVm = new HomeViewModel
            {
                Properties = propertiesVm,
                PropertyTypes = propertyTypesVm
            };
            
            return View(homeVm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
