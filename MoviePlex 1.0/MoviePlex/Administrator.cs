using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Xml.Serialization;

namespace MoviePlex
{
    class Administrator
    {

        public static void createNewPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var random = RandomNumberGenerator.Create()) {
                random.GetBytes(salt);
            }

            byte [] hashed = KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8);

            XmlSerializer serializer = new XmlSerializer(typeof(Password));
            using (FileStream stream = File.OpenWrite("password.cust"))
            {
                serializer.Serialize(stream, new Password(hashed, salt));
            }
        }

        public static bool matchPassword(string password)
        {
            XmlSerializer dser = new XmlSerializer(typeof(Password));

            using (FileStream stream = File.OpenRead("password.cust"))
            {
                Password pass = (Password)dser.Deserialize(stream);
                
                return Convert.ToBase64String(KeyDerivation.Pbkdf2(password, pass.salt, KeyDerivationPrf.HMACSHA1, 10000, 256 / 8)) == pass.getHashed();
            }

        }
    }
    public class Password
    {
        public byte[] hashed { get; set; }
        public byte[] salt { get; set; }

        public Password()
        {

        }
        public Password(byte[] hashed, byte[] salt)
        {
            this.hashed = hashed;
            this.salt = salt;
        }

        public string getHashed()
        {
            return Convert.ToBase64String(hashed);
        }
    }
}
