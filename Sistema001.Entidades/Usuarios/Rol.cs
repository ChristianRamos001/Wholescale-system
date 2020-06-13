using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sistema001.Entidades.Usuarios
{
    public class Rol
    {
        public int idrol { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener más de 50 caracteres, ni menos de 3 caracteres.")]
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public bool condicion { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
