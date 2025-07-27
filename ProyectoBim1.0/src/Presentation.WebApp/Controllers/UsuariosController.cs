using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data;
using Application.Services; // Reemplaza con el espacio de nombres correcto para FileConverterService
using System.Linq; // Necesario para Linq

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

        // Modifica este método para aceptar un parámetro de búsqueda
        public IActionResult Index(string searchId)
        {
            // Obtener todos los usuarios inicialmente
            var data = _usuariosDbContext.List();

            // Si hay un término de búsqueda, filtrar la lista
            if (!string.IsNullOrEmpty(searchId))
            {
                // Intentar convertir el searchId a Guid
                if (Guid.TryParse(searchId, out Guid idToSearch))
                {
                    // Filtrar por ID
                    data = data.Where(u => u.Id == idToSearch).ToList();
                }
                else
                {
                    // Si el ID no es un GUID válido, puedes decidir cómo manejarlo.
                    // Por ahora, no se encontrarán resultados por ID,
                    // y podrías agregar un mensaje de error si lo deseas.
                    // Para este ejemplo, simplemente no se filtrará por ID si es inválido.
                    data = new List<IM253E01Usuario>(); // O data = data; si prefieres que se muestren todos si el ID es inválido.
                    ViewData["ErrorMessage"] = "El ID de búsqueda no es un formato válido."; // Opcional: mensaje de error
                }

                // Guardar el término de búsqueda actual para que el cuadro de texto lo mantenga
                ViewData["CurrentFilter"] = searchId;
            }

            return View(data);
        }

        // ... (resto de tus métodos Create, Edit, Details, Delete, DeleteConfirmed) ...

        public IActionResult Details(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
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

        // GET: Usuarios/Delete/{id}
        public IActionResult Delete(Guid id)
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        // POST: Usuarios/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _usuariosDbContext.Delete(id);
            return RedirectToAction("Index");
        }
    }
}