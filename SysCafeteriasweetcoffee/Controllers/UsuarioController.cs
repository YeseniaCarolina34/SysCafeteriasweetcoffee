using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SysCafeteriasweetcoffee.Models;
using SysCafeteriasweetcoffee.Services;

namespace SysCafeteriasweetcoffee.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly BDContext _context;

        public UsuarioController(BDContext context)
        {
            _context = context;
        }

        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: Usuario/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Usuario/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (ModelState.IsValid)
            {
                // Buscar el usuario por su login
                var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Login == login);
                if (usuario != null)
                {
                    // Verificar si la contraseña ingresada es correcta comparando con el hash
                    bool passwordValida = BCrypt.Net.BCrypt.Verify(password, usuario.Password);

                    if (passwordValida)
                    {
                        // Almacenar el login del usuario en la sesión (puedes agregar más lógica según lo que necesites)
                        HttpContext.Session.SetString("Login", usuario.Login);
                        return RedirectToAction("Index", "Home"); // Redirigir a una página después del login exitoso
                    }
                }

                // Si llega aquí, la autenticación falló
                ModelState.AddModelError("", "Login o contraseña incorrecta");
            }
            return View();
        }

        // GET: Usuario
        public async Task<IActionResult> Index()
        {
            var bDContext = _context.Usuario.Include(u => u.IdRolNavigation);
            return View(await bDContext.ToListAsync());
        }

        // GET: Usuario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            ViewData["IdRol"] = new SelectList(_context.Rol, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdRol,Nombre,Apellido,Login,Password,Estatus,FechaRegistro")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Encriptar la contraseña antes de guardar el usuario
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRol"] = new SelectList(_context.Rol, "Id", "Id", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,IdRol,Nombre,Apellido,Login,Password,Estatus,FechaRegistro")] Usuario usuario)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(usuario);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["IdRol"] = new SelectList(_context.Rol, "Id", "Id", usuario.IdRol);
        //    return View(usuario);
        //}



        // GET: Usuario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdRol"] = new SelectList(_context.Rol, "Id", "Id", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdRol,Nombre,Apellido,Login,Password,Estatus,FechaRegistro")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            ViewData["IdRol"] = new SelectList(_context.Rol, "Id", "Id", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}
