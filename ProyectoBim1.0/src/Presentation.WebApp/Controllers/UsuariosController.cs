using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data; // Asumiendo que UsuariosDbContext está aquí
using Application.Services; 
using System.Linq; 
using System.Collections.Generic; // Para List<IM253E01Usuario> si el filtro no encuentra nada

namespace Presentation.WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuariosDbContext _usuariosDbContext;

        public UsuariosController(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "El connectionString no puede ser nulo o vacío.");
            }

            _usuariosDbContext = new UsuariosDbContext(connectionString);
        }

        public IActionResult Index(string searchId)
        {
            var data = _usuariosDbContext.List();

            if (!string.IsNullOrEmpty(searchId))
            {
                if (Guid.TryParse(searchId, out Guid idToSearch))
                {
                    data = data.Where(u => u.Id == idToSearch).ToList();
                }
                else
                {
                    data = new List<IM253E01Usuario>(); 
                    ViewData["ErrorMessage"] = "El ID de búsqueda no es un formato válido."; 
                }

                ViewData["CurrentFilter"] = searchId;
            }

            return View(data);
        }

        public IActionResult Details(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IM253E01Usuario data, IFormFile file)
        {
            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }

            _usuariosDbContext.Create(data);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        public IActionResult Edit(IM253E01Usuario data, IFormFile file)
        {
            if (file != null)
            {
                data.Foto = FileConverterService.ConvertToBase64(file.OpenReadStream());
            }
            _usuariosDbContext.Edit(data);
            return RedirectToAction("Index");
        }
        [HttpPost, ActionName("Delete")] 
        public IActionResult DeleteConfirmed(Guid id)
        {
            var usuarioToDelete = _usuariosDbContext.Details(id); 
            if (usuarioToDelete == null)
            {
                return RedirectToAction("Index"); 
            }

            _usuariosDbContext.Delete(id); 
            return RedirectToAction("Index");
        }
    }
}