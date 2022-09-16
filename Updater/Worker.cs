
using System.Text.Json;
using System.Net;
using System.Net.Http;

namespace Updater
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private int Delay { get; set; } = 15; // in minutes
        private string ApiUri { get; set; } = "http://192.168.0.200";
        private AppSettings Settings { get; set; }

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            if (File.Exists(App.MapPath("config.json")))
            {
                var config = File.ReadAllText(App.MapPath("config.json"));
                Settings = JsonSerializer.Deserialize<AppSettings>(config) ?? new AppSettings();
                if(Settings.Api == "")
                {
                    _logger.LogInformation("{time}: Error getting Api Uri from config.json", DateTimeOffset.Now);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //check for version update
                var request = new HttpClient();
                var response = request.GetAsync(Settings.Api).Result;
                if(response != null)
                {
                    var v = response.Content.ToString();
                    if(v != Settings.Version)
                    {
                        Settings.Version = v;
                        _logger.LogInformation("{time}: Version changed from " + Settings.Version + " to " + v, DateTimeOffset.Now);
                    }
                    
                }
                else
                {
                    _logger.LogInformation("{time}: Error accessing remote API '" + ApiUri + "'", DateTimeOffset.Now);
                }
                await Task.Delay(60000 * Delay, stoppingToken);
            }
        }
    }
}