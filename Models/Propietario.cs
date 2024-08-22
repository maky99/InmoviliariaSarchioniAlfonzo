namespace InmoviliariaSarchioniAlfonzo.Models;

public class Propietario
{
    public int Id_Propietario { get; set; }

    public int Dni { get; set; }

    public string? Apellido { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }
    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public int Estado_Propietario { get; set; }

}