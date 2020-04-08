using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace toupiao.Controllers
{
    public class PublicController : Controller
    {

        private readonly IWebHostEnvironment _env;
        public PublicController(
            IWebHostEnvironment env)
        {
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetFile(string FileName)
        {
            var imageBytes = System.IO.File.OpenRead(
                $"{_env.ContentRootPath}/Data/Files/"+FileName);

            return File(
                imageBytes,
                ControllerMix.GetContentType(FileName.Split(".")[0]));
        }
    }
}