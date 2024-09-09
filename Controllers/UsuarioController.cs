
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InmoviliariaSarchioniAlfonzo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace InmoviliariaSarchioniAlfonzo.Controllers;

public class UsuarioController : Controller
{

	private readonly IWebHostEnvironment environment;
	//Proporciona información sobre el entorno de alojamiento web (como rutas)
	private readonly IConfiguration configuration;

	public UsuarioController(IWebHostEnvironment environment, IConfiguration configuration)
	{

		this.environment = environment;
		this.configuration = configuration;
	}
	UsuarioRepositorio usuarioRepo = new UsuarioRepositorio();

	[Authorize]
	public IActionResult ListUsuario()
	{
		var lista = usuarioRepo.OptenerUsuarios();
		return View("ListaUsuario", lista);
	}




	[Authorize(Policy = "Administrador")]
	public IActionResult CrearUsuario(int id)
	{
		if (id == 0)
		{
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View();
		}
		else
		{
			var usuario = usuarioRepo.UsuariosPorId(id);
			ViewBag.Roles = Usuario.ObtenerRoles();
			return View("CrearUsuario", usuario);
		}
	}
	[Authorize(Policy = "Administrador")]
	public IActionResult EditarUsuario(int id)
	{

		var usuario = usuarioRepo.UsuariosPorId(id);
		ViewBag.Roles = Usuario.ObtenerRoles();
		return View("EditarUsuario", usuario);
	}
	[Authorize]
	[HttpPost]
	public IActionResult Guardar(Usuario usuario)
	{

		int id = usuario.Id_Usuario;
		int dni = usuario.Dni;

		if (id == 0)
		{
			usuarioRepo.AltaUsuario(usuario);  // Crear nuevo Usuario
			TempData["Advertencia"] = " se dio de alta nuevo Usuario .";

		}
		else
		{
			usuarioRepo.ModificarUsuarioSoloDatos(usuario);  // Editar Usuario existente
			TempData["Advertencia"] = " se edito el Usuario .";
		}


		return RedirectToAction(nameof(ListUsuario));

	}
	[Authorize]
	[HttpPost]
	public IActionResult CreateUsuario(Usuario usuario)
	{

		string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: usuario.Password,
						salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));

		usuario.Password = hashed;
		var nbreRnd = Guid.NewGuid();//posible nombre aleatorio
		int res = usuarioRepo.AltaUsuario(usuario);

		if (usuario.AvatarFile != null && usuario.Id_Usuario > 0)
		{
			string wwwPath = environment.WebRootPath;
			string path = Path.Combine(wwwPath, "Uploads");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			//Path.GetFileName(u.AvatarFile.FileName);//este nombre se puede repetir
			string fileName = "avatar_" + usuario.Id_Usuario + Path.GetExtension(usuario.AvatarFile.FileName);
			string pathCompleto = Path.Combine(path, fileName);
			usuario.Avatar = Path.Combine("/Uploads", fileName);
			// Esta operación guarda la foto en memoria en la ruta que necesitamos
			using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
			{
				usuario.AvatarFile.CopyTo(stream);
			}
			usuarioRepo.ModificarUsuario(usuario);
		}
		return RedirectToAction(nameof(ListUsuario));

	}



}



















