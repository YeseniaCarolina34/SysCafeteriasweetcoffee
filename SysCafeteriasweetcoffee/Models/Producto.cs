using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SysCafeteriasweetcoffee.Models;

public partial class Producto
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Precio { get; set; }

    [Column(TypeName = "text")]
    public string? Descripcion { get; set; }

    [Column(TypeName = "max")]
    public string? img { get; set; }

    [Column("idCategoria")]
    public int? IdCategoria { get; set; }

    [InverseProperty("IdProductoNavigation")]
    public virtual ICollection<DetalleOrden> DetalleOrden { get; set; } = new List<DetalleOrden>();

    [ForeignKey("IdCategoria")]
    [InverseProperty("Producto")]
    public virtual Categoria? IdCategoriaNavigation { get; set; }
}
