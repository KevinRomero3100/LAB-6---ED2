using LAB_6___Encryption_Algorithms.Encryption_Public_Key;
using LAB_6___Encryption_Algorithms.Auxiliares;
using System;
using System.Security.Cryptography;

namespace LAB_6___ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            RSA_Algorithm rsa = new RSA_Algorithm();
            Parameters parameters = new Parameters() { p = 461, q = 631};
            string texto = "Mejor que buscar la verdad sin método es no pensar nunca en ella, porque los estudios desordenados y las meditaciones oscuras turban las luces naturales de la razón y ciegan la inteligencia. -René Descártes";

            parameters = rsa.GetKey(parameters);
            Console.WriteLine("\n-------- Key -----------------------------------------------------------------------------------------------\nTEXTO CIFRADO");

            Console.WriteLine("Valor n: " + parameters.n + "\nValor D: " + parameters.d + "\nValor E: " + parameters.e);


            Console.WriteLine("\n\nTEXTO ORIGINAL          \n" + texto);

            Console.WriteLine("\n-------- RSA Encriptado-----------------------------------------------------------------------------------------------\nTEXTO CIFRADO");
            byte[] result_encrypt1 = rsa.Encrypt( parameters,ConvertToByte(texto));
            Console.WriteLine(ConvertToChar(result_encrypt1));



            Console.ReadLine();
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

        public static char[] ConvertToChar(byte[] content)
        {
            char[] array = new char[content.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Convert.ToChar(content[i]);
            }
            return array;
        }
    }
}
