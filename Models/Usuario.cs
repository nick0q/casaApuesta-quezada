using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace pc1.Models
{
    public class Usuario
    {
        [Required(ErrorMessage = "El nombre es requerido.")]
        [StringLength(100)]
        public string? Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es requerido.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        [StringLength(100)]
        public string? Correo { get; set; }

        [Required(ErrorMessage = "El DNI es requerido.")]
        [RegularExpression(@"^\d{8}$", ErrorMessage = "El DNI debe tener 8 dígitos.")]
        [StringLength(8)]
        public string? Dni { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido.")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        [StringLength(15)]
        public string? Telefono { get; set; }
    }
}