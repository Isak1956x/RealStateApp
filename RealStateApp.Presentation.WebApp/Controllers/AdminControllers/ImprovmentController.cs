using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels.Base;
using RealStateApp.Core.Application.ViewModels.PropertyImprovment;
using RealStateApp.Core.Application.ViewModels.PropertyTypes;

namespace RealStateApp.Presentation.WebApp.Controllers.AdminControllers
{
    public class ImprovmentController : Controller
    {
        private readonly IImprovementService _improvmentService;
        private readonly IMapper _mapper;

        public ImprovmentController(IImprovementService propertyImprovmentService, IMapper mapper)
        {
            _improvmentService = propertyImprovmentService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var imps = await _improvmentService.GetAll();
            var vms = imps.Select(i => _mapper.Map<ImprovmentReadVM>(i));
            return View(vms);
        }

        public IActionResult New()
        {
            return View(new ImprovmentWritteVM());
        }

        [HttpPost]
        public async Task<IActionResult> New(ImprovmentWritteVM VM)
        {
            if (!ModelState.IsValid)
            {
                return View(VM);
            }
            var DTO = _mapper.Map<ImprovementDto>(VM);
            var result = await _improvmentService.AddAsync(DTO);
            if (result != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error creating property type. Please try again.";
            return View(VM);
        }

        public async Task<IActionResult> Update(int id)
        {
            var improvement = await _improvmentService.GetById(id);
            if (improvement == null)
            {
                return NotFound();
            }
            var vm = _mapper.Map<ImprovmentWritteVM>(improvement);
            vm.IsCreating = false;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ImprovmentWritteVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var dto = _mapper.Map<ImprovementDto>(vm);
            var result = await _improvmentService.UpdateAsync(dto);
            if (result != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error updating property type. Please try again.";
            return View(vm);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var imp = await _improvmentService.GetById(id);

            var Vm = new GenericAlertVM<int>
            {
                Title = "Delete Property Improvement type",
                Value = id,
                Message = $"Are you sure you want to delete the Improvement type {imp.Name}?",
                AlertType = "warning",
                Controller = "Improvment",
                ActionDestination = "Delete"
            };
            return View(Vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GenericAlertVM<int> vm)
        {
            var result = await _improvmentService.DeleteAsync(vm.Value);
            if (result)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Error deleting improvement type. Please try again.";
            return View(vm);

        }
    }
}
