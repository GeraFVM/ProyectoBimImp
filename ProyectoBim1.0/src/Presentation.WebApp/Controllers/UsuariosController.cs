using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Domain.Entities;
using Infrastructure.Data; 
using Application.Services; 
using System.Linq; 
using System.Collections.Generic; 
using Microsoft.AspNetCore.Mvc.Rendering;       
using Microsoft.AspNetCore.Mvc.ViewEngines;    
using Microsoft.AspNetCore.Mvc.ViewFeatures;   
using System.IO;                                
using System.Threading.Tasks;                   

namespace Presentation.WebApp.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuariosDbContext _usuariosDbContext;
        private readonly IRazorViewEngine _razorViewEngine;     
        private readonly ITempDataProvider _tempDataProvider;   

        public UsuariosController(IConfiguration configuration, IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "El connectionString no puede ser nulo o vacío.");
            }

            _usuariosDbContext = new UsuariosDbContext(connectionString);
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
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

        public async Task<IActionResult> Details(Guid id) 
        {
            var data = _usuariosDbContext.Details(id);
            if (data == null)
            {
                return Content("<p class='text-danger'>Usuario no encontrado.</p>", "text/html");
            }

            var viewResult = _razorViewEngine.FindView(ControllerContext, "Details", false); 

            if (viewResult.View == null)
            {
                throw new ArgumentNullException($"La vista 'Details' no fue encontrada.");
            }

            var viewDictionary = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), ModelState)
            {
                Model = data 
            };

            using (var writer = new StringWriter())
            {
                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider),
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext); 
                return Content(writer.ToString(), "text/html"); 
            }
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