using System;
using System.Security.Cryptography;
using System.Text;

namespace UserService.Utilities.Logic
{
    public class PasswordUtility
    {
        public virtual string ToHash(string password)
        {
            //password check
            if (password == null)
                throw new ArgumentException("Null password");

            if (password == string.Empty)
                throw new ArgumentException("Empty password");

            //hashing
            UnicodeEncoding encoding = new();
            byte[] passwordBytes = encoding.GetBytes(password);
            SHA512 provider = SHA512.Create();
            byte[] hashBytes = provider.ComputeHash(passwordBytes);

            //from bytes to string
            string hash = "";
            foreach (byte hashByte in hashBytes)
                hash += string.Format("{0:x2}", hashByte);

            return hash.ToUpper();
        }
    }
}