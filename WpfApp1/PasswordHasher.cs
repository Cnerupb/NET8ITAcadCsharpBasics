using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utilities
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int Iterations = 2000;

        /// <summary>
        /// Создает хэш пароля в формате: {iterations}.{base64(salt+hash)}
        /// </summary>
        public static string HashPassword(string password)
        {
            // Генерируем соль
            byte[] salt;
            using (var rng = new RNGCryptoServiceProvider())
            {
                salt = new byte[SaltSize];
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Объединяем соль и хэш
                byte[] hashBytes = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                // Преобразуем в строку (base64)
                string base64Hash = Convert.ToBase64String(hashBytes);

                return $"{Iterations}.{base64Hash}";
            }

        }

        /// <summary>
        /// Проверяет введенный пароль по хранимому хэшу.
        /// </summary>
        public static bool VerifyPassword(string enteredPassword, string storedHash) 
        {
            // Разбиваем строку по разделителю '.'
            var splitted = storedHash.Split('.');
            if (splitted.Length != 2) return false;

            int iterations = int.Parse(splitted[0]);
            byte[] hashBytes = Convert.FromBase64String(splitted[1]);

            // Извлекаем соль
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Вычисляем хэш для введенного пароля
            using (var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Сравниваем полученный хэш с тем, что хранится в БД
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash[i]) return false;
                }
            }
            return true;

        }

    }

}
