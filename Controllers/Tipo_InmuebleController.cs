using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
using InmoviliariaSarchioniAlfonzo.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class Tipo_InmuebleController : Controller
{
    private readonly ILogger<Tipo_InmuebleController> _logger;

    private Tipo_InmuebleRepositorio ti = new Tipo_InmuebleRepositorio();
    private readonly ILogRepository _logRepository;


    public Tipo_InmuebleController(ILogger<Tipo_InmuebleController> logger, ILogRepository logRepository)
    {
        _logger = logger;
        _logRepository = logRepository;

    }
    [Authorize]
    public IActionResult ListTipo()
    {
        var list = ti.TipoInmu();

        return View("ListTipoInmueble", list);
    }

    [Authorize]
    public IActionResult NuevoTipo()
    {
        return View("NuevoTipoInmueble");
    }
    [Authorize]
    public IActionResult AgregarNuevoTipo(Tipo_Inmueble tipo_inmueble)
    {
        if (ModelState.IsValid)
        {

            ti.NuevoTipo(tipo_inmueble);
            _logRepository.AddLog(new Log
            {
                LogLevel = "Guardar",
                Message = "Alta de tipo de inmueble: " + tipo_inmueble.Tipo,
                Timestamp = DateTime.Now,
                Usuario = User.Identity.Name
            });
            TempData["SuccessMessage"] = $"{tipo_inmueble.Tipo} ha sido agregado exitosamente.";
            return RedirectToAction("ListTipo");
        }
        return View("NuevoTipoInmueble");

    }

    [Authorize]
    public IActionResult EditarTipo(int id)
    {
        var tipo = ti.BuscaparaEditar(id);
        return View("EditaTipoInmueble", tipo);
    }
    [Authorize]
    [HttpPost]
    public IActionResult GuardarCambiosTipo(Tipo_Inmueble tipo_inmueble)
    {
        if (ModelState.IsValid)
        {
            ti.EditaDos(tipo_inmueble);
            _logRepository.AddLog(new Log
            {
                LogLevel = "Edita",
                Message = "Edita tipo de inmueble: " + tipo_inmueble.Tipo,
                Timestamp = DateTime.Now,
                Usuario = User.Identity.Name
            });
            TempData["SuccessMessage"] = $"El tipo {tipo_inmueble.Tipo}, ha sido editado exitosamente.";
            return RedirectToAction("ListTipo");
        }
        return View("EditaTipoInmueble", tipo_inmueble);
    }
    [Authorize(Policy = "Administrador")]
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
            _logRepository.AddLog(new Log
            {
                LogLevel = "Eliminar",
                Message = "Desactiva tipo de inmueble ID: " + id,
                Timestamp = DateTime.Now,
                Usuario = User.Identity.Name
            });
            TempData["SuccessMessage"] = "El tipo ha sido desactivado exitosamente.";
        }
        return RedirectToAction("ListTipo");
    }


}
