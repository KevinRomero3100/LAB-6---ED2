using System;
using System.Collections.Generic;
using LAB_6___Encryption_Algorithms.Auxiliares;
using System.Dynamic;
using System.Text;

namespace LAB_6___Encryption_Algorithms.Interfaces
{
    interface EncyptionPublicKey
    {
        public Parameters GetKey(Parameters data);
        public byte[] Encrypt(Parameters data, byte[] bufer);
        //public byte[] Decrypt(Parameters data, byte[] bufer);
    }
}
