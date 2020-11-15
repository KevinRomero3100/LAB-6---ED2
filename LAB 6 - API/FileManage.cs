
using System;
using System.Collections.Generic;
using LAB_6___Encryption_Algorithms.Encryption_Public_Key;
using LAB_6___Encryption_Algorithms.Auxiliares;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LAB_6___API
{
    public class FileManage
    {
        public string InFilePath { get; set; }
        public string InFileName { get; set; }
        public string OutFilePath { get; set; }
        public string OutFileName { get; set; }

        public void SaveFile(IFormFile file, string path, string name)
        {
            string file_path = path + $"\\Data\\Entrada\\{name}";
            if (File.Exists(file_path))
            {
                File.Delete(file_path);
            }
            using (var fs = new FileStream(file_path, FileMode.OpenOrCreate))
            {
                file.CopyTo(fs);
            }
            return;
        }

        public void DeleteFile(string path, string name)
        {
            string file_path = $"{path}\\Data\\temporal\\{name}";
            File.Delete(file_path);
        }

        public void CreateDirectoriesForData(string path)
        {
            var file_path = path + "\\Data";
            if (!Directory.Exists(file_path))
            {
                DirectoryInfo Data = Directory.CreateDirectory(file_path);
                DirectoryInfo Entrada = Directory.CreateDirectory(file_path + "\\Entrada");
                DirectoryInfo Salida = Directory.CreateDirectory(file_path + "\\Salida");

            }
        }

        public void GeyKeys(string path, string p, string q)
        {
            Parameters parameters = new Parameters();
            try
            {
                if(File.Exists($"{path}/../keys.zip"))
                    File.Delete($"{path}/../keys.zip");
                if (File.Exists(path + "\\public.key"))
                    File.Delete(path + "\\public.key");
                if (File.Exists(path + "\\private.key"))
                    File.Delete(path + "\\private.key");

                parameters.p = int.Parse(p);
                parameters.q = int.Parse(q);
                RSA_Algorithm keys = new RSA_Algorithm();
                parameters = keys.GetKey(parameters); ;

                FileStream publickeys = new FileStream((path + "\\public.key"), FileMode.Create) ;
                publickeys.Close();
                FileStream privatekeys = new FileStream((path + "\\private.key"), FileMode.Create);
                privatekeys.Close();

                using (StreamWriter writer = new StreamWriter(path + "\\public.key"))
                {
                    writer.WriteLine(parameters.n);
                    writer.WriteLine(parameters.e);
                }

                using (StreamWriter writer = new StreamWriter(path + "\\private.key"))
                {
                    writer.WriteLine(parameters.n);
                    writer.WriteLine(parameters.d);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}

