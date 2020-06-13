using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema001.Entidades.Almacen
{
    public class DetalleIngreso
    {
        public int iddetalle_ingreso { get; set; }
        [Required]
        public int idingreso { get; set; }
        [Required]
        public int idarticulo { get; set; }
        [Required]
        public int cantidad { get; set; }
        [Required]
        public decimal precio { get; set; }

        [ForeignKey("idingreso")]
        public Ingreso ingreso { get; set; }
        [ForeignKey("idarticulo")]
        public Articulo articulo{ get; set; }


    }
}
