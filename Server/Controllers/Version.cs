using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Version : ControllerBase
    {
        private readonly ILogger<Version> _logger;

        public Version(ILogger<Version> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return JsonSerializer.Serialize(App.Config.Apps);
        }
    }
}