using InmoviliariaSarchioniAlfonzo.Models;
using System.Collections.Generic;

namespace InmoviliariaSarchioniAlfonzo.Repositories
{
    public interface ILogRepository
    {
        void AddLog(Log log);
        IEnumerable<Log> GetAllLogs();
    }
}
