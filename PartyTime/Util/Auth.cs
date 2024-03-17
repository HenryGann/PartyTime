using System.Text;
using System.Security.Cryptography;
using PartyTime.Models;

namespace PartyTime.Util
{
    public class Auth
    {
        public static string CalculateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string CalculateUserSHA256(string password, string salt)
        {
            return CalculateSHA256(password + salt);
        }
    }
}
