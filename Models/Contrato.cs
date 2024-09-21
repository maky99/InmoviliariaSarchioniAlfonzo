
using System.ComponentModel.DataAnnotations;
namespace InmoviliariaSarchioniAlfonzo.Models;

public class Contrato
{
   public int Id_Contrato { get; set; }
   public int Id_Inmueble { get; set; }
   public int Id_Propietario { get; set; }
   public int Id_Inquilino { get; set; }
   [Required(ErrorMessage = "El campo Fecha de Inicio es obligatorio.")]
   public DateTime Fecha_Inicio { get; set; }
   [Required(ErrorMessage = "El campo Mes es obligatorio.")]
   [RegularExpression(@"^\d+$", ErrorMessage = "El campo debe contener solo números.")]

   public int Meses { get; set; }
   public DateTime Fecha_Finalizacion { get; set; }



   public double Monto { get; set; }
   public DateTime Finalizacion_Anticipada { get; set; }
   public int Id_Creado_Por { get; set; }
   public int Id_Terminado_Por { get; set; }
   public int Estado_Contrato { get; set; }
   public Inmueble? inmueble { get; set; }
   public Tipo_Inmueble? tipo_inmueble { get; set; }
   public Propietario? propietario { get; set; }
   public Inquilino? inquilino { get; set; }
   public int MesesPagos { get; set; }
   public Usuario? usuario { get; set; }



}
