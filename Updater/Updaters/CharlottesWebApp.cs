namespace Updater.Updaters
{
    public class CharlottesWebApp : Update
    {
        public override void Run()
        {
            DownloadApp("charlottes-web");
            var app = App.Config.Apps.Where(a => a.Name == "charlottes-web").FirstOrDefault() ?? new Models.ConfigApp() { Port = "7077" };

            //update web.config (which will restart the app)
            var configfile = App.Config.InstallPath + "charlottes-web" + "\\web.config";
            var webconfig = File.ReadAllText(configfile);
            webconfig = webconfig.Replace("7070", app.Port);
            File.WriteAllText(configfile, webconfig);
        }
    }
}
