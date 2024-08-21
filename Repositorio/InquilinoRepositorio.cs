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
}