using SysCafeteriasweetcoffee.Models;
using System.Collections.Generic;

namespace SysCafeteriasweetcoffee.Models.ViewModel
{
    public class CatalogoViewModel
    {
        public List<Producto> Productos { get; set; }
        public Orden OrdenActual { get; set; }
    }
}

