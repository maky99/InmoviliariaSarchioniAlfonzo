using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;
using System.Runtime.Intrinsics.X86;
using InmoviliariaSarchioniAlfonzo.Repositories;
using System;
using Microsoft.AspNetCore.Authorization;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class PagoController : Controller
{
    private readonly ILogger<PagoController> _logger;
    private readonly ILogRepository _logRepository;
    private PagoRepositorio pa = new PagoRepositorio();
    private ContratoRepositorio cr = new ContratoRepositorio();
    private UsuarioRepositorio usuarioRepo = new UsuarioRepositorio();

    public PagoController(ILogger<PagoController> logger, ILogRepository logRepository)
    {
        _logger = logger;
        _logRepository = logRepository;
    }
    [Authorize]
    public IActionResult ListPagos()
    {
        var pago = pa.ObtenerPagos();
        return View("ListaPago", pago);
    }
    [Authorize]
    public IActionResult ListPagosOrdexInqu()
    {
        var pago = pa.ObtenerPagosOrdxInqui();
        return View("ListaPago", pago);
    }
    [Authorize]
    public IActionResult ListContVigentes()
    {
        var contratos = cr.ContratoVigente();
        return View("ListContraVgernteAPago", contratos);
    }
    [Authorize]
    public IActionResult NuevoPago(int id, string source, string previousUrl)
    {

        _logRepository.AddLog(new Log
        {
            LogLevel = "Information",
            Message = "El usuario ha accedido a nuevo pago de id: " + id,
            Timestamp = DateTime.Now,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });



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
    [Authorize]

    [HttpPost]
    public IActionResult GuardarPago(Pago pago, string previousUrl, string source)
    {

        _logRepository.AddLog(new Log
        {
            LogLevel = "Guardar",
            Message = "El usuario ha accedido a guardar pago cuota paga:" + pago.CuotaPaga,
            Timestamp = DateTime.Now,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });



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
    [Authorize]

    [HttpPost]
    public IActionResult GuardarPagoAnulado(Pago pago)
    {

        _logRepository.AddLog(new Log
        {
            LogLevel = "Guardar",
            Message = "El usuario ha accedido a guardar pago anulado.",
            Timestamp = DateTime.Now,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });


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

        return RedirectToAction("ListPagos");

    }
    [Authorize]
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
    [Authorize]
    public IActionResult DetallePagContrato(int id, string previousUrl, string source)
    {

        var pago = pa.VerPagosContrato(id);
        ViewData["QuienLlamo"] = source;
        TempData["PreviousUrl"] = previousUrl;
        return View("ListaPago", pago);
    }
    [Authorize]

    public IActionResult AnularPago(int id, int id_Usuario, string previousUrl, string source, int id_Contrato)
    {

        _logRepository.AddLog(new Log
        {
            LogLevel = "Anular",
            Message = "El usuario ha accedido a anular pago.",
            Timestamp = DateTime.Now,
            Usuario = User.Identity.Name // Nombre del usuario autenticado
        });

        pa.AnularPago(id, id_Usuario);
        TempData["SuccessMessage"] = "Se anulo Correctamente";
        if (source == "pagarContrato")
        {
            return RedirectToAction("DetallePagContrato", new { id = id_Contrato, previousUrl = previousUrl, source = source });

        }
        return RedirectToAction("ListPagos");

    }



}
