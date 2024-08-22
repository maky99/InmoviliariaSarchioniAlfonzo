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
                TempData["SuccessMessage"] = "El inquilino ha sido agregado exitosamente.";
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
        ir.EditarDatos(inquilino);
        return RedirectToAction("ListInquilino");
    }


}
