
using System.Text.Json;
using System.Net;
using System.Net.Http;
using Updater.Models;

namespace Updater
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private int Delay { get; set; } = 15; // in minutes
        private string ApiUri { get; set; } = "http://192.168.0.200";
        private Config Config { get; set; } = new Config();

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;

            if (File.Exists(App.MapPath("config.json")))
            {
                var config = File.ReadAllText(App.MapPath("config.json"));
                Config = JsonSerializer.Deserialize<Config>(config) ?? new Config();
                App.Config = Config;
                if(Config.Api == "")
                {
                    _logger.LogInformation("{time}: Error getting Api Uri from config.json", DateTimeOffset.Now);
                }
                if (!Directory.Exists(App.Config.InstallPath))
                {
                    Directory.CreateDirectory(App.Config.InstallPath);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //check for version update
                var request = new HttpClient();
                var response = request.GetAsync(Config.Api + "/Version").Result;
                if(response != null)
                {
                    var versions = JsonSerializer.Deserialize<List<ConfigApp>>(response.Content.ReadAsStringAsync().Result) ?? new List<ConfigApp>();
                    var isChanged = false;
                    foreach(var version in versions)
                    {
                        var app = Config.Apps.Where(a => a.Name == version.Name).FirstOrDefault();
                        if(app != null)
                        {
                            if (app.Version != version.Version)
                            {
                                var oldv = app.Version;
                                app.Version = version.Version;
                                isChanged = true;
                                Update updater = null;
                                switch (app.Name.ToLower())
                                {
                                    case "collector":
                                        updater = new Updaters.CollectorApp();
                                        break;
                                    case "charlotte":
                                        updater = new Updaters.CharlotteApp();
                                        break;
                                    case "charlottes-web":
                                        updater = new Updaters.CharlottesWebApp();
                                        break;
                                }
                                if (updater != null)
                                {
                                    _logger.LogInformation("{time}: running updater for " + app.Name, DateTimeOffset.Now);
                                    updater.Run();
                                }
                                _logger.LogInformation("{time}: Version for \"" + app.Name + "\" changed from " + oldv + " to " + version.Version, DateTimeOffset.Now);
                            }
                        }
                    }

                    if (isChanged)
                    {
                        //save config file
                        File.WriteAllText(App.MapPath("config.json"), JsonSerializer.Serialize(App.Config, new JsonSerializerOptions()
                        {
                            WriteIndented = true
                        }));
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