using System.Diagnostics;
using System.Net;
using System.IO.Compression;

namespace Updater
{
    public class Update: IDisposable
    {
        public void Execute(string exe, string args, string path)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.CreateNoWindow = true;
            info.RedirectStandardError = true;
            info.RedirectStandardOutput = true;
            info.FileName = exe;
            info.UseShellExecute = false;
            info.WorkingDirectory = path;
            info.Arguments = args;
            Process fetchProcess = new Process();
            fetchProcess.StartInfo = info;
            fetchProcess.Start();
            //output Standard Error and Standard Output here.
            fetchProcess.WaitForExit();
            fetchProcess.Close();
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
            Download(App.Config.Api + $"/releases/{name}/{name}-{App.Config.Version}.zip", App.Config.InstallPath, name + ".zip");
            //extract zip file
            var zip = ZipFile.OpenRead(App.Config.InstallPath + name + ".zip");
            zip.ExtractToDirectory(App.Config.InstallPath + name, true);
            //delete zip file
            zip.Dispose();
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
