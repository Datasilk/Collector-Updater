using Microsoft.AspNetCore.Mvc;

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
            return App.Settings.Version;
        }
    }
}