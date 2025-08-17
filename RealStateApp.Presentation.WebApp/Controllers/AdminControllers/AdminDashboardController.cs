using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Interfaces;
using RealStateApp.Core.Application.ViewModels.Login;
using System.Threading.Tasks;

namespace RealStateApp.Presentation.WebApp.Controllers.AdminControllers
{
    public class AdminDashboardController : Controller
    {
        private readonly IAccountServiceForWebApp _accountService;

        public AdminDashboardController(IAccountServiceForWebApp accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var dto = await _accountService.GetDashboardResume();
            var vm = new AdminDashboardVM
            {
                ActiveAgents = dto.ActiveAgents,
                InactiveAgents = dto.InactiveAgents,
                ActiveClients = dto.ActiveClients,
                ActiveDevelopers = dto.ActiveDevelopers,
                InactiveClients = dto.InactiveClients,
                InactiveDevelopers = dto.InactiveDevelopers,
                AvailableProperties = dto.AvailableProperties,
                SoldProperties = dto.SoldProperties
            };
            return View(vm);
        }
    }
}
