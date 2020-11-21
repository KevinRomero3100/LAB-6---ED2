using System;
using LAB_6___Encryption_Algorithms.Auxiliares;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
using System.Numerics;

namespace LAB_6___Encryption_Algorithms.Encryption_Public_Key
{
    public class RSA_Algorithm : Interfaces.EncyptionPublicKey
    {
        private int Fhi { get; set; }
        private int P { get; set; }
        private int Q { get; set; }
        private int E { get; set; }
        private int D { get; set; }

        private int N { get; set; }

        List<long> outrsaEncrypt = new List<long>();
        List<byte> outrsaDencrypt = new List<byte>();

        #region Encrypt

        public byte[] Encrypt(Parameters data, byte[] bufer)
        {
            foreach (var item in bufer)
            {
                outrsaEncrypt.Add((long)BigInteger.ModPow(item, data.e, data.n));
            }
            return ConvertToByte();
        }
        byte[] ConvertToByte()
        {
            List<byte> validBytes = new List<byte>();
            string binaryCode = "";
            int binaryMax = Convert.ToString(outrsaEncrypt.Max(), 2).Length;
            foreach (var item in outrsaEncrypt)
            {
                binaryCode += Convert.ToString(item, 2).PadLeft( binaryMax, '0');
                while(binaryCode.Length > 8)
                {
                    validBytes.Add(Convert.ToByte(binaryCode.Substring(0, 8), 2));
                    binaryCode = binaryCode.Remove(0, 8);
                }
            }
            if (binaryCode.Length > 0 && binaryCode.Length < 8 )
            {
                
                validBytes.Add(Convert.ToByte(binaryCode.PadRight(8, '0'), 2));
                binaryCode = "";
            }
            else if (binaryCode.Length == 8)
            {
                validBytes.Add(Convert.ToByte(binaryCode, 2));
            }
            validBytes.Insert(0, Convert.ToByte(binaryMax));
            return validBytes.ToArray();
        } // Pasa a bytes todos los bytes cifrados por rsa
        #endregion

        #region KEY
        public Parameters GetKey(Parameters data)
        {
            P = data.p;
            Q = data.q;
            N = P * Q;
            if (CheckPrimeNumber(P) && CheckPrimeNumber(Q))
            {
                Parameters keys = new Parameters() { p = P, q = Q};
                keys.n = P * Q;
                Fhi = (P - 1) * (Q - 1);
                GetE();
                D = GetD();

                if (D == E)
                {
                    D += Fhi;
                }

                keys.e = E;
                keys.d = D;
                return keys;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
        int GetD()
        {
            List<int> filaA = new List<int>();
            List<int> filaB = new List<int>();
            int auxSalida = 0;
            int auxDivisor;
            int newA;
            int newB;

            filaA.Add(Fhi);
            filaB.Add(Fhi);
            filaA.Add(E);
            filaB.Add(1);

            for (int i = 0; auxSalida != 1; i++)
            {
                auxDivisor = filaA[i] / filaA[i + 1];
                newA = filaA[i] -(auxDivisor* filaA[i + 1]);
                newB = filaB[i]- (auxDivisor * filaB[i + 1]);

                while (newB < 0)
                {
                    newB = ((newB % Fhi) + Fhi) % Fhi;
                }
                filaA.Add(newA);
                filaB.Add(newB);
                auxSalida = newA;
            }
            return filaB[filaB.Count - 1];
        }
        void GetE()
        {
            List<int> randoms = new List<int>();
            Random eRandom = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int e = 1;
            do
            { 
                e++;

            } while (!chekedE(e) && e < Fhi);
            E = e;
        }

        
        bool CheckPrimeNumber(int valor)
        {
            int count = 0;
            for (int i = 1; i < (valor + 1); i++)
            {
                if (valor % i == 0)
                {
                    count++;
                }
            }
            if (count != 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        List<int> GetMultiples(int value)
        {
            List<int> multiples = new List<int>();
            for (int i = 1; i <= value; i++)
            {
                if (value % i == 0 && i > 1) multiples.Add(i);
            }
            return multiples;
        }
        bool chekedE(int e)
        {

            if (CheckPrimeNumber(e))
            {
                List<int> multiplesFhi = GetMultiples(Fhi);
                List<int> multiplesE = GetMultiples(e);
                List<int> multiplesN = GetMultiples(N);

                foreach (var item in multiplesFhi)
                {
                    if (multiplesE.Contains(item))
                    {
                        return false;
                    }
                }
                foreach (var item in multiplesN)
                {
                    if (multiplesE.Contains(item))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        #endregion

        public byte[] Decrypt(Parameters data, byte[] bufer)
        {
            List<byte> decrypt = bufer.ToList();
            int originalBinariLength = decrypt[0];
            decrypt.RemoveAt(0);
            N = data.n;
            D = data.d;
            ConvertToByte(decrypt, originalBinariLength);
            return outrsaDencrypt.ToArray();
        }

        void ConvertToByte(List<byte> content, int originalBinary)
        {
            int cout = 0;
            try
            {

           
            string binaryCode = "";

            foreach (var item in content)
            {
                    cout++;
                 binaryCode += Convert.ToString(item, 2).PadLeft(8, '0');
                while(binaryCode.Length > originalBinary)
                {
                    var newInt = Convert.ToInt64(binaryCode.Substring(0, originalBinary), 2);
                    var test = (byte)BigInteger.ModPow(newInt, D, N);
                    outrsaDencrypt.Add(Convert.ToByte(test));
                    binaryCode = binaryCode.Remove(0, originalBinary);
                }

            }
            }
            catch (Exception)
            {
                var test = cout;
                throw;
            }
        }
    }
}
