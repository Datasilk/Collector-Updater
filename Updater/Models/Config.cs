namespace Updater.Models
{
    public class Config
    {
        public string Api { get; set; } = "";
        public string InstallPath { get; set; } = "";
        public List<ConfigApp> Apps { get; set; } = new List<ConfigApp>();
    }

    public class ConfigApp
    {
        public string Name { get; set; } = "";
        public List<ConfigReplaceFile> Replace { get; set; } = new List<ConfigReplaceFile>();
        public string Version { get; set; } = "";
    }

    public class ConfigReplaceFile
    {
        public string File { get; set; } = "";
        public string With { get; set; } = "";
    }
}
