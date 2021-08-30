using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RPS.Web.Controllers
{
    [Route("api/upload/[action]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        public IWebHostEnvironment WebHostEnvironment { get; set; }

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Content("Test");
        }

        [HttpPost]
        public IActionResult SaveTest(IFormFile file)
        {
            return Content("SaveTest");
        }

        [HttpPost]
        public IActionResult Save(IFormFile file)
        {
            // The Name of the Upload component is "files".
            if (file != null)
            {
                //foreach (var file in files)
                //{
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    // Some browsers send file names with full path.
                    // The sample is interested only in the file name.
                    var fileName = Path.GetFileName(fileContent.FileName.ToString().Trim('"'));
                    var physicalPath = Path.Combine(WebHostEnvironment.WebRootPath, "App_Data", fileName);

                    using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                    {

                         file.CopyTo(fileStream);
                    }
                //}
            }

            // Return an empty string to signify success.
            return Content("");
        }
    }
}
