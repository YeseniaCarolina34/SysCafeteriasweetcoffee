using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SysCafeteriasweetcoffee.Models;

public partial class DetalleOrden
{
    [Key]
    public int Id { get; set; }

    public int? IdOrden { get; set; }

    public int? IdProducto { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Precio { get; set; }

    [ForeignKey("IdOrden")]
    [InverseProperty("DetalleOrden")]
    public virtual Orden? IdOrdenNavigation { get; set; }

    [ForeignKey("IdProducto")]
    [InverseProperty("DetalleOrden")]
    public virtual Producto? IdProductoNavigation { get; set; }
}
