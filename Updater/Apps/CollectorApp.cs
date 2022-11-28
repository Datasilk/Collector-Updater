namespace Updater.Apps
{
    public class CollectorApp : AppBase
    {
        public CollectorApp(ILogger<UpdaterLogger> logger) : base(logger)
        {
            _logger = logger;
        }

        public override void Update()
        {
            Console.WriteLine("Updating Collector...");
            DownloadApp("collector");
            var app = App.Config.Apps.Where(a => a.Name == "collector").FirstOrDefault() ?? new Models.ConfigApp() { Port = "7070" };
            Console.WriteLine("Updating web.config, changing port for Collector to " + app.Port);
            //update web.config (which will restart the app)
            var configfile = App.Config.InstallPath + "collector" + "\\web.config";
            var webconfig = File.ReadAllText(configfile);
            webconfig = webconfig.Replace("7070", app.Port);
            File.WriteAllText(configfile, webconfig);
            Console.WriteLine("Saved web.config");
        }

        public override void Start()
        {
            StartApp("collector", ".\\Saber.exe", "Collector");
        }

        public override void Stop()
        {
            StopApp();
        }
    }
}
