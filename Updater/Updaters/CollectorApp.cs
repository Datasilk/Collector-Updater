namespace Updater.Updaters
{
    public class CollectorApp : Update
    {
        public override void Run()
        {
            Console.WriteLine("Updating Collector...");
            DownloadApp("collector");
            var app = App.Config.Apps.Where(a => a.Name == "collector").FirstOrDefault() ?? new Models.ConfigApp() { Port = "7077" };
            Console.WriteLine("Updating web.config, changing port for Collector to " + app.Port);
            //update web.config (which will restart the app)
            var configfile = App.Config.InstallPath + "collector" + "\\web.config";
            var webconfig = File.ReadAllText(configfile);
            webconfig = webconfig.Replace("7070", app.Port);
            File.WriteAllText(configfile, webconfig);
            Console.WriteLine("Saved web.config");
        }
    }
}
