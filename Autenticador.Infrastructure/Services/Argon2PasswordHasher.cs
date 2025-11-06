using Autenticador.Application.Common.Interfaces;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace Autenticador.Infrastructure.Services
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        private readonly int _saltLength = 16;
        private readonly string _argonPrefix = "$argon2id$";

        public string HashPassword(string password)
        {
            var salt = GenerateSalt(_saltLength);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                MemorySize = 65536,
                Iterations = 4
            };

            var hashBytes = argon2.GetBytes(32);
            var saltString = Convert.ToBase64String(salt);
            var hashString = Convert.ToBase64String(hashBytes);

            return $"{_argonPrefix}v=19$m=65536,t=4,p=8${saltString}${hashString}";

            static byte[] GenerateSalt(int length)
            {
                var buffer = new byte[length];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(buffer);
                }
                return buffer;
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                // $argon2id$v=19$m=65536,t=4,p=8$SALT$HASH)
                var parts = hash.Split('$');
                if (parts.Length != 6) return false;

                var saltBytes = Convert.FromBase64String(parts[4]);
                var hashBytes = Convert.FromBase64String(parts[5]);

                var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = saltBytes,
                    DegreeOfParallelism = 8,
                    MemorySize = 65536,
                    Iterations = 4
                };

                var computedHashBytes = argon2.GetBytes(32);

                return CryptographicOperations.FixedTimeEquals(hashBytes, computedHashBytes);
            }
            catch
            {
                return false;
            }
        }
    }
}
