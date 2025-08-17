using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.DTOs.Users;
using RealStateApp.Core.Application.Helpers.Enums;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Login;
using RealStateApp.Core.Domain.Enums;
using RealStateApp.Presentation.WebApp.Helpers;

namespace RealStateApp.Presentation.WebApp.Controllers.Login
{
    public class LoginController : Controller
    {
        private readonly IAccountServiceForWebApp _accountService;
        private readonly IMapper _mapper;

        public LoginController(IAccountServiceForWebApp accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public IActionResult Index(string Error = null, string Message = null)
        {
            if (!string.IsNullOrEmpty(Error))
            {
               ViewBag.Error = Error;
            }
            if (!string.IsNullOrEmpty(Message))
            {
                ViewBag.Message = Message;
            }
            return View(new LoginVM());
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            var dto = new LoginRequestDTO
            {
                Email = loginVM.Email,
                Password = loginVM.Password
            };
            var result = await _accountService.Login(dto);
            if (result.IsSuccess)
            {
                // Handle successful login, e.g., redirect to a dashboard
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = result.Error;   
            return View(loginVM);
        }

        public IActionResult Register()
        {
            return View(new RegisterVM() { Roles = EnumHelper.GetEnumsAsIdent<UserRoles>().Where(i => i.Id != 1 && i.Id != 4)}); 
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                registerVM.Roles = EnumHelper.GetEnumsAsIdent<UserRoles>().Where(i => i.Id != 1 && i.Id != 4);
                return View(registerVM);
            }
            string origin = Request.Headers["origin"].ToString();
            var dTO = new RegisterRequestDTO
            {
                IdCardNumber = registerVM.IdNumber,
                Email = registerVM.Email,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Password = registerVM.Password,
                PhoneNumber = registerVM.PhoneNumber,
                RoleId = registerVM.RoleId,
                UserName = registerVM.UserName,

            };
            var result = await _accountService.RegisterAsync(dTO, origin);
            if (result.IsSuccess)
            {
                var path = FileManager.Upload(registerVM.PhotoPath, result.Value, "Users");
                await _accountService.UpdateProfilePhoto(result.Value,path);
                return RedirectToAction("Index", "Login", new { Message = "Please confirm your email."});
            }
            return RedirectToAction("Index", "Login", new { Error = result.Error });
            
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                ViewBag.ErrorMessage = "Invalid email confirmation link.";
                return View("Error");
            }
            var result = _accountService.ConfirmEmailAsync(userId, token).Result;
            if (result.IsSuccess)
            {
                return RedirectToAction("Index", new { Message = "EmailSended" });
            }
            return RedirectToAction("Index", new { Error = result.Error });
            return View("Error");
        }


        public async Task<IActionResult> ForgottPassword()
        {
            var vm = new ForgottPasswordVM();
            return View("ForgottPassword", vm);
        }

        [HttpPost]
        public async Task<IActionResult> ForgottPassword(ForgottPasswordVM vm)
        {
            string origin = Request.Headers["origin"].ToString();
            var res = await _accountService.SendResetPasswordEmail(vm.Email, origin);
            if (res.IsSuccess)
                return RedirectToAction("Index", new { Message = "Se le envio el correo de restauracion" });
            return RedirectToAction("Index", new { Error = res.Error });
        }

        public IActionResult ResetPassword(string token, string userId)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login");
            }

            return View(new ResetPasswordVM
            {
                Token = token,
                UserId = userId
            });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _accountService.ResetPasswordAsync(vm.UserId, vm.Token, vm.Password);

            if (!result.IsSuccess)
            {
                return RedirectToAction("Index", new { errors = result.Error });
            }

            return RedirectToAction("Index", new { Message = "Se el ha reiniciado la contraseña" });
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
