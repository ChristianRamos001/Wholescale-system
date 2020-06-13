using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema001.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos.Mapping.Almacen
{
    public class CategoriaMap : IEntityTypeConfiguration <Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categoria")
                .HasKey(c => c.idcategoria);
            builder.Property(c => c.nombre)
                .HasMaxLength(50);
            builder.Property(c => c.descripcion)
                .HasMaxLength(256);

        }
    }
}
