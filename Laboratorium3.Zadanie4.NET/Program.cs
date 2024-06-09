using System.Security.Cryptography;

namespace szyfr_RSA.zuzanna_lukiewska
{
    class Program
    {
        static void Main()
        {
            // Inicjalizacja RSA i AES
            var rsa = new RSACryptoServiceProvider(2048);
            var aes = Aes.Create();
            const string filePath = "test.txt"; 
            
            // Odczyt danych
            var data = File.ReadAllBytes(filePath);
            
            // Generowanie klucza AES
            aes.GenerateKey();
            
            // Szyfrowanie klucza AES RSA
            var encryptedAesKey = rsa.Encrypt(aes.Key, true);
            using var aesEncryptor = aes.CreateEncryptor();
            var encryptedData = aesEncryptor.TransformFinalBlock(data, 0, data.Length);
            
            // Zapis zaszyfrowanych danych
            File.WriteAllBytes(filePath + ".zaszyfrowane", encryptedData);
            File.WriteAllBytes(filePath + ".zaszyfrowanyKluczAes", encryptedAesKey);

            Console.WriteLine("Plik został zaszyfrowany.");
        
            // Odczyt danych z pliku
            var encryptedDataFromFile = File.ReadAllBytes(filePath + ".zaszyfrowane");
            var encryptedAesKeyFromFile = File.ReadAllBytes(filePath + ".zaszyfrowanyKluczAes");
            
            // Odszyfrowanie klucza AES RSA
            var decryptedAesKey = rsa.Decrypt(encryptedAesKeyFromFile, true);
            aes.Key = decryptedAesKey;
            using var aesDecryptor = aes.CreateDecryptor();
            var decryptedData = aesDecryptor.TransformFinalBlock(encryptedDataFromFile, 0, encryptedDataFromFile.Length);
            
            // Zapis odszyfrowanych danych
            File.WriteAllBytes(filePath + ".odszyfrowane", decryptedData);

            Console.WriteLine("Plik został odszyfrowany.");
        }
    }
}