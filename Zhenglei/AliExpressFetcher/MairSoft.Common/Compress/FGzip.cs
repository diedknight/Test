//----------------------------------------------------------------------------
// FZip.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2004 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// Zip file class. Lets us easily create, ie zip and unzip files (compression
// and decompression). This lets us handle smaller files.

// REVISION HISTORY:
// Date           Author            Changes
// 25 Oct 2004    Francis Mair      1st implementation
//---------------------------------------------------------------------------
using System;
using System.IO;

using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Checksums;

namespace FSuite
{
    public class FGzip
    {
        public FGzip()
        {
        }

        public static void Gzip(string inputFile, string outputFile)
        {
            try
            {
                GZipOutputStream gzipOutputStream = new GZipOutputStream(File.Create(outputFile));
                FileStream fileStream = File.OpenRead(inputFile);

                byte[] buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, buffer.Length);

                fileStream.Close();
                gzipOutputStream.Write(buffer, 0, buffer.Length);
                gzipOutputStream.Flush();
                gzipOutputStream.Close();
            }
            catch (Exception ex)
            {
                FLog.Write(FLog.Level.Error, "FZip.Zip : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public static void GzipBigFile(string inputFile, string outputFile)
        {
            long sectionLength = 1024 * 1024 * 100;
            try
            {
                GZipOutputStream gzipOutputStream = new GZipOutputStream(File.Create(outputFile));
                FileStream fileStream = File.OpenRead(inputFile);

                if (fileStream.Length > sectionLength)
                {
                    byte[] buffer;
                    long allLength = 0;
                    while ((allLength + sectionLength) <= fileStream.Length)//拷贝主体部分
                    {
                        allLength += sectionLength;

                        buffer = new byte[sectionLength];
                        fileStream.Read(buffer, 0, buffer.Length);
                        fileStream.Position = allLength;

                        gzipOutputStream.Write(buffer, 0, buffer.Length);
                        //gzipOutputStream.Position = allLength;
                        gzipOutputStream.Flush();
                    }
                    long left = fileStream.Length - allLength;//拷贝剩余部分
                    buffer = new byte[left];
                    fileStream.Read(buffer, 0, buffer.Length);
                    fileStream.Close();
                    gzipOutputStream.Write(buffer, 0, buffer.Length);
                    gzipOutputStream.Flush();
                    gzipOutputStream.Close();
                }
                else
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);

                    fileStream.Close();
                    gzipOutputStream.Write(buffer, 0, buffer.Length);
                    gzipOutputStream.Flush();
                    gzipOutputStream.Close();
                }
            }
            catch (Exception ex)
            {
                FLog.Write(FLog.Level.Error, "FZip.Zip : " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}

