using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sistema001.Web.Models.Ventas.Persona
{
    public class ActualizarViewModel
    {
        [Required]
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
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}
