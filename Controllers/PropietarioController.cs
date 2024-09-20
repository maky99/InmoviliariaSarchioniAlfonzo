using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
using InmoviliariaSarchioniAlfonzo.Repositories;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;

    private PropietarioRepositorio po = new PropietarioRepositorio();
    private InmuebleRepositorio ir = new InmuebleRepositorio();
    private ContratoRepositorio cr = new ContratoRepositorio();
    private readonly ILogRepository _logRepository;


    public PropietarioController(ILogger<PropietarioController> logger, ILogRepository logRepository)
    {
        _logger = logger;
        _logRepository = logRepository;
    }

    public IActionResult ListPropietario()
    {
        var lista = po.ObtenerPropietarios();
        return View("ListaPropietario", lista);
    }



    public IActionResult EditarPropietario(int id)
    {
        if (id == 0)
            return View();
        else
        {
            var propietario = po.BuscarPropietario(id);
            return View(propietario);
        }
    }
    [HttpPost]
    public IActionResult Guardar(int id, Propietario propietario)
    {

        id = propietario.Id_Propietario;
        int dni = propietario.Dni;
        if (id == 0)
        {
            if (po.DniyaExiste(dni))
            {
                TempData["error"] = " dni ya existe no puede introducirlo";
                return RedirectToAction(nameof(EditarPropietario));
            }
            else
            {
                po.Alta(propietario);  // Crear nuevo propietario
                _logRepository.AddLog(new Log
                {
                    LogLevel = "Guardar",
                    Message = "Alta de nuevo propietario DNI: " + propietario.Dni,
                    Timestamp = DateTime.Now,
                    Usuario = User.Identity.Name
                });
                TempData["Advertencia"] = " se dio de alta nuevo propietario .";
            }
        }
        else
        {
            if (po.EsDniDelPropietarioActual(id, dni))
            {
                TempData["error"] = " dni ya existe no puede introducirlo .";
                return RedirectToAction("EditarPropietario", new { id = propietario.Id_Propietario });
            }
            else
            {
                po.EditarDatosPropietario(propietario);  // Editar propietario existente
                _logRepository.AddLog(new Log
                {
                    LogLevel = "Edita",
                    Message = "Edita propietario ID: " + propietario.Id_Propietario,
                    Timestamp = DateTime.Now,
                    Usuario = User.Identity.Name
                });
                TempData["Advertencia"] = " se edito el propietario .";
            }
        }


        return RedirectToAction(nameof(ListPropietario));
    }


    public IActionResult EliminarPropietario(int id)
    {
        var propiedad = ir.InformacionInmueblePropietario(id);
        var contratosVigentes = cr.ContratoVigente();

        var propiedadEnContrato = propiedad.Any(p => contratosVigentes.Any(c => c.Id_Inmueble == p.Id_Inmueble));
        if (propiedadEnContrato)
        {
            // Si se encontró una propiedad en un contrato vigente
            TempData["ErrorMessage"] = "No se puede desactivar ya que una de las propiedades tiene un contrato vigente.";
        }
        else
        {
            po.Baja(id);
            TempData["Advertencia"] = "El propietario se desactivo correctamente .";
        }
        return RedirectToAction(nameof(ListPropietario));
    }

    public IActionResult ListInmueblePropietario(int id)
    {
        ViewBag.DesdeListInmueblePropietario = true;
        TempData["PreviousUrl"] = Url.Action("ListInmueblePropietario", "Propietario", new { id });
        var inmueble = ir.InformacionInmueblePropietario(id);
        ViewBag.PropietarioNombre = ir.ObtenerPropietarioPorId(id);
        return View("ListInmueblePropietario", inmueble);
    }



}
