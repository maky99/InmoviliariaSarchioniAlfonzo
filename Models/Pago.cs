namespace InmoviliariaSarchioniAlfonzo.Models;
using System.ComponentModel.DataAnnotations;


public class Pago
{
   [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Id_Pago { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Id_Contrato { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public double Importe { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int CuotaPaga { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int MesesPagos { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public DateTime Fecha { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]

    public double Multa { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Id_Creado_Por { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Id_Terminado_Por { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public int Estado_Pago { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public Inquilino? inquilino { get; set; }
     [Required(ErrorMessage = "El campo es obligatorio.")]
    public Contrato? contrato { get; set; }

}

