
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
            string file_path = path + $"\\Data\\temporal\\{name}";
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


        public void EncryptOrDecryptManage(string path, string file_name, string key_name, string nameOut)
        {

            byte[] buffer;

            string file_path = path + $"\\Data\\temporal\\{file_name}";
            using (FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate))
            {
                buffer = new byte[fs.Length];
                using (var br = new BinaryReader(fs))
                {
                    br.Read(buffer, 0, buffer.Length);
                }
            }

            Parameters parameters = new Parameters();
            string file_path_key = path + $"\\Data\\temporal\\{key_name}";
            string[] nameKey = key_name.Split(".");
            using(StreamReader keys = new StreamReader(file_path_key))
            {
                parameters.n = int.Parse(keys.ReadLine());
                if (nameKey[0] == "private")
                {
                    parameters.d = int.Parse(keys.ReadLine());
                    parameters.e = parameters.d;
                }
                else if (nameKey[0] == "public")
                {
                    parameters.e = int.Parse(keys.ReadLine());
                    parameters.d = parameters.e;
                }

            }

            byte[] result;
            string extension = file_name.Split(".")[1];
            switch (extension)
            {
                case "txt":
                    result = new RSA_Algorithm().Encrypt(parameters,buffer);
                    OutFileName = nameOut + ".rsa";
                    OutFilePath = path + $"\\Data\\Salida\\" + OutFileName + ".rsa";
                    break;
                case "rsa":
                    result = new RSA_Algorithm().Decrypt(parameters, buffer);
                    OutFileName = nameOut + ".txt";
                    OutFilePath = path + $"\\Data\\Salida\\" + OutFileName + ".txt";
                    break;
                default: throw new Exception();
            }
            using (var fs = new FileStream(OutFilePath, FileMode.OpenOrCreate))
            {
                fs.Write(result, 0, result.Length);
            }
        }


        public static byte[] ConvertToByte(string content)
        {
            byte[] array = new byte[content.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToByte(content[i]);
            }
            return array;
        }
    }
}

