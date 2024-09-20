
namespace InmoviliariaSarchioniAlfonzo.Models
{
public class Log
{
    public int Id { get; set; }
    public string LogLevel { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public string Usuario { get; set; }
}
}