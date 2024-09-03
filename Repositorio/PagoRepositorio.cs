using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;
using System.Collections.Generic;

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
                    Pago.{nameof(Pago.Mes)},
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
                    Inquilino ON Contrato.{nameof(Contrato.Id_Inquilino)} = Inquilino.{nameof(Inquilino.Id_Inquilino)}";

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

    public IList<Contrato> ContratoVigente()
    {
        List<Contrato> Contratos = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"
            SELECT 
            Contrato.{nameof(Contrato.Id_Contrato)},
            Contrato.{nameof(Contrato.Id_Inmueble)} AS Id_Inmueble_Contrato,
            Inmueble.{nameof(Inmueble.Uso)} AS Uso,
            Inmueble.{nameof(Inmueble.Direccion)} AS Direccion,
            Tipo_Inmueble.{nameof(Tipo_Inmueble.Tipo)} AS Tipo,
            Contrato.{nameof(Contrato.Id_Propietario)},
            Propietario.{nameof(Propietario.Nombre)} AS Nombre,
            Propietario.{nameof(Propietario.Apellido)} AS Apellido,
            Inquilino.{nameof(Inquilino.Nombre)} AS NombreI,
            Inquilino.{nameof(Inquilino.Apellido)} AS ApellidoI,
            Inquilino.{nameof(Inquilino.Dni)} AS DniI,
            Contrato.{nameof(Contrato.Id_Inquilino)},
            Contrato.{nameof(Contrato.Fecha_Inicio)}, 
            Contrato.{nameof(Contrato.Fecha_Finalizacion)},
            Contrato.{nameof(Contrato.Monto)},
            Contrato.{nameof(Contrato.Finalizacion_Anticipada)},
            Contrato.{nameof(Contrato.Id_Creado_Por)},
            Contrato.{nameof(Contrato.Id_Terminado_Por)}, 
            Contrato.{nameof(Contrato.Estado_Contrato)}
        FROM Contrato 
        JOIN Inmueble ON Contrato.{nameof(Contrato.Id_Inmueble)} = Inmueble.{nameof(Inmueble.Id_Inmueble)}
        LEFT JOIN Tipo_Inmueble ON Inmueble.{nameof(Inmueble.Id_Tipo_Inmueble)} = Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}
        JOIN Propietario ON Contrato.{nameof(Contrato.Id_Propietario)} = Propietario.{nameof(Propietario.Id_Propietario)}
        JOIN Inquilino ON Contrato.{nameof(Contrato.Id_Inquilino)} = Inquilino.{nameof(Inquilino.Id_Inquilino)}
        WHERE Contrato.{nameof(Contrato.Estado_Contrato)}=1
        ORDER BY Contrato.{nameof(Contrato.Estado_Contrato)} DESC";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Contratos.Add(new Contrato
                        {
                            Id_Contrato = reader.GetInt32("Id_Contrato"),
                            Id_Inmueble = reader.GetInt32("Id_Inmueble_Contrato"), // Usando el alias especificado
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Id_Inquilino = reader.GetInt32("Id_Inquilino"),
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Fecha_Finalizacion = reader.GetDateTime("Fecha_Finalizacion"),
                            Monto = reader.GetDouble("Monto"),
                            Finalizacion_Anticipada = !reader.IsDBNull(reader.GetOrdinal("Finalizacion_Anticipada")) ? reader.GetDateTime("Finalizacion_Anticipada") : default(DateTime),
                            Id_Creado_Por = reader.GetInt32("Id_Creado_Por"),
                            Id_Terminado_Por = reader.IsDBNull(reader.GetOrdinal("Id_Terminado_Por")) ? default(int) : reader.GetInt32("Id_Terminado_Por"),
                            Estado_Contrato = reader.GetInt32("Estado_Contrato"),
                            inmueble = new Inmueble
                            {
                                Uso = reader.GetString("Uso"),
                                Direccion = reader.GetString("Direccion"),
                                // Id_Tipo_Inmueble no estaba en el SELECT. Si lo necesitas, inclúyelo en la consulta SQL.
                            },
                            tipo_inmueble = new Tipo_Inmueble
                            {
                                Tipo = reader.GetString("Tipo")
                            },
                            propietario = new Propietario
                            {
                                Nombre = reader.GetString("Nombre"),
                                Apellido = reader.GetString("Apellido")
                            },
                            inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("NombreI"),
                                Apellido = reader.GetString("ApellidoI"),
                                Dni = reader.GetInt32("DniI")

                            }
                        });
                    }
                    connection.Close();
                }
            }
        }
        return Contratos;
    }

    public Contrato ContratoAPagar(int id)
    {
        Contrato contrato = new Contrato();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT 
            Contrato.{nameof(Contrato.Id_Contrato)},
            Contrato.{nameof(Contrato.Id_Inmueble)},
            Contrato.{nameof(Contrato.Id_Propietario)},
            Contrato.{nameof(Contrato.Id_Inquilino)} AS Contrato_Id_Inquilino,
            Contrato.{nameof(Contrato.Fecha_Inicio)},
            Contrato.{nameof(Contrato.Meses)},
            Contrato.{nameof(Contrato.Fecha_Finalizacion)},
            Contrato.{nameof(Contrato.Monto)},
            Contrato.{nameof(Contrato.Finalizacion_Anticipada)},
            Contrato.{nameof(Contrato.Estado_Contrato)},
            Inquilino.{nameof(Inquilino.Id_Inquilino)},
            Inquilino.{nameof(Inquilino.Dni)},
            Inquilino.{nameof(Inquilino.Apellido)},
            Inquilino.{nameof(Inquilino.Nombre)},
            Inquilino.{nameof(Inquilino.Telefono)},
            Inquilino.{nameof(Inquilino.Email)},
            Inquilino.{nameof(Inquilino.Estado_Inquilino)}
        FROM 
            Contrato
        JOIN 
            Inquilino ON Inquilino.{nameof(Inquilino.Id_Inquilino)} = Contrato.{nameof(Contrato.Id_Inquilino)}
        WHERE 
            Contrato.{nameof(Contrato.Id_Contrato)} = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id_Contrato = reader.GetInt32("Id_Contrato"), // Corregido aquí
                            Id_Inmueble = reader.GetInt32("Id_Inmueble"),
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Id_Inquilino = reader.GetInt32("Contrato_Id_Inquilino"), // Este se mantiene igual
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Meses = reader.GetInt32("Meses"),
                            Fecha_Finalizacion = reader.GetDateTime("Fecha_Finalizacion"),
                            Monto = reader.GetDouble("Monto"),
                            Finalizacion_Anticipada = reader.GetDateTime("Finalizacion_Anticipada"),
                            Estado_Contrato = reader.GetInt32("Estado_Contrato"),
                            inquilino = new Inquilino
                            {
                                Id_Inquilino = reader.GetInt32("Id_Inquilino"),
                                Dni = reader.GetInt32("Dni"),
                                Apellido = reader.GetString("Apellido"),
                                Nombre = reader.GetString("Nombre"),
                                Telefono = reader.GetString("Telefono"),
                                Email = reader.GetString("Email"),
                                Estado_Inquilino = reader.GetInt32("Estado_Inquilino")
                            }
                        };
                    }
                    connection.Close();
                }
            }
        }

        return contrato;
    }


}







