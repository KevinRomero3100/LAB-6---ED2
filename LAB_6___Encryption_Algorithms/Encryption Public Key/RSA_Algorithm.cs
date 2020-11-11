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

        List<long> outrsaEncryp = new List<long>();
        List<byte> outrsaDencryp = new List<byte>();
        List<long> inRsa = new List<long>();

        #region Encrypt

        public byte[] Encrypt(Parameters data, byte[] bufer)
        {
            foreach (var item in bufer)
            {
                var bigIntegerOperation = BigInteger.Pow(item, data.e) % data.n;
                outrsaEncryp.Add((long)bigIntegerOperation);
            }
            return ConvertToByte();
        }
        byte[] ConvertToByte()
        {
            List<byte> validBytes = new List<byte>();
            string binaryCode = "";
            int splitSize = 8;
            byte newbyte;

            int binaryMax = Convert.ToString(outrsaEncryp.Max(), 2).Length;

            foreach (var item in outrsaEncryp)
            {
                var newBinary = Convert.ToString(item, 2);
                if (newBinary.Length <= binaryMax)
                {
                    //Balance de 0 faltantes para llagar al maximo re querido
                    while (newBinary.Length != binaryMax)
                    {
                        newBinary = "0" + newBinary;
                    }
                    binaryCode += newBinary;
                }
            }

            for (int i = 0; i < binaryCode.Length; i += splitSize)
            {
                if (i + splitSize > binaryCode.Length)
                {
                    splitSize = binaryCode.Length - i;
                    var split = binaryCode.Substring(i, splitSize);
                    while (split.Length < 8)
                    {
                        split = split + "0";
                    }
                    newbyte = Convert.ToByte(split, 2);
                    validBytes.Add(newbyte);
                }
                else
                {
                    newbyte = Convert.ToByte(binaryCode.Substring(i, splitSize), 2);
                    validBytes.Add(newbyte);
                }
            }
            validBytes.Insert(0, Convert.ToByte(binaryMax));
            return validBytes.ToArray();
        } // Pasa a bytes todos los bytes cifrados por rca
        #endregion

        #region KEY
        public Parameters GetKey(Parameters data)
        {
            P = data.p;
            Q = data.q;
            if (CheckPrimeNumber(P) && CheckPrimeNumber(Q))
            {
                Parameters keys = new Parameters() { p = P, q = Q};
                keys.n = P * Q;
                Fhi = (P - 1) * (Q - 1);
                GetE();
                D = GetD();
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

                foreach (var item in multiplesFhi)
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


    }
}
