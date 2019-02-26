using NETCore.Encrypt;
using NETCore.Encrypt.Internal;
using System;
using System.Security.Cryptography;

namespace GenerateRsaKeys {
    class Program {
        static void Main(string[] args) {

            RSA rsa = RSA.Create();
            //RSAKey key = EncryptProvider.CreateRsaKey();
            string publicKey = rsa.ToXmlString(true);
            Console.ReadKey();
        }
    }
}
