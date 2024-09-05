using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
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
    public IActionResult NuevoPago(int id, string source)
    {
        var contrato = cr.ObtenerDetalle(id);
        var pagos = pa.ObtenerPagosPorContrato(id);
        ViewData["pagos"] = pagos;
        ViewData["contrato"] = contrato;
        if (source == "pagar")
        {
            return View("NuevoPago");
        }
        else
        {
            return View("NuevoPagoMulta");
        }
    }
    [HttpPost]
    public IActionResult GuardarPago(Pago pago)
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
            {
                TempData["SuccessMessage"] = "Pago guardado exitosamente.";
            }

            return RedirectToAction("ListContVigentes");
        }

        TempData["ErrorMessage"] = "Error al guardar el pago.";
        return View("NuevoPago", pago);
    }


}
