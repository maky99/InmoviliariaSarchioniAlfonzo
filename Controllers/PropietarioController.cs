using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Models;

namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class PropietarioController : Controller
{
    private readonly ILogger<PropietarioController> _logger;

    private PropietarioRepositorio po = new PropietarioRepositorio();

    public PropietarioController(ILogger<PropietarioController> logger)
    {
        _logger = logger;
    }

    public IActionResult ListPropietario()
    {
        var lista = po.OptenerPropietarios();
        return View("ListaPropietario", lista);
    }

  

  public IActionResult EditarPropietario(int id)
    {
    if(id == 0)
        return View();
        else{
            var propietario = po.BuscarPropietario(id);
            return View(propietario);
        }
    }
[HttpPost]
public IActionResult Guardar( int id, Propietario propietario)
    {
        id =  propietario.Id_Propietario;
        if(id == 0)
            {
               po.Alta(propietario);
            }
        else
            {
              po.EditarDatosPropietario(propietario);
            }
        return RedirectToAction(nameof(ListPropietario));
    }

public IActionResult EliminarPropietario( int id)
    {
        po.Baja(id);
        return RedirectToAction(nameof(ListPropietario));
    }

}
