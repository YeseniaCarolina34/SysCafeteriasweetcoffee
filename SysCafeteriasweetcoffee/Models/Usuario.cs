﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SysCafeteriasweetcoffee.Models;

public partial class Usuario
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo Rol es obligatorio.")]
    public int IdRol { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(30)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(30)]
    [Unicode(false)]
    public string Apellido { get; set; } = null!;

    [Required(ErrorMessage = "El login es obligatorio.")]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [Unicode(false)]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(32)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Required]
    public byte Estatus { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [ForeignKey("IdRol")]
    [InverseProperty("Usuario")]
    public virtual Rol IdRolNavigation { get; set; } = null!;

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Orden> Orden { get; set; } = new List<Orden>();
}
