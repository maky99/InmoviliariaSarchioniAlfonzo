using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;

    private InmuebleRepositorio ir = new InmuebleRepositorio();

    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListInmueble()
    {
        var inmueble = ir.ObtenerInmuebles();

        return View("ListInmueble", inmueble);
    }




}
