using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
using InmoviliariaSarchioniAlfonzo.Repositories;
using Microsoft.AspNetCore.Authorization;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class InquilinoController : Controller
{
    private readonly ILogger<InquilinoController> _logger;

    private InquilinoRepositorio ir = new InquilinoRepositorio();
    private readonly ILogRepository _logRepository;

    public InquilinoController(ILogger<InquilinoController> logger, ILogRepository logRepository)
    {
        _logger = logger;
        _logRepository = logRepository;
    }
    [Authorize]
    public IActionResult ListInquilino()
    {
        var lista = ir.OptenerInquilinos();
        return View("ListaInquilino", lista);
    }
    [Authorize]
    public IActionResult NuevoInquilino()
    {
        return View("NuevoInquilino"); // Devuelve la vista para crear un nuevo inquilino
    }
    [Authorize]
    [HttpPost]
    public IActionResult CrearInquilino(Inquilino inquilino)
    {
        if (ModelState.IsValid)
        {

            try
            {
                _logRepository.AddLog(new Log
                {
                    LogLevel = "Guardar",
                    Message = "Alta de " + inquilino.Apellido + inquilino.Dni,
                    Timestamp = DateTime.Now,
                    Usuario = User.Identity.Name // Nombre del usuario autenticado
                });
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

    [Authorize]
    public IActionResult EditarInquilino(int id)
    {
        var inquilino = ir.BuscarInquilino(id);
        return View(inquilino);
    }
    [Authorize(Policy = "Administrador")]
    public IActionResult EditarDatos(Inquilino inquilino)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _logRepository.AddLog(new Log
                {
                    LogLevel = "Edita",
                    Message = "Edita inquilino id: " + inquilino.Id_Inquilino,
                    Timestamp = DateTime.Now,
                    Usuario = User.Identity.Name // Nombre del usuario autenticado
                });
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
        // si el modelo esta mal vuelve a la vista
        return View("EditarInquilino", inquilino);
    }
    [Authorize]
    [HttpGet]
    public IActionResult CambEstadoInquilino(int id)
    {
        _logRepository.AddLog(new Log
        {
            LogLevel = "Eliminar",
            Message = "Eliminar Inquilino de id: " + id,
            Timestamp = DateTime.Now,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });
        ir.DesactivarInquilino(id);
        TempData["SuccessMessage"] = "El inquilino ha sido desactivado exitosamente.";
        return RedirectToAction("ListInquilino");
    }


}
