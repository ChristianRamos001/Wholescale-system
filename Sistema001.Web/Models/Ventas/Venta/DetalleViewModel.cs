using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema001.Web.Models.Ventas.Venta
{
    public class DetalleViewModel
    {
        public int idarticulo { get; set; }
        public string articulo { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal descuento { get; set; }
    }
}
