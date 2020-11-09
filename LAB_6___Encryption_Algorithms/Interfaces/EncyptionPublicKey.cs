using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace LAB_6___Encryption_Algorithms.Interfaces
{
    interface EncyptionPublicKey
    {
        public void GetKey(int p, int q);
        public byte[] EncryptOrDecrypt(int n, int d, int e, byte[] bufer);
    }
}
