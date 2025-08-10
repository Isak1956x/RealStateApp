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
        private readonly IMapper _mapper;

        public AgentHomeController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var propertiesVm = _mapper.Map<List<PropertyViewModel>>(properties).ToList();

            return View(propertiesVm);
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
