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
            DownloadApp("collector");
            var app = App.Config.Apps.Where(a => a.Name == "collector").FirstOrDefault() ?? new Models.ConfigApp() { };
            ReplaceFiles(app.Replace, app.Name);
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
