using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace FileUploader.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileSystemProvider _provider;

        public HomeController(IFileSystemProvider provider)
        {
            _provider = provider;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(IFormCollection files)
        {
            var result = new UploadResult();

            foreach (var item in files.Files)
            {
                #region 把文件流转化为字节流
                byte[] buffer = new byte[item.Length];

                Stream fs = item.OpenReadStream();

                fs.Read(buffer, 0, buffer.Length);
                #endregion

                UploadConfig config = new UploadConfig
                {
                    Buffer = buffer,
                    FileName = item.FileName,
                    Chunked = files.Keys.Contains("chunk"),
                    PreviosName = files["previosName"]
                };

                result = _provider.Upload(config);
            }
            return Json(result);
        }
    }
}

