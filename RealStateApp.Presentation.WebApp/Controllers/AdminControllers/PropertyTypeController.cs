using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.Base;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;

namespace RealStateApp.Presentation.WebApp.Controllers.AdminControllers
{
    public class PropertyTypeController : Controller
    {
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMapper _mapper;

        public PropertyTypeController(IPropertyTypeService propertyTypeService, IMapper mapper)
        {
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var propertyTypes = await _propertyTypeService.GetAllPropertyTypesAsync();
            return View(propertyTypes.Select(pt => _mapper.Map<PropertyTypeReadVM>(pt)));
        }

        public IActionResult New()
        {
            return View("Save", new PropertyTypeWriteVM());
        }

        [HttpPost]
        public async Task<IActionResult> New(PropertyTypeWriteVM propertyTypeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyTypeVM);
            }
            var propertyTypeDTO = _mapper.Map<PropertyTypeDto>(propertyTypeVM);
            var result = await _propertyTypeService.AddAsync(propertyTypeDTO);
            if (result != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error creating property type. Please try again.";
            return View("Save",propertyTypeVM);
        }

        public async Task<IActionResult> Update(int id)
        {
            var propertyType = await _propertyTypeService.GetById(id);
            if (propertyType == null)
            {
                return NotFound();
            }
            var propertyTypeVM = _mapper.Map<PropertyTypeWriteVM>(propertyType);
            propertyTypeVM.IsCreating = false;
            return View("Save", propertyTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(PropertyTypeWriteVM propertyTypeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(propertyTypeVM);
            }
            var propertyTypeDTO = _mapper.Map<PropertyTypeDto>(propertyTypeVM);
            var result = await _propertyTypeService.UpdateAsync(propertyTypeDTO);
            if (result != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error updating property type. Please try again.";
            return View("Save", propertyTypeVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pt = await _propertyTypeService.GetById(id);

            var Vm = new GenericAlertVM<int>
            {
                Title = "Delete Property Type",
                Value = id,
                Message = $"Are you sure you want to delete the property type {pt.Name}?",
                AlertType = "warning",
                Controller = "PropertyType",
                ActionDestination = "Delete"
            };
            return View(Vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GenericAlertVM<int> vm)
        {
            var result = await _propertyTypeService.DeleteAsync(vm.Value);
            if (result)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error deleting property type. Please try again.";  
            return View(vm);

        }
    }
}
