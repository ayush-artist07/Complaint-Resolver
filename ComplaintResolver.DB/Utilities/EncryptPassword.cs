using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComplaintResolver.DB.Utilities
{
    public class EncryptPassword
    {
        /// <summary>
        /// Encrypt the password given in the form of strings
        /// </summary>
        /// <param name="password"></param>
        /// <returns>SHA hashed password for the given string</returns>
        public static string Hash(string password)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
