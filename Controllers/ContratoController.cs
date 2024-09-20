using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
using InmoviliariaSarchioniAlfonzo.Repositories;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class ContratoController : Controller
{
    private readonly ILogger<ContratoController> _logger;

    private ContratoRepositorio co = new ContratoRepositorio();

    private InmuebleRepositorio ir = new InmuebleRepositorio();
    private Tipo_InmuebleRepositorio ti = new Tipo_InmuebleRepositorio();
    private PropietarioRepositorio po = new PropietarioRepositorio();

    private InquilinoRepositorio inq = new InquilinoRepositorio();
    private UsuarioRepositorio usuarioRepo = new UsuarioRepositorio();
    private PagoRepositorio pa = new PagoRepositorio();
 private readonly ILogRepository _logRepository;


    public ContratoController(ILogger<ContratoController> logger,ILogRepository logRepository)
    {
        _logger = logger;
          _logRepository = logRepository;
    }

    public IActionResult ListContrato()
    {
        var lista = co.ObtenerContratos();
        return View("ListContrato", lista);
    }



    public IActionResult EditarContrato(int id)
    {
        if (id == 0)
        {

            var tiposInmuebles = ti.TipoInmuActivo();
            ViewData["tipoInmueble"] = tiposInmuebles;
            var Inmuebles = ir.ObtenerInmueblesConPropietario();
            ViewData["Inmueble"] = Inmuebles;
            var propietario = po.ObtenerPropietariosActivos();
            ViewData["propietario"] = propietario;
            var inquilino = inq.OptenerInquilinosActivos();
            ViewData["inquilino"] = inquilino;

            return View("NuevoContrato");
        }
        else
        {
            var Contrato = co.ObtenerContratoActivo(id);
            if (Contrato.inmueble == null)
            {
                TempData["ErrorMessage"] = "Contrato finalizado no se puede editar ";

                return RedirectToAction(nameof(ListContrato));
            }
            return View(Contrato);
        }
    }
    [HttpPost]
    public IActionResult Guardar(int id, Contrato Contrato)
    {

        id = Contrato.Id_Contrato;

        if (id == 0)
        {
 _logRepository.AddLog(new Log
        {
            LogLevel = "Information",
            Message = " nuevo contrato de id propietario: "+ Contrato.Id_Propietario +" " +" id inquilino :"+Contrato.Id_Inquilino,
            Timestamp = DateTime.UtcNow,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });



            co.Alta(Contrato);  // Crear nuevo Contrato
            TempData["Advertencia"] = " se dio de alta nuevo Contrato .";

        }
        else
        {


            co.EditarDatosContrato(Contrato);  // Editar Contrato existente
            TempData["Advertencia"] = " se edito el Contrato .";

        }


        return RedirectToAction(nameof(ListContrato));
    }
    public IActionResult EliminarContrato(int id)
    {
 _logRepository.AddLog(new Log
        {
            LogLevel = "Information",
            Message = "El usuario ha accedido a eliminar contrato de id: "+ id,
            Timestamp = DateTime.UtcNow,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });

        var Contrato = co.ObtenerContratoActivo(id);
        var tiposInmuebles = ti.TipoInmuActivo();
        ViewData["tipoInmueble"] = tiposInmuebles;
        var Inmuebles = ir.ObtenerInmueblesConPropietario();
        ViewData["Inmueble"] = Inmuebles;
        var propietario = po.ObtenerPropietariosActivos();
        ViewData["propietario"] = propietario;
        var inquilino = inq.OptenerInquilinosActivos();
        ViewData["inquilino"] = inquilino;

        return View("FinalizaContrato", Contrato);
    }

    public IActionResult FinalizaFechaContrato(Contrato contrato)
    {

 _logRepository.AddLog(new Log
        {
            LogLevel = "Information",
            Message = "El usuario ha accedido a finalizar contrato de id: "+ contrato.Id_Contrato,
            Timestamp = DateTime.UtcNow,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });


        co.Baja(contrato);
        return RedirectToAction(nameof(ListContrato));
    }


    public IActionResult DetalleContrato(int id)

    {
        var Contrato = co.ObtenerDetalle(id);
        var usuarioCrea = usuarioRepo.UsuariosPorId(Contrato.Id_Creado_Por);
        ViewData["usuarioCrea"] = usuarioCrea;
        if (Contrato.Id_Terminado_Por != 0)
        {
            var usuarioTermina = usuarioRepo.UsuariosPorId(Contrato.Id_Terminado_Por);
            ViewData["usuarioTermina"] = usuarioTermina;
        }
        return View("DetalleContrato", Contrato);

    }
    public IActionResult CrearContrato(int id)
    {
        var inmueble = ir.ObtenerInmueblePorId(id);

        var tiposInmuebles = ti.TipoInmu();
        ViewData["inmueble"] = inmueble;
        var propietarios = po.ObtenerPropietarios();
        ViewData["propietario"] = propietarios;
        var inquilino = inq.OptenerInquilinosActivos();
        ViewData["inquilino"] = inquilino;

        return View("NuevoContratoDirecto");
    }
    public IActionResult FiltroContrato()
    {
        return View("FiltarContrato", new List<Contrato>());
    }
    public IActionResult BuscarContra(int Meses)
    {
        var contratosVigentes = co.ContratoVigente();
        var pagos = pa.ObtenerPagosActivosSinMulta();
        var contratosFiltrados = contratosVigentes
            .Where(contrato =>
            {
                var cuotasPagadas = pagos.Count(p => p.Id_Contrato == contrato.Id_Contrato);
                var mesesRestantes = contrato.Meses - cuotasPagadas;
                return mesesRestantes == Meses;
            })
            .ToList();
        if (contratosFiltrados.Count == 0)
        {
            TempData["ErrorMessage"] = $"No hay contratos que finalicen en {Meses} meses.";
        }
        if (contratosFiltrados.Count == 1)
        {
            TempData["SuccessMessage"] = $"Contrato que finaliza en {Meses} mese.";
        }
        if (contratosFiltrados.Count > 1)
        {
            TempData["SuccessMessage"] = $"Contratos que finalizan en {Meses} meses.";
        }

        return View("FiltarContrato", contratosFiltrados);
    }


}