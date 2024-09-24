
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
		// Obtener el rol del usuario desde las claims
		var roleClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

		// Si el rol es "Empleado", redirigir al Index del HomeController
		if (roleClaim == "Empleado")
		{
			return RedirectToAction(nameof(HomeController.Index), "Home");
		}

		// Si es "Administrador", mostrar la lista de usuarios
		if (roleClaim == "Administrador")
		{
			var lista = usuarioRepo.OptenerUsuarios();
			return View("ListaUsuario", lista);
		}

		// En caso de roles no definidos, redirigir al Index por defecto
		return RedirectToAction(nameof(HomeController.Index), "Home");
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



		if (usuarioRepo.EsDniDelUsuarioActual(usuario.Id_Usuario, usuario.Dni))
		{
			TempData["error"] = " dni ya existe no puede introducirlo";
			return RedirectToAction("EditarUsuario", new { id = usuario.Id_Usuario });
		}
		else if (usuarioRepo.EsEmailDelUsuarioActual(usuario.Id_Usuario, usuario.Email))
		{
			TempData["error"] = " Email ya existe no puede introducirlo";
			return RedirectToAction("EditarUsuario", new { id = usuario.Id_Usuario });

		}
		else
		{

			usuarioRepo.ModificarUsuarioSoloDatos(usuario);  // Editar Usuario existente
			TempData["Advertencia"] = " se edito el Usuario .";

		}

		return RedirectToAction("EditarUsuario", new { id = usuario.Id_Usuario });

	}
	[Authorize]
	[HttpPost]
	public IActionResult CreateUsuario(Usuario usuario)
	{

		if (usuarioRepo.DniUsuarioyaExiste(usuario.Dni))
		{

			TempData["error"] = " dni ya existe no puede introducirlo";
			return RedirectToAction(nameof(CrearUsuario));

		}
		else if (usuarioRepo.EmailUsuarioyaExiste(usuario.Email))
		{
			TempData["error"] = " Email ya existe no puede introducirlo";
			return RedirectToAction(nameof(CrearUsuario));
		}
		else
		{

			// Generar el hash de la contraseña
			string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: usuario.Password,
							salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));

			usuario.Password = hashed;

			// Insertar usuario en la base de datos
			int res = usuarioRepo.AltaUsuario(usuario);

			// Preparar ruta de carga
			string wwwPath = environment.WebRootPath;
			string path = Path.Combine(wwwPath, "Uploads");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			if (usuario.AvatarFile != null && usuario.Id_Usuario > 0)
			{
				// Si se ha cargado un avatar, guardar el archivo
				string fileName = "avatar_" + usuario.Id_Usuario + Path.GetExtension(usuario.AvatarFile.FileName);
				string pathCompleto = Path.Combine(path, fileName);
				usuario.Avatar = Path.Combine("/Uploads", fileName);

				// Guardar el archivo en la carpeta Uploads
				using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
				{
					usuario.AvatarFile.CopyTo(stream);
				}
			}
			else
			{
				// Si no se carga un avatar, asignar el avatar predeterminado "avatar_0"
				usuario.Avatar = Path.Combine("/Uploads", "avatar_0.JPG");
			}

			// Actualizar el usuario con la ruta del avatar
			usuarioRepo.ModificarUsuario(usuario);

			return RedirectToAction(nameof(ListUsuario));
		}
	}



	[HttpGet]
	[Authorize]
	public ActionResult CambioAvatar(int id)
	{
		var usuario = usuarioRepo.UsuariosPorId(id);
		// Obtener el ID del usuario autenticado
		var usuarioId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.PrimarySid));
		// Verificar si el usuario autenticado coincide con el ID del perfil solicitado
		if (usuarioId == id)
		{

			RedirectToAction(nameof(ListUsuario));
		}

		return View(usuarioRepo.UsuariosPorId(id));
	}


	[HttpPost]
	[Authorize]
	public ActionResult cambioAvatarPost(Usuario usuario)
	{

		// Obtener el ID del usuario autenticado
		var usuarioId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.PrimarySid));

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

		}
		else
		{
			// Si no se carga un avatar, asignar el avatar predeterminado "avatar_0"
			usuario.Avatar = Path.Combine("/Uploads", "avatar.png");
		}
		usuarioRepo.ModificarAvatarUsuario(usuario);
		return RedirectToAction(nameof(ListUsuario));

	}

	[Authorize]
	public ActionResult CambioPassword()
	{
		//audit
		return View();
	}

	[HttpPost]
	[Authorize]
	public ActionResult CambioPassword(string PasswordAnterior, string PasswordNueva)
	{
		var IdClaim = (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.PrimarySid).Value);
		UsuarioRepositorio repoUsu = new UsuarioRepositorio();
		var Mensaje = "";

		string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: PasswordAnterior,
							salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));

		PasswordAnterior = hashed;

		var resultado = repoUsu.esIgualPassword(Convert.ToInt32(IdClaim), hashed);
		if (resultado == 1)
		{

			string PasswordNuevaHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: PasswordNueva,
								salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));

			PasswordNueva = PasswordNuevaHash;


			repoUsu.updateClave(Convert.ToInt32(IdClaim), PasswordNuevaHash);

			Mensaje = "EL CAMBIO SE REALIZÓ CORRECTAMENTE";
		}
		else
		{
			Mensaje = "PASSWORD INCORRECTA, INTENTE DE NUEVO";
		}
		ViewBag.Mensaje = Mensaje;
		return View();


	}

	[Authorize(Policy = "Administrador")]
	public IActionResult Baja(int id)
	{

		UsuarioRepositorio usRe = new UsuarioRepositorio();

		usRe.Baja(id);
		return RedirectToAction(nameof(ListUsuario));
	}
	[Authorize]
	public ActionResult ModificaPerfil(Usuario usuario)
	{
		// Obtener la identidad del usuario autenticado
		var claimsIdentity = User.Identity as ClaimsIdentity;

		// Obtener el ID y el rol del usuario autenticado
		var claimId = claimsIdentity?.FindFirst(ClaimTypes.PrimarySid)?.Value; // Usamos el mismo ClaimType que antes
		var claimRole = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value; // Obtener el rol del usuario autenticado

		// Convertir el claimId a entero para compararlo con el Id_Usuario
		if (int.TryParse(claimId, out int id_usuario))
		{
			// Si el usuario es empleado, verificar que esté modificando solo su propio perfil
			if (claimRole == "Empleado" && id_usuario != usuario.Id_Usuario)
			{
				// Si los IDs no coinciden, devolver un error 403 (Prohibido)
				return new ForbidResult();
			}

			// Si es administrador o el usuario autenticado está modificando su propio perfil, permitir la modificación
			UsuarioRepositorio usuRepo = new UsuarioRepositorio();
			usuRepo.ModificarPerfil(usuario);
			TempData["Advertencia"] = " se edito el perfil .";
			return RedirectToAction("Perfil", new { id = usuario.Id_Usuario });

		}
		else
		{
			// Si no se puede convertir el claimId, devolver un error 400 (Bad Request)
			return BadRequest("ID de usuario no válido");
		}
	}

	[HttpGet]
	[Authorize]
	public ActionResult Perfil(int id)
	{
		// Obtener la identidad del usuario autenticado
		var claimsIdentity = User.Identity as ClaimsIdentity;

		// Obtener el valor del claim PrimarySid (que usas en la vista)
		var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.PrimarySid)?.Value;
		var claimRole = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value; // Obtener el rol del usuario autenticado

		// Convertir el claim PrimarySid (userIdClaim) a int
		if (int.TryParse(userIdClaim, out int id_usuario))
		{
			// Verificar si el usuario es empleado y está intentando acceder a un perfil distinto del suyo
			if (claimRole == "Empleado" && id_usuario != id)
			{
				// Si es un empleado e intenta acceder a un perfil que no es el suyo, devolver un error 403 (Prohibido)
				return new ForbidResult();
			}

			// Si es administrador o el usuario autenticado está accediendo a su propio perfil, cargar la vista
			ViewBag.Roles = Usuario.ObtenerRoles();

			// Mostrar el perfil del usuario solicitado por el ID
			return View(usuarioRepo.UsuariosPorId(id));
		}
		else
		{
			// Si no se puede convertir el claimId, devolver un error 400 (Bad Request)
			return BadRequest("ID de usuario no válido");
		}
	}

	[Authorize(Policy = "Administrador")]
	public IActionResult ResetearContrasena(int id)
	{
		// Obtener el usuario por ID
		var usuario = usuarioRepo.UsuariosPorId(id);
		if (usuario == null)
		{
			TempData["ErrorMessage"] = "Usuario no encontrado.";
			return RedirectToAction(nameof(ListUsuario));
		}

		// Establecer nueva contraseña
		string nuevaContrasena = "pepe";
		string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
			password: nuevaContrasena,
			salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
			prf: KeyDerivationPrf.HMACSHA1,
			iterationCount: 1000,
			numBytesRequested: 256 / 8));

		// Actualizar la contraseña en el repositorio
		usuarioRepo.updateClave(id, hashedPassword);

		TempData["SuccessMessage"] = "Contraseña restablecida correctamente.";
		return RedirectToAction(nameof(ListUsuario));
	}


}



















