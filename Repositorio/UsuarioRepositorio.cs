using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class UsuarioRepositorio
{
    private readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

    public int AltaUsuario(Usuario usuario)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {

            string sql = @"INSERT INTO usuario ( Apellido, Nombre, Dni, Telefono, Rol, Email, Password, Avatar, Estado_Usuario)
                        VALUES ( @Apellido, @Nombre, @Dni, @Telefono, @Rol, @Email, @Password, @Avatar, @Estado_Usuario);   
                        SELECT LAST_INSERT_ID();";

            using (var command = new MySqlCommand(sql, connection))
            {

                command.Parameters.AddWithValue("@Apellido", usuario.Apellido);
                command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@Dni", usuario.Dni);
                command.Parameters.AddWithValue("@Telefono", usuario.Telefono);
                command.Parameters.AddWithValue("@Rol", usuario.Rol);
                command.Parameters.AddWithValue("@Email", usuario.Email);
                command.Parameters.AddWithValue("@Avatar", string.IsNullOrEmpty(usuario.Avatar) ? (object)DBNull.Value : usuario.Avatar); ;
                command.Parameters.AddWithValue("@Password", usuario.Password);
                command.Parameters.AddWithValue("@Estado_Usuario", usuario.Estado_Usuario);
                connection.Open();
                res = Convert.ToInt32(command.ExecuteScalar());
                usuario.Id_Usuario = res;
                connection.Close();

            }

        }
        return res;
    }


    public int ModificarUsuario(Usuario usuario)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE usuario
					SET Nombre=@nombre, Apellido=@apellido, Avatar=@avatar, Email=@email, Password=@Password, Rol=@rol, Dni=@dni, Telefono=@telefono
					WHERE Id_Usuario = @id_usuario;";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id_usuario", usuario.Id_Usuario);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);
                command.Parameters.AddWithValue("Avatar", usuario.Avatar);
                command.Parameters.AddWithValue("Email", usuario.Email);
                command.Parameters.AddWithValue("Password", usuario.Password);
                command.Parameters.AddWithValue("Rol", usuario.Rol);
                command.Parameters.AddWithValue("Dni", usuario.Dni);
                command.Parameters.AddWithValue("Telefono", usuario.Telefono);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public int ModificarUsuarioSoloDatos(Usuario usuario)
    {
        int res = -1;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"UPDATE usuario
					SET Nombre=@nombre, Apellido=@apellido, Email=@email, Rol=@rol, Dni=@dni, Telefono=@telefono
					WHERE Id_Usuario = @id_usuario;";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.AddWithValue("@id_usuario", usuario.Id_Usuario);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@apellido", usuario.Apellido);

                command.Parameters.AddWithValue("Email", usuario.Email);

                command.Parameters.AddWithValue("Rol", usuario.Rol);
                command.Parameters.AddWithValue("Dni", usuario.Dni);
                command.Parameters.AddWithValue("Telefono", usuario.Telefono);
                connection.Open();
                res = command.ExecuteNonQuery();
                connection.Close();
            }
        }
        return res;
    }
    public IList<Usuario> OptenerUsuarios()
    {
        List<Usuario> Usuarios = new List<Usuario>();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Usuario.Id_Usuario)}, {nameof(Usuario.Dni)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Telefono)}, {nameof(Usuario.Email)}, {nameof(Usuario.Rol)}, {nameof(Usuario.Avatar)}, {nameof(Usuario.Estado_Usuario)} 
                     FROM usuario 
                     ORDER BY {nameof(Usuario.Estado_Usuario)} DESC, {nameof(Usuario.Apellido)}";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuarios.Add(new Usuario
                        {
                            Id_Usuario = reader.GetInt32("Id_Usuario"),
                            Dni = reader.GetInt32("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Rol = reader.GetInt32("Rol"),
                            Avatar = reader["Avatar"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Avatar")) : "",
                            Estado_Usuario = reader.GetInt32("Estado_Usuario")

                        });
                    }
                    connection.Close();
                }
            }
        }
        return Usuarios;
    }


    public Usuario UsuariosPorId(int id)
    {
        var usuario = new Usuario();
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"Select {nameof(Usuario.Id_Usuario)}, {nameof(Usuario.Dni)}, {nameof(Usuario.Apellido)}, {nameof(Usuario.Nombre)}, {nameof(Usuario.Telefono)}, {nameof(Usuario.Email)}, {nameof(Usuario.Rol)}, {nameof(Usuario.Avatar)}, {nameof(Usuario.Estado_Usuario)} 
                     FROM usuario 
                     WHERE {nameof(Usuario.Id_Usuario)} = '{id}'
                     ";
            using (var command = new MySqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario = (new Usuario
                        {
                            Id_Usuario = reader.GetInt32("Id_Usuario"),
                            Dni = reader.GetInt32("Dni"),
                            Apellido = reader.GetString("Apellido"),
                            Nombre = reader.GetString("Nombre"),
                            Telefono = reader.GetString("Telefono"),
                            Email = reader.GetString("Email"),
                            Rol = reader.GetInt32("Rol"),
                            Avatar = reader["Avatar"] != DBNull.Value ? reader.GetString(reader.GetOrdinal("Avatar")) : "",
                            Estado_Usuario = reader.GetInt32("Estado_Usuario")

                        });
                    }
                    connection.Close();
                }
            }
        }
        return usuario;
    }

    public Usuario ObtenerPorEmail(string email)
    {
        Usuario? usuario = null;
        using (var connection = new MySqlConnection(connectionString))
        {
            string sql = @"SELECT
					Id_Usuario, Nombre, Apellido, Avatar, Email, Pasword, Rol FROM usuario
					WHERE Email=@email";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    usuario = new Usuario
                    {
                        Id_Usuario = reader.GetInt32("Id_Usuario"),
                        Nombre = reader.GetString("Nombre"),
                        Apellido = reader.GetString("Apellido"),
                        Avatar = reader.GetString("Avatar"),
                        Email = reader.GetString("Email"),
                        Password = reader.GetString("Password"),
                        Rol = reader.GetInt32("Rol"),
                    };
                }
                connection.Close();
            }
        }
        return usuario;
    }

    public Usuario? ObtenerUsuarioLogin(string Email, string Password)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            var sql = @$"SELECT {nameof(Usuario.Id_Usuario)},{nameof(Usuario.Nombre)},{nameof(Usuario.Apellido)},
    {nameof(Usuario.Email)},{nameof(Usuario.Password)},{nameof(Usuario.Rol)},{nameof(Usuario.Avatar)}
     FROM usuario WHERE {nameof(Usuario.Email)} = @Email and {nameof(Usuario.Password)} = @Password;";
            Usuario? usuario = null;
            using (MySqlCommand cmd = new MySqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);
                connection.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario = new Usuario
                        {
                            Id_Usuario = reader.GetInt32(nameof(Usuario.Id_Usuario)),
                            Nombre = reader.GetString(nameof(Usuario.Nombre)),
                            Apellido = reader.GetString(nameof(Usuario.Apellido)),
                            Email = reader.GetString(nameof(Usuario.Email)),
                            Password = reader.GetString(nameof(Usuario.Password)),
                            Rol = reader.GetInt32(nameof(Usuario.Rol)),
                            Avatar = reader[nameof(Usuario.Avatar)] != DBNull.Value ? reader.GetString(nameof(Usuario.Avatar)) : ""
                        };
                    }
                }
                connection.Close();
            }
            return usuario;
        }


    }




}










