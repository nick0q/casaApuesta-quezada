using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pc1.Models
{
    public class Boleta
    {
        public Usuario? DatosUsuario { get; set; }
        public OperacionCambio? DatosOperacion { get; set; }
        public DateTime FechaOperacion { get; set; }
        public string? NumeroBoleta { get; set; }

        public Boleta() { }

        public Boleta(Usuario usuario, OperacionCambio operacion)
        {
            DatosUsuario = usuario;
            DatosOperacion = operacion;
            FechaOperacion = DateTime.Now;
            NumeroBoleta = $"BOL-{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}