using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; // Necesario para IConfiguration
using Domain.Entities; // Asegúrate de que este namespace contenga IM253E01Prestamo e IM253E01Libro
using Infrastructure.Data; // Asegúrate de que este namespace contenga PrestamosDbContext

namespace Presentation.WebApp.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamosDbContext _prestamosDbContext;

        public PrestamosController(IConfiguration configuration)
        {
            // **CORRECCIÓN DE LA ADVERTENCIA CS8604:**
            // Se valida que el connectionString no sea nulo o vacío antes de pasarlo al DbContext.
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "El connectionString 'DefaultConnection' no puede ser nulo o vacío para PrestamosDbContext.");
            }
            _prestamosDbContext = new PrestamosDbContext(connectionString);
        }

        public IActionResult Index()
        {
            var data = _prestamosDbContext.List();
            // Considera agregar manejo de errores o un mensaje si la lista está vacía.
            return View(data);
        }

        public IActionResult Details(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            if (data == null)
            {
                // Si el préstamo no se encuentra, devuelve un NotFound (HTTP 404)
                return NotFound();
            }
            return View(data);
        }

        public IActionResult Create()
        {
            // Puedes pasar un nuevo objeto Prestamo o un ViewModel si es necesario
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Se recomienda encarecidamente para POST para prevenir ataques CSRF
        public IActionResult Create(IM253E01Prestamo data)
        {
            // Aquí puedes agregar validaciones de modelo adicionales si no usas Data Annotations en el modelo.
            // if (ModelState.IsValid) // Si estás usando Data Annotations y quieres validación automática
            // {
                // Generar un nuevo ID si tu lógica de negocio lo requiere, o si la base de datos no lo hace automáticamente
                if (data.Id == Guid.Empty) // O si no se asigna en la vista
                {
                    data.Id = Guid.NewGuid();
                }
                
                _prestamosDbContext.Create(data);
                return RedirectToAction(nameof(Index)); // Redirige usando nameof para seguridad de tipo
            // }
            // return View(data); // Si ModelState.IsValid falla, vuelve a mostrar la vista con errores
        }

        public IActionResult Edit(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Se recomienda encarecidamente para POST
        public IActionResult Edit(IM253E01Prestamo data)
        {
            // if (ModelState.IsValid) // Si estás usando Data Annotations y quieres validación automática
            // {
                // Generalmente no se genera un nuevo ID en Edit, solo se usa el existente.
                _prestamosDbContext.Edit(data);
                return RedirectToAction(nameof(Index));
            // }
            // return View(data);
        }

        // Se recomienda tener un GET para mostrar la confirmación de eliminación
        public IActionResult Delete(Guid id)
        {
            var data = _prestamosDbContext.Details(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data); // Mostrar una vista de confirmación de eliminación
        }

        // El método POST para ejecutar la eliminación real
        [HttpPost, ActionName("Delete")] // Mapea esta acción al mismo nombre de ruta "Delete"
        [ValidateAntiForgeryToken] // Crucial para POST para prevenir ataques CSRF
        public IActionResult DeleteConfirmed(Guid id)
        {
            // Primero, podrías verificar si el elemento existe antes de intentar eliminarlo
            // var data = _prestamosDbContext.Details(id);
            // if (data == null) { return NotFound(); }

            _prestamosDbContext.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}