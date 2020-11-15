using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            FileManage data = new FileManage();
            data.CreateDirectoriesForData(environment.ContentRootPath);
            return text;
        }

        [HttpGet("rsa/keys/{p}/{q}")]
        public ActionResult GetKey(string p, string q)
        {
            try
            {
                var file_path = environment.ContentRootPath + "\\Data\\temporalKeys";
                if (!Directory.Exists(file_path))
                {
                    DirectoryInfo temporal = Directory.CreateDirectory(file_path);
                }
                FileManage keys = new FileManage();
                keys.GeyKeys(file_path, p, q);
                ZipFile.CreateFromDirectory($"{file_path}", $"{file_path}/../keys.zip");
                var fileStream = new FileStream($"{file_path}/../keys.zip", FileMode.Open);
                return File(fileStream, "application/zip");
            }
            catch (Exception)
            {

                return StatusCode(500);
            }
        }

        [HttpPost("rsa/{nombre}")]
        public ActionResult EncryptOrDecryptFile([FromForm] IFormFile file, [FromForm] IFormFile key, string nombre)
        {
            try
            {
                string file_path = environment.ContentRootPath;
                string file_name = file.FileName;
                string key_name = key.FileName;

                FileManage file_manager = new FileManage();
                file_manager.SaveFile(file, file_path, file_name);
                file_manager.SaveFile(key, file_path, key_name);
                file_manager.EncryptOrDecryptManage(file_path, file_name, key_name, nombre);

                FileStream result = new FileStream(file_manager.OutFilePath, FileMode.Open);
                return File(result, "text/plain", file_manager.OutFileName);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
