using System.Diagnostics;
using System.Net;
using System.IO.Compression;

namespace Updater
{
    public class Update: IDisposable
    {
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

        public string RegisterService(string path, string filename)
        {
            
            Ps.Command("cd " + App.Config.InstallPath + path);
            var output = Ps.Command(".\\" + filename + " -register");
            return output;
        }

        public virtual void Run() { }

        public void Dispose() { }
    }
}
