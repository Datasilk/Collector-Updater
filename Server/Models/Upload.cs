namespace Server.Models
{
    public class Upload
    {
        public string[] Versions { get; set; } = new string[] { };
        public bool FailedUpload { get; set; } = false;
        public string Error { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
