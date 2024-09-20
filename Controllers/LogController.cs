using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InmoviliariaSarchioniAlfonzo.Repositories;
using System.Linq;

namespace InmoviliariaSarchioniAlfonzo.Controllers
{
    [Authorize(Policy = "Administrador")]
    public class LogController : Controller
    {
        private readonly ILogRepository _logRepository;

        public LogController(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public IActionResult VerLogs()
        {
            var logs = _logRepository.GetAllLogs();
            return View("VerLogs", logs);
        }
    }
}
