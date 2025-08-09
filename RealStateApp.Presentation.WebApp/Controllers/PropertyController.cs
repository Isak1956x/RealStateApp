using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Core.Domain.Entities;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class PropertyController : Controller
    {

        private readonly IPropertyService _propertyService;
        private readonly IImprovementService _improvementService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyController(IPropertyService propertyService, IImprovementService improvementService, ISaleTypeService saleTypeService, IPropertyTypeService propertyTypeService)
        {
            _propertyService = propertyService;
            _improvementService = improvementService;
            _saleTypeService = saleTypeService;
            _propertyTypeService = propertyTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements"]);
            var vm = properties.Select(p => new PropertyViewModel
            {
                Id = p.Id,
                Description = p.Description,
                Price = p.Price,
                AgentId = p.AgentId,
                Code = p.Code,
                PropertyTypeId = p.PropertyTypeId,
                Bathrooms = p.Bathrooms,
                Bedrooms = p.Bedrooms,
                SizeInMeters = p.SizeInMeters,
                SaleTypeId = p.SaleTypeId,
                PropertyType = new PropertyTypeViewModel
                {
                    Id = p.PropertyType.Id,
                    Name = p.PropertyType.Name
                },
                SaleType = new SaleTypeViewModel
                {
                    Id = p.SaleType.Id,
                    Name = p.SaleType.Name
                },
                PropertyImprovements = p.PropertyImprovements.Select(i => new PropertyImprovementViewModel
                {
                    ImprovementId = i.ImprovementId,
                    Improvement = new ImprovementViewModel
                    {
                        Id = i.Improvement.Id,
                        Name = i.Improvement.Name
                    },
                    PropertyId = i.PropertyId

                }).ToList(),
            }).ToList();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var saleTypes = await _saleTypeService.GetAll();
            var improvements = await _improvementService.GetAll();
            var propertTypes = await _propertyTypeService.GetAll();
            SavePropertyViewModel propertyViewModel = new SavePropertyViewModel
            {
                Price = 0,
                SizeInMeters = 0,
                Bedrooms = 0,
                Bathrooms = 0,
                IsAvailable = true,
                PropertyTypeId = 0,
                PropertyTypes = propertTypes.Select(pt => new PropertyTypeViewModel
                {
                    Id = pt.Id,
                    Name = pt.Name
                }).ToList(),
                Improvements = improvements.Select(i => new ImprovementViewModel
                {
                    Id = i.Id,
                    Name = i.Name
                }).ToList(),
                SaleTypes = saleTypes.Select(st => new SaleTypeViewModel
                {
                    Id = st.Id,
                    Name = st.Name
                }).ToList(),
                SaleTypeId = 0,
                AgentId= "s",
                Description = ""
            };
            return View(propertyViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyViewModel vm)
        {
           var propertyDto = new PropertyDto
           {
               Price = vm.Price,
               SizeInMeters = vm.SizeInMeters,
               Bedrooms = vm.Bedrooms,
               Bathrooms = vm.Bathrooms,
               Description = vm.Description,
               IsAvailable = vm.IsAvailable,
               PropertyTypeId = vm.PropertyTypeId,
               SaleTypeId = vm.SaleTypeId,
               AgentId = "d",
               Code = Guid.NewGuid().ToString().Substring(0, 6).ToUpper(),
               Id = 0
           };
            var result = await _propertyService.AddAsync(propertyDto);
            return RedirectToAction("Index");
        }

    }
}
