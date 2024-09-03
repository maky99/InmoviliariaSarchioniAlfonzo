using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;

    private PagoRepositorio pa = new PagoRepositorio();

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
        return View("ListContraVgernteAPago", contratos);
    }
    public IActionResult NuevoPago(int id)
    {
        var aPagar = pa.ContratoAPagar(id);

        return View("NuevoPago", aPagar);
    }



}
