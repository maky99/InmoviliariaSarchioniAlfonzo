
using System.ComponentModel.DataAnnotations;
namespace InmoviliariaSarchioniAlfonzo.Models;

public class Contrato
{
   public int Id_Contrato { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public int Id_Inmueble { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public int Id_Propietario { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public int Id_Inquilino { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public DateTime Fecha_Inicio { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   [RegularExpression(@"^\d+$", ErrorMessage = "El campo debe contener solo números.")]

   public int Meses { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public DateTime Fecha_Finalizacion { get; set; }


   [Required(ErrorMessage = "El campo es obligatorio.")]
   [RegularExpression(@"^\d+$", ErrorMessage = "El campo debe contener solo números.")]

   public double Monto { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public DateTime Finalizacion_Anticipada { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public int Id_Creado_Por { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public int Id_Terminado_Por { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public int Estado_Contrato { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public Inmueble? inmueble { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public Tipo_Inmueble? tipo_inmueble { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public Propietario? propietario { get; set; }
   [Required(ErrorMessage = "El campo es obligatorio.")]
   public Inquilino? inquilino { get; set; }
   public int MesesPagos { get; set; }
   public Usuario? usuario { get; set; }



}
