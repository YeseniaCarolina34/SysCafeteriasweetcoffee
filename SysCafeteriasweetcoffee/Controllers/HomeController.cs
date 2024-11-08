using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SysCafeteriasweetcoffee.Models;
using System.Diagnostics;

namespace SysCafeteriasweetcoffee.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly BDContext _context;
        public HomeController(ILogger<HomeController> logger, BDContext context)
        {
            _logger = logger;
            _context = context;
        }


        // Acci�n p�blica que muestra la p�gina de inicio de la cafeter�a
        [AllowAnonymous]
        public IActionResult Inicio()
        {
            return View(); // Esta vista ser� accesible sin necesidad de autenticaci�n
        }


        // Acci�n principal que redirige al login
        public IActionResult Index()
        {
            var categorias = _context.Categoria.ToList();
            return View(categorias);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize(Policy = "Administrador")]
        public IActionResult AdminOnly()
        {
            return View(); // Solo accesible para administradores
        }

        [Authorize(Policy = "Cliente")]
        public IActionResult ClientOnly()
        {
            return View(); // Solo accesible para clientes
        }
    }
}
