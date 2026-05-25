using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace NewLife.Helpers
{
    public static class HashHelper
    {
        public static string Sha256(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Concat(bytes.Select(b => b.ToString("x2")));
            }
        }

        // Detecta si un string ya es un hash SHA-256 (64 hex chars)
        public static bool EsHash(string s)
            => s != null && s.Length == 64 && Regex.IsMatch(s, "^[0-9a-f]{64}$");
    }
}
