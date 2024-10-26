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
    [Authorize(Roles = "Cliente, Administrador")] // Clientes y administradores pueden acceder a las vistas
    public class DetalleOrdenController : Controller
    {
        private readonly BDContext _context;

        public DetalleOrdenController(BDContext context)
        {
            _context = context;
        }

        // GET: DetalleOrden
        public async Task<IActionResult> Index()
        {
            var bDContext = _context.DetalleOrden.Include(d => d.IdOrdenNavigation).Include(d => d.IdProductoNavigation);
            return View(await bDContext.ToListAsync());
        }

        // GET: DetalleOrden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleOrden = await _context.DetalleOrden
                .Include(d => d.IdOrdenNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleOrden == null)
            {
                return NotFound();
            }

            return View(detalleOrden);
        }

        // GET: DetalleOrden/Create
        public IActionResult Create()
        {
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id");
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id");
            return View();
        }

        // POST: DetalleOrden/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdOrden,IdProducto,Cantidad,Precio")] DetalleOrden detalleOrden)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detalleOrden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id", detalleOrden.IdOrden);
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id", detalleOrden.IdProducto);
            return View(detalleOrden);
        }

        // GET: DetalleOrden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleOrden = await _context.DetalleOrden.FindAsync(id);
            if (detalleOrden == null)
            {
                return NotFound();
            }
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id", detalleOrden.IdOrden);
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id", detalleOrden.IdProducto);
            return View(detalleOrden);
        }

        // POST: DetalleOrden/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdOrden,IdProducto,Cantidad,Precio")] DetalleOrden detalleOrden)
        {
            if (id != detalleOrden.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleOrden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleOrdenExists(detalleOrden.Id))
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
            ViewData["IdOrden"] = new SelectList(_context.Orden, "Id", "Id", detalleOrden.IdOrden);
            ViewData["IdProducto"] = new SelectList(_context.Producto, "Id", "Id", detalleOrden.IdProducto);
            return View(detalleOrden);
        }

        // GET: DetalleOrden/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleOrden = await _context.DetalleOrden
                .Include(d => d.IdOrdenNavigation)
                .Include(d => d.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (detalleOrden == null)
            {
                return NotFound();
            }

            return View(detalleOrden);
        }

        // POST: DetalleOrden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detalleOrden = await _context.DetalleOrden.FindAsync(id);
            if (detalleOrden != null)
            {
                _context.DetalleOrden.Remove(detalleOrden);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleOrdenExists(int id)
        {
            return _context.DetalleOrden.Any(e => e.Id == id);
        }
    }
}
