﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema001.Entidades.Ventas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos.Mapping.Ventas
{
    public class DetalleVentaMap : IEntityTypeConfiguration<DetalleVenta>
    {
        public void Configure(EntityTypeBuilder<DetalleVenta> builder)
        {
            builder.ToTable("detalle_venta")
               .HasKey(d => d.iddetalle_venta);
        }
    }
}
