using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema001.Entidades.Usuarios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos.Mapping.Usuarios
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("usuario")
                .HasKey(u => u.idusuario);
            builder.Property(u => u.nombre)
               .HasMaxLength(100);


        }
    }
}
