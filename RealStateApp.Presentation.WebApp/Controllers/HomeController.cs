using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RealStateApp.Core.Application.Services;
using RealStateApp.Presentation.WebApp.Models;

namespace RealStateApp.Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISaleTypeService _saleTypeService;

        public HomeController(ILogger<HomeController> logger, ISaleTypeService saleTypeService)
        {
            _logger = logger;
            _saleTypeService = saleTypeService;
        }

        public async Task<IActionResult> Index()
        {
            var saleTypes = await _saleTypeService.GetAll();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
