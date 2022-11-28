
using System.Text.Json;
using System.Net;
using System.Net.Http;
using Updater.Models;
using Updater.Apps;

namespace Updater
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<UpdaterLogger> _logger;
        private int Delay { get; set; } = 15; // in minutes
        private string ApiUri { get; set; } = "http://192.168.0.200";
        private Config Config { get; set; } = new Config();
        private string timeFormat = "hh:mm:ss";

        public Worker(ILogger<UpdaterLogger> logger)
        {
            _logger = logger;

            if (File.Exists(App.MapPath("config.json")))
            {
                var config = File.ReadAllText(App.MapPath("config.json"));
                Config = JsonSerializer.Deserialize<Config>(config) ?? new Config();
                App.Config = Config;
                if(Config.Api == "")
                {
                    _logger.LogInformation("{time}: Error getting Api Uri from config.json", DateTimeOffset.Now.ToString(timeFormat));
                }
                if (!Directory.Exists(App.Config.InstallPath))
                {
                    Directory.CreateDirectory(App.Config.InstallPath);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var startup = true;
            CollectorApp collectorApp = null;
            CharlotteApp charlotteApp = null;
            CharlottesWebApp charlottesWebApp = null;
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
                        var appInfo = Config.Apps.Where(a => a.Name == version.Name).FirstOrDefault();
                        if(appInfo != null)
                        {
                            if (appInfo.Version != version.Version || startup == true)
                            {
                                var oldv = appInfo.Version;
                                appInfo.Version = version.Version;
                                isChanged = true;
                                AppBase app = null;
                                switch (appInfo.Name.ToLower())
                                {
                                    case "collector":
                                        if(collectorApp == null) { collectorApp = new CollectorApp(_logger); }
                                        app = collectorApp;
                                        break;
                                    case "charlotte":
                                        if (charlotteApp == null) { charlotteApp = new CharlotteApp(_logger); }
                                        app = charlotteApp;
                                        break;
                                    case "charlottes-web":
                                        if (charlottesWebApp == null) { charlottesWebApp = new CharlottesWebApp(_logger); }
                                        app = charlottesWebApp;
                                        break;
                                }
                                if (app != null)
                                {
                                    if (oldv != appInfo.Version)
                                    {
                                        //update the application
                                        _logger.LogInformation("{time}: running updater for " + appInfo.Name, DateTimeOffset.Now.ToString(timeFormat));
                                        app.Stop(); //stop the app before updating
                                        app.Update();
                                        _logger.LogInformation("{time}: Version for \"" + appInfo.Name + "\" changed from " + oldv + " to " + version.Version, DateTimeOffset.Now.ToString(timeFormat));
                                    }
                                    if(oldv != appInfo.Version || startup == true)
                                    {
                                        //start the application
                                        _logger.LogInformation("{time}: starting app " + appInfo.Name, DateTimeOffset.Now.ToString(timeFormat));
                                        app.Start();
                                    }
                                }
                            }
                        }
                    }

                    if (startup == true) { startup = false; }

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
                    _logger.LogInformation("{time}: Error accessing remote API '" + ApiUri + "'", DateTimeOffset.Now.ToString(timeFormat));
                }
                await Task.Delay(60000 * Delay, stoppingToken);
            }
        }
    }
}