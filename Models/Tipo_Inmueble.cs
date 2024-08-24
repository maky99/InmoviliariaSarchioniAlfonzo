namespace InmoviliariaSarchioniAlfonzo.Models;
using System.ComponentModel.DataAnnotations;


public class Tipo_Inmueble
{
    public int Id_Tipo_Inmueble { get; set; }
    [Required(ErrorMessage = "El Tipo es obligatorio.")]
    public string? Tipo { get; set; }
    public int Estado_Tipo_Inmueble { get; set; }
}