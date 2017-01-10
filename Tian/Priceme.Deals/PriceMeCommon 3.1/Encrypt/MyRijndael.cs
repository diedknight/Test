/**
 * 
 * 改自：
 * http://social.microsoft.com/Forums/zh-CN/1761/thread/f9b64c85-8032-4417-a38d-e73b2134b50c
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PriceMeCommon.Encrypt{
    public class MyRijndael {
        private const ulong TAG = 0xFC010203040506CF;
        private const int BUFFER_SIZE = 128 * 1024;

        private static bool EqualTwoByteArray(byte[] fByteA, byte[] fByteB) {
            if (fByteA.Length == fByteB.Length) {
                for (int i = 0; i < fByteA.Length; i++) {
                    if (fByteA[i] != fByteB[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建加密对象以执行 Rijndael 算法
        /// </summary>
        /// <param name="fPassword">从其导出密钥的密码</param>
        /// <param name="fSalt">用以导出密钥的密钥 salt</param>
        /// <returns>加密对象</returns>
        private static SymmetricAlgorithm CreateRijndael(string fPassword, byte[] fSalt) {
            PasswordDeriveBytes fPasswordDeriveBytes = new PasswordDeriveBytes(fPassword, fSalt, "SHA256", 1000);
            SymmetricAlgorithm fSymmetricAlgorithm = Rijndael.Create();
            fSymmetricAlgorithm.KeySize = 256;
            fSymmetricAlgorithm.Key = fPasswordDeriveBytes.GetBytes(32);
            fSymmetricAlgorithm.Padding = PaddingMode.PKCS7;
            return fSymmetricAlgorithm;
        }

        /// <summary>
        /// 加密文件随机数生成
        /// </summary>
        private static RandomNumberGenerator fRandomNumberGenerator = new RNGCryptoServiceProvider();

        /// <summary>
        /// 生成指定长度的随机 Byte 数组
        /// </summary>
        /// <param name="fCount"> Byte 数组长度</param>
        /// <returns>随机 Byte 数组</returns>
        private static byte[] GenerateRandomBytes(int fCount) {
            byte[] bytes = new byte[fCount];
            fRandomNumberGenerator.GetBytes(bytes);

            return bytes;
        }

        public static Stream EncryptStream(Stream sourceStream, string pwd) {
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

            while ((read = sourceStream.Read(bytes, 0, bytes.Length)) != 0) {
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

        public static Stream DecryptStream(Stream sourceStream, string pwd) {
            sourceStream.Position = 0;
            MemoryStream targetStream = new MemoryStream();

            byte[] bytes = new byte[BUFFER_SIZE];
            int read = -1;
            int value = 0;
            int outValue = 0;

            byte[] iv = new byte[16];
            sourceStream.Read(iv, 0, 16);
            byte[] salt = new byte[16];
            sourceStream.Read(salt, 0, 16);

            SymmetricAlgorithm sam = CreateRijndael(pwd, salt);
            sam.IV = iv;

            value = 32;
            long sourceSize = -1;

            HashAlgorithm hasher = SHA256.Create();

            CryptoStream cin = new CryptoStream(sourceStream, sam.CreateDecryptor(), CryptoStreamMode.Read);
            CryptoStream chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write);
            BinaryReader br = new BinaryReader(cin);
            sourceSize = br.ReadInt64();
            ulong tag = br.ReadUInt64();

            if (tag != TAG) {
                throw new CryptographicException("文件以损坏");
            }

            long numReads = sourceSize / BUFFER_SIZE;
            long slack = (long)sourceSize % BUFFER_SIZE;

            for (int i = 0; i < numReads; ++i) {
                read = cin.Read(bytes, 0, bytes.Length);
                targetStream.Write(bytes, 0, read);
                chash.Write(bytes, 0, read);
                value += read;
                outValue += read;
            }

            if (slack > 0) {
                read = cin.Read(bytes, 0, (int)slack);
                targetStream.Write(bytes, 0, read);
                chash.Write(bytes, 0, read);
                value += read;
                outValue += read;
            }

            chash.Flush();
            chash.Close();

            targetStream.Flush();

            byte[] curHash = hasher.Hash;

            byte[] oldHash = new byte[hasher.HashSize / 8];
            read = cin.Read(oldHash, 0, oldHash.Length);
            if ((oldHash.Length != read) || (!EqualTwoByteArray(oldHash, curHash)))
                throw new CryptographyException("文件被破坏");

            if (outValue != sourceSize)
                throw new CryptographyException("文件大小不匹配");

            targetStream.Position = 0;
            return targetStream;
        }

        public static void EncryptStreamToFile(Stream orgStream, string pwd, string file) {
            try {

                FileInfo fifo = new FileInfo(file);
                if (!fifo.Directory.Exists) {
                    fifo.Directory.Create();
                }

                Stream saveStream = new FileStream(file, FileMode.Create,FileAccess.Write,FileShare.None);

                Stream encryptedStream = EncryptStream(orgStream, pwd);
                encryptedStream.Position = 0;
                int read = 0;
                byte[] bytes = new byte[BUFFER_SIZE];
                while ((read = encryptedStream.Read(bytes, 0, bytes.Length)) != 0) {
                    saveStream.Write(bytes, 0, read);
                }
                saveStream.Flush();
                saveStream.Close();
                encryptedStream.Close();
            } catch (Exception e1) {
                throw e1;
            }
        }

        public static Stream DecryptStreamFromFile(string file, string pwd) {
            try {
                Stream orgStream = new FileStream(file, FileMode.Open,FileAccess.Read,FileShare.Delete);
                Stream targetStream = DecryptStream(orgStream, pwd);
                orgStream.Close();
                return targetStream;
            } catch (Exception e1) {
                throw e1;
            }
        }
    }

    /// <summary>
    /// 异常处理类
    /// </summary>
    public class CryptographyException : ApplicationException {
        public CryptographyException(string fMessage)
            : base(fMessage) {
        }
    }
}
