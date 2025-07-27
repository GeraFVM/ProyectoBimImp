using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data;
using Application.Services; 
using System.Linq; 

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

        // GET: Libros
        public IActionResult Index(string searchString, string searchId)
        {
            var data = _librosDbContext.List();

            // Filtrar por ID (similar a usuarios)
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
            // Filtrar por Título o Autor si no se buscó por ID, o adicionalmente
            else if (!string.IsNullOrEmpty(searchString))
            {
                data = data.Where(l => l.Titulo.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                       l.Autor.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewData["CurrentStringFilter"] = searchString;
            }

            return View(data);
        }

        // GET: Libros/Details/5
        public IActionResult Details(Guid id)
        {
            var data = _librosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IM253E01Libro data, IFormFile file)
        {
            data.Id = Guid.NewGuid();
            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }

            _librosDbContext.Create(data);
            return RedirectToAction("Index");
        }

        // GET: Libros/Edit/5
        public IActionResult Edit(Guid id)
        {
            var data = _librosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        // POST: Libros/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(IM253E01Libro data, IFormFile file)
        {
            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }

            _librosDbContext.Update(data);
            return RedirectToAction("Index");
        }

        // NO HAY MÁS MÉTODO GET Delete(Guid id) QUE RETORNE UNA VISTA

        // POST: Libros/Delete/5 (para la eliminación real)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _librosDbContext.Delete(id);
            return RedirectToAction("Index");
        }
    }
}