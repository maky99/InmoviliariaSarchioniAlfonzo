
using System.ComponentModel.DataAnnotations;
namespace InmoviliariaSarchioniAlfonzo.Models;

public class Contrato
{
   public int Id_Contrato { get; set; }
   public int Id_Inmueble { get; set; }
   public int Id_Propietario { get; set; }
   public int Id_Inquilino { get; set; }
   public DateTime Fecha_Inicio { get; set; }
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
   

}
