using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysCafeteriasweetcoffee.Models;

namespace SysCafeteriasweetcoffee.Controllers
{
    

    public class CategoriaController : Controller
    {
        private readonly BDContext _context;

        public CategoriaController(BDContext context)
        {
            _context = context;
        }

        // GET: Categoria
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categoria.ToListAsync());
        }
        
        // GET: Categoria/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // GET: Categoria/Create
        [HttpGet]
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        // POST: Categoria/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,img")] Categoria categoria, IFormFile img)
        {
            ModelState.Remove("img");
            if (ModelState.IsValid)
            {
                string rutaImagen = null;

                // Verificar si se ha subido una imagen
                if (img != null && img.Length > 0)
                {
                    // Obtener la ruta donde se guardará la imagen
                    string rutaCarpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "NewFolder");

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
                    rutaImagen = "/NewFolder/" + nombreArchivo;
                }
                categoria.img = rutaImagen;
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones

        // GET: Categoria/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        // POST: Categoria/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,img")] Categoria categoria, IFormFile img)
        {
            if (id != categoria.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Si se subió una nueva imagen
                    if (img != null && img.Length > 0)
                    {
                        // Aquí puedes agregar la lógica para guardar la imagen en una ruta específica
                        // Ejemplo: guardarla en el servidor en la carpeta "wwwroot/images"
                        var filePath = Path.Combine("wwwroot/NewFolder", img.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await img.CopyToAsync(stream);
                        }

                        // Actualizar la propiedad de imagen con la ruta del archivo guardado
                        categoria.img = "/NewFolder/" + img.FileName;
                    }
                    else
                    {
                        // Si no se subió ninguna imagen nueva, conservar la imagen existente
                        _context.Entry(categoria).Property(p => p.img).IsModified = false;
                    }
                    _context.Update(categoria);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExists(categoria.Id))
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
            return View(categoria);
        }

        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        // GET: Categoria/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        // POST: Categoria/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _context.Categoria.FindAsync(id);
            if (categoria != null)
            {
                _context.Categoria.Remove(categoria);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.Id == id);
        }

        public IActionResult C1()
        {
            return View();
        }

        public IActionResult C2()
        {
            return View();
        }

        public IActionResult C3()
        {
            return View();
        }
        public IActionResult C4()
        {
            return View();
        }

        public IActionResult C5()
        {
            return View();
        }
        public IActionResult C6()
        {
            return View();
        }
        public IActionResult C7()
        {
            return View();
        }
        public IActionResult C8()
        {
            return View();
        }
    }
}
