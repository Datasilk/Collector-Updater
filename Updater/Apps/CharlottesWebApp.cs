namespace Updater.Apps
{
    public class CharlottesWebApp : AppBase
    {
        public override string Name { get; set; } = "Router";
        public CharlottesWebApp(ILogger<UpdaterLogger> logger) : base(logger)
        {
            _logger = logger;
        }

        public override void Update()
        {
            DownloadApp("charlottes-web");
            var app = App.Config.Apps.Where(a => a.Name == "charlottes-web").FirstOrDefault() ?? new Models.ConfigApp() { };
            ReplaceFiles(app.Replace, app.Name);
        }

        public override void Start()
        {
            StartApp("charlottes-web", ".\\Router.exe", Name);
        }

        public override void Stop()
        {
            StopApp();
        }
    }
}
