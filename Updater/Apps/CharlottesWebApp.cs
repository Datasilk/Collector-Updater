namespace Updater.Apps
{
    public class CharlottesWebApp : AppBase
    {
        public CharlottesWebApp(ILogger<UpdaterLogger> logger) : base(logger)
        {
            _logger = logger;
        }

        public override void Update()
        {
            DownloadApp("charlottes-web");
            var app = App.Config.Apps.Where(a => a.Name == "charlottes-web").FirstOrDefault() ?? new Models.ConfigApp() { Address = "localhost:7007" };

            //update web.config (which will restart the app)
            var configfile = App.Config.InstallPath + "charlottes-web" + "\\appsettings.json";
            var contents = File.ReadAllText(configfile);
            contents = contents.Replace("localhost:7007", app.Address);
            File.WriteAllText(configfile, contents);
        }

        public override void Start()
        {
            StartApp("charlottes-web", ".\\Router.exe", "Router");
        }

        public override void Stop()
        {
            StopApp();
        }
    }
}
