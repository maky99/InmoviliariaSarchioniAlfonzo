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

    public PagoController(ILogger<PagoController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListPagos()
    {
        var pago = pa.ObtenerPagos();
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

            if (pagos.Count >= contrato.Meses)
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

        TempData["ErrorMessage"] = "Error al guardar el pago.";
        return View("NuevoPago", pago);
    }


    [HttpPost]
    public IActionResult GuardarPagoAnulado(Pago pago)
    {
        if (ModelState.IsValid)
        {
            pa.GuardarPago(pago);
            var contrato = cr.ObtenerDetalle(pago.Id_Contrato);
            contrato.Estado_Contrato = 0;
            contrato.Finalizacion_Anticipada = pago.Fecha;
            cr.ActualizarContratoFecha(contrato);
            TempData["NotificationMessage"] = "Pago guardado exitosamente. Y se ha finalizado el contrato.";
        }

        return RedirectToAction("ListContVigentes");

    }
    public IActionResult DetallePago(int id)
    {
        var pago = pa.DetallePago(id);
        ViewData["pago"] = pago;
        var idcontrato = pago.Id_Contrato;
        var contrato = cr.ObtenerTodosContrato(idcontrato);
        ViewData["contrato"] = contrato;
        return View("DetallePago");
    }

    public IActionResult AnularPago(int id, int id_Usuario)
    {
        pa.AnularPago(id, id_Usuario);

        return RedirectToAction("ListPagos");

    }



}
