using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SysCafeteriasweetcoffee.Models;

public partial class BDContext : DbContext
{
    public BDContext()
    {
    }

    public BDContext(DbContextOptions<BDContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<DetalleOrden> DetalleOrden { get; set; }

    public virtual DbSet<Orden> Orden { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07417DE854");
        });

        modelBuilder.Entity<DetalleOrden>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DetalleO__3214EC07BE906E40");

            entity.HasOne(d => d.IdOrdenNavigation).WithMany(p => p.DetalleOrden)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DetalleOr__IdOrd__4316F928");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleOrden)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__DetalleOr__IdPro__440B1D61");
        });

        modelBuilder.Entity<Orden>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orden__3214EC074415B1E6");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Orden)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Orden__IdUsuario__403A8C7D");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC073CA1EBEE");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Producto)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Producto__idCate__3D5E1FD2");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rol__3214EC0736AC5827");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuario__3214EC072B265B97");

            entity.Property(e => e.Password).IsFixedLength();

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK1_Rol_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
