using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pc1.Models
{
    public class CasaCambioViewModel
    {
        public Usuario Usuario { get; set; } = new Usuario();
        public OperacionCambio Operacion { get; set; } = new OperacionCambio();
    }
}