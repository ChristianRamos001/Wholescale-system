using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema001.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos.Mapping.Usuarios
{
    public class RolMap : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("rol")
                .HasKey(r => r.idrol);
            builder.Property(r => r.nombre)
               .HasMaxLength(50);

        }
    }
}
