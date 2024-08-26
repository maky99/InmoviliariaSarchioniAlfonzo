using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class InmuebleController : Controller
{
    private readonly ILogger<InmuebleController> _logger;

    private InmuebleRepositorio ir = new InmuebleRepositorio();
    private Tipo_InmuebleRepositorio ti = new Tipo_InmuebleRepositorio();
    private PropietarioRepositorio po = new PropietarioRepositorio();


    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListInmueble()
    {
        var inmueble = ir.ObtenerInmuebles();

        return View("ListInmueble", inmueble);
    }

    public IActionResult NuevoInmueble()
    {
        var tiposInmuebles = ti.TipoInmuActivo();
        ViewData["tipoInmueble"] = tiposInmuebles;
        var propietario = po.OptenerPropietariosActivos();
        ViewData["propietario"] = propietario;

        return View("NuevoInmueble");
    }
    [HttpPost]
    public IActionResult GuardarInmueble(Inmueble inmueble)
    {
        ir.GuardarInmueble(inmueble);

        return RedirectToAction("ListInmueble");
    }

    public IActionResult DetalleInmueble(int id)
    {
        var inmueble = ir.InformacionInmueble(id);

        return View("DetalleInmueble", inmueble);
    }




}
