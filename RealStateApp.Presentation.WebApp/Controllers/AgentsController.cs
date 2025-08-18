using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.Services;
using RealStateApp.Core.Application.ViewModels;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Domain.Entities;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Infraestructure.Identity.Entities;
using RealStateApp.Presentation.WebApp.Handlers;
using RealStateApp.Presentation.WebApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class AgentsController : Controller
    {

        private readonly IMapper _mapper;
        private readonly IAccountServiceForWebApp _accountService;
        private readonly UserManager<AppUser> _userManager;

        public AgentsController(IMapper mapper, IAccountServiceForWebApp accountService, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _accountService = accountService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string? agentName)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);
            var user = await _accountService.GetUserByIdAsync(userSession!.Id);
            var agents = await _accountService.GetByRole(UserRoles.Agent);
            var agentsVm = _mapper.Map<List<UserViewModel>>(agents).OrderBy(a => a.FirstName).ThenBy(a => a.LastName).Where(a => a.IsActive).ToList();
            if (agentName != null)
            {
                var agentNameUpper = agentName.ToUpper();
                agentsVm = agentsVm
                    .Where(a => a.FirstName.ToUpper().Contains(agentNameUpper) || a.LastName.ToUpper().Contains(agentNameUpper))
                    .ToList();
            }
            ViewBag.IsClient = user.Value.Role == "Client";

            return View(agentsVm);
        }
        [Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IActionResult> UpdateAgent(UpdateUserViewModel userVm)
        {
            AppUser? userSession = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                return View("~/Views/AgentHome/MyProfile.cshtml", userVm);
            }
            var userDto = _mapper.Map<UserUpdateDTO>(userVm);
            userDto.Id = userSession!.Id;
            if(userVm.Photo != null)
            {
                var path = FileHandler.Upload(userVm.Photo, userSession.Id, "Users");
                userDto.PhotoPath = path;
            }
            var result = await _accountService.EditUser(userDto);
          
                return RedirectToRoute(new { controller = "AgentHome", action = "Index" });
       
        }

    }
}
