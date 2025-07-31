using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data;
using Application.Services;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Presentation.WebApp.Controllers
{
    public class LibrosController : Controller
    {
        private readonly LibrosDbContext _librosDbContext;

        public LibrosController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "El connectionString para LibrosDbContext no puede ser nulo o vacío.");
            }
            _librosDbContext = new LibrosDbContext(connectionString);
        }

        public IActionResult Index(string searchString, string searchId)
        {
            var data = _librosDbContext.List();

            if (!string.IsNullOrEmpty(searchId))
            {
                if (Guid.TryParse(searchId, out Guid idToSearch))
                {
                    data = data.Where(l => l.Id == idToSearch).ToList();
                }
                else
                {
                    data = new List<IM253E01Libro>();
                    ViewData["ErrorMessage"] = "El ID de búsqueda no es un formato válido.";
                }
                ViewData["CurrentIdFilter"] = searchId;
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(l => l.Autor != null && l.Autor.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["CurrentStringFilter"] = searchString;
            }

            return View(data);
        }

        // GET: Libros/Details/5
        // ¡CAMBIO AQUÍ! Ahora devuelve una VISTA COMPLETA, como en Usuarios
        public IActionResult Details(Guid id)
        {
            var data = _librosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            // Retorna la vista completa "Details.cshtml"
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IM253E01Libro data, IFormFile? file)
        {
            data.Id = Guid.NewGuid();

            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }
            else
            {
                data.Foto = null;
            }

            _librosDbContext.Create(data);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            var data = _librosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IM253E01Libro data, IFormFile? file)
        {
            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }

            _librosDbContext.Update(data);
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _librosDbContext.Delete(id);
            return RedirectToAction("Index");
        }
    }
}