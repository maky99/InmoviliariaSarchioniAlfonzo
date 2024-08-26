using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;


public class Tipo_InmuebleRepositorio
{

    readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    //lista todos los tipos de inmuebles que hay 
    public IList<Tipo_Inmueble> TipoInmu()
    {
        List<Tipo_Inmueble> tipos = new List<Tipo_Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$" Select {nameof(Tipo_Inmueble.Id_Tipo_Inmueble)},{nameof(Tipo_Inmueble.Tipo)},{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}
            FROM Tipo_Inmueble";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipos.Add(new Tipo_Inmueble
                        {
                            Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                            Tipo = reader.GetString("Tipo"),
                            Estado_Tipo_Inmueble = reader.GetInt32("Estado_Tipo_Inmueble")
                        });
                    }
                    connection.Close();
                }
            }
        }
        return tipos;
    }
    //lista todos los tipos de inmuebles que hay 
    public IList<Tipo_Inmueble> TipoInmuActivo()
    {
        List<Tipo_Inmueble> tipos = new List<Tipo_Inmueble>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$" Select {nameof(Tipo_Inmueble.Id_Tipo_Inmueble)},{nameof(Tipo_Inmueble.Tipo)},{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}
            FROM Tipo_Inmueble
            WHERE {nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}=1";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tipos.Add(new Tipo_Inmueble
                        {
                            Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                            Tipo = reader.GetString("Tipo"),
                            Estado_Tipo_Inmueble = reader.GetInt32("Estado_Tipo_Inmueble")
                        });
                    }
                    connection.Close();
                }
            }
        }
        return tipos;
    }

    //metodo para nuevo tipo de inmueble
    public void NuevoTipo(Tipo_Inmueble tipo_Inmueble)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $@"INSERT INTO Tipo_Inmueble
            ({nameof(Tipo_Inmueble.Tipo)},{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)})
            VALUES (@{nameof(Tipo_Inmueble.Tipo)}, @{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)})";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Tipo_Inmueble.Tipo)}", tipo_Inmueble.Tipo);
                command.Parameters.AddWithValue($"@{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}", tipo_Inmueble.Estado_Tipo_Inmueble);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    //edita el tipo de inmueble

    public Tipo_Inmueble BuscaparaEditar(int id)
    {
        var tipo = new Tipo_Inmueble();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var sql = $"SELECT * FROM Tipo_Inmueble WHERE Id_Tipo_Inmueble ='{id}'";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        tipo = (new Tipo_Inmueble
                        {
                            Id_Tipo_Inmueble = reader.GetInt32("Id_Tipo_Inmueble"),
                            Tipo = reader.GetString("Tipo"),
                            Estado_Tipo_Inmueble = reader.GetInt32("Estado_Tipo_Inmueble")

                        });
                    }
                }
            }
        }
        return tipo;
    }
    //Metodo para guardar lo editado del tipo Inmueble

    public void EditaDos(Tipo_Inmueble tipo_Inmueble)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"UPDATE Tipo_Inmueble 
Set {nameof(Tipo_Inmueble.Tipo)}=@{nameof(Tipo_Inmueble.Tipo)},{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}=@{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}
WHERE {nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}=@{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}", tipo_Inmueble.Id_Tipo_Inmueble);
                command.Parameters.AddWithValue($"@{nameof(Tipo_Inmueble.Tipo)}", tipo_Inmueble.Tipo);
                command.Parameters.AddWithValue($"@{nameof(Tipo_Inmueble.Estado_Tipo_Inmueble)}", tipo_Inmueble.Estado_Tipo_Inmueble);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }

    //metodo para cambiar de estado 
    public void DesactivarTipo(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE Tipo_Inmueble SET Estado_Tipo_Inmueble=  0 WHERE Id_Tipo_Inmueble = @Id_Tipo_Inmueble";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id_Tipo_Inmueble", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

            }

        }

    }
}