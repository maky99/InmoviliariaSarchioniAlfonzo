using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
using System.Runtime.Intrinsics.X86;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;

    private PagoRepositorio pa = new PagoRepositorio();
    private ContratoRepositorio cr = new ContratoRepositorio();
    private UsuarioRepositorio usuarioRepo = new UsuarioRepositorio();
    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListPagos()
    {
        var pago = pa.ObtenerPagos();
        return View("ListaPago", pago);
    }
    public IActionResult ListPagosOrdexInqu()
    {
        var pago = pa.ObtenerPagosOrdxInqui();
        return View("ListaPago", pago);
    }
    public IActionResult ListContVigentes()
    {
        var contratos = cr.ContratoVigente();
        return View("ListContraVgernteAPago", contratos);
    }
    public IActionResult NuevoPago(int id, string source, string previousUrl)
    {
        var contrato = cr.ObtenerDetalle(id);
        var pagos = pa.ObtenerPagosPorContrato(id);
        ViewData["pagos"] = pagos;
        ViewData["contrato"] = contrato;
        if (source == "pagar" || source == "pagarContrato")
        {
            ViewData["QuienLlamo"] = source;
            TempData["PreviousUrl"] = previousUrl;
            return View("NuevoPago");
        }
        else
        {
            return View("NuevoPagoMulta");
        }
    }
    [HttpPost]
    public IActionResult GuardarPago(Pago pago, string previousUrl, string source)
    {
        if (ModelState.IsValid)
        {
            pa.GuardarPago(pago);
            //  busco los datos para ver si es la ultima cuota para infomrale y cambiarle el estado al contrato 
            var contrato = cr.ObtenerDetalle(pago.Id_Contrato);
            var pagos = pa.ObtenerPagosPorContrato(pago.Id_Contrato);

            if (pagos.Count == contrato.Meses)
            {
                contrato.Estado_Contrato = 0;
                cr.ActualizarContrato(contrato);
                TempData["NotificationMessage"] = "Pago guardado exitosamente. Has pagado la última cuota.";
            }
            else

            if (source == "pagarContrato")
            {
                TempData["SuccessMessage"] = "Pago guardado exitosamente.";
                string redirectUrl = !string.IsNullOrEmpty(previousUrl) ? previousUrl : "/";
                return Redirect(redirectUrl);
            }
            else
            {
                TempData["SuccessMessage"] = "Pago guardado exitosamente.";
                return RedirectToAction("ListContVigentes");
            }
        }
        return RedirectToAction("ListContVigentes");

    }


    [HttpPost]
    public IActionResult GuardarPagoAnulado(Pago pago)
    {
        if (ModelState.IsValid)
        {

            pa.GuardarPago(pago);
            var contrato = cr.ObtenerDetalle(pago.Id_Contrato);
            contrato.Estado_Contrato = 0;
            contrato.Id_Terminado_Por = pago.Id_Creado_Por;
            contrato.Finalizacion_Anticipada = pago.Fecha;
            cr.ActualizarContratoFecha(contrato);
            TempData["NotificationMessage"] = "Pago guardado exitosamente. Y se ha finalizado el contrato.";
        }

        return RedirectToAction("ListContVigentes");

    }
    public IActionResult DetallePago(int id, string previousUrl, string source)
    {
        var pago = pa.DetallePago(id);
        ViewData["pago"] = pago;
        var idcontrato = pago.Id_Contrato;
        var contrato = cr.ObtenerTodosContrato(idcontrato);
        var usuarioCrea = usuarioRepo.UsuariosPorId(pago.Id_Creado_Por);
        ViewData["usuarioCrea"] = usuarioCrea;
        if (pago.Id_Terminado_Por != 0)
        {
            var usuarioTermina = usuarioRepo.UsuariosPorId(pago.Id_Terminado_Por);
            ViewData["usuarioTermina"] = usuarioTermina;
        }
        ViewData["contrato"] = contrato;
        ViewData["QuienLlamo"] = source;
        TempData["PreviousUrl"] = previousUrl;
        return View("DetallePago");
    }
    public IActionResult DetallePagContrato(int id, string previousUrl, string source)
    {

        var pago = pa.VerPagosContrato(id);
        ViewData["QuienLlamo"] = source;
        TempData["PreviousUrl"] = previousUrl;
        return View("ListaPago", pago);
    }

    public IActionResult AnularPago(int id, int id_Usuario, string previousUrl, string source, int id_Contrato)
    {
        pa.AnularPago(id, id_Usuario);
        var contrato = cr.ObtenerDetalle(id_Contrato);
        var cuota = pa.CuotaPagosRestantesPorContrato(id_Contrato);//traigo la cantidad de cuotas pagas 
        if (contrato.Meses > cuota && contrato.Estado_Contrato == 0)
        {
            contrato.Estado_Contrato = 1;
            cr.ActualizarContrato(contrato);
        }

        TempData["SuccessMessage"] = "Se anulo Correctamente";
        if (source == "pagarContrato")
        {
            return RedirectToAction("DetallePagContrato", new { id = id_Contrato, previousUrl = previousUrl, source = source });

        }
        return RedirectToAction("ListPagos");

    }



}
