using System.Runtime.InteropServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Presentation.WebApp.Handlers;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class PropertyController : Controller
    {

        private readonly IPropertyService _propertyService;
        private readonly IImprovementService _improvementService;
        private readonly ISaleTypeService _saleTypeService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IPropertyImageService _propertyImageService;
        private readonly IFavoritePropertyService _favoritePropertyService;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyService propertyService, IImprovementService improvementService, ISaleTypeService saleTypeService, IPropertyTypeService propertyTypeService, IPropertyImageService propertyImageService, IFavoritePropertyService favoritePropertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _improvementService = improvementService;
            _saleTypeService = saleTypeService;
            _propertyTypeService = propertyTypeService;
            _propertyImageService = propertyImageService;
            _favoritePropertyService = favoritePropertyService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
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
            if(!vm.Images.Any())
            {
                ModelState.AddModelError("Images", "A Image is required");
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
            if (result != null)
            {
                if (vm.Images.Any())
                {
                    foreach (var image in vm.Images)
                    {
                        var imageDto = new PropertyImageDto
                        {
                            PropertyId = result.Id,
                            Url = FileHandler.Upload(image, result.Id!, "Images")!
                        };
                        await _propertyImageService.AddAsync(imageDto);
                    }
                }
            }
                return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int Id)
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
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
                Description = property.Description!,
                Property = _mapper.Map<PropertyViewModel>(property)
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
            var result = await _propertyService.UpdateAsync(property.Id, property);
            if (result != null)
            {
                if (vm.Images.Any())
                {
                    foreach (var image in vm.Images)
                    {
                        var imageDto = new PropertyImageDto
                        {
                            PropertyId = result.Id,
                            Url = FileHandler.Upload(image, result.Id!, "Images")!
                        };
                        await _propertyImageService.AddAsync(imageDto);
                    }
                }
            }
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
        public async Task< IActionResult> Details(int Id, bool isAgent = false)
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var improvements = await _improvementService.GetAll();
            var improvementsVm = _mapper.Map<List<ImprovementViewModel>>(improvements);
            var property = properties.FirstOrDefault(p => p.Id == Id);
           
            var vm = _mapper.Map<PropertyViewModel>(property);
            foreach (var pi in vm.PropertyImprovements)
            {
                var improvement = improvementsVm.FirstOrDefault(i => i.Id == pi.ImprovementId);
                if (improvement != null)
                {
                    pi.Improvement = _mapper.Map<ImprovementViewModel>(improvement);
                }
            }
            if (isAgent)
            {
                ViewBag.IsAgent = true;
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavoriteProperty(int Id, bool isFavorite, bool myProperties = false)
        {
            if(isFavorite)
            {
                var deleteProperty = (await _favoritePropertyService.GetAll())
                    .FirstOrDefault(f => f.PropertyId == Id && f.UserId == "g");
                if (deleteProperty != null)
                {
                    await _favoritePropertyService.DeleteAsync(deleteProperty.FavoritePropertyId);
                }
                if(myProperties)
                {
                    return RedirectToRoute(new { controller = "ClientHome", action = "MyProperties" });
                }
                    return RedirectToRoute(new { controller = "ClientHome", action = "Index" });
            }
            var favProperty = new FavoritePropertyDto
            {
                PropertyId = Id,
                UserId = "g"
            };
            await _favoritePropertyService.AddAsync(favProperty);
            if (myProperties)
            {
                return RedirectToRoute(new { controller = "ClientHome", action = "MyProperties" });
            }
            return RedirectToRoute(new { controller = "ClientHome", action = "Index" });
        }
    }
}
