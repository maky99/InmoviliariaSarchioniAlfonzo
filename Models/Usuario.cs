


using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
namespace InmoviliariaSarchioniAlfonzo.Models;

public enum enRoles
{
    Administrador = 1,
    Empleado = 2,
}


public class Usuario
{

    public int Id_Usuario { get; set; }


    public string? Apellido { get; set; }


    public string? Nombre { get; set; }


    public int Dni { get; set; }


    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Avatar { get; set; }

    [NotMapped]
    public IFormFile? AvatarFile { get; set; }

    public int Estado_Usuario { get; set; }
    public int Rol { get; set; }
    [NotMapped]//Para EF
    public string RolNombre => Rol > 0 ? ((enRoles)Rol).ToString() : "";

    public static IDictionary<int, string> ObtenerRoles()
    {
        SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
        Type tipoEnumRol = typeof(enRoles);
        foreach (var valor in Enum.GetValues(tipoEnumRol))
        {
            roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
        }
        return roles;
    }
    public override string ToString()
    {
        return $"{Apellido},{Nombre}-({Id_Usuario})";
    }


}
