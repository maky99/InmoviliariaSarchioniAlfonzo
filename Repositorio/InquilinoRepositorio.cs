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
                     ORDER BY {nameof(Inquilino.Estado_Inquilino)} DESC, {nameof(Inquilino.Apellido)}";
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

    public IList<Inquilino> OptenerInquilinosActivos()
    {
        List<Inquilino> inquilinos = new List<Inquilino>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Inquilino.Id_Inquilino)}, {nameof(Inquilino.Dni)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Nombre)}, {nameof(Inquilino.Telefono)}, {nameof(Inquilino.Email)}, {nameof(Inquilino.Estado_Inquilino)} 
                     FROM inquilino 
                     WHERE {nameof(Inquilino.Estado_Inquilino)} =1
                     ORDER BY {nameof(Inquilino.Estado_Inquilino)} DESC, {nameof(Inquilino.Apellido)}";
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



    //metodo para agregar un nuevo inquilino
    public void NuevoInquilino(Inquilino inquilino)
    {
        // Verificar si el DNI ya existe en la base de datos
        if (DniExiste(inquilino.Dni))
        {
            throw new InvalidOperationException("El DNI ingresado ya está registrado.");
        }
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = $@"INSERT INTO inquilino 
                        ({nameof(Inquilino.Dni)}, {nameof(Inquilino.Apellido)}, {nameof(Inquilino.Nombre)}, 
                        {nameof(Inquilino.Telefono)}, {nameof(Inquilino.Email)}, {nameof(Inquilino.Estado_Inquilino)})
                        VALUES (@{nameof(Inquilino.Dni)}, @{nameof(Inquilino.Apellido)}, @{nameof(Inquilino.Nombre)}, 
                        @{nameof(Inquilino.Telefono)}, @{nameof(Inquilino.Email)}, @{nameof(Inquilino.Estado_Inquilino)})";

            using (var command = new MySqlCommand(sql, connection))
            {
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
    //metodo para controlar si el dni nuevo no esta ingresado en la base de datos 
    public bool DniExiste(int dni)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT COUNT(*) FROM inquilino WHERE Dni = @Dni";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Dni", dni);
                connection.Open();
                var count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }
        }
    }
    //metodo para comparar si el dni que se esta editando no exista 
    public bool EsDniDelInquilinoActual(int idInquilino, int dni)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT COUNT(*) FROM inquilino WHERE Dni = @Dni AND Id_Inquilino != @Id_Inquilino";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Dni", dni);
                command.Parameters.AddWithValue("@Id_Inquilino", idInquilino);
                connection.Open();
                var count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }
        }
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
        WHERE {nameof(Inquilino.Id_Inquilino)}=@{nameof(Inquilino.Id_Inquilino)}";
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
    //metodo para desactivar inquilino
    public void DesactivarInquilino(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE inquilino SET Estado_Inquilino = 0 WHERE Id_Inquilino = @Id_Inquilino";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id_Inquilino", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }


}