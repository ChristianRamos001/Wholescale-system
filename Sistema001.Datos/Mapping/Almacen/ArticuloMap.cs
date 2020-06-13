using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema001.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos.Mapping.Almacen
{
    public class ArticuloMap : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder)
        {
            builder.ToTable("articulo")
                .HasKey(a => a.idarticulo);
            builder.Property(a => a.nombre)
                .HasMaxLength(50);

        }
    }
}
