using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysCafeteriasweetcoffee.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using SysCafeteriasweetcoffee.Services;

namespace SysCafeteriasweetcoffee.Controllers
{

    public class ProductoController : Controller
    {

        private readonly BDContext _context;

        public ProductoController(BDContext context)
        {
            _context = context;

        }


        // GET: Producto
        public async Task<IActionResult> Index(string searchString, int? idCategoria)
        {
            // Obtener las categorías
            ViewData["Categorias"] = new SelectList(await _context.Categoria.ToListAsync(), "Id", "Nombre");

            var productos = _context.Producto.Include(p => p.IdCategoriaNavigation).AsQueryable();

            // Filtro por nombre de producto
            if (!string.IsNullOrEmpty(searchString))
            {
                productos = productos.Where(p => p.Nombre.Contains(searchString));
            }

            // Filtro por categoría
            if (idCategoria.HasValue)
            {
                productos = productos.Where(p => p.IdCategoria == idCategoria);
            }


            return View(await productos.ToListAsync());
        }

      

        // GET:Producto PARA QUE FUNCIONE IMAGEN
        // public async Task<IActionResult> Index()
        // {
        // return View(await_context.Producto.ToListAsync());
        //}


        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
        

        // GET: Producto/Create
        [HttpGet]
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Id");
            return View();
        }

        //POST:Producto/create
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Precio,Descripcion,IdCategoria,img")] Producto producto, IFormFile img)
        {
            ModelState.Remove("img");
            if (ModelState.IsValid)
            {
                string rutaImagen = null;

                // Verificar si se ha subido una imagen
                if (img != null && img.Length > 0)
                {
                    // Obtener la ruta donde se guardará la imagen
                    string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img");

                    // Crear el nombre único para la imagen
                    string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                    // Combinar la ruta de la carpeta con el nombre del archivo
                    string rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                    // Guardar la imagen en la ruta especificada
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }

                    // Almacenar la ruta para guardarla en la base de datos
                    rutaImagen = "/img/" + nombreArchivo;
                }

                producto.img = rutaImagen;
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Id", producto.IdCategoria);
            return View(producto);

        }



        // GET: Producto/Edit/5
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Id", producto.IdCategoria);
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Descripcion,IdCategoria,img")] Producto producto, IFormFile img)
        {
            if (id != producto.Id)
            {
                return NotFound();
            }

            ModelState.Remove("img");
            if (ModelState.IsValid)
            {
                try
                {
                    // Si se subió una nueva imagen
                    if (img != null && img.Length > 0)
                    {
                        // Aquí puedes agregar la lógica para guardar la imagen en una ruta específica
                        // Ejemplo: guardarla en el servidor en la carpeta "wwwroot/images"
                        var filePath = Path.Combine("wwwroot/img", img.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }

                        // Actualizar la propiedad de imagen con la ruta del archivo guardado
                        producto.img = "/img/" + img.FileName;
                    }
                    else
                    {
                        // Si no se subió ninguna imagen nueva, conservar la imagen existente
                        _context.Entry(producto).Property(p => p.img).IsModified = false;
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Id", producto.IdCategoria);
            return View(producto);
        }


        // GET: Producto/Delete/5
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Producto/Delete/5
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }

       



    }



}


