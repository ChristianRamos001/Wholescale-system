using Sistema001.Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sistema001.Entidades.Ventas
{
    public class Persona
    {
        public int idpersona { get; set; }
        [Required]
        public string tipo_persona { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe ser mayor a 3 caracteres y menor a 100")]
        public string nombre { get; set; }
        public string tipo_documento { get; set; }
        public string num_documento { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public ICollection<Ingreso> ingresos { get; set; }
        public ICollection<Venta> ventas { get; set; }
    }
}
