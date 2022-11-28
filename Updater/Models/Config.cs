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
        public string Port { get; set; } = "";
        public string Address { get; set; } = "";
        public string Version { get; set; } = "";
    }
}
