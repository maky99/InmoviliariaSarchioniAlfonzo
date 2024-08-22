using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;

namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;

    private PropietarioRepositorio po = new PropietarioRepositorio();

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListPropietario()
    {
        var lista = po.OptenerPropietarios();
        return View("ListaPropietario", lista);
    }

    public IActionResult EditarPropietario(int id)
    {
        var Propietario = po.BuscarPropietario(id);
        return View(Propietario);
    }


}
