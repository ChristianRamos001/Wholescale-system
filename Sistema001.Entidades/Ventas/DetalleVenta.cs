using Sistema001.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sistema001.Entidades.Ventas
{
    public class DetalleVenta
    {
        public int iddetalle_venta { get; set; }
        [Required]
        public int idventa { get; set; }
        [Required]
        public int idarticulo { get; set; }
        [Required]
        public int cantidad { get; set; }
        [Required]
        public decimal precio { get; set; }
        [Required]
        public decimal descuento { get; set; }

        [ForeignKey("idventa")]
        public Venta venta { get; set; }
        [ForeignKey("idarticulo")]
        public Articulo articulo { get; set; }
    }
}
