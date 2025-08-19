using System.Runtime.InteropServices;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;
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
        private readonly IAccountServiceForWebApp _accountService;
        private readonly IChatService _chatService;
        private readonly IOfferService _offeService;
        private readonly UserManager<AppUser> _userManager;

        private readonly IPropertyImprovementService _propertyImprovementService;

        public PropertyController(IPropertyService propertyService, IImprovementService improvementService, ISaleTypeService saleTypeService, IPropertyTypeService propertyTypeService, IPropertyImageService propertyImageService, IFavoritePropertyService favoritePropertyService, IMapper mapper, IAccountServiceForWebApp accountService, IChatService chatService, IOfferService offeService, UserManager<AppUser> userManager, IPropertyImprovementService propertyImprovementService)
        {
            _propertyService = propertyService;
            _improvementService = improvementService;
            _saleTypeService = saleTypeService;
            _propertyTypeService = propertyTypeService;
            _propertyImageService = propertyImageService;
            _favoritePropertyService = favoritePropertyService;
            _mapper = mapper;
            _accountService = accountService;
            _chatService = chatService;
            _offeService = offeService;
            _userManager = userManager;
            _propertyImprovementService = propertyImprovementService;
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> PropertiesMaintenance()
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var vm = _mapper.Map<List<PropertyViewModel>>(properties)
                .Where(p => p.IsAvailable).ToList();
            return View(vm);
        }
        public async Task<IActionResult> agentProperties(string agentId, bool isCLient = false)
        {
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var vm = _mapper.Map<List<PropertyViewModel>>(properties)
                .Where(p => p.IsAvailable && p.AgentId == agentId).ToList();
            var agent = await _accountService.GetUserByIdAsync(agentId);
            ViewBag.agentName = $"{agent.Value.FirstName} {agent.Value.LastName} ";
            ViewBag.IsClient = isCLient;
            return View("Index", vm);
        }

        [Authorize(Roles = "Agent")]
        public async Task<IActionResult> Create()
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
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
                Improvements = _mapper.Map<List<ImprovementViewModel>>(improvements),
                SaleTypes = saleTypes.Select(st => new SaleTypeViewModel
                {
                    Id = st.Id,
                    Name = st.Name
                }).ToList(),
                SaleTypeId = 0,
                AgentId = userSession!.Id,
                Description = "",
                ImprovementIds = new List<int>()
                };
            return View(propertyViewModel);
        }
        [Authorize(Roles = "Agent")]
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
            if(!vm.ImprovementIds.Any())
            {
                ModelState.AddModelError("ImprovementIds", "Improvement is alo");
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
                AgentId = vm.AgentId,
                Code = Guid.NewGuid().ToString().Substring(0, 6).ToUpper(),
                Id = 0
            };
            var result = await _propertyService.AddAsync(propertyDto);
            if (result != null)

                foreach (var improvement in vm.ImprovementIds)
                {
                    var propertyImprovementDto = new PropertyImprovementDto
                    {
                        PropertyId = result.Id,
                        ImprovementId = improvement
                    };
                    await _propertyImprovementService.AddAsync(propertyImprovementDto);
                }
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
            
                return RedirectToAction("PropertiesMaintenance");
        }
        [Authorize(Roles = "Agent")]
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
        [Authorize(Roles = "Agent")]
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
        [Authorize(Roles = "Agent")]
        public IActionResult Delete(int Id)
        {
            var vm = new DeleteViewModel
            {
                Id = Id
            };
            return View(vm);
        }
        [Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteViewModel vm)
        {
            
           
            await _propertyService.DeleteAsync(vm.Id);
                
          
            return RedirectToAction("Index");
        }
        public async Task< IActionResult> Details(int Id)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            var properties = await _propertyService.GetAllListWithInclude(["PropertyType", "SaleType", "PropertyImprovements", "Images"]);
            var improvements = await _improvementService.GetAll();
            var improvementsVm = _mapper.Map<List<ImprovementViewModel>>(improvements);
            var property = properties.FirstOrDefault(p => p.Id == Id);
            var agent = await _accountService.GetUserByIdAsync(property!.AgentId!);
            var agentVm = _mapper.Map<UserViewModel>(agent.Value);

            var propertiesVm = _mapper.Map<PropertyViewModel>(property);
            foreach (var pi in propertiesVm.PropertyImprovements!)
            {
                var improvement = improvementsVm.FirstOrDefault(i => i.Id == pi.ImprovementId);
                if (improvement != null)
                {
                    pi.Improvement = _mapper.Map<ImprovementViewModel>(improvement);
                }
            }
            var vm = new PropertyDetailsViewModel
            {
                Property = propertiesVm,
                Agent = agentVm
            }; 
            
            if (userSession != null)
            {

                var user = await _accountService.GetUserByIdAsync(userSession!.Id!);
                if (user.Value.Role == UserRoles.Agent.ToString() || user.Value.Role == UserRoles.Client.ToString())
                    vm.Addons = true;
                vm.Rol = user.Value.Role;


            }

            if (vm.Addons)
            {
                var user = await _accountService.GetUserByIdAsync(userSession!.Id!);
                var chats = await _chatService.GetAllListWithInclude(["Messages"]);
                var offers = (await _offeService.GetAll()).Where(o => o.PropertyId == Id);
               
                if (user.Value.Role == UserRoles.Client.ToString())
                {

                   offers = offers.Where(o => o.ClientId == userSession!.Id && o.PropertyId == Id).ToList();

                    var chat = chats.FirstOrDefault(c => c.PropertyId == Id && c.CustomerId == userSession!.Id);
                    if (chat == null)
                    {
                        var chatDto = new ChatDto
                        {
                            PropertyId = Id,
                            CustomerId = userSession!.Id,
                            AgentId = property.AgentId!
                        };
                        chat = await _chatService.AddAsync(chatDto);
                    }
                    vm.Chat = _mapper.Map<ChatViewModel>(chat);
                    vm.Offers = _mapper.Map<List<OfferViewModel>>(offers);
                    if (vm.Chat.Messages != null)
                    {
                        foreach (var message in vm.Chat.Messages)
                        {
                            var messageUser = await _accountService.GetUserByIdAsync(message.SenderID);
                            message.isFromClient = messageUser.Value.Role == UserRoles.Client.ToString();

                        }
                    }
                }
                else
                {

                    vm.Offers = _mapper.Map<List<OfferViewModel>>(offers);
                    var agentChats = chats.Where(c => c.AgentId == userSession!.Id && c.PropertyId == Id).ToList();
                    if(agentChats != null)
                    { 
                    vm.AgentChat = _mapper.Map<List<ChatViewModel>>(agentChats);
                        foreach (var chat in vm.AgentChat)
                        {
                            var chatUser = await _accountService.GetUserByIdAsync(chat.CustomerId);
                            chat.SenderName = $"{chatUser.Value.FirstName} {chatUser.Value.LastName}";
                            chat.LastMessage = chat.Messages?.LastOrDefault()?.Content;
                            if (chat.Messages != null)
                            {
                                foreach (var message in chat.Messages)
                                {

                                    var messageUser = await _accountService.GetUserByIdAsync(message.SenderID);
                                    message.isFromClient = messageUser.Value.Role == UserRoles.Client.ToString();
                                    if (message.isFromClient)
                                    {
                                        message.SenderName = chat.SenderName;
                                    }

                                }
                            }
                        }
                        if (vm.Offers != null)
                        {

                            foreach (var offer in vm.Offers!)
                            {
                                var client = await _accountService.GetUserByIdAsync(offer.ClientId);
                                offer.ClientName = $"{client.Value.FirstName} {client.Value.LastName}";
                            }
                        }
                    }
                }

            }
        
            return View(vm);
        }
        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> AddFavoriteProperty(int Id, bool isFavorite, bool myProperties = false)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            if (isFavorite)
            {
                var deleteProperty = (await _favoritePropertyService.GetAll())
                    .FirstOrDefault(f => f.PropertyId == Id && f.UserId == userSession!.Id);
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
                UserId = userSession!.Id
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
