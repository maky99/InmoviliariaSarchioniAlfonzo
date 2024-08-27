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
        var propietario = po.ObtenerPropietariosActivos();
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

    public IActionResult EditarInmueble(int id)
    {
        var inmueble = ir.ObtenerInmueblePorId(id);
        if (inmueble.Estado_Inmueble == 0)
        {
            TempData["ErrorMessage"] = "No s epuede editar este inmueble el mismo no se encuentra activo .";
            return RedirectToAction("ListInmueble");
        }
        var tiposInmuebles = ti.TipoInmu();
        ViewData["tipoInmueble"] = tiposInmuebles;
        var propietario = po.ObtenerPropietarios();
        ViewData["propietario"] = propietario;

        return View("EditarInmueble", inmueble);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActualizarInmueble(Inmueble inmueble)
    {
        if (!ModelState.IsValid)
        {
            // Volver a mostrar la vista con los errores de validación
            ViewData["propietario"] = po.ObtenerPropietarios(); // Método para obtener propietarios
            ViewData["tipoInmueble"] = ti.TipoInmu(); // Método para obtener tipos de inmueble
            return View("EditarInmueble", inmueble);
        }

        try
        {
            ir.ActualizarInmueble(inmueble);
            TempData["SuccessMessage"] = "Inmueble actualizado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = $"Error al actualizar el inmueble: {ex.Message}";
        }

        return RedirectToAction(nameof(ListInmueble));
    }




}
