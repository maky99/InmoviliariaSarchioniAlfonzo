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
    private ContratoRepositorio co = new ContratoRepositorio();


    public InmuebleController(ILogger<InmuebleController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListInmueble()
    {
        var inmueble = ir.ObtenerInmuebles();

        return View("ListInmueble", inmueble);
    }

    public IActionResult NuevoInmueble(int idPropietario)
    {
        if (idPropietario == 0)
        {
            var tiposInmuebles = ti.TipoInmuActivo();
            ViewData["tipoInmueble"] = tiposInmuebles;
            var propietario = po.ObtenerPropietariosActivos();
            ViewData["propietario"] = propietario;

            return View("NuevoInmueble");
        }
        else
        {
            var tiposInmuebles = ti.TipoInmuActivo();
            ViewData["tipoInmueble"] = tiposInmuebles;
            var propietario = po.BuscarPropietario(idPropietario);
            ViewData["propietario"] = propietario;
            return View("NuevoInmueblePropietario");
        }
    }
    [HttpPost]
    public IActionResult GuardarInmueble(Inmueble inmueble, string source, string previousUrl)
    {
        ir.GuardarInmueble(inmueble);
        if (source == "propietario")
        {
            string redirectUrl = !string.IsNullOrEmpty(previousUrl) ? previousUrl : "/";
            return Redirect(redirectUrl);
        }
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
    public IActionResult BuscarInmueble()
    {

        var inmueble = ir.ObtenerInmuebles();
        ViewData["inmueble"] = inmueble;

        var tiposInmuebles = ti.TipoInmuActivo();
        ViewData["tipoInmueble"] = tiposInmuebles;

        return View("BuscarInmueble");
    }

    public IActionResult Buscador(int Id_Tipo_Inmueble, int Ambientes, double PrecioMin, double PrecioMax, DateTime? FechaInicio, DateTime? FechaFin)
    {
        // Obtener todos los inmuebles
        var inmuebles = ir.ObtenerInmuebles();
        var contratosVigentes = co.ContratoVigente();
        var inmueblesConContrato = contratosVigentes.Select(c => c.Id_Inmueble).ToList();


        // Filtrar inmuebles para excluir aquellos con contrato vigente
        inmuebles = inmuebles.Where(i => !inmueblesConContrato.Contains(i.Id_Inmueble)).ToList();

        // Aplicar filtros
        if (Id_Tipo_Inmueble > 0)
        {
            inmuebles = inmuebles.Where(i => i.tipo?.Id_Tipo_Inmueble == Id_Tipo_Inmueble).ToList();
        }

        if (Ambientes > 0)
        {
            inmuebles = inmuebles.Where(i => i.Ambientes == Ambientes).ToList();
        }
        if (PrecioMin > 0)
        {
            inmuebles = inmuebles.Where(i => i.Precio >= PrecioMin).ToList();
        }
        if (PrecioMax > 0)
        {
            inmuebles = inmuebles.Where(i => i.Precio <= PrecioMax).ToList();
        }
        if (FechaInicio.HasValue)
        {
            // Ajusta el filtro según el campo de fecha en tu modelo
            inmuebles = inmuebles.Where(i => i.FechaInicioAlquiler < FechaInicio.Value).ToList();
        }

        if (FechaFin.HasValue)
        {
            // Ajusta el filtro según el campo de fecha en tu modelo
            inmuebles = inmuebles.Where(i => i.FechaFinAlquiler > FechaFin.Value).ToList();
        }
        // Verificar si no hay resultados
        if (!inmuebles.Any())
        {
            // Usar TempData para enviar el mensaje a la vista
            TempData["ErrorMessage"] = "No se encontraron resultados. Intenta con otros filtros.";

            // Redirigir a la vista de búsqueda
            return RedirectToAction("BuscarInmueble");
        }

        // Si hay resultados, mostrar la vista de lista de inmuebles filtrados
        return View("ListInmuebleBusqueda", inmuebles);

    }



}
