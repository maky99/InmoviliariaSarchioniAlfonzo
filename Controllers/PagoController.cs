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
        var contratos = pa.ContratoVigente();
        var suma = pa.CantPagos();//quiero sumar y restar la cantidad de pagos con los contratos y ver cuantas cuotas le falta pagar 
        return View("ListContraVgernteAPago", contratos);
    }
    public IActionResult NuevoPago(int id)
    {
        var contrato = cr.ObtenerDetalle(id);
        ViewData["contrato"] = contrato;

        return View("NuevoPago");
    }
    [HttpPost]
    public IActionResult GuardarPago(Pago pago)
    {
        if (ModelState.IsValid)
        {
            pa.GuardarPago(pago);

            TempData["SuccessMessage"] = "Pago guardado exitosamente.";
            return RedirectToAction("ListContVigentes");
        }

        TempData["ErrorMessage"] = $"Error al guardar el pago";
        return View("NuevoPago", pago);

    }


}
