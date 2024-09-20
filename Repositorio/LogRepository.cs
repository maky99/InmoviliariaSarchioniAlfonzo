using MySql.Data.MySqlClient;
using InmoviliariaSarchioniAlfonzo.Models;
using System;
using System.Collections.Generic;

namespace InmoviliariaSarchioniAlfonzo.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string connectionString = "Server=localhost; Port=3306; Database=inmobiliaria; User=root;";

        public void AddLog(Log log)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Logs (LogLevel, Message, Timestamp, Usuario) 
                               VALUES (@LogLevel, @Message, @Timestamp, @Usuario);";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@LogLevel", log.LogLevel);
                    command.Parameters.AddWithValue("@Message", log.Message);
                    command.Parameters.AddWithValue("@Timestamp", log.Timestamp);
                    command.Parameters.AddWithValue("@Usuario", log.Usuario);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Log> GetAllLogs()
        {
            var logs = new List<Log>();

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Logs ORDER BY Id DESC;";

                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            logs.Add(new Log
                            {
                                Id = reader.GetInt32("Id"),
                                LogLevel = reader.GetString("LogLevel"),
                                Message = reader.GetString("Message"),
                                Timestamp = reader.GetDateTime("Timestamp"),
                                Usuario = reader.GetString("Usuario")
                            });
                        }
                    }
                }
            }

            return logs;
        }
    }
}

