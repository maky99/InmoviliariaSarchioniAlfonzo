using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;
using System.Collections.Generic;

public class PagoRepositorio
{
    private readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    // lista todos los pagos que hay

    // public IList<Pago> ObtenerPagos()
    // {
    //     List<Pago> pagos = new List<Pago>();

    //     using (var connection = new MySqlConnection(connectionString))
    //     {
    //         var sql = @$"SELECT 
    //                     {nameof(Pago.Id_Pago)},
    //                     {nameof(Pago.Id_Contrato)},
    //                     {nameof(Pago.Importe)},
    //                     {nameof(Pago.Mes)},
    //                     {nameof(Pago.Fecha)},
    //                     {nameof(Pago.Multa)},
    //                     {nameof(Pago.Id_Creado_Por)},
    //                     {nameof(Pago.Id_Terminado_Por)},
    //                     {nameof(Pago.Estado_Pago)},
    //                     Inquilino.{nameof(Inquilino.Id_Inquilino)},
    //                     Inquilino.{nameof(Inquilino.Dni)},
    //                     Inquilino.{nameof(Inquilino.Apellido)},
    //                     Inquilino.{nameof(Inquilino.Nombre)},
    //                     Inquilino.{nameof(Inquilino.Telefono)},
    //                     Inquilino.{nameof(Inquilino.Email)},
    //                     Inquilino.{nameof(Inquilino.Estado_Inquilino)}
    //                 FROM 
    //                     Pago
    //                 JOIN 
    //                     Inquilino ON Pago.{nameof(Pago.Id_Terminado_Por)} = Inquilino.{nameof(Inquilino.Id_Inquilino)}";

    //         using (var command = new MySqlCommand(sql, connection))
    //         {
    //             connection.Open();
    //             using (var reader = command.ExecuteReader())
    //             {
    //                 while (reader.Read())
    //                 {
    //                     pagos.Add(new Pago
    //                     {
    //                         Id_Pago = reader.GetInt32("Id_Pago"),
    //                         Id_Contrato = reader.GetInt32("Id_Contrato"),
    //                         Importe = reader.GetDouble("Importe"),
    //                         Mes = reader.GetInt32("Mes"),
    //                         Fecha = reader.GetDateTime("Fecha"),
    //                         Multa = reader.GetDouble("Multa"),
    //                         Id_Creado_Por = reader.GetInt32("Id_Creado_Por"),
    //                         Id_Terminado_Por = reader.GetInt32("Id_Terminado_Por"),
    //                         Estado_Pago = reader.GetInt32("Estado_Pago"),
    //                         inquilino = new Inquilino
    //                         {
    //                             Id_Inquilino = reader.GetInt32("Id_Inquilino"),
    //                             Dni = reader.GetInt32("Dni"),
    //                             Apellido = reader.GetString("Apellido"),
    //                             Nombre = reader.GetString("Nombre"),
    //                             Telefono = reader.GetString("Telefono"),
    //                             Email = reader.GetString("Email"),
    //                             Estado_Inquilino = reader.GetInt32("Estado_Inquilino")
    //                         }
    //                     });

    //                 }
    //                 connection.Close();
    //             }
    //         }
    //     }
    //     return pagos;
    // }


    public IList<Pago> ObtenerPagos()
    {
        List<Pago> pagos = new List<Pago>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT 
                        {nameof(Pago.Id_Pago)},
                        {nameof(Pago.Id_Contrato)},
                        {nameof(Pago.Importe)},
                        {nameof(Pago.Mes)},
                        {nameof(Pago.Fecha)},
                        {nameof(Pago.Multa)},
                        {nameof(Pago.Id_Creado_Por)},
                        {nameof(Pago.Id_Terminado_Por)},
                        {nameof(Pago.Estado_Pago)}
                    FROM 
                        Pago";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pagos.Add(new Pago
                        {
                            Id_Pago = reader.GetInt32("Id_Pago"),
                            Id_Contrato = reader.GetInt32("Id_Contrato"),
                            Importe = reader.GetDouble("Importe"),
                            Mes = reader.GetInt32("Mes"),
                            Fecha = reader.GetDateTime("Fecha"),
                            Multa = reader.GetDouble("Multa"),
                            Id_Creado_Por = reader.GetInt32("Id_Creado_Por"),
                            Id_Terminado_Por = reader.IsDBNull(reader.GetOrdinal(nameof(Pago.Id_Terminado_Por))) ? default : reader.GetInt32(nameof(Pago.Id_Terminado_Por)),
                            Estado_Pago = reader.GetInt32("Estado_Pago"),

                        });

                    }
                    connection.Close();
                }
            }
        }
        return pagos;
    }



}







