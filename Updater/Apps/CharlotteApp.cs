namespace Updater.Apps
{
    public class CharlotteApp: AppBase
    {
        public CharlotteApp(ILogger<UpdaterLogger> logger) : base(logger)
        {
            _logger = logger;
        }

        public override void Update()
        {
            DownloadApp("charlotte");
            var app = App.Config.Apps.Where(a => a.Name == "charlotte").FirstOrDefault() ?? new Models.ConfigApp() { Address = "localhost:7077"};

            //try to register Charlotte as a Windows Service
            try
            {
                Ps.Command("New-Service -Name \"Charlotte\" -BinaryPathName \"" + App.Config.InstallPath + "charlotte\\Charlotte.exe\" -Description \"Navigates to a given URL and extracts all relevant DOM data as JSON\" -DisplayName \"Charlotte\" -StartupType Automatic");
            }
            catch (Exception) { }

            //update appsettings.json with updated port number
            var configfile = App.Config.InstallPath + "charlotte" + "\\appsettings.json";
            var contents = File.ReadAllText(configfile);
            contents = contents.Replace("localhost:7077", app.Address);
            File.WriteAllText(configfile, contents);
        }

        public override void Start()
        {
            StartApp("charlotte", ".\\Charlotte.exe", "Charlotte");
        }

        public override void Stop()
        {
            StopApp();
        }
    }
}
