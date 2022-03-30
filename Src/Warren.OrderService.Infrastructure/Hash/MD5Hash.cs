using System;
using System.Security.Cryptography;
using System.Text;

namespace Warren.OrderService.Infrastructure.Hash
{
    public static class MD5Hash
    {
        public static ulong Md5Hash(string key)
        {
            using (var hash = System.Security.Cryptography.MD5.Create())
            {
                byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(key));
                var a = BitConverter.ToUInt64(data, 0);
                var b = BitConverter.ToUInt64(data, 8);
                ulong hashCode = a ^ b;
                return hashCode;
            }
        }

        public static string MD5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(source);
            string result = BitConverter.ToString(md5.ComputeHash(bytes));
            return result.Replace("-", "");
        }
    }
}
