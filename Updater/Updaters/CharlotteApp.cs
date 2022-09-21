namespace Updater.Updaters
{
    public class CharlotteApp: Update
    {
        public override void Run()
        {
            DownloadApp("charlotte");
            var app = App.Config.Apps.Where(a => a.Name == "charlotte").FirstOrDefault() ?? new Models.ConfigApp() { Port = "7077"};

            //update web.config (which will restart the app)
            var configfile = App.Config.InstallPath + "charlotte" + "\\web.config";
            var webconfig = File.ReadAllText(configfile);
            webconfig = webconfig.Replace("7077", app.Port);
            File.WriteAllText(configfile, webconfig);
        }
    }
}
