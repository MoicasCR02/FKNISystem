using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FKNI.Application.Utils
{
    internal class Cryptography
    {
        public static string Encrypt(string texto, string secret)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(texto);
            string hash = ComputeHash(secret.Substring(0, 32));
            byte[] key = Encoding.UTF8.GetBytes(hash); // 32 bytes        
            byte[] iv = [33, 24, 31, 46, 75, 64, 97, 18, 89, 10, 111, 132, 131, 144, 145, 250]; //16 bytes
            byte[] encryptedBytes;

            // Set up the encryption objects
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Encrypt the input plaintext using the AES algorithm
                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }
            //return string encrypt
            return Convert.ToBase64String(encryptedBytes);
        }

        private static string ComputeHash(string input)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (var c in data)
                {
                    sb.Append(c.ToString("x2"));
                }
                return sb.ToString();
            }
        }

    }
        public static class PasswordHasher
        {
            // Genera un hash seguro con salt usando PBKDF2
            public static string HashPassword(string password)
            {
                // Generar un salt aleatorio (16 bytes)
                byte[] salt = RandomNumberGenerator.GetBytes(16);

                // Generar el hash con PBKDF2
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
                byte[] hash = pbkdf2.GetBytes(32); // hash de 256 bits

                // Combinar salt + hash en un solo arreglo
                byte[] hashBytes = new byte[48]; // 16 + 32
                Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
                Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

                // Retornar en Base64 para guardar en DB
                return Convert.ToBase64String(hashBytes);
            }

            // Verifica si la contraseña es correcta comparando con el hash guardado
            public static bool VerifyPassword(string password, string hashedPassword)
            {
                // Convertir de Base64 a bytes
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Extraer el salt (primeros 16 bytes)
                byte[] salt = new byte[16];
                Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

                // Extraer el hash original (últimos 32 bytes)
                byte[] storedHash = new byte[32];
                Buffer.BlockCopy(hashBytes, 16, storedHash, 0, 32);

                // Generar un nuevo hash con el salt original
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
                byte[] newHash = pbkdf2.GetBytes(32);

                // Comparar byte por byte
                for (int i = 0; i < 32; i++)
                {
                    if (storedHash[i] != newHash[i])
                        return false;
                }
                return true;
            }
        }
    }