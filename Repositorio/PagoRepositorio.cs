using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;
using System.Collections.Generic;
using System.Dynamic;

public class PagoRepositorio
{
    private readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    // lista todos los pagos que hay
    public IList<Pago> ObtenerPagos()
    {
        List<Pago> pagos = new List<Pago>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT 
                    Pago.{nameof(Pago.Id_Pago)},
                    Pago.{nameof(Pago.Id_Contrato)},
                    Pago.{nameof(Pago.Importe)},
                    Pago.{nameof(Pago.CuotaPaga)},
                    Pago.{nameof(Pago.Fecha)},
                    Pago.{nameof(Pago.Multa)},
                    Pago.{nameof(Pago.Id_Creado_Por)},
                    Pago.{nameof(Pago.Id_Terminado_Por)},
                    Pago.{nameof(Pago.Estado_Pago)},
                    Inquilino.{nameof(Inquilino.Id_Inquilino)},
                    Inquilino.{nameof(Inquilino.Dni)},
                    Inquilino.{nameof(Inquilino.Apellido)},
                    Inquilino.{nameof(Inquilino.Nombre)},
                    Inquilino.{nameof(Inquilino.Telefono)},
                    Inquilino.{nameof(Inquilino.Email)},
                    Inquilino.{nameof(Inquilino.Estado_Inquilino)},
                    Contrato.{nameof(Contrato.Id_Contrato)} AS Contrato_Id_Contrato,
                    Contrato.{nameof(Contrato.Id_Inmueble)},
                    Contrato.{nameof(Contrato.Id_Propietario)},
                    Contrato.{nameof(Contrato.Id_Inquilino)} AS Contrato_Id_Inquilino,
                    Contrato.{nameof(Contrato.Fecha_Inicio)},
                    Contrato.{nameof(Contrato.Meses)},
                    Contrato.{nameof(Contrato.Fecha_Finalizacion)},
                    Contrato.{nameof(Contrato.Monto)},
                    Contrato.{nameof(Contrato.Finalizacion_Anticipada)},
                    Contrato.{nameof(Contrato.Estado_Contrato)}
                FROM 
                    Pago
                JOIN 
                    Contrato ON Pago.{nameof(Pago.Id_Contrato)} = Contrato.{nameof(Contrato.Id_Contrato)}
                JOIN 
                    Inquilino ON Contrato.{nameof(Contrato.Id_Inquilino)} = Inquilino.{nameof(Inquilino.Id_Inquilino)}
                ORDER BY 
                    Pago.{nameof(Pago.Id_Contrato)}, 
                    Pago.{nameof(Pago.CuotaPaga)}, 
                    Inquilino.{nameof(Inquilino.Apellido)}, 
                    Inquilino.{nameof(Inquilino.Nombre)}";

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
                            CuotaPaga = reader.GetInt32("CuotaPaga"),
                            Fecha = reader.GetDateTime("Fecha"),
                            Multa = reader.GetDouble("Multa"),
                            Id_Creado_Por = reader.GetInt32("Id_Creado_Por"),
                            Id_Terminado_Por = reader.GetInt32("Id_Terminado_Por"),
                            Estado_Pago = reader.GetInt32("Estado_Pago"),
                            inquilino = new Inquilino
                            {
                                Id_Inquilino = reader.GetInt32("Id_Inquilino"),
                                Dni = reader.GetInt32("Dni"),
                                Apellido = reader.GetString("Apellido"),
                                Nombre = reader.GetString("Nombre"),
                                Telefono = reader.GetString("Telefono"),
                                Email = reader.GetString("Email"),
                                Estado_Inquilino = reader.GetInt32("Estado_Inquilino")
                            },
                            contrato = new Contrato
                            {
                                Id_Contrato = reader.GetInt32("Contrato_Id_Contrato"),
                                Id_Inmueble = reader.GetInt32("Id_Inmueble"),
                                Id_Propietario = reader.GetInt32("Id_Propietario"),
                                Id_Inquilino = reader.GetInt32("Contrato_Id_Inquilino"),
                                Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                                Meses = reader.GetInt32("Meses"),
                                Fecha_Finalizacion = reader.GetDateTime("Fecha_Finalizacion"),
                                Monto = reader.GetDouble("Monto"),
                                Finalizacion_Anticipada = reader.GetDateTime("Finalizacion_Anticipada"),
                                Estado_Contrato = reader.GetInt32("Estado_Contrato")
                            }
                        });
                    }
                    connection.Close();
                }
            }
        }
        return pagos;
    }

    //listar pagos de un contrato especifico 
    public IList<Pago> ObtenerPagosPorContrato(int idContrato)
    {
        List<Pago> pagos = new List<Pago>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"
        SELECT 
            Pago.{nameof(Pago.Id_Pago)},
            Pago.{nameof(Pago.Id_Contrato)},
            Pago.{nameof(Pago.Importe)},
            Pago.{nameof(Pago.CuotaPaga)},
            Pago.{nameof(Pago.Fecha)},
            Pago.{nameof(Pago.Multa)},
            Pago.{nameof(Pago.Id_Creado_Por)},
            Pago.{nameof(Pago.Id_Terminado_Por)},
            Pago.{nameof(Pago.Estado_Pago)}
        FROM Pago
        WHERE Pago.{nameof(Pago.Id_Contrato)} = @IdContrato";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@IdContrato", idContrato);
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
                            CuotaPaga = reader.GetInt32("CuotaPaga"),
                            Fecha = reader.GetDateTime("Fecha"),
                            Multa = reader.GetDouble("Multa"),
                            Id_Creado_Por = reader.GetInt32("Id_Creado_Por"),
                            Id_Terminado_Por = reader.GetInt32("Id_Terminado_Por"),
                            Estado_Pago = reader.GetInt32("Estado_Pago")
                        });
                    }
                }
            }
        }
        return pagos;
    }





    // //metodo para guardar pago 

    public void GuardarPago(Pago pago)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $@"INSERT INTO pago ({nameof(Pago.Id_Contrato)},{nameof(Pago.Importe)},{nameof(Pago.CuotaPaga)},{nameof(Pago.Fecha)},{nameof(Pago.Multa)},{nameof(Pago.Id_Creado_Por)},{nameof(Pago.Id_Terminado_Por)},{nameof(Pago.Estado_Pago)})
        VALUES
        (@{nameof(Pago.Id_Contrato)},@{nameof(Pago.Importe)},@{nameof(Pago.CuotaPaga)},@{nameof(Pago.Fecha)},@{nameof(Pago.Multa)},@{nameof(Pago.Id_Creado_Por)},@{nameof(Pago.Id_Terminado_Por)},@{nameof(Pago.Estado_Pago)})";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Pago.Id_Contrato)}", pago.Id_Contrato);
                command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
                command.Parameters.AddWithValue($"@{nameof(Pago.CuotaPaga)}", pago.CuotaPaga);  // Aquí se usa la cuota seleccionada
                command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
                command.Parameters.AddWithValue($"@{nameof(Pago.Multa)}", pago.Multa);
                command.Parameters.AddWithValue($"@{nameof(Pago.Id_Creado_Por)}", pago.Id_Creado_Por);
                command.Parameters.AddWithValue($"@{nameof(Pago.Id_Terminado_Por)}", pago.Id_Terminado_Por);
                command.Parameters.AddWithValue($"@{nameof(Pago.Estado_Pago)}", pago.Estado_Pago);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    // public void GuardarPago(Pago pago)
    // {

    //     using (var connection = new MySqlConnection(connectionString))
    //     {
    //         var sql = $@"INSERT INTO pago ({nameof(Pago.Id_Contrato)},{nameof(Pago.Importe)},{nameof(Pago.Mes)},{nameof(Pago.Fecha)},{nameof(Pago.Multa)},{nameof(Pago.Id_Creado_Por)},{nameof(Pago.Id_Terminado_Por)},{nameof(Pago.Estado_Pago)})
    //         VALUES
    //         (@{nameof(Pago.Id_Contrato)},@{nameof(Pago.Importe)},@{nameof(Pago.Mes)},@{nameof(Pago.Fecha)},@{nameof(Pago.Multa)},@{nameof(Pago.Id_Creado_Por)},@{nameof(Pago.Id_Terminado_Por)},@{nameof(Pago.Estado_Pago)})";
    //         using (var command = new MySqlCommand(sql, connection))
    //         {
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Id_Contrato)}", pago.Id_Contrato);
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Importe)}", pago.Importe);
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Mes)}", pago.Mes);
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Fecha)}", pago.Fecha);
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Multa)}", pago.Multa);
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Id_Creado_Por)}", pago.Id_Creado_Por);
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Id_Terminado_Por)}", pago.Id_Terminado_Por);
    //             command.Parameters.AddWithValue($"@{nameof(Pago.Estado_Pago)}", pago.Estado_Pago);
    //             connection.Open();
    //             command.ExecuteNonQuery();
    //             connection.Close();
    //         }

    //     }
    // }
}






