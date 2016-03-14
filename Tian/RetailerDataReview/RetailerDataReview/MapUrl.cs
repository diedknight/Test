using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RetailerDataReview
{
    public class MapUrl
    {
        private const int BUFFER_SIZE = 128 * 1024;
        private const ulong TAG = 0xFC010203040506CF;

        public string GetUrl(int retailerId, int retailerCountry, string retailerName)
        {
           return this.EncodeSolidShopLocationUpdParam(retailerId, retailerCountry, retailerName);
        }

        private string EncodeSolidShopLocationUpdParam(int retailerId, int retailerCountry, string retailerName)
        {
            string pwd = string.Format("ag*1({0}%4^a", retailerId);
            string param = string.Format("{0}:{1}:{2}", DateTime.Now.Year, DateTime.Now.Month, retailerName);
            byte[] bytes = Encoding.UTF8.GetBytes(param);
            MemoryStream msm = new MemoryStream(bytes);
            Stream st = EncryptStream(msm, pwd);
            bytes = new byte[st.Length];
            st.Read(bytes, 0, bytes.Length);
            st.Close();
            msm.Close();

            string siteUrl = GetRootUrl(retailerCountry);

            return string.Format(siteUrl + "/SolidShopLocationUpd.aspx?rid={0}&param={1}", retailerId, Convert.ToBase64String(bytes).Replace('+', '_').Replace('/', '-'));
        }

        private Stream EncryptStream(Stream sourceStream, string pwd)
        {
            sourceStream.Position = 0;
            MemoryStream targetStream = new MemoryStream();

            long sourceSize = sourceStream.Length;
            byte[] bytes = new byte[BUFFER_SIZE];
            int read = -1;
            int value = 0;

            byte[] iv = GenerateRandomBytes(16);
            byte[] salt = GenerateRandomBytes(16);

            SymmetricAlgorithm sam = CreateRijndael(pwd, salt);
            sam.IV = iv;

            targetStream.Write(iv, 0, iv.Length);
            targetStream.Write(salt, 0, salt.Length);

            HashAlgorithm hasher = SHA256.Create();
            CryptoStream count = new CryptoStream(targetStream, sam.CreateEncryptor(), CryptoStreamMode.Write), chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write);
            BinaryWriter bw = new BinaryWriter(count);
            bw.Write(sourceSize);
            bw.Write(TAG);

            while ((read = sourceStream.Read(bytes, 0, bytes.Length)) != 0)
            {
                count.Write(bytes, 0, read);
                chash.Write(bytes, 0, read);
                value += read;
            }

            chash.Flush();
            chash.Close();

            byte[] hash = hasher.Hash;

            count.Write(hash, 0, hash.Length);
            count.FlushFinalBlock();// 不写这句的话，解码里的这句：cin.Read(oldHash, 0, oldHash.Length); 会报错。
            count.Flush();
            //count.Close();
            targetStream.Position = 0;
            return targetStream;
        }

        private byte[] GenerateRandomBytes(int fCount)
        {
            byte[] bytes = new byte[fCount];
            RandomNumberGenerator fRandomNumberGenerator = new RNGCryptoServiceProvider();
            fRandomNumberGenerator.GetBytes(bytes);

            return bytes;
        }

        private SymmetricAlgorithm CreateRijndael(string fPassword, byte[] fSalt)
        {
            PasswordDeriveBytes fPasswordDeriveBytes = new PasswordDeriveBytes(fPassword, fSalt, "SHA256", 1000);
            SymmetricAlgorithm fSymmetricAlgorithm = Rijndael.Create();
            fSymmetricAlgorithm.KeySize = 256;
            fSymmetricAlgorithm.Key = fPasswordDeriveBytes.GetBytes(32);
            fSymmetricAlgorithm.Padding = PaddingMode.PKCS7;
            return fSymmetricAlgorithm;
        }

        private string GetRootUrl(int countryID)
        {
            var rootUrl = "";
            if (countryID == 3)
                rootUrl = "http://www.priceme.co.nz";
            else if (countryID == 1)
            {
                rootUrl = "http://www.priceme.com.au";
            }
            else if (countryID == 28)
            {
                rootUrl = "http://www.priceme.com.ph";
            }
            else if (countryID == 41)
            {
                rootUrl = "http://www.priceme.com.hk";
            }
            else if (countryID == 36)
            {
                rootUrl = "http://www.priceme.com.sg";
            }
            else if (countryID == 45)
            {
                rootUrl = "http://www.priceme.com.my";
            }
            else if (countryID == 51)
            {
                rootUrl = "http://track.priceme.co.id";
            }
            else if (countryID == 55)
            {
                rootUrl = "http://www.priceme.com";
            }
            else if (countryID == 56)
            {
                rootUrl = "http://www.priceme.com.vn";
            }
            else
                rootUrl = "http://www.priceme.co.nz";
            return rootUrl;
        }

    }
}
