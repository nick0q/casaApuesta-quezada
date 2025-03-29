using Microsoft.AspNetCore.Mvc;
using pc1.Models;       
using System.Text.Json;         

namespace pc1.Controllers
{
    public class BoletaController : Controller
    {

        public IActionResult Mostrar()
        {
          
            if (TempData.TryGetValue("BoletaData", out object? boletaDataObj) && boletaDataObj is string boletaJson)
            {
                Boleta? boletaModel = null;
                try
                {
                   
                    boletaModel = JsonSerializer.Deserialize<Boleta>(boletaJson);
                }
                catch (JsonException ex)
                {
                   
                    Console.WriteLine($"Error al deserializar Boleta desde TempData: {ex.Message}");
                    
                    TempData["ErrorMessage"] = "Ocurrió un error inesperado al generar la boleta.";
                    return RedirectToAction("Index", "Casa"); 
                }

                
                if (boletaModel != null)
                {
                    
                    return View(boletaModel);
                }
            }

 
            TempData["ErrorMessage"] = "No se encontró información de la operación para mostrar la boleta.";
            return RedirectToAction("Index", "Casa"); 
        }
    }
}