using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;


public class PropietarioRepositorio
{

    readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    //metodo para listar todos los propietario
    public IList<Propietario> OptenerPropietarios()
    {
        List<Propietario>propietarios = new List<Propietario>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Propietario.Id_Propietario)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Email)},{nameof(Propietario.Direccion)}, {nameof(Propietario.Estado_Propietario)} 
                     FROM propietario 
                     WHERE {nameof(Propietario.Estado_Propietario)}=1 
                     ORDER BY {nameof(Propietario.Apellido)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        propietarios.Add(new Propietario
                        {
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Dni = reader.GetInt32("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Direccion = reader.GetString("Direccion"),
                            Estado_Propietario = reader.GetInt32("Estado_Propietario")
                        });
                    }
                    connection.Close();
                }

            }


        }
        return propietarios;
    }


    //metodo para buscar Propietario antes de editar
    public Propietario BuscarPropietario(int id)
    {
        var Propietario = new Propietario();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var sql = $"SELECT * FROM Propietario WHERE Id_Propietario = '{id}'";
            using (var comando = new MySqlCommand(sql, connection))
            {
                comando.Parameters.AddWithValue("@Id", id);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Propietario = (new Propietario
                        {
                            Id_Propietario = reader.GetInt32("Id_Propietario"),
                            Dni = reader.GetInt32("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Direccion = reader.GetString("Direccion"),
                            Estado_Propietario = reader.GetInt32("Estado_Propietario")
                        });
                    }
                }
            }

        }

        return Propietario;
    }


    //metodo para guardar un Propietario editado
    public void EditarDatosPropietario(Propietario Propietario)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"UPDATE Propietario
        SET {nameof(Propietario.Dni)}=@{nameof(Propietario.Dni)},{nameof(Propietario.Apellido)}=@{nameof(Propietario.Apellido)},{nameof(Propietario.Nombre)}=@{nameof(Propietario.Nombre)},{nameof(Propietario.Telefono)}=@{nameof(Propietario.Telefono)},{nameof(Propietario.Email)}=@{nameof(Propietario.Email)},{nameof(Propietario.Direccion)}=@{nameof(Propietario.Direccion)},{nameof(Propietario.Estado_Propietario)}=@{nameof(Propietario.Estado_Propietario)}
        FROM Propietario
        WHER {nameof(Propietario.Id_Propietario)}=@{nameof(Propietario.Id_Propietario)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue($"@{nameof(Propietario.Id_Propietario)}", Propietario.Id_Propietario);
                command.Parameters.AddWithValue($"@{nameof(Propietario.Dni)}", Propietario.Dni);
                command.Parameters.AddWithValue($"@{nameof(Propietario.Apellido)}", Propietario.Apellido);
                command.Parameters.AddWithValue($"@{nameof(Propietario.Nombre)}", Propietario.Nombre);
                command.Parameters.AddWithValue($"@{nameof(Propietario.Telefono)}", Propietario.Telefono);
                command.Parameters.AddWithValue($"@{nameof(Propietario.Email)}", Propietario.Email);
                command.Parameters.AddWithValue($"@{nameof(Propietario.Direccion)}", Propietario.Direccion);
                command.Parameters.AddWithValue($"@{nameof(Propietario.Estado_Propietario)}", Propietario.Estado_Propietario);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}