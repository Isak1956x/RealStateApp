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
    [Authorize(Roles = "Agent,Client")]
    public class AddonsController : Controller
    {
        private readonly IOfferService _offerService;
        private readonly IMessageService _messageService;
        private readonly IPropertyService _propertyService;
        private readonly UserManager<AppUser> _userManager;

        public AddonsController(IOfferService offerService, IMessageService messageService, IPropertyService propertyService, UserManager<AppUser> userManager)
        {
            _offerService = offerService;
            _messageService = messageService;
            _propertyService = propertyService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(int propertyId, int chatId, string message)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            if (!string.IsNullOrWhiteSpace(message))
            {
                var messageDto = new MessageDto
                {
                    ChatID = chatId,
                    Content = message,
                    SenderID = userSession!.Id, 
                    Date = DateTime.Now
                };
                await _messageService.AddAsync(messageDto);
            }
            return RedirectToRoute(new { controller = "Property", action = "Details", Id = propertyId, Addons = true });
        }


        [HttpPost]
        public async Task<IActionResult> AddOffer(int propertyId, decimal amountOffer)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            if (amountOffer > 0)
            {
                var offerDto = new OfferDto
                {
                    PropertyId = propertyId,
                    Amount = amountOffer,
                    Date = DateTime.Now,
                    Status = Status.Pending,
                    ClientId = userSession!.Id
                };
                await _offerService.AddAsync(offerDto);
            }
            return RedirectToRoute(new { controller = "Property", action = "Details", Id = propertyId, Addons = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOffer(int offerId, bool accept)
        {

            var offer = await _offerService.GetById(offerId);

            
            if (offer != null)
            {
                if (accept)
                {
                    offer.Status = Status.Accepted;
                    await _offerService.UpdateAsync(offerId,offer);
                    var property = await _propertyService.GetById(offer.PropertyId);
                    property!.IsAvailable = false;
                    await _propertyService.UpdateAsync(offer.PropertyId,property);
                    var offers = await _offerService.GetAll();
                    var pendingOffers = offers
                        .Where(o => o.PropertyId == offer.PropertyId && o.Status == Status.Pending).ToList();
                    if (pendingOffers != null)
                    {
                        foreach (var off in pendingOffers)
                        {

                            off.Status = Status.Rejected;
                            await _offerService.UpdateAsync(off.Id,off);

                        }
                    }
                }
                else
                {
                    offer.Status = Status.Rejected;
                await _offerService.UpdateAsync(offerId,offer);
                }
            }
                return RedirectToRoute(new { controller = "Property", action = "Details", Id = offer!.PropertyId, Addons = true });
 
        }
    }

}
