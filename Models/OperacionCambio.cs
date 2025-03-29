using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace pc1.Models
{
    public class OperacionCambio
    {
        [Required(ErrorMessage = "Debe seleccionar la moneda que envía.")]
        public string? MonedaOrigen { get; set; } 

        [Required(ErrorMessage = "Debe seleccionar la moneda que recibe.")]
        public string? MonedaDestino { get; set; } 

        [Required(ErrorMessage = "Debe ingresar el monto a enviar.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto a enviar debe ser positivo.")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? MontoEnviar { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? MontoRecibir { get; set; } 

        public decimal? TipoCambioAplicado { get; set; }

    
        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public List<SelectListItem> MonedasDisponibles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "Seleccione..." },
            new SelectListItem { Value = "BRL", Text = "Real Brasileño (BRL)" },
            new SelectListItem { Value = "PEN", Text = "Sol Peruano (PEN)" },
            new SelectListItem { Value = "USD", Text = "Dólar Americano (USD)" }
        };
    }
}