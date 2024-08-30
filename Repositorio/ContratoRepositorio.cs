using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;


public class ContratoRepositorio


{

    readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    //metodo para listar todos los Contrato
    public IList<Contrato> ObtenerContratos()
    {
        List<Contrato> Contratos = new List<Contrato>();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"
    SELECT 
        Contrato.{nameof(Contrato.Id_Contrato)},
        Contrato.{nameof(Contrato.Id_Inmueble)} AS Id_Inmueble_Contrato, -- Especifica que es de Contrato
        Inmueble.{nameof(Inmueble.Uso)} AS Uso,
        Inmueble.{nameof(Inmueble.Direccion)} AS Direccion,
        Tipo_Inmueble.{nameof(Tipo_Inmueble.Tipo)} AS Tipo,
        Contrato.{nameof(Contrato.Id_Propietario)},
        Propietario.{nameof(Propietario.Nombre)} AS Nombre,
        Propietario.{nameof(Propietario.Apellido)} AS Apellido,
        Inquilino.{nameof(Inquilino.Nombre)} AS NombreI,
        Inquilino.{nameof(Inquilino.Apellido)} AS ApellidoI,
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
                                Apellido = reader.GetString("ApellidoI")
                            }
                        });


                    }
                    connection.Close();
                }

            }


        }
        return Contratos;
    }

    //metodo para listar todos los Contrato ACTIVOS 
    public Contrato ObtenerContratoActivo(int id)
    {
        var contrato = new Contrato();

        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT 
            Contrato.{nameof(Contrato.Id_Contrato)},
            Contrato.{nameof(Contrato.Id_Inmueble)} AS Id_Inmueble_Contrato, -- Especifica que es de Contrato
            Inmueble.{nameof(Inmueble.Uso)} AS Uso,
            Inmueble.{nameof(Inmueble.Direccion)} AS Direccion,
            Tipo_Inmueble.{nameof(Tipo_Inmueble.Tipo)} AS Tipo,
            Contrato.{nameof(Contrato.Id_Propietario)},
            Propietario.{nameof(Propietario.Nombre)} AS Nombre,
            Propietario.{nameof(Propietario.Apellido)} AS Apellido,
            Inquilino.{nameof(Inquilino.Nombre)} AS NombreI,
            Inquilino.{nameof(Inquilino.Apellido)} AS ApellidoI,
            Contrato.{nameof(Contrato.Id_Inquilino)},
            Contrato.{nameof(Contrato.Fecha_Inicio)}, 
            Contrato.{nameof(Contrato.Meses)}, 
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
        WHERE Contrato.{nameof(Contrato.Estado_Contrato)} = 1 AND Contrato.{nameof(Contrato.Id_Contrato)} = @Id
        ORDER BY Contrato.{nameof(Contrato.Estado_Contrato)} DESC";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id_Contrato = reader.GetInt32("Id_Contrato"),
                            Id_Inmueble = reader.GetInt32("Id_Inmueble_Contrato"), // Usando el alias especificado
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Id_Inquilino = reader.GetInt32("Id_Inquilino"),
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Meses = reader.GetInt32("Meses"),
                            Fecha_Finalizacion = reader.GetDateTime("Fecha_Finalizacion"),
                            Monto = reader.GetDouble("Monto"),
                            Finalizacion_Anticipada = reader.GetDateTime("Finalizacion_Anticipada"),
                            Id_Creado_Por = reader.GetInt32("Id_Creado_Por"),
                            Id_Terminado_Por = reader.GetInt32("Id_Terminado_Por"),
                            Estado_Contrato = reader.GetInt32("Estado_Contrato"),
                            inmueble = new Inmueble
                            {
                                Uso = reader.GetString("Uso"),
                                Direccion = reader.GetString("Direccion"),
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
                                Apellido = reader.GetString("ApellidoI")
                            }
                        };
                    }
                }
                connection.Close();
            }
        }

        return contrato;
    }





    //metodo para buscar Contrato antes de editar
    public Contrato BuscarContrato(int id)
    {
        var Contrato = new Contrato();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var sql = $"SELECT * FROM Contrato WHERE Id_Contrato = '{id}'";
            using (var comando = new MySqlCommand(sql, connection))
            {
                comando.Parameters.AddWithValue("@Id_Contrato", id);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Contrato = (new Contrato
                        {
                            Id_Contrato = reader.GetInt32("Id_Contrato"),
                            Id_Inmueble = reader.GetInt32("Id_Inmueble"),
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Id_Inquilino = reader.GetInt32("Id_Inquilino"),
                            Fecha_Inicio = reader.GetDateTime("Fecha_Inicio"),
                            Fecha_Finalizacion = reader.GetDateTime("Fecha_Finalizacion"),
                            Monto = reader.GetDouble("Monto"),
                            Finalizacion_Anticipada = reader.GetDateTime("Finalizacion_Anticipada"),
                            Id_Creado_Por = reader.GetInt32("Id_Creado_Por"),
                            Id_Terminado_Por = reader.GetInt32("Id_Terminado_Por"),
                            Estado_Contrato = reader.GetInt32("Estado_Contrato")
                        });
                    }
                }
            }

        }

        return Contrato;
    }


    //metodo para guardar un Contrato editado
    public int EditarDatosContrato(Contrato Contrato)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"
    UPDATE Contrato
    SET
        {nameof(Contrato.Id_Inmueble)} = @Id_Inmueble,
        {nameof(Contrato.Id_Propietario)} = @Id_Propietario,
        {nameof(Contrato.Id_Inquilino)} = @Id_Inquilino,
        {nameof(Contrato.Fecha_Inicio)} = @Fecha_Inicio,
        {nameof(Contrato.Fecha_Finalizacion)} = @Fecha_Finalizacion,
        {nameof(Contrato.Monto)} = @Monto,
        {nameof(Contrato.Finalizacion_Anticipada)} = @Finalizacion_Anticipada,
        {nameof(Contrato.Id_Creado_Por)} = @Id_Creado_Por,
        {nameof(Contrato.Id_Terminado_Por)} = @Id_Terminado_Por,
        {nameof(Contrato.Estado_Contrato)} = @Estado_Contrato
    WHERE
        {nameof(Contrato.Id_Contrato)} = @Id_Contrato";

            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id_Contrato", Contrato.Id_Contrato);
                command.Parameters.AddWithValue("@Id_Inmueble", Contrato.Id_Inmueble);
                command.Parameters.AddWithValue("@Id_Propietario", Contrato.Id_Propietario);
                command.Parameters.AddWithValue("@Id_Inquilino", Contrato.Id_Inquilino);
                command.Parameters.AddWithValue("@Fecha_Inicio", Contrato.Fecha_Inicio);
                command.Parameters.AddWithValue("@Fecha_Finalizacion", Contrato.Fecha_Finalizacion);
                command.Parameters.AddWithValue("@Monto", Contrato.Monto);
                command.Parameters.AddWithValue("@Finalizacion_Anticipada", Contrato.Finalizacion_Anticipada);
                command.Parameters.AddWithValue("@Id_Creado_Por", Contrato.Id_Creado_Por);
                command.Parameters.AddWithValue("@Id_Terminado_Por", Contrato.Id_Terminado_Por);
                command.Parameters.AddWithValue("@Estado_Contrato", Contrato.Estado_Contrato);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Alta(Contrato Contrato)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"
            INSERT INTO Contrato
            ({nameof(Contrato.Id_Inmueble)}, {nameof(Contrato.Id_Propietario)}, {nameof(Contrato.Id_Inquilino)}, 
             {nameof(Contrato.Fecha_Inicio)}, 
             {nameof(Contrato.Meses)}, {nameof(Contrato.Fecha_Finalizacion)}, {nameof(Contrato.Monto)}, 
             {nameof(Contrato.Id_Creado_Por)}, {nameof(Contrato.Estado_Contrato)}) 
            VALUES (@Id_Inmueble, @Id_Propietario, @Id_Inquilino, @Fecha_Inicio,@Meses,@Fecha_Finalizacion, @Monto, @Id_Creado_Por, @Estado_Contrato);
            SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id_Inmueble", Contrato.Id_Inmueble);
                command.Parameters.AddWithValue("@Id_Propietario", Contrato.Id_Propietario);
                command.Parameters.AddWithValue("@Id_Inquilino", Contrato.Id_Inquilino);
                command.Parameters.AddWithValue("@Fecha_Inicio", Contrato.Fecha_Inicio);
                command.Parameters.AddWithValue("@Meses", Contrato.Meses);
                command.Parameters.AddWithValue("@Fecha_Finalizacion", Contrato.Fecha_Finalizacion);
                command.Parameters.AddWithValue("@Monto", Contrato.Monto);
                command.Parameters.AddWithValue("@Finalizacion_Anticipada", Contrato.Finalizacion_Anticipada);
                command.Parameters.AddWithValue("@Id_Creado_Por", Contrato.Id_Creado_Por);
                command.Parameters.AddWithValue("@Estado_Contrato", Contrato.Estado_Contrato);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }

    public Contrato Baja(Contrato contrato)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"UPDATE Contrato SET {nameof(Contrato.Finalizacion_Anticipada)} = @{nameof(Contrato.Finalizacion_Anticipada)}
             WHERE {nameof(Contrato.Id_Contrato)} = @{nameof(Contrato.Id_Contrato)}";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Contrato.Id_Contrato)}", contrato.Id_Contrato);
                command.Parameters.AddWithValue($"@{nameof(Contrato.Finalizacion_Anticipada)}", contrato.Finalizacion_Anticipada);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
            return new Contrato();
        }
    }

    //metodo para controlar si el dni nuevo no esta ingresado en la base de datos 
    public bool ContratoExiste(int Id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT COUNT(*) FROM Contrato WHERE Dni = @Dni";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Dni", Id);
                connection.Open();
                var count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }
        }
    }
    //metodo para comparar si el dni que se esta editando no exista 
    public bool EsIdDelContratoActual(int idContrato, int dni)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT COUNT(*) FROM Contrato WHERE Dni = @Dni AND Id_Contrato != @Id_Contrato";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Dni", dni);
                command.Parameters.AddWithValue("@Id_Contrato", idContrato);
                connection.Open();
                var count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }
        }
    }

}