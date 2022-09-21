namespace Updater.Updaters
{
    public class CollectorApp : Update
    {
        public override void Run()
        {
            DownloadApp("collector");
            var app = App.Config.Apps.Where(a => a.Name == "collector").FirstOrDefault() ?? new Models.ConfigApp() { Port = "7077" };

            //update web.config (which will restart the app)
            var configfile = App.Config.InstallPath + "collector" + "\\web.config";
            var webconfig = File.ReadAllText(configfile);
            webconfig = webconfig.Replace("7070", app.Port);
            File.WriteAllText(configfile, webconfig);
        }
    }
}
