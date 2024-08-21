using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;


public class InquilinoRepositorio
{

    readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    //metodo para listar todos los inquilinos
    public IList<Inquilino> OptenerInquilinos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Inquilino.Id_Inquilino)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Telefono)}, {nameof(Inquilino.Email)}, {nameof(Inquilino.Estado_Inquilino)} 
                     FROM inquilino 
                     WHERE {nameof(Inquilino.Estado_Inquilino)}=1 
                     ORDER BY {nameof(Inquilino.Apellido)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        inquilinos.Add(new Inquilino
                        {
                            Id_Inquilino = reader.GetInt32("Id_Inquilino"),
                            Dni = reader.GetInt32("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Estado_Inquilino = reader.GetInt32("Estado_Inquilino")
                        });
                    }
                    connection.Close();
                }

            }


        }
        return inquilinos;
    }


    //metodo para buscar inquilino antes de editar
    public Inquilino BuscarInquilino(int id)
    {
        var inquilino = new Inquilino();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var sql = $"SELECT * FROM Inquilino WHERE Id_Inquilino = '{id}'";
            using (var comando = new MySqlCommand(sql, connection))
            {
                comando.Parameters.AddWithValue("@Id", id);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        inquilino = (new Inquilino
                        {
                            Id_Inquilino = reader.GetInt32("Id_Inquilino"),
                            Dni = reader.GetInt32("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Estado_Inquilino = reader.GetInt32("Estado_Inquilino")
                        });
                    }
                }
            }

        }

        return inquilino;
    }


    //metodo para guardar un inquilino editado
    public void EditarDatos(Inquilino inquilino)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"UPDATE inquilino
        SET {nameof(Inquilino.Dni)}=@{nameof(Inquilino.Dni)},{nameof(Inquilino.Apellido)}=@{nameof(Inquilino.Apellido)},{nameof(Inquilino.Nombre)}=@{nameof(Inquilino.Nombre)},{nameof(Inquilino.Telefono)}=@{nameof(Inquilino.Telefono)},{nameof(Inquilino.Email)}=@{nameof(Inquilino.Email)},{nameof(Inquilino.Estado_Inquilino)}=@{nameof(Inquilino.Estado_Inquilino)}
        FROM inquilino
        WHER {nameof(Inquilino.Id_Inquilino)}=@{nameof(Inquilino.Id_Inquilino)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Inquilino.Id_Inquilino)}", inquilino.Id_Inquilino);
                command.Parameters.AddWithValue($"@{nameof(Inquilino.Dni)}", inquilino.Dni);
                command.Parameters.AddWithValue($"@{nameof(Inquilino.Apellido)}", inquilino.Apellido);
                command.Parameters.AddWithValue($"@{nameof(Inquilino.Nombre)}", inquilino.Nombre);
                command.Parameters.AddWithValue($"@{nameof(Inquilino.Telefono)}", inquilino.Telefono);
                command.Parameters.AddWithValue($"@{nameof(Inquilino.Email)}", inquilino.Email);
                command.Parameters.AddWithValue($"@{nameof(Inquilino.Estado_Inquilino)}", inquilino.Estado_Inquilino);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}