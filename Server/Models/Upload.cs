namespace Server.Models
{
    public class Upload
    {
        public List<AppVersion> Versions { get; set; } = new List<AppVersion>();
        public bool FailedUpload { get; set; } = false;
        public string Error { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
