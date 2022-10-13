namespace Server.Models
{
    public class Config
    {
        public List<AppVersion> Apps { get; set; } = new List<AppVersion>();
    }

    public class AppVersion
    {
        public string Name { get; set; } = "";
        public string Version { get; set; } = "";
    }
}
