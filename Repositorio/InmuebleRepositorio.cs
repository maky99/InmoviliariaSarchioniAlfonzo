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
            var sql = @$"SELECT {nameof(Inmueble.Id_Inmueble)},{nameof(Inmueble.Id_Propietario)},{nameof(Inmueble.Direccion)},{nameof(Inmueble.Uso)},{nameof(Inmueble.Ambientes)},{nameof(Inmueble.Coordenadas)},{nameof(Inmueble.Tamano)},Tipo_Inmueble.{nameof(Tipo_Inmueble.Tipo)} AS Tipo_Inmueble,{nameof(Inmueble.Servicios)},{nameof(Inmueble.Bano)},{nameof(Inmueble.Cochera)},{nameof(Inmueble.Patio)},{nameof(Inmueble.Precio)},{nameof(Inmueble.Condicion)},{nameof(Inmueble.Estado_Inmueble)}
            FROM Inmueble
            JOIN
            Tipo_Inmueble ON Inmueble.{nameof(Inmueble.Id_Tipo_Inmueble)} = Tipo_Inmueble.{nameof(Tipo_Inmueble.Id_Tipo_Inmueble)}";

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
                            Coordenadas = reader.GetInt32("Coordenadas"),
                            Tamano = reader.GetDouble("Tamano"),
                            Tipo_Inmueble = reader.GetString("Tipo_Inmueble"),
                            Servicios = reader.GetString("Servicios"),
                            Bano = reader.GetInt32("Bano"),
                            Cochera = reader.GetInt32("Cochera"),
                            Patio = reader.GetInt32("Patio"),
                            Precio = reader.GetDouble("Precio"),
                            Condicion = reader.GetString("Condicion"),
                            Estado_Inmueble = reader.GetInt32("Estado_Inmueble")
                        });
                    }
                    connection.Close();
                }
            }
        }
        return inmuebles;
    }

    // Agrega otros métodos necesarios para manejar los inmuebles
}
