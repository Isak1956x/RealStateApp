using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Base;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Presentation.WebApp.Controllers.AdminControllers
{
    public class DeveloperController : Controller
    {
        private readonly IAccountServiceForWebApp _accountService;
        private readonly IMapper _mapper;

        public DeveloperController(IAccountServiceForWebApp accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GenericUserReadVM> users = (await _accountService.GetDevs()).Select(u => _mapper.Map<GenericUserReadVM>(u));
            return View(users);
        }

        public async Task<IActionResult> New()
        {
            return View(new GenericUserWritteVM() { Role = (int)UserRoles.Admin });
        }

        [HttpPost]
        public async Task<IActionResult> New(GenericUserWritteVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var registerRequest = _mapper.Map<RegisterRequestDTO>(user);
            var result = await _accountService.RegisterAsync(registerRequest, Request.Scheme + "://" + Request.Host);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Error);
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userVM = _mapper.Map<GenericUserWritteVM>(user.Value);
            return View(userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GenericUserWritteVM user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            var userUpdateDTO = _mapper.Map<UserUpdateDTO>(user);
            var result = await _accountService.EditUser(userUpdateDTO);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Error);
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);

            var Vm = new GenericAlertVM<string>
            {
                Title = "Delete Admin",
                Value = id,
                Message = $"Are you sure you want to delete the admin {user.Value.UserName}?",
                AlertType = "warning",
                Controller = "Developer",
                ActionDestination = "Delete"
            };
            return View(Vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GenericAlertVM<string> vm)
        {
            var result = await _accountService.DeleteUserAsyn(vm.Value);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Error);
            return View(vm);

        }
    }
}
