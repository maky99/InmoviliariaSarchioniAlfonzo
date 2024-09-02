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

    public IActionResult DetalleInmueble(int id, string source)
    {

        var inmueble = ir.InformacionInmueble(id);
        if (source == "propietario") // manejo los botones de volver segun la vista que lo llama 
        {
            return View("DetalleInmueble", inmueble);

        }
        else
        {
            TempData["PreviousUrl"] = null;
            return View("DetalleInmueble", inmueble);
        }
    }

    public IActionResult EditarInmueble(int id, string source)
    {
        var inmueble = ir.ObtenerInmueblePorId(id);

        var tiposInmuebles = ti.TipoInmu();
        ViewData["tipoInmueble"] = tiposInmuebles;
        var propietarios = po.ObtenerPropietarios();
        ViewData["propietario"] = propietarios;
        ViewData["QuienLlamo"] = source;


        if (inmueble.Estado_Inmueble == 0)
        {
            TempData["ErrorMessage"] = "El inmueble se encuentra actualmente inactivo.";

            return View("EditarInmueble", inmueble);
        }

        return View("EditarInmueble", inmueble);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActualizarInmueble(Inmueble inmueble, string source, string previousUrl)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                Console.WriteLine(error.ErrorMessage); // O bien, usar cualquier método de logging.
            }
            // Volver a mostrar la vista con los errores de validación
            ViewData["propietario"] = po.ObtenerPropietarios(); // Método para obtener propietarios
            ViewData["tipoInmueble"] = ti.TipoInmu(); // Método para obtener tipos de inmueble
            return View("EditarInmueble", inmueble);
        }
        ir.ActualizarInmueble(inmueble);
        TempData["SuccessMessage"] = "Inmueble actualizado correctamente.";
        if (source == "Propietario")
        {
            string redirectUrl = !string.IsNullOrEmpty(previousUrl) ? previousUrl : "/";
            return Redirect(redirectUrl);
        }
        return RedirectToAction(nameof(ListInmueble));
    }


}
