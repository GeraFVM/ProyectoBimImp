using System;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities; // Asegúrate de que este es el correcto
using Infrastructure.Data; // Asegúrate de que este es el correcto

namespace Presentation.WebApp.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamosDbContext _prestamosDbContext;

        public PrestamosController(IConfiguration configuration)
        {
            _prestamosDbContext = new PrestamosDbContext(configuration.GetConnectionString("DefaultConnection"));
        }

        public IActionResult Index()
        {
            var data = _prestamosDbContext.List();
            return View(data);
        }

        public IActionResult Details(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IM253E01Prestamo data)
        {
            _prestamosDbContext.Create(data);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(IM253E01Prestamo data)
        {
            _prestamosDbContext.Edit(data);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid id)
        {
            _prestamosDbContext.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
