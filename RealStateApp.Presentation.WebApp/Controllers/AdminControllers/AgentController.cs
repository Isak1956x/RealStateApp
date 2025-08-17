using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Base;
using RealStateApp.Core.Application.ViewModels.Login;

namespace RealStateApp.Presentation.WebApp.Controllers.AdminControllers
{
    public class AgentController : Controller
    {
        private readonly IAccountServiceForWebApp _accountService;

        public AgentController(IAccountServiceForWebApp accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var agents = await _accountService.GetAgents();
            var vm = new UserGenericList
            {
                Controller = "Agent",
                List = agents.Select(a => new GenericUserReadVM
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    Email = a.Email,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    IdCardNumber = a.IdCardNumber
                }),
                UserType = "Agent"
            };
            return View("GenericUserRead", vm);
        }

        public async Task<IActionResult> Deactive(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);

            var Vm = new GenericAlertVM<string>
            {
                Title = "Deactive Agent",
                Value = id,
                Message = $"Are you sure you want to deactive the Agent {user.Value.UserName}?",
                AlertType = "warning",
                Controller = "Agent",
                ActionDestination = "Deactive"
            };
            return View("GenericAlertStr", Vm);
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
                Title = "Active Agent",
                Value = id,
                Message = $"Are you sure you want to Active the Agent {user.Value.UserName}?",
                AlertType = "primary",
                Controller = "Agent",
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

        public async Task<IActionResult> Delete(string id)
        {
            var user = await _accountService.GetUserByIdAsync(id);

            var Vm = new GenericAlertVM<string>
            {
                Title = "Delete Agent",
                Value = id,
                Message = $"Are you sure you want to delete the Agent {user.Value.UserName}?",
                AlertType = "danger",
                Controller = "Agent",
                ActionDestination = "Deactive"
            };
            return View("GenericAlertStr", Vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(GenericAlertVM<string> vm)
        {
            var result = await _accountService.DeleteAgentAsyn(vm.Value);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            ModelState.AddModelError(string.Empty, result.Error);
            return View("GenericAlertStr", vm);

        }
    }
}
