namespace Updater.Apps
{
    public class CharlotteApp: AppBase
    {
        public override string Name { get; set; } = "Charlotte";
        public CharlotteApp(ILogger<UpdaterLogger> logger) : base(logger)
        {
            _logger = logger;
        }

        public override void Update()
        {
            DownloadApp("charlotte");
            var app = App.Config.Apps.Where(a => a.Name == "charlotte").FirstOrDefault() ?? new Models.ConfigApp() { };
            ReplaceFiles(app.Replace, app.Name);
        }

        public override void Start()
        {
            StartApp("charlotte", ".\\Charlotte.exe", Name);
        }

        public override void Stop()
        {
            StopApp();
        }
    }
}
