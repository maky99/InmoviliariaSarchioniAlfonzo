using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;

    private InquilinoRepositorio ir = new InquilinoRepositorio();

    public InquilinoController(ILogger<InquilinoController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListInquilino()
    {
        var lista = ir.OptenerInquilinos();
        return View("ListaInquilino", lista);
    }
    public IActionResult NuevoInquilino()
    {
        return View("NuevoInquilino"); // Devuelve la vista para crear un nuevo inquilino
    }

    [HttpPost]
    public IActionResult CrearInquilino(Inquilino inquilino)
    {
        if (ModelState.IsValid)
        {
            try
            {
                ir.NuevoInquilino(inquilino);
                TempData["SuccessMessage"] = $"{inquilino.Apellido}, {inquilino.Nombre} ha sido agregado exitosamente.";
                return RedirectToAction("ListInquilino");
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
        }

        // En caso de error o invalidación del modelo, regresar a la vista con el mensaje de error
        return View("NuevoInquilino", inquilino);
    }


    public IActionResult EditarInquilino(int id)
    {
        var inquilino = ir.BuscarInquilino(id);
        return View(inquilino);
    }

    public IActionResult EditarDatos(Inquilino inquilino)
    {
        if (ModelState.IsValid)
        {
            if (ir.EsDniDelInquilinoActual(inquilino.Id_Inquilino, inquilino.Dni))
            {
                ModelState.AddModelError("Dni", "El DNI ingresado ya está registrado.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ir.EditarDatos(inquilino);
                    TempData["SuccessMessage"] = $"El inquilino {inquilino.Apellido} {inquilino.Nombre} ha sido editado exitosamente.";
                    return RedirectToAction("ListInquilino");
                }
                catch (InvalidOperationException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                    return RedirectToAction("EditarDatos", new { id = inquilino.Id_Inquilino });
                }

            }
        }

        // si el modelo esta mal vuelve a la vista
        return View("EditarInquilino", inquilino);
    }

    [HttpGet]
    public IActionResult CambEstadoInquilino(int id)
    {
        ir.DesactivarInquilino(id);
        TempData["SuccessMessage"] = "El inquilino ha sido desactivado exitosamente.";
        return RedirectToAction("ListInquilino");

    }


}
