using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging; // Asegúrate de incluir esto para ILogger
using System.Diagnostics; // Necesario para Activity
using Presentation.WebApp.Models; // Asegúrate de incluir esto para ErrorViewModel
using Domain.Entities; // Asegúrate de que este es el correcto
using Infrastructure.Data; // Asegúrate de que este es el correcto

namespace Presentation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
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
