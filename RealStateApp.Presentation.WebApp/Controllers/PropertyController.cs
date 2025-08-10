using AutoMapper;
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
        private readonly IMapper _mapper;

        public PropertyController(IPropertyService propertyService, IImprovementService improvementService, ISaleTypeService saleTypeService, IPropertyTypeService propertyTypeService, IMapper mapper)
        {
            _propertyService = propertyService;
            _improvementService = improvementService;
            _saleTypeService = saleTypeService;
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements"]);
            var vm = _mapper.Map<List<PropertyViewModel>>(properties)
                .Where(p => p.IsAvailable).ToList();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var saleTypes = await _saleTypeService.GetAll();
            var improvements = await _improvementService.GetAll();
            var propertTypes = await _propertyTypeService.GetAll();
            SavePropertyViewModel propertyViewModel = new SavePropertyViewModel
            {
                Id = 0,
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
                AgentId = "s",
                Description = ""
            };
            return View(propertyViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var saleTypes = await _saleTypeService.GetAll();
                var improvements = await _improvementService.GetAll();
                var propertTypes = await _propertyTypeService.GetAll();
                vm.PropertyTypes = _mapper.Map<List<PropertyTypeViewModel>>(propertTypes);
                vm.Improvements = _mapper.Map<List<ImprovementViewModel>>(improvements);
                vm.SaleTypes = _mapper.Map<List<SaleTypeViewModel>>(saleTypes);


                return View(vm);
            }
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
        public async Task<IActionResult> Edit(int Id)
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements"]);
            var property = properties.FirstOrDefault(p => p.Id == Id);
            var saleTypes = await _saleTypeService.GetAll();
            var improvements = await _improvementService.GetAll();
            var propertTypes = await _propertyTypeService.GetAll();
            SavePropertyViewModel propertyViewModel = new SavePropertyViewModel
            {
                Id = property!.Id,
                Price = property.Price,
                SizeInMeters = property.SizeInMeters,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                IsAvailable = property.IsAvailable,
                PropertyTypeId = property.PropertyTypeId,
                Code = property.Code!,
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
                SaleTypeId = property.SaleTypeId,
                AgentId = property.AgentId,
                Description = property.Description!
            };
            ViewBag.EditMode = true;
            return View("Create", propertyViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SavePropertyViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Create", vm);
            }
            var property = new PropertyDto
            {
                AgentId = vm.AgentId,
                Bathrooms = vm.Bathrooms,
                Bedrooms = vm.Bedrooms,
                Code = vm.Code!,
                Description = vm.Description,
                Id = vm.Id,
                IsAvailable = vm.IsAvailable,
                Price = vm.Price,
                PropertyTypeId = vm.PropertyTypeId,
                SaleTypeId = vm.SaleTypeId,
                SizeInMeters = vm.SizeInMeters

            };
            await _propertyService.UpdateAsync(property.Id, property);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int Id)
        {
            var vm = new DeleteViewModel
            {
                Id = Id
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteViewModel vm)
        {
            
           
            await _propertyService.DeleteAsync(vm.Id);
                
          
            return RedirectToAction("Index");
        }
        public async Task< IActionResult> Details(int Id)
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var property = properties.FirstOrDefault(p => p.Id == Id);
            var vm = _mapper.Map<PropertyViewModel>(property);
            return View(vm);
        }
    }
}
