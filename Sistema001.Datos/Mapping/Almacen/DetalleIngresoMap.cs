using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema001.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos.Mapping.Almacen
{
    public class DetalleIngresoMap : IEntityTypeConfiguration<DetalleIngreso>
    {
        public void Configure(EntityTypeBuilder<DetalleIngreso> builder)
        {
            builder.ToTable("detalle_ingreso")
                .HasKey(d => d.iddetalle_ingreso);
        }
    }
}
