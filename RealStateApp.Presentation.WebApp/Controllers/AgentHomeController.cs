using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Presentation.WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class AgentHomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMapper _mapper;

        public AgentHomeController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var propertyTypes = await _propertyTypeService.GetAll();
            var propertiesVm = _mapper.Map<List<PropertyViewModel>>(properties)
                .Where(p => p.AgentId == "d").ToList();

            var AgentHomeVm = new HomeViewModel
            {
                Properties = propertiesVm,
                PropertyTypes = _mapper.Map<List<PropertyTypeViewModel>>(propertyTypes)
            };
            
            return View(AgentHomeVm);
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
