using System.Diagnostics;
using System.Net;
using System.IO.Compression;
using System.Management.Automation;
using System.Reflection;

namespace Updater
{
    /// <summary>
    /// Base Class used for all Apps defined within Collector's Updater
    /// </summary>
    public class AppBase: IDisposable
    {
        public virtual string Name { get; set; } = "";
        protected ILogger<UpdaterLogger> _logger { get; set; }

        public AppBase(ILogger<UpdaterLogger> logger)
        {
            _logger = logger;
        }
        public void Download(string url, string path, string filename)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            else
            {
                if(File.Exists(path + filename))
                {
                    File.Delete(path + filename);
                }
            }
            using (var client = new HttpClient())
            {
                byte[] bytes = client.GetByteArrayAsync(url).Result;
                File.WriteAllBytes(path + filename, bytes);

            }
        }

        public void DownloadApp(string name)
        {
            Console.WriteLine("Downloading app " + name + "...");
            var app = App.Config.Apps.Where(a => a.Name == name).FirstOrDefault();
            if(app == null) { return; }
            Download(App.Config.Api + $"/releases/{name}/{name}-{app.Version}.zip", App.Config.InstallPath, name + ".zip");
            //extract zip file
            Console.WriteLine("Extracting zip " + App.Config.InstallPath + name + ".zip to " + App.Config.InstallPath + name + "/");
            var zip = ZipFile.OpenRead(App.Config.InstallPath + name + ".zip");
            zip.ExtractToDirectory(App.Config.InstallPath + name, true);
            //delete zip file
            zip.Dispose();
            Console.WriteLine("Deleting zip file...");
            File.Delete(App.Config.InstallPath + name + ".zip");
        }

        #region "PowerShell"

        private PowerShell ps { get; set; } = PowerShell.Create();

        protected void StartApp(string path, string command, string appName = "")
        {
            var task = new Task(() =>
            {
                _logger.LogInformation("cd " + App.Config.InstallPath + path, DateTimeOffset.Now);
                ps.AddScript("cd " + App.Config.InstallPath + path);
                _logger.LogInformation(command, DateTimeOffset.Now);
                ps.AddScript(command).AddStatement();
                ps.AddCommand("Out-String");

                var outputCollection = new PSDataCollection<PSObject>();

                ps.Streams.Error.DataAdded += (object? sender, DataAddedEventArgs e) =>
                {
                    try
                    {
                        _logger.LogInformation(appName + ": " + ((PSDataCollection<ErrorRecord>?)sender)?[e.Index].ToString(), DateTimeOffset.Now);
                    }
                    catch (Exception) { }
                };
                var result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                while (!result.IsCompleted)
                {
                    if (outputCollection.Count > 0)
                    {
                        foreach (var o in outputCollection)
                        {
                            _logger.LogInformation(appName + ": " + o.ToString(), DateTimeOffset.Now);
                        }
                        outputCollection = new PSDataCollection<PSObject>();
                    }
                }
            });
            task.Start();
        }

        protected void StopApp()
        {
            try
            {
                ps.BeginStop(null, null);
            }
            catch (Exception) { }
        }


        #endregion

        #region "Events"
        public virtual void Update() { }

        public virtual void Start() { }

        public virtual void Stop() { }

        public void Dispose() { }
        #endregion
    }
}
