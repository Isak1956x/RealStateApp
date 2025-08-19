using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Presentation.WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Agent")]
    public class AgentHomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountServiceForWebApp _accountService;

        public AgentHomeController(IPropertyService propertyService, IPropertyTypeService propertyTypeService, IMapper mapper, UserManager<AppUser> userManager, IAccountServiceForWebApp accountService)
        {
            _propertyService = propertyService;
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
            _userManager = userManager;
            _accountService = accountService;
        }

        public async Task<IActionResult> Index(HomeViewModel? filters)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            ViewBag.AgentName = $"{userSession!.FirstName} {userSession.LastName}";
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var propertyTypes = await _propertyTypeService.GetAll();
            var propertiesVm = _mapper.Map<List<PropertyViewModel>>(properties)
                .Where(p => p.AgentId == userSession!.Id).OrderByDescending(p => p.IsAvailable).ToList();
            if (filters != null && propertiesVm != null)
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
            var AgentHomeVm = new HomeViewModel
            {
                Properties = propertiesVm,
                PropertyTypes = _mapper.Map<List<PropertyTypeViewModel>>(propertyTypes)
            };
            
            return View(AgentHomeVm);
        }

        public async Task< IActionResult> MyProfile()
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            var userDto = await _accountService.GetUserByIdAsync(userSession!.Id);
            var userVm = _mapper.Map<UpdateUserViewModel>(userDto.Value);
            return View(userVm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
