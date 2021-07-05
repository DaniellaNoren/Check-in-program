//using System;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using System.Text;
//using Xunit;

//namespace CheckInProgram.Test
//{
//    public class SymmetricEncryptionTest
//    {

//        [Fact]
//        public void GenerateRandomAESKey()
//        {
//            SymmetricAlgorithm aes = SymmetricAlgorithm.Create("AES");
//            byte[] key = aes.Key;
//            byte[] iv = aes.IV;

//            Assert.True(key.Length == 32);
//            Assert.True(iv.Length == 16);
//        }

//        [Fact]
//        public void EncryptMessageWithAES()
//        {
//            string inputMessage = "Alice knows Bob's secret.";
//            SymmetricAlgorithm aes = SymmetricAlgorithm.Create("AES");   //create aes key

//            byte[] cipherText = AESHelper.EncryptWithAes(inputMessage, aes);

//            byte[] key = aes.Key;
//            byte[] iv = aes.IV;

//            string outputMessage = AESHelper.DecryptWithAes("", key, iv);

//            Assert.Equal(outputMessage, inputMessage);
        
//        }
//    }
//}
