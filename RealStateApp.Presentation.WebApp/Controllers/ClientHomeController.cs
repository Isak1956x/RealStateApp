using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Presentation.WebApp.Models;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class ClientHomeController : Controller
    {

 private readonly IMapper _mapper;
 private readonly IFavoritePropertyService _favoritePropertyService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyTypeService _propertyTypeServices;
        private readonly UserManager<AppUser> _userManager;

        public ClientHomeController(IMapper mapper, IFavoritePropertyService favoritePropertyService, IPropertyService propertyService, IPropertyTypeService propertyTypeServices, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _favoritePropertyService = favoritePropertyService;
            _propertyService = propertyService;
            _propertyTypeServices = propertyTypeServices;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(HomeViewModel? filters)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var propertyTypes = await _propertyTypeServices.GetAll();

            var favPropertiesIds = (await _favoritePropertyService.GetAll()).Where(f => f.UserId == userSession!.Id).Select(f => f.PropertyId).ToList();
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
                PropertyTypes = propertyTypesVm,
                FavPropertiesIds = favPropertiesIds
            };
            if(userSession != null)
            {
            ViewBag.ClientName = $"{userSession.FirstName} {userSession.LastName}";

            }
            return View(homeVm);
        }

        public async Task<IActionResult> MyProperties(HomeViewModel? filters)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            var propertyTypes = await _propertyTypeServices.GetAll();
            var favProperties = (await _favoritePropertyService.GetAllListWithInclude(["Property"])).Where(f => f.UserId == userSession!.Id).Select(p => p.PropertyId);
            var properties = (await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"])).Where(p => favProperties.Contains(p.Id) && p.IsAvailable);
            var favPropertiesVm = _mapper.Map<List<PropertyViewModel>>(properties);
            if (filters != null && favPropertiesVm != null)
            {
                if (filters.Code != null)
                {
                    var codeUpper = filters.Code.ToUpper();
                    favPropertiesVm = favPropertiesVm
                        .Where(p => p.Code.Contains(codeUpper))
                        .ToList();
                }
                else
                {
                    if (filters.PropertyTypeId.HasValue)
                        favPropertiesVm = favPropertiesVm
                            .Where(p => p.PropertyTypeId == filters.PropertyTypeId)
                            .ToList();

                    if (filters.MinPrice.HasValue)
                        favPropertiesVm = favPropertiesVm
                            .Where(p => p.Price >= filters.MinPrice)
                            .ToList();

                    if (filters.MaxPrice.HasValue)
                        favPropertiesVm = favPropertiesVm
                            .Where(p => p.Price <= filters.MaxPrice)
                            .ToList();

                    if (filters.Bedrooms.HasValue)
                        favPropertiesVm = favPropertiesVm
                            .Where(p => p.Bedrooms == filters.Bedrooms)
                            .ToList();

                    if (filters.Bathrooms.HasValue)
                        favPropertiesVm = favPropertiesVm
                            .Where(p => p.Bathrooms == filters.Bathrooms)
                            .ToList();
                }
            }
            var myPropertiesVm = new MyPropertiesViewModel
            {
                Properties = favPropertiesVm,
                PropertyTypes = _mapper.Map<List<PropertyTypeViewModel>>(propertyTypes),
            };
            return View(myPropertiesVm);
        }
      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
