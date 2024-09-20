namespace InmoviliariaSarchioniAlfonzo.Models;
using System.ComponentModel.DataAnnotations;


public class Inmueble
{
    [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Id_Inmueble { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Id_Propietario { get; set; }
    [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Direccion { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Uso { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Ambientes { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Latitud { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Longitud { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public double Tamano { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Id_Tipo_Inmueble { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Servicios { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Bano { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Cochera { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Patio { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public double Precio { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public string? Condicion { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Estado_Inmueble { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public Tipo_Inmueble? tipo { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]

    public Propietario? propietario { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Mes { get; set; }
    public double PrecioMin { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public double PrecioMax { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public DateTime FechaInicioAlquiler { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public DateTime FechaFinAlquiler { get; set; }
     

}

