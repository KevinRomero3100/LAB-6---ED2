using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LAB_6___API.Controllers
{

    [Route("api")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        private IWebHostEnvironment environment;
        public EncryptionController(IWebHostEnvironment env)
        {
            environment = env;
        }

        [HttpGet]
        public string Index()
        {
            string text = "\t\t\t- LAB 6 -\n\nKevin Romero 1047519\nJosé De León 1072619";
            return text;
        }
        [HttpPost("rsa/{nombre}")]
        public ActionResult EncryptOrDecryptFile([FromForm] IFormFile file, [FromForm] IFormFile key, string nombre)
        {
           /* try
            {*/
                string file_path = environment.ContentRootPath;
                string file_name = file.FileName;
                string key_name = key.FileName;

                FileManage file_manager = new FileManage();
                file_manager.SaveFile(file, file_path, file_name);
                file_manager.SaveFile(key, file_path, key_name);
                file_manager.EncryptOrDecryptManage(file_path, file_name, key_name , nombre);
                file_manager.DeleteFile(file_path, file_name);
                file_manager.DeleteFile(file_path, key_name);

                FileStream result = new FileStream(file_manager.OutFilePath, FileMode.Open);
                return File(result, "text/plain", file_manager.OutFileName);
           /* }
            catch (Exception)
            {
                return StatusCode(500);
            }*/
        }
    }
}
