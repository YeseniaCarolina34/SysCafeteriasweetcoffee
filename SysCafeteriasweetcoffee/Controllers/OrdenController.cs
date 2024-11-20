using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysCafeteriasweetcoffee.Models;
using SysCafeteriasweetcoffee.Models.ViewModel;
using SysCafeteriasweetcoffee.ViewModel;

namespace SysCafeteriasweetcoffee.Controllers
{
    [Authorize(Roles = "Cliente, Administrador")] // Clientes y administradores pueden acceder a las vistas
    public class OrdenController : Controller
    {
        private readonly BDContext _context;

        public OrdenController(BDContext context)
        {
            _context = context;
        }

        // Método para mostrar el catálogo de productos
        public IActionResult MostrarCatalogo()
        {
            // Obtén la lista de productos para el catálogo
            var productos = _context.Producto.Include(p => p.IdCategoria).ToList();

            // Obtener el cliente actual
            var clienteId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Convertir clienteId a int
            int clienteIdInt;
            Orden ordenActual = null;

            if (int.TryParse(clienteId, out clienteIdInt))
            {
                // Buscar si el cliente ya tiene una orden en proceso
                ordenActual = _context.Orden.FirstOrDefault(o => o.IdUsuario == clienteIdInt && o.Estado == "en_proceso");
            }

            // Crear el ViewModel con los datos de productos y la orden actual (si existe)
            var viewModel = new CatalogoViewModel
            {
                Productos = productos,
                OrdenActual = ordenActual
            };
            
            // Enviar el ViewModel a la vista
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AgregarOrden(decimal total, List<DetalleOrden> DetallesDeOrden)
        {
            Orden objOrden = new Orden();
            objOrden.Total = total;
            objOrden.Estado = "EN PROCESO";
            objOrden.Fecha = DateTime.Now;
            objOrden.IdUsuario = Global.IdUsuarioLog;

            _context.Orden.Add(objOrden);
            _context.SaveChanges();
            //aqui se debe de procesar los detalles de la orden con un foreach y capturar en el parametro la lista de orden
            
            foreach ( var item in DetallesDeOrden)
            {
                item.IdOrden = objOrden.Id;

                _context.DetalleOrden.Add(item);
                _context.SaveChanges();
            }

            return RedirectToAction("Index","Producto");
        }


        [HttpPost]
        public IActionResult AgregarAlOrden(Dictionary<int, int> productos)
        {
            // Obtener el cliente actual
            var clienteId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Validar clienteId y convertirlo a int
            if (string.IsNullOrEmpty(clienteId) || !int.TryParse(clienteId, out int clienteIdInt))
            {
                return BadRequest("ID de cliente inválido");
            }

            // Buscar o crear la orden en proceso
            var orden = _context.Orden.FirstOrDefault(o => o.IdUsuario == clienteIdInt && o.Estado == "en_proceso");
            if (orden == null)
            {
                orden = new Orden
                {
                    IdUsuario = clienteIdInt,
                    Fecha = DateTime.Now,
                    Estado = "en_proceso",
                    Total = 0
                };
                _context.Orden.Add(orden);
                _context.SaveChanges();
            }

            // Rellenar DetalleOrden con los productos seleccionados
            foreach (var item in productos)
            {
                var productoId = item.Key;
                var cantidad = item.Value;

                // Validar la cantidad y existencia del producto
                if (cantidad > 0)
                {
                    var producto = _context.Producto.Find(productoId);
                    if (producto != null)
                    {
                        var detalleOrden = new DetalleOrden
                        {
                            IdOrden = orden.Id,
                            IdProducto = producto.Id,
                            Cantidad = cantidad,
                            Precio = producto.Precio
                        };
                        _context.DetalleOrden.Add(detalleOrden);
                    }
                    else
                    {
                        return NotFound($"Producto con ID {productoId} no encontrado.");
                    }
                }
            }

            _context.SaveChanges();

            // Redireccionar al resumen de la orden
            return RedirectToAction("MostrarResumenOrden", new { ordenId = orden.Id });
        }



        // Método para mostrar el resumen de la orden
        public IActionResult MostrarResumenOrden(int ordenId)
        {
            var orden = _context.Orden
                        .Include(o => o.DetalleOrden)
                        .ThenInclude(d => d.IdProductoNavigation)
                        .FirstOrDefault(o => o.Id == ordenId);

            if (orden == null)
            {
                return NotFound();
            }

            // Validar que cada DetalleOrden tenga un producto asociado
            foreach (var detalle in orden.DetalleOrden)
            {
                if (detalle.IdProductoNavigation == null)
                {
                    detalle.IdProductoNavigation = new Producto { Nombre = "Producto no disponible" };
                }
            }

            return View(orden);
        }


        // Método para confirmar la orden
        [HttpPost]
        public IActionResult ConfirmarOrden(int ordenId)
		{
			var orden = _context.Orden.Include(o => o.DetalleOrden).FirstOrDefault(o => o.Id == ordenId);
			if (orden == null || orden.DetalleOrden == null || !orden.DetalleOrden.Any())
			{
				return NotFound("La orden no existe o no tiene detalles.");
			}

			var total = orden.DetalleOrden
				.Where(d => d.Precio != null)  // Asegurarse de que el precio no sea null
				.Sum(d => d.Precio * d.Cantidad);

			orden.Total = total;
			orden.Estado = "confirmada";
			_context.SaveChanges();

			return View("Confirmacion", orden);
		}












		// GET: Orden
		public async Task<IActionResult> Index()
        {
            var bDContext = _context.Orden
    .Include(o => o.IdUsuarioNavigation)
    .Where(o => o.IdUsuario != null);
            return View(await bDContext.ToListAsync());
        }

        // GET: Orden/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.Orden
                .Include(o => o.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        // GET: Orden/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Id");
            return View();
        }

        // POST: Orden/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Total,IdUsuario")] Orden orden)
        {
            if (ModelState.IsValid)
            {
                orden.Estado = "pendiente";
                _context.Add(orden);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Id", orden.IdUsuario);
            return View(orden);
        }

        // GET: Orden/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.Orden.FindAsync(id);
            if (orden == null)
            {
                return NotFound();
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Id", orden.IdUsuario);
            return View(orden);
        }

        // POST: Orden/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Total,IdUsuario,Estado")] Orden orden)
        {
            if (id != orden.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orden);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdenExists(orden.Id))
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
            ViewData["IdUsuario"] = new SelectList(_context.Usuario, "Id", "Id", orden.IdUsuario);
            return View(orden);
        }

        // GET: Orden/Delete/5
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.Orden
                .Include(o => o.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        // POST: Orden/Delete/5
        [Authorize(Roles = "Administrador")] // Solo los administradores pueden acceder a estas acciones
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orden = await _context.Orden.FindAsync(id);
            if (orden != null)
            {
                _context.Orden.Remove(orden);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdenExists(int id)
        {
            return _context.Orden.Any(e => e.Id == id);
        }
    }
}
