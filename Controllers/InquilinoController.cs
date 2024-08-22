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
            ir.NuevoInquilino(inquilino);
            return RedirectToAction("ListInquilino");
        }
        return View("NuevoInquilino", inquilino); //
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
