using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;
using System.Collections.Generic;

public class InmuebleRepositorio
{
    private readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    // lista todos los inmuebles que hay
    public IList<Inmueble> ObtenerInmuebles()
    {
        List<Inmueble> inmuebles = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT {nameof(Inmueble.Id_Inmueble)},{nameof(Inmueble.Id_Propietario)},{nameof(Inmueble.Direccion)},
             {nameof(Inmueble.Uso)},{nameof(Inmueble.Ambientes)},{nameof(Inmueble.Latitud)},{nameof(Inmueble.Longitud)},
             {nameof(Inmueble.Tamano)}, 
             Tipo_Inmueble.{nameof(Tipo_Inmueble.Tipo)} AS Tipo,
             Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)} AS Id_Tipo_Inmueble,
             Tipo_Inmueble.{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)} AS Estado_Tipo_Inmueble,  -- Agrega esta línea
             {nameof(Inmueble.Servicios)},{nameof(Inmueble.Bano)},{nameof(Inmueble.Cochera)},
             {nameof(Inmueble.Patio)},{nameof(Inmueble.Precio)},{nameof(Inmueble.Condicion)},
             {nameof(Inmueble.Estado_Inmueble)}
             FROM Inmueble
             JOIN Tipo_Inmueble ON Inmueble.{nameof(Inmueble.Id_Tipo_Inmueble)} = Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}
              ORDER BY {nameof(Inmueble.Estado_Inmueble)} DESC";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inmuebles.Add(new Inmueble
                        {
                            Id_Inmueble = reader.GetInt32("Id_Inmueble"),
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetString("Uso"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Tamano = reader.GetDouble("Tamano"),
                            Servicios = reader.GetString("Servicios"),
                            Bano = reader.GetInt32("Bano"),
                            Cochera = reader.GetInt32("Cochera"),
                            Patio = reader.GetInt32("Patio"),
                            Precio = reader.GetDouble("Precio"),
                            Condicion = reader.GetString("Condicion"),
                            Estado_Inmueble = reader.GetInt32("Estado_Inmueble"),
                            tipo = new Tipo_Inmueble
                            {
                                Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                                Tipo = reader.GetString("Tipo"),
                                Estado_Tipo_Inmueble = reader.GetInt32("Estado_Tipo_Inmueble")
                            }
                        });
                    }
                    connection.Close();
                }
            }
        }
        return inmuebles;
    }

    //metodo de guardar nuevo inmueble
    public void GuardarInmueble(Inmueble inmueble)
    {

        using (var connection = new MySqlConnection(connectionString))
        {

            var sql = $@"INSERT INTO inmueble ({nameof(Inmueble.Id_Propietario)},{nameof(Inmueble.Direccion)},{nameof(Inmueble.Uso)},{nameof(Inmueble.Ambientes)},{nameof(Inmueble.Latitud)},{nameof(Inmueble.Longitud)},{nameof(Inmueble.Tamano)},{nameof(Inmueble.Id_Tipo_Inmueble)},{nameof(Inmueble.Servicios)},{nameof(Inmueble.Bano)},{nameof(Inmueble.Cochera)},{nameof(Inmueble.Patio)},{nameof(Inmueble.Precio)},{nameof(Inmueble.Condicion)},{nameof(Inmueble.Estado_Inmueble)})
            VALUES (@{nameof(Inmueble.Id_Propietario)}, @{nameof(Inmueble.Direccion)},@{nameof(Inmueble.Uso)},@{nameof(Inmueble.Ambientes)},@{nameof(Inmueble.Latitud)},@{nameof(Inmueble.Longitud)},@{nameof(Inmueble.Tamano)},@{nameof(Inmueble.Id_Tipo_Inmueble)},@{nameof(Inmueble.Servicios)},@{nameof(Inmueble.Bano)},@{nameof(Inmueble.Cochera)},@{nameof(Inmueble.Patio)},@{nameof(Inmueble.Precio)},@{nameof(Inmueble.Condicion)},@{nameof(Inmueble.Estado_Inmueble)})";


            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Id_Propietario)}", inmueble.Id_Propietario);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Direccion)}", inmueble.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Uso)}", inmueble.Uso);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Ambientes)}", inmueble.Ambientes);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Latitud)}", inmueble.Latitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Longitud)}", inmueble.Longitud);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Tamano)}", inmueble.Tamano);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Id_Tipo_Inmueble)}", inmueble.Id_Tipo_Inmueble);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Servicios)}", inmueble.Servicios);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Bano)}", inmueble.Bano);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Cochera)}", inmueble.Cochera);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Patio)}", inmueble.Patio);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Precio)}", inmueble.Precio);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Condicion)}", inmueble.Condicion);
                command.Parameters.AddWithValue($"@{nameof(Inmueble.Estado_Inmueble)}", inmueble.Estado_Inmueble);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    // metodo para detalle de inmueble 
    public Inmueble InformacionInmueble(int id)
    {
        var inmueble = new Inmueble();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT Inmueble.{nameof(Inmueble.Id_Inmueble)},
                            Inmueble.{nameof(Inmueble.Id_Propietario)},
                            Propietario.{nameof(Propietario.Apellido)} AS Apellido_Propietario,
                            Propietario.{nameof(Propietario.Nombre)} AS Nombre_Propietario,
                            Propietario.{nameof(Propietario.Dni)} AS Dni_Propietario,
                            Propietario.{nameof(Propietario.Telefono)} AS Telefono_Propietario,
                            Propietario.{nameof(Propietario.Email)} AS Email_Propietario,
                            Propietario.{nameof(Propietario.Direccion)} AS Direccion_Propietario,
                            Propietario.{nameof(Propietario.Estado_Propietario)} AS Estado_Propietario,
                            Inmueble.{nameof(Inmueble.Direccion)},
                            Inmueble.{nameof(Inmueble.Uso)},
                            Inmueble.{nameof(Inmueble.Ambientes)},
                            Inmueble.{nameof(Inmueble.Latitud)},
                            Inmueble.{nameof(Inmueble.Longitud)},
                            Inmueble.{nameof(Inmueble.Tamano)},
                            Tipo_Inmueble.{nameof(Tipo_Inmueble.Tipo)} AS Tipo,
                            Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)} AS Id_Tipo_Inmueble,
                            Tipo_Inmueble.{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)} AS Estado_Tipo_Inmueble,
                            Inmueble.{nameof(Inmueble.Servicios)},
                            Inmueble.{nameof(Inmueble.Bano)},
                            Inmueble.{nameof(Inmueble.Cochera)},
                            Inmueble.{nameof(Inmueble.Patio)},
                            Inmueble.{nameof(Inmueble.Precio)},
                            Inmueble.{nameof(Inmueble.Condicion)},
                            Inmueble.{nameof(Inmueble.Estado_Inmueble)}
                     FROM Inmueble
                     JOIN Tipo_Inmueble ON Inmueble.{nameof(Inmueble.Id_Tipo_Inmueble)} = Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}
                     JOIN Propietario ON Inmueble.{nameof(Inmueble.Id_Propietario)} = Propietario.{nameof(Propietario.Id_Propietario)}
                     WHERE Inmueble.{nameof(Inmueble.Id_Inmueble)} = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            Id_Inmueble = reader.GetInt32("Id_Inmueble"),
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetString("Uso"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Tamano = reader.GetDouble("Tamano"),
                            Servicios = reader.GetString("Servicios"),
                            Bano = reader.GetInt32("Bano"),
                            Cochera = reader.GetInt32("Cochera"),
                            Patio = reader.GetInt32("Patio"),
                            Precio = reader.GetDouble("Precio"),
                            Condicion = reader.GetString("Condicion"),
                            Estado_Inmueble = reader.GetInt32("Estado_Inmueble"),
                            tipo = new Tipo_Inmueble
                            {
                                Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                                Tipo = reader.GetString("Tipo"),
                                Estado_Tipo_Inmueble = reader.GetInt32("Estado_Tipo_Inmueble")
                            },
                            propietario = new Propietario
                            {
                                Id_Propietario = reader.GetInt32("Id_Propietario"),
                                Apellido = reader.GetString("Apellido_Propietario"),
                                Nombre = reader.GetString("Nombre_Propietario"),
                                Dni = reader.GetInt32("Dni_Propietario"),
                                Telefono = reader.GetString("Telefono_Propietario"),
                                Email = reader.GetString("Email_Propietario"),
                                Direccion = reader.GetString("Direccion_Propietario"),
                                Estado_Propietario = reader.GetInt32("Estado_Propietario")
                            }
                        };

                        connection.Close();
                    }
                }

                return inmueble;
            }
        }
    }

    //metodo para buscar el inquilino para editar 
    public Inmueble ObtenerInmueblePorId(int id)
    {
        var inmueble = new Inmueble();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT {nameof(Inmueble)}.*, 
                     {nameof(Tipo_Inmueble)}.{nameof(Tipo_Inmueble.Tipo)}, 
                     {nameof(Tipo_Inmueble)}.{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}, 
                     {nameof(Propietario)}.{nameof(Propietario.Apellido)} AS Propietario_Apellido, 
                     {nameof(Propietario)}.{nameof(Propietario.Nombre)} AS Propietario_Nombre
              FROM {nameof(Inmueble)} 
              JOIN {nameof(Tipo_Inmueble)} ON {nameof(Inmueble)}.{nameof(Inmueble.Id_Tipo_Inmueble)} = {nameof(Tipo_Inmueble)}.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}
              JOIN {nameof(Propietario)} ON {nameof(Inmueble)}.{nameof(Inmueble.Id_Propietario)} = {nameof(Propietario)}.{nameof(Propietario.Id_Propietario)}
              WHERE {nameof(Inmueble)}.{nameof(Inmueble.Id_Inmueble)} = @Id_Inmueble";

            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id_Inmueble", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            Id_Inmueble = reader.GetInt32("Id_Inmueble"),
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetString("Uso"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Tamano = reader.GetDouble("Tamano"),
                            Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                            Servicios = reader.GetString("Servicios"),
                            Bano = reader.GetInt32("Bano"),
                            Cochera = reader.GetInt32("Cochera"),
                            Patio = reader.GetInt32("Patio"),
                            Precio = reader.GetDouble("Precio"),
                            Condicion = reader.GetString("Condicion"),
                            Estado_Inmueble = reader.GetInt32("Estado_Inmueble"),
                            tipo = new Tipo_Inmueble
                            {
                                Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                                Tipo = reader.GetString("Tipo"),
                                Estado_Tipo_Inmueble = reader.GetInt32("Estado_Tipo_Inmueble")
                            },
                            propietario = new Propietario
                            {
                                Id_Propietario = reader.GetInt32("Id_Propietario"),
                                Apellido = reader.GetString("Propietario_Apellido"),
                                Nombre = reader.GetString("Propietario_Nombre")
                            }
                        };
                    }
                }
            }
            connection.Close();
        }
        return inmueble;
    }

    //metodo para guardar lo editado 
    public void ActualizarInmueble(Inmueble inmueble)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"UPDATE inmueble
                SET Direccion = @Direccion,
                    Uso = @Uso,
                    Ambientes = @Ambientes,
                    Latitud = @Latitud,
                    Longitud = @Longitud,
                    Tamano = @Tamano,
                    Servicios = @Servicios,
                    Bano = @Bano,
                    Cochera = @Cochera,
                    Patio = @Patio,
                    Precio = @Precio,
                    Condicion = @Condicion,
                    Id_Tipo_Inmueble = @Id_Tipo_Inmueble,
                    Id_Propietario = @Id_Propietario,
                    Estado_Inmueble=@Estado_Inmueble
                WHERE Id_Inmueble = @Id_Inmueble";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id_Inmueble", inmueble.Id_Inmueble);
                command.Parameters.AddWithValue("@Direccion", inmueble.Direccion);
                command.Parameters.AddWithValue("@Uso", inmueble.Uso);
                command.Parameters.AddWithValue("@Ambientes", inmueble.Ambientes);
                command.Parameters.AddWithValue("@Latitud", inmueble.Latitud);
                command.Parameters.AddWithValue("@Longitud", inmueble.Longitud);
                command.Parameters.AddWithValue("@Tamano", inmueble.Tamano);
                command.Parameters.AddWithValue("@Servicios", inmueble.Servicios);
                command.Parameters.AddWithValue("@Bano", inmueble.Bano);
                command.Parameters.AddWithValue("@Cochera", inmueble.Cochera);
                command.Parameters.AddWithValue("@Patio", inmueble.Patio);
                command.Parameters.AddWithValue("@Precio", inmueble.Precio);
                command.Parameters.AddWithValue("@Condicion", inmueble.Condicion);
                command.Parameters.AddWithValue("@Id_Tipo_Inmueble", inmueble.Id_Tipo_Inmueble);
                command.Parameters.AddWithValue("@Id_Propietario", inmueble.Id_Propietario);
                command.Parameters.AddWithValue("@Estado_Inmueble", inmueble.Estado_Inmueble);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    // metodo para detalle de inmueble 
    public IList<Inmueble> InformacionInmueblePropietario(int id)
    {
        List<Inmueble> inmueble = new List<Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT Inmueble.{nameof(Inmueble.Id_Inmueble)},
                            Inmueble.{nameof(Inmueble.Id_Propietario)},
                            Propietario.{nameof(Propietario.Apellido)} AS Apellido_Propietario,
                            Propietario.{nameof(Propietario.Nombre)} AS Nombre_Propietario,
                            Propietario.{nameof(Propietario.Dni)} AS Dni_Propietario,
                            Propietario.{nameof(Propietario.Telefono)} AS Telefono_Propietario,
                            Propietario.{nameof(Propietario.Email)} AS Email_Propietario,
                            Propietario.{nameof(Propietario.Direccion)} AS Direccion_Propietario,
                            Propietario.{nameof(Propietario.Estado_Propietario)} AS Estado_Propietario,
                            Inmueble.{nameof(Inmueble.Direccion)},
                            Inmueble.{nameof(Inmueble.Uso)},
                            Inmueble.{nameof(Inmueble.Ambientes)},
                            Inmueble.{nameof(Inmueble.Latitud)},
                            Inmueble.{nameof(Inmueble.Longitud)},
                            Inmueble.{nameof(Inmueble.Tamano)},
                            Tipo_Inmueble.{nameof(Tipo_Inmueble.Tipo)} AS Tipo,
                            Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)} AS Id_Tipo_Inmueble,
                            Tipo_Inmueble.{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)} AS Estado_Tipo_Inmueble,
                            Inmueble.{nameof(Inmueble.Servicios)},
                            Inmueble.{nameof(Inmueble.Bano)},
                            Inmueble.{nameof(Inmueble.Cochera)},
                            Inmueble.{nameof(Inmueble.Patio)},
                            Inmueble.{nameof(Inmueble.Precio)},
                            Inmueble.{nameof(Inmueble.Condicion)},
                            Inmueble.{nameof(Inmueble.Estado_Inmueble)}
                     FROM Inmueble
                     JOIN Tipo_Inmueble ON Inmueble.{nameof(Inmueble.Id_Tipo_Inmueble)} = Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}
                     JOIN Propietario ON Inmueble.{nameof(Inmueble.Id_Propietario)} = Propietario.{nameof(Propietario.Id_Propietario)}
                     WHERE Inmueble.{nameof(Inmueble.Id_Propietario)} = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inmueble.Add(new Inmueble
                        {
                            Id_Inmueble = reader.GetInt32("Id_Inmueble"),
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Direccion = reader.GetString("Direccion"),
                            Uso = reader.GetString("Uso"),
                            Ambientes = reader.GetInt32("Ambientes"),
                            Latitud = reader.GetString("Latitud"),
                            Longitud = reader.GetString("Longitud"),
                            Tamano = reader.GetDouble("Tamano"),
                            Servicios = reader.GetString("Servicios"),
                            Bano = reader.GetInt32("Bano"),
                            Cochera = reader.GetInt32("Cochera"),
                            Patio = reader.GetInt32("Patio"),
                            Precio = reader.GetDouble("Precio"),
                            Condicion = reader.GetString("Condicion"),
                            Estado_Inmueble = reader.GetInt32("Estado_Inmueble"),
                            tipo = new Tipo_Inmueble
                            {
                                Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                                Tipo = reader.GetString("Tipo"),
                                Estado_Tipo_Inmueble = reader.GetInt32("Estado_Tipo_Inmueble")
                            },
                            propietario = new Propietario
                            {
                                Id_Propietario = reader.GetInt32("Id_Propietario"),
                                Apellido = reader.GetString("Apellido_Propietario"),
                                Nombre = reader.GetString("Nombre_Propietario"),
                                Dni = reader.GetInt32("Dni_Propietario"),
                                Telefono = reader.GetString("Telefono_Propietario"),
                                Email = reader.GetString("Email_Propietario"),
                                Direccion = reader.GetString("Direccion_Propietario"),
                                Estado_Propietario = reader.GetInt32("Estado_Propietario")
                            }
                        });

                        connection.Close();
                    }
                }

                return inmueble;
            }
        }
    }

    public Propietario ObtenerPropietarioPorId(int id)
    {
        Propietario propietario = new Propietario();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT {nameof(Propietario.Id_Propietario)},
                                {nameof(Propietario.Apellido)},
                                {nameof(Propietario.Nombre)},
                                {nameof(Propietario.Dni)},
                                {nameof(Propietario.Telefono)},
                                {nameof(Propietario.Email)},
                                {nameof(Propietario.Direccion)},
                                {nameof(Propietario.Estado_Propietario)}
                         FROM Propietario
                         WHERE {nameof(Propietario.Id_Propietario)} = @id";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        propietario = new Propietario
                        {
                            Id_Propietario = reader.GetInt32(nameof(Propietario.Id_Propietario)),
                            Apellido = reader.GetString(nameof(Propietario.Apellido)),
                            Nombre = reader.GetString(nameof(Propietario.Nombre)),
                            Dni = reader.GetInt32(nameof(Propietario.Dni)),
                            Telefono = reader.GetString(nameof(Propietario.Telefono)),
                            Email = reader.GetString(nameof(Propietario.Email)),
                            Direccion = reader.GetString(nameof(Propietario.Direccion)),
                            Estado_Propietario = reader.GetInt32(nameof(Propietario.Estado_Propietario))
                        };
                    }
                }
            }
        }
        return propietario;
    }


}




