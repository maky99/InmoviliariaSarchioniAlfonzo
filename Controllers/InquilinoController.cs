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

    public IActionResult EditarInquilino(int id)
    {
        var inquilino = ir.BuscarInquilino(id);
        return View(inquilino);
    }

    public IActionResult EditarDatos(Inquilino inquilino)
    {
        ir.EditarDatos(inquilino);
        var lista = ir.OptenerInquilinos();
        return View("ListaInquilino", lista);
    }


}
