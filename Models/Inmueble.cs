namespace InmoviliariaSarchioniAlfonzo.Models;
using System.ComponentModel.DataAnnotations;


public class Inmueble
{
    public int Id_Inmueble { get; set; }
    public int Id_Propietario { get; set; }
    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string? Direccion { get; set; }
    [Required(ErrorMessage = "El uso es obligatorio.")]
    public string? Uso { get; set; }
    public int Ambientes { get; set; }
    public string? Latitud { get; set; }
    public string? Longitud { get; set; }
    [Required(ErrorMessage = "El tamaño es obligatorio.")]
    public double Tamano { get; set; }
    public int Id_Tipo_Inmueble { get; set; }
    [Required(ErrorMessage = "Los servicios son obligatorios.")]
    public string? Servicios { get; set; }
    [Required(ErrorMessage = "La cantidad de baños es obligatoria.")]
    public int Bano { get; set; }
    [Required(ErrorMessage = "La cochera es obligatoria.")]
    public int Cochera { get; set; }
    [Required(ErrorMessage = "El patio es obligatorio.")]
    public int Patio { get; set; }
    public double Precio { get; set; }
    [Required(ErrorMessage = "La condición es obligatoria.")]
    public string? Condicion { get; set; }
    public int Estado_Inmueble { get; set; }
    public Tipo_Inmueble? tipo { get; set; }
    public Propietario? propietario { get; set; }
    public int Mes { get; set; }
    public double PrecioMin { get; set; }
    public double PrecioMax { get; set; }
    public DateTime FechaInicioAlquiler { get; set; }
    public DateTime FechaFinAlquiler { get; set; }


}

