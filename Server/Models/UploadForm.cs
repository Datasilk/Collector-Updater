using System.Collections.Generic;

namespace Server.Models
{
    public class UploadForm
    {
        public string Version { get; set; }
        public string App { get; set; }
        public IFormFile File { get; set; }
    }
}
