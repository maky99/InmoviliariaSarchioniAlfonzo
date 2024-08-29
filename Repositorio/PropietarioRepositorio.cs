using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;


public class PropietarioRepositorio
{

    readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    //metodo para listar todos los propietario
    public IList<Propietario> ObtenerPropietarios()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Propietario.Id_Propietario)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Email)},{nameof(Propietario.Direccion)}, {nameof(Propietario.Estado_Propietario)} 
                     FROM propietario 
                     ORDER BY {nameof(Propietario.Estado_Propietario)} DESC, {nameof(Propietario.Apellido)}";
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

    //metodo para listar todos los propietario ACTIVOS 
    public IList<Propietario> ObtenerPropietariosActivos()
    {
        List<Propietario> propietarios = new List<Propietario>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Propietario.Id_Propietario)}, {nameof(Propietario.Dni)}, {nameof(Propietario.Apellido)}, {nameof(Propietario.Nombre)}, {nameof(Propietario.Telefono)}, {nameof(Propietario.Email)},{nameof(Propietario.Direccion)}, {nameof(Propietario.Estado_Propietario)} 
                     FROM propietario 
                     WHERE {nameof(Propietario.Estado_Propietario)}=1
                     ORDER BY {nameof(Propietario.Estado_Propietario)} DESC, {nameof(Propietario.Apellido)}";
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
            var sql = $"SELECT * FROM propietario WHERE Id_Propietario = '{id}'";
            using (var comando = new MySqlCommand(sql, connection))
            {
                comando.Parameters.AddWithValue("@Id_Propietario", id);

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
    public int EditarDatosPropietario(Propietario propietario)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"UPDATE propietario SET
         {nameof(Propietario.Dni)}=@Dni,
         {nameof(Propietario.Apellido)}=@Apellido,
         {nameof(Propietario.Nombre)}=@Nombre,
         {nameof(Propietario.Telefono)}=@Telefono,
         {nameof(Propietario.Email)}=@Email,
         {nameof(Propietario.Direccion)}=@Direccion,
         {nameof(Propietario.Estado_Propietario)}=@Estado_Propietario
         WHERE {nameof(Propietario.Id_Propietario)}=@Id_Propietario";
            using (MySqlCommand command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id_Propietario", propietario.Id_Propietario);
                command.Parameters.AddWithValue("@Dni", propietario.Dni);
                command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@Email", propietario.Email);
                command.Parameters.AddWithValue("@Direccion", propietario.Direccion);
                command.Parameters.AddWithValue("@Estado_Propietario", propietario.Estado_Propietario);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }

    public int Alta(Propietario propietario)
    {


        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"INSERT INTO propietario
        ({nameof(Propietario.Dni)},{nameof(Propietario.Apellido)},{nameof(Propietario.Nombre)},{nameof(Propietario.Direccion)},{nameof(Propietario.Telefono)},{nameof(Propietario.Email)},{nameof(Propietario.Estado_Propietario)})
        VALUES (@Dni, @Apellido, @Nombre, @Direccion, @Telefono, @Email, @Estado_Propietario);
         SELECT LAST_INSERT_ID();";

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Dni", propietario.Dni);
                command.Parameters.AddWithValue("@Apellido", propietario.Apellido);
                command.Parameters.AddWithValue("@Nombre", propietario.Nombre);
                command.Parameters.AddWithValue("@Telefono", propietario.Telefono);
                command.Parameters.AddWithValue("@Email", propietario.Email);
                command.Parameters.AddWithValue("@Direccion", propietario.Direccion);
                command.Parameters.AddWithValue("@Estado_Propietario", propietario.Estado_Propietario);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
        }
        return res;
    }
    public int Baja(int id)
    {
        int res = -1;
        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            var query = $@"UPDATE propietario SET {nameof(Propietario.Estado_Propietario)} = 0 WHERE {nameof(Propietario.Id_Propietario)} = @id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
            return res;
        }
    }

    //metodo para controlar si el dni nuevo no esta ingresado en la base de datos 
    public bool DniyaExiste(int dni)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT COUNT(*) FROM propietario WHERE Dni = @Dni";
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
    public bool EsDniDelPropietarioActual(int idPropietario, int dni)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = "SELECT COUNT(*) FROM propietario WHERE Dni = @Dni AND Id_Propietario != @Id_Propietario";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Dni", dni);
                command.Parameters.AddWithValue("@Id_Propietario", idPropietario);
                connection.Open();
                var count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                return count > 0;
            }
        }
    }






}