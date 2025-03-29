using Microsoft.AspNetCore.Mvc;
using pc1.Models;
using System.Text.Json; 

namespace pc1.Controllers
{
    public class CasaController : Controller
    {
        private const decimal TCBrlPen = 1.577726m;
        private const decimal TCPenBrl = 0.6338236m;

        private const decimal TC_USD_PEN = 0.27m;
        
        private const decimal TC_PEN_USD = 3.64m;
        
        private const decimal TC_USD_BRL = 0.1736m;
        
        private const decimal TC_BRL_USD = 5.8477m;

       
        public IActionResult Index()
        {
            var model = new CasaCambioViewModel();
          
            if (TempData.ContainsKey("ViewModelData")) {
                 var viewModelJson = TempData["ViewModelData"] as string;
                 if (!string.IsNullOrEmpty(viewModelJson)) {
                     try {
                        var recoveredModel = JsonSerializer.Deserialize<CasaCambioViewModel>(viewModelJson);
                        if (recoveredModel != null) model = recoveredModel;
                     } catch (JsonException) { /* Ignorar error de deserialización */ }
                 }
                
                 TempData.Remove("ViewModelData");
            }

             if (TempData.ContainsKey("SuccessMessage")) {
                 ViewBag.SuccessMessage = TempData["SuccessMessage"];
             }


            return View(model);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CasaCambioViewModel model, string actionButton)
        {
            if (actionButton == "calcular")
            {
              
                ModelState.Remove("Usuario.Nombre");
                ModelState.Remove("Usuario.Correo");
                ModelState.Remove("Usuario.Dni");
                ModelState.Remove("Usuario.Telefono");

               
                if (ModelState.IsValid)
                {
                    try
                    {
                        model.Operacion.MontoRecibir = CalcularMontoRecibir(
                            model.Operacion.MonedaOrigen,
                            model.Operacion.MonedaDestino,
                            model.Operacion.MontoEnviar ?? 0,
                            out decimal tipoCambioUsado);
                        model.Operacion.TipoCambioAplicado = tipoCambioUsado;
                         ModelState.Remove("Operacion.MontoRecibir");
                         ModelState.Remove("Operacion.TipoCambioAplicado");
                    }
                    catch (ArgumentException ex)
                    {
                        ModelState.AddModelError("Operacion.MonedaDestino", ex.Message);
                        model.Operacion.MontoRecibir = null;
                        model.Operacion.TipoCambioAplicado = null;
                    }
                } else {
                     model.Operacion.MontoRecibir = null;
                     model.Operacion.TipoCambioAplicado = null;
                }
                
                return View(model);
            }
            else if (actionButton == "completar")
            {
              
                if (ModelState.IsValid)
                {
           
                    try
                    {
                         
                         model.Operacion.MontoRecibir = CalcularMontoRecibir(
                             model.Operacion.MonedaOrigen,
                             model.Operacion.MonedaDestino,
                             model.Operacion.MontoEnviar ?? 0,
                             out decimal tipoCambioUsado);
                         model.Operacion.TipoCambioAplicado = tipoCambioUsado;

                         
                         TempData["SuccessMessage"] = "¡Operación validada correctamente! La generación de boleta se implementará pronto.";
                          
                         TempData["ViewModelData"] = JsonSerializer.Serialize(model);
                         return RedirectToAction("Index"); 

                    } catch (ArgumentException ex) {
                         
                         ModelState.AddModelError("Operacion.MonedaDestino", ex.Message);
                          model.Operacion.MontoRecibir = null;
                          model.Operacion.TipoCambioAplicado = null;
                         
                         return View(model);
                    }



                }
                else
                {

                     if (model.Operacion.MontoEnviar.HasValue &&
                        !string.IsNullOrEmpty(model.Operacion.MonedaOrigen) &&
                        !string.IsNullOrEmpty(model.Operacion.MonedaDestino) &&
                        model.Operacion.MonedaOrigen != model.Operacion.MonedaDestino)
                     {
                         try {
                             model.Operacion.MontoRecibir = CalcularMontoRecibir(
                                 model.Operacion.MonedaOrigen,
                                 model.Operacion.MonedaDestino,
                                 model.Operacion.MontoEnviar.Value,
                                 out decimal tipoCambioUsado);
                             model.Operacion.TipoCambioAplicado = tipoCambioUsado;
                         } catch { /* Ignorar error de cálculo si validación principal falló */ }
                     } else {
                          model.Operacion.MontoRecibir = null;
                          model.Operacion.TipoCambioAplicado = null;
                     }

                    
                    TempData["ViewModelData"] = JsonSerializer.Serialize(model);
                    return RedirectToAction("Index"); 
                    
                }
            }

            
            return RedirectToAction("Index");
        }

 
        private decimal CalcularMontoRecibir(string? origen, string? destino, decimal montoEnviar, out decimal tipoCambioAplicado)
        {
             tipoCambioAplicado = 0m;
             if (string.IsNullOrEmpty(origen) || string.IsNullOrEmpty(destino) || montoEnviar <= 0)
                 throw new ArgumentException("Datos de entrada inválidos.");
             if (origen == destino)
                 throw new ArgumentException("Monedas origen y destino deben ser diferentes.");

             if (origen == "BRL" && destino == "PEN") {
                 tipoCambioAplicado = TCPenBrl;
                 return montoEnviar * TCPenBrl;
             } else if (origen == "PEN" && destino == "BRL") {
                 tipoCambioAplicado = TCBrlPen;
                 return montoEnviar * TCBrlPen;
             }  else if (origen == "USD" && destino == "PEN") {
                 tipoCambioAplicado = TC_USD_PEN;
                 return montoEnviar * TC_USD_PEN;
             }   else if (origen == "PEN" && destino == "USD") {
                 tipoCambioAplicado = TC_PEN_USD;
                 return montoEnviar * TC_PEN_USD;
             }   else if (origen == "USD" && destino == "BRL") {
                 tipoCambioAplicado = TC_USD_BRL;
                 return montoEnviar * TC_USD_BRL;
             }  else if (origen == "BRL" && destino == "USD") {
                 tipoCambioAplicado = TC_BRL_USD;
                 return montoEnviar * TC_BRL_USD;
             } else {
                 throw new ArgumentException($"El cambio de {origen} a {destino} no está soportado.");
             }
        }
    }
}