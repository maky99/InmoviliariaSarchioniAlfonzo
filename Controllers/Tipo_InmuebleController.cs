using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class Tipo_InmuebleController : Controller
{
    private readonly ILogger<Tipo_InmuebleController> _logger;

    private Tipo_InmuebleRepositorio ti = new Tipo_InmuebleRepositorio();

    public Tipo_InmuebleController(ILogger<Tipo_InmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListTipo()
    {
        var list = ti.TipoInmu();

        return View("ListTipoInmueble", list);
    }
    public IActionResult NuevoTipo()
    {
        return View("NuevoTipoInmueble");
    }
    public IActionResult AgregarNuevoTipo(Tipo_Inmueble tipo_inmueble)
    {
        if (ModelState.IsValid)
        {

            ti.NuevoTipo(tipo_inmueble);
            TempData["SuccessMessage"] = $"{tipo_inmueble.Tipo} ha sido agregado exitosamente.";
            return RedirectToAction("ListTipo");
        }
        return View("NuevoTipoInmueble");

    }
    public IActionResult EditarTipo(int id)
    {
        var tipo = ti.BuscaparaEditar(id);
        return View("EditaTipoInmueble", tipo);
    }
    [HttpPost]
    public IActionResult GuardarCambiosTipo(Tipo_Inmueble tipo_inmueble)
    {
        if (ModelState.IsValid)
        {
            ti.EditaDos(tipo_inmueble);
            TempData["SuccessMessage"] = $"El tipo {tipo_inmueble.Tipo}, ha sido editado exitosamente.";
            return RedirectToAction("ListTipo");
        }
        return View("EditaTipoInmueble", tipo_inmueble);
    }
    public IActionResult CambioEstado(int id)
    {
        var tipo = ti.BuscaparaEditar(id);
        if (tipo.Estado_Tipo_Inmueble == 0)
        {
            TempData["InfoMessage"] = "El tipo de inmueble ya está inactivo.";
        }
        else
        {
            ti.DesactivarTipo(id);
            TempData["SuccessMessage"] = "El tipo ha sido desactivado exitosamente.";
        }
        return RedirectToAction("ListTipo");
    }


}
