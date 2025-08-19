using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Base;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Domain.Enums;

namespace RealStateApp.Presentation.WebApp.Controllers.AdminControllers
{
    public class AdminsController : Controller
    {
        private readonly IAccountServiceForWebApp _accountService;
        private readonly IMapper _mapper;

        public AdminsController(IAccountServiceForWebApp accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<GenericUserReadVM> users = (await _accountService.GetAdmins()).Select(u => _mapper.Map<GenericUserReadVM>(u));
            return View("GenericUserRead",new UserGenericList
            {
                List = users,
                Controller = "Admins",

            });
        }

        public async Task<IActionResult> New()
        {
            return View("GenericUserWritte",new GenericUserWritteVM() { Role = (int)UserRoles.Admin, Controller = "Admins" });
        }

        [HttpPost]
        public async Task<IActionResult> New(GenericUserWritteVM user)
        {
            if (!ModelState.IsValid)
            {
                return View("GenericUserWritte", user);
            }
            var registerRequest = new RegisterRequestDTO
            {
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                RoleId = user.Role,
                FirstName = user.FirstName,
                IdCardNumber = user.IdCardNumber,
                LastName = user.LastName
            };
            var result = await _accountService.RegisterByAdmin(registerRequest);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = result.Error;
            return View("GenericUserWritte", user);
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userVM = new GenericUserWritteEditVM
            {
                Id = user.Value.Id,
                UserName = user.Value.UserName,
                Email = user.Value.Email,
                IdCardNumber = user.Value.IdCardNumber,
                FirstName = user.Value.FirstName,
                LastName = user.Value.LastName,
                IsCreating = false,
                Controller = "Admins"
            };
            return View("GenericUserWritteEdit", userVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(GenericUserWritteEditVM user)
        {
            if (!ModelState.IsValid)
            {
                return View("GenericUserWritteEdit", user);
            }
            var userUpdateDTO = _mapper.Map<UserUpdateDTO>(user);
            var result = await _accountService.EditUser(userUpdateDTO);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Error);
            return View("GenericUserWritteEdit", user);
        }

        public async Task<IActionResult> Deactive(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);

            var Vm = new GenericAlertVM<string>
            {
                Title = "Deactive Admin",
                Value = id,
                Message = $"Are you sure you want to deactive the admin {user.Value.UserName}?",
                AlertType = "warning",
                Controller = "Admins",
                ActionDestination = "Deactive"
            };
            return View("GenericAlertStr",Vm);
        }

        [HttpPost]
        public async Task<IActionResult> Deactive(GenericAlertVM<string> vm)
        {
            var result = await _accountService.ChangueUserActive(vm.Value, false);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Error);
            return View("GenericAlertStr", vm);

        }

        public async Task<IActionResult> Active(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);

            var Vm = new GenericAlertVM<string>
            {
                Title = "Active Admin",
                Value = id,
                Message = $"Are you sure you want to Active the admin {user.Value.UserName}?",
                AlertType = "primary",
                Controller = "Admins",
                ActionDestination = "Active"
            };
            return View("GenericAlertStr", Vm);
        }

        [HttpPost]
        public async Task<IActionResult> Active(GenericAlertVM<string> vm)
        {
            var result = await _accountService.ChangueUserActive(vm.Value, true);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Error);
            return View("GenericAlertStr", vm);

        }
    }
}
