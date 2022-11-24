namespace Updater.Updaters
{
    public class CharlotteApp: Update
    {
        public override void Run()
        {
            DownloadApp("charlotte");
            var app = App.Config.Apps.Where(a => a.Name == "charlotte").FirstOrDefault() ?? new Models.ConfigApp() { Port = "7077"};

            //try to register Charlotte as a Windows Service
            try
            {
                Ps.Command("New-Service -Name \"Charlotte\" -BinaryPathName \"" + App.Config.InstallPath + "charlotte\\Charlotte.exe\" -Description \"Navigates to a given URL and extracts all relevant DOM data as JSON\" -DisplayName \"Charlotte\" -StartupType Automatic");
            }
            catch (Exception) { }

            //update appsettings.json with updated port number
            var configfile = App.Config.InstallPath + "charlotte" + "\\appsettings.json";
            var contents = File.ReadAllText(configfile);
            contents = contents.Replace("7077", app.Port);
            File.WriteAllText(configfile, contents);

            //update web.config (which will restart the app)
            configfile = App.Config.InstallPath + "charlotte" + "\\web.config";
            contents = File.ReadAllText(configfile);
            contents = contents.Replace("7077", app.Port);
            File.WriteAllText(configfile, contents);
        }
    }
}
