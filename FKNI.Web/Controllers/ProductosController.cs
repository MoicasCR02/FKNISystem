using FKNI.Application.DTOs;
using FKNI.Application.Services.Implementations;
using FKNI.Application.Services.Interfaces;
using FKNI.Infraestructure.Data;
using FKNI.Infraestructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using NuGet.Protocol.Core.Types;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace FKNI.Web.Controllers
{
    public class ProductosController : Controller
    {
        private readonly IServiceProductos _serviceProductos;
        private readonly IServiceImagenes _serviceImagenes;
        private readonly IServiceEtiquetas _serviceEtiquetas;
        private readonly IServiceCategorias _serviceCategorias;
        //private readonly FKNIContext _context;

        public ProductosController(IServiceProductos serviceProductos,IServiceEtiquetas serviceEtiquetas, IServiceCategorias serviceCategorias,IServiceImagenes serviceImagenes)
        {
            _serviceProductos = serviceProductos;
            _serviceEtiquetas = serviceEtiquetas;
            _serviceCategorias = serviceCategorias;
            _serviceImagenes = serviceImagenes;

        }
        [HttpGet]

        public async Task<IActionResult> GetProductoByName(string filtro)
        {

            var collection = await _serviceProductos.FindByNameAsync(filtro);
            return Json(collection, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
            //return Json(collection);
        }

        public async Task<IActionResult> Index()
        {
            var collection = await _serviceProductos.ListAsync();
            return View(collection);
        }

        public async Task<IActionResult> IndexAdmin()
        {
            var collection = await _serviceProductos.ListAsync();
            return View(collection);
        }

        public async Task<ActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("IndexAdmin");
                }
                var @object = await _serviceProductos.FindByIdAsync(id.Value);
                if (@object == null)
                {
                    throw new Exception("Producto no existente");

                }

                return View(@object);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // GET: LibroController/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ListCategoria = await _serviceCategorias.ListAsync();
            var etiquetas = await _serviceEtiquetas.ListAsync();
            ViewBag.ListEtiquetas = new MultiSelectList(etiquetas, "IdEtiqueta", "NombreEtiqueta");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductosDTO dto, List<IFormFile> ImageFiles, string[] selectedEtiquetas)
        {
            MemoryStream target = new MemoryStream();
            
            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                // Response errores
                return BadRequest(errors);

            }
            dto.Precio = (int)(dto.Precio);
            await _serviceProductos.AddAsync(dto, selectedEtiquetas);
            var collection = await _serviceProductos.ListAsync();
            var ultimoProducto = collection.OrderByDescending(p => p.IdProducto).FirstOrDefault();

            

            // 3. Guardar imágenes
            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                foreach (var imgs in ImageFiles)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imgs.CopyToAsync(memoryStream);

                        await _serviceImagenes.AddAsync(new ImagenesDTO
                        {

                            IdProducto = (ultimoProducto?.IdProducto ?? 0),
                            UrlImagen = memoryStream.ToArray()
                        });
                    }
                }
            }
            
            return RedirectToAction("Index");
        }

        // GET: ProductosController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var @object = await _serviceProductos.FindByIdAsync(id);
            var etiquetas = await _serviceEtiquetas.ListAsync();
            ViewBag.ListCategoria = await _serviceCategorias.ListAsync();
            var etiquetasSelected = @object.IdEtiqueta.Select(x => x.IdEtiqueta.ToString()).ToList();

            ViewBag.ListEtiquetas = new MultiSelectList(
                    items: etiquetas,
                    dataValueField: nameof(EtiquetasDTO.IdEtiqueta),
                    dataTextField: nameof(EtiquetasDTO.NombreEtiqueta),
                    selectedValues: etiquetasSelected
                );

            ViewBag.ListImagenes = await _serviceImagenes.ObtenerPorProductoAsync(id);

            return View(@object);
        }


        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductosDTO dto, List<IFormFile> ImageFiles, string[] selectedEtiquetas, List<int> imagenesEliminadas)
        {
            ModelState.Remove("imagenesEliminadas");

            if (!ModelState.IsValid)
            {
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                ViewBag.ErrorMessage = errors;
                return View(dto);
            }

            // 1. Eliminar imágenes si hay IDs
            if (imagenesEliminadas != null && imagenesEliminadas.Any())
            {
                foreach (var imgId in imagenesEliminadas)
                {
                    await _serviceImagenes.DeleteAsync(imgId, id);
                }
            }

            // 2. Obtener imágenes actuales actualizadas después de la eliminación
            var imagenesActuales = await _serviceImagenes.ObtenerPorProductoAsync(id);

            // 3. Agregar nuevas imágenes si existen
            if (ImageFiles != null && ImageFiles.Count > 0)
            {
                foreach (var img in ImageFiles)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await img.CopyToAsync(memoryStream);

                        var nuevaImagen = new ImagenesDTO
                        {
                            IdProducto = id,
                            UrlImagen = memoryStream.ToArray()
                        };

                        int id_imagen = await _serviceImagenes.AddAsync(nuevaImagen);
                        nuevaImagen.IdImagen = id_imagen;
                        imagenesActuales.Add(nuevaImagen);
                    }
                }
            }

            // 4. Mapear las imágenes DTO a entidad para asignar al DTO del producto
            var imagenesEntidad = imagenesActuales.Select(img => new Imagenes
            {
                IdImagen = img.IdImagen,
                IdProducto = img.IdProducto,
                UrlImagen = img.UrlImagen
            }).ToList();

            dto.IdProducto = id;
            dto.IdImagen = imagenesEntidad;

            // 5. Actualizar producto y colecciones con método que cargue bien el entity desde BD
            dto.Precio = (int)(dto.Precio);
            await _serviceProductos.UpdateAsync(id, dto, selectedEtiquetas);

            return RedirectToAction("Index");
        }


    }
}
