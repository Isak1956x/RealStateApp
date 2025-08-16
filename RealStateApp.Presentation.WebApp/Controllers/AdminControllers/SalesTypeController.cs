using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.Base;
using RealStateApp.Core.Application.ViewModels.SalesType;

namespace RealStateApp.Presentation.WebApp.Controllers.AdminControllers
{
    public class SalesTypeController : Controller
    {
        private readonly ISaleTypeService _saleTypeService;
        private readonly IMapper _mapper;

        public SalesTypeController(ISaleTypeService saleTypeService, IMapper mapper)
        {
            _saleTypeService = saleTypeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var saleTypes = await _saleTypeService.GetAllSalesTypesAsync();
            return View(saleTypes.Select(st => _mapper.Map<SalesTypeReadVM>(st)));
        }

        public IActionResult New()
        {
            return View(new SalesTypeWritteVM());
        }

        [HttpPost]
        public async Task<IActionResult> New(SalesTypeWritteVM salesTypeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(salesTypeVM);
            }
            var salesTypeDTO = _mapper.Map<SaleTypeDto>(salesTypeVM);
            var result = await _saleTypeService.AddAsync(salesTypeDTO);
            if (result != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error creating property type. Please try again.";
            return View(salesTypeVM);
        }

        public async Task<IActionResult> Update(int id)
        {
            var salesType = await _saleTypeService.GetById(id);
            if (salesType == null)
            {
                return NotFound();
            }
            var salesTypeVM = _mapper.Map<SalesTypeWritteVM>(salesType);
            salesTypeVM.IsCreating = false;
            return View(salesTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(SalesTypeWritteVM salesTypeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(salesTypeVM);
            }
            var salesTypeDTO = _mapper.Map<SaleTypeDto>(salesTypeVM);
            var result = await _saleTypeService.UpdateAsync(salesTypeDTO);
            if (result != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error updating property type. Please try again.";
            return View(salesTypeVM);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var salesType = await _saleTypeService.GetById(id);
            var Vm = new GenericAlertVM<int>
            {
                Title = "Delete Sales type",
                Value = id,
                Message = $"Are you sure you want to delete the property type {salesType.Name}?",
                AlertType = "warning",
                Controller = "SalesType",
                ActionDestination = "Delete"
            };
            return View(Vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GenericAlertVM<int> alertVM)
        {
            if (alertVM.Value <= 0)
            {
                return BadRequest("Invalid Sales Type ID.");
            }
            var result = await _saleTypeService.DeleteAsync(alertVM.Value);
            if (result)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error deleting property type. Please try again.";
            return View(alertVM);
        }
    }
}
