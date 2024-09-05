namespace InmoviliariaSarchioniAlfonzo.Models;
using System.ComponentModel.DataAnnotations;


public class Pago
{
    public int Id_Pago { get; set; }
    public int Id_Contrato { get; set; }
    public double Importe { get; set; }
    public int CuotaPaga { get; set; }
    public int MesesPagos { get; set; }
    public DateTime Fecha { get; set; }
    public double Multa { get; set; }
    public int Id_Creado_Por { get; set; }
    public int Id_Terminado_Por { get; set; }
    public int Estado_Pago { get; set; }
    public Inquilino? inquilino { get; set; }
    public Contrato? contrato { get; set; }

}

