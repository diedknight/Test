using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Infrastructure.Compress
{
    public class GZip : ICompress
    {
        public byte[] Compress(byte[] bytes)
        {
            byte[] result;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
                gZipStream.Write(bytes, 0, bytes.Length);
                gZipStream.Close();

                result = memoryStream.ToArray();
            }

            return result;
        }

        public byte[] Decompress(byte[] bytes)
        {
            byte[] result;

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (MemoryStream memoryStream2 = new MemoryStream(bytes))
                {
                    GZipStream gZipStream = new GZipStream(memoryStream2, CompressionMode.Decompress);
                    gZipStream.CopyTo(memoryStream);
                    gZipStream.Close();

                    result = memoryStream.ToArray();
                }
            }

            return result;
        }
    }
}
