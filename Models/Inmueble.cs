namespace InmoviliariaSarchioniAlfonzo.Models;
using System.ComponentModel.DataAnnotations;


public class Inmueble
{
    public int Id_Inmueble { get; set; }
    public int Id_Propietario { get; set; }
    public string? Direccion { get; set; }
    public string? Uso { get; set; }
    public int Ambientes { get; set; }
    public string? Latitud { get; set; }
    public string? Longitud { get; set; }
    public double Tamano { get; set; }
    public int Id_Tipo_Inmueble { get; set; }
    public string? Servicios { get; set; }
    public int Bano { get; set; }
    public int Cochera { get; set; }
    public int Patio { get; set; }
    public double Precio { get; set; }
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

