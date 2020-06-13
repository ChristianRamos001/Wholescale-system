using Microsoft.EntityFrameworkCore;
using Sistema001.Datos.Mapping.Almacen;
using Sistema001.Datos.Mapping.Usuarios;
using Sistema001.Datos.Mapping.Ventas;
using Sistema001.Entidades.Almacen;
using Sistema001.Entidades.Usuarios;
using Sistema001.Entidades.Ventas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos
{
    public class DbContextSistema : DbContext
    {
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Articulo> Articulos{ get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios{ get; set; }
        public DbSet<Persona> Personas{ get; set; }
        public DbSet<Ingreso> Ingresos{ get; set; }
        public DbSet<DetalleIngreso> DetalleIngresos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }

        public DbContextSistema(DbContextOptions<DbContextSistema> options) : base(options)
        { 
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
            modelBuilder.ApplyConfiguration(new RolMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new IngresoMap());
            modelBuilder.ApplyConfiguration(new DetalleIngresoMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new DetalleVentaMap());


        }
    }
}
