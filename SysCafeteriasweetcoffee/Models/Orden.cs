using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SysCafeteriasweetcoffee.Models;

public partial class Orden
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Total { get; set; }

    public int? IdUsuario { get; set; }

    [MaxLength(20)]  
    public string? Estado { get; set; }

    [InverseProperty("IdOrdenNavigation")]
    public virtual ICollection<DetalleOrden> DetalleOrden { get; set; } = new List<DetalleOrden>();

    [ForeignKey("IdUsuario")]
    [InverseProperty("Orden")]
    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
