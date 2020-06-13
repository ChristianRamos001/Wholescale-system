using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sistema001.Entidades.Ventas;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sistema001.Datos.Mapping.Ventas
{
    public class VentaMap : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
           
                builder.ToTable("venta")
                    .HasKey(i => i.idventa);
                builder.HasOne(i => i.persona)
                    .WithMany(p => p.ventas)
                    .HasForeignKey(i => i.idcliente);

            
        }
    }
}
