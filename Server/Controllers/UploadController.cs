using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Controller]
    [Route("Upload")]
    public class UploadController : Controller
    {
        public ActionResult Index()
        {
            return View(new Models.Upload());
        }

        [HttpPost]
        public ActionResult Index([FromForm]Models.UploadForm form)
        {
            if(form == null || form.File == null) { 
                return View(new Models.Upload() { Error = "No file uploads specified"}); 
            }
            //save file as next app release version zip file in public folder
            var file = form.File;
            var app = form.App;
            var version = form.Version;
            var filename = "releases/" + app + "/" + app + "-" + version + ".zip";
            using (var stream = new FileStream(App.MapPath("/wwwroot/" + filename), FileMode.Create))
            {
                file.CopyToAsync(stream).Wait();
            }
            try
            {
                //update config.json with updated app version information
                App.Config.Apps.Where(a => a.Name == app).First().Version = version;
                System.IO.File.WriteAllText(App.MapPath("config.json"), JsonSerializer.Serialize(App.Config, new JsonSerializerOptions()
                {
                    WriteIndented = true
                }));
            }
            catch (Exception ex)
            {
                return View(new Models.Upload() { Error = ex.Message });
            }

            return View(new Models.Upload()
            {
                Message = "Uploaded <a href=\"/" + filename + "\" target=\"_blank\">" + filename + "</a> successfully."
            });
        }
    }
}
