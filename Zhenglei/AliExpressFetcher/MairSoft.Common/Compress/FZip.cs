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

//---------------------------------------------------------------------------
// Using

using System;
using System.IO;

using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.Checksums;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition

   public class FZip
	{
      //---------------------------------------------------------------------------
      // Constructor

		public FZip()
		{
		}

      //---------------------------------------------------------------------------
      // Creates the zip file on disk (outputFile) from the specified file to be
      // zipped (inputFile)

      public static void Zip(string inputFile,string outputFile)
      {	
         try
         {
            Crc32 crc = new Crc32();

            //GZipOutputStream gzipOutputStream = new GZipOutputStream(File.Create(outputFile));
            ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(outputFile));
            zipOutputStream.SetLevel(9);    // 0 - store only to 9 - means best compression
   		
            FileStream fileStream = File.OpenRead(inputFile);
   		
            byte[] buffer = new byte[fileStream.Length];
            fileStream.Read(buffer,0,buffer.Length);

            //fileStream.Close();
            //gzipOutputStream.Write(buffer, 0, buffer.Length);
            //gzipOutputStream.Flush();
            //gzipOutputStream.Close();
            
            ZipEntry entry = new ZipEntry(inputFile);

            entry.DateTime = DateTime.Now;
   		
            //// set Size and the crc, because the information about the size and crc should be 
            //// stored in the header if it is not set it is automatically written in the footer.
            //// (in this case size == crc == -1 in the header). Some ZIP programs have problems 
            //// with zip files that don't store the size and crc in the header.
            entry.Size = fileStream.Length;
            fileStream.Close();

            crc.Reset();
            crc.Update(buffer);

            entry.Crc = crc.Value;


            zipOutputStream.PutNextEntry(entry);
            zipOutputStream.Write(buffer, 0, buffer.Length);

            zipOutputStream.Finish();
            zipOutputStream.Close();
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Error,"FZip.Zip : " + ex.Message);
            throw new Exception(ex.Message);
         }
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

      //---------------------------------------------------------------------------
      // Unzips the passed zip file into its component decompressed files

      public static void Unzip(string zipFile,string password)
      {
         ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFile));

         // load password if one exists
         if(password.Length > 0) zipInputStream.Password = password;
		        
         ZipEntry zipEntry = null;

         // pull out all files in zipfile
         while((zipEntry = zipInputStream.GetNextEntry()) != null) 
         {		
			   // pull out subdirectory location and filename
            string directoryName = Path.GetDirectoryName(zipEntry.Name);
            string fileName = Path.GetFileName(zipEntry.Name);
			
            // create subdirectory if necessary
            if(directoryName.Length > 0)
               Directory.CreateDirectory(directoryName);
			
            // do we have a valid filename
            if(fileName.Length > 0) 
            {
               FileStream fileStream = File.Create(zipEntry.Name);
				
               int size = 2048;
               byte[] data = new byte[2048];
               
               while(true) 
               {
                  size = zipInputStream.Read(data,0,data.Length);
                  if(size > 0) 
                  {
                     fileStream.Write(data,0,size);
                  } 
                  else 
                  {
                     break;
                  }
               }
				
               fileStream.Close();
            }
         }
         zipInputStream.Close();
      }

      public static void Unzip(string zipFile, string directoryPath, string password)
      {
          ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFile));

          // load password if one exists
          if (password.Length > 0) zipInputStream.Password = password;

          ZipEntry zipEntry = null;

          // pull out all files in zipfile
          while ((zipEntry = zipInputStream.GetNextEntry()) != null)
          {
              // pull out subdirectory location and filename
              string directoryName = directoryPath;
              string fileName = Path.GetFileName(zipEntry.Name);

              // create subdirectory if necessary
              if (directoryName.Length > 0)
                  Directory.CreateDirectory(directoryName);

              // do we have a valid filename
              if (fileName.Length > 0)
              {
                  FileStream fileStream = File.Create(directoryName + "\\" + fileName);

                  int size = 2048;
                  byte[] data = new byte[2048];

                  while (true)
                  {
                      size = zipInputStream.Read(data, 0, data.Length);
                      if (size > 0)
                      {
                          fileStream.Write(data, 0, size);
                      }
                      else
                      {
                          break;
                      }
                  }

                  fileStream.Close();
              }
          }
          zipInputStream.Close();
      }

      //---------------------------------------------------------------------------
      // Unzips the passed zipped stream and returns the uncompressed stream. To view
      // the data being written to the memory stream you can use:
      // string out = System.Text.Encoding.UTF8.GetString(data,0,size);

      public static Stream Unzip(Stream inputStream,string password)
      {
         // convert input stream into a zip input stream
         ZipInputStream zipInputStream = new ZipInputStream(inputStream);
         
         // load password if one exists
         if(password.Length > 0) zipInputStream.Password = password;
		        
         // pull out first file in zipfile
         ZipEntry zipEntry = zipInputStream.GetNextEntry();
         if(zipEntry == null) throw new Exception("FZip.Unzip : Zip file empty");
         if(zipEntry.Name.Length == 0) throw new Exception("FZip.Unzip : No file name specified");

			// create memory stream to hold decompressed data
         MemoryStream memoryStream = new MemoryStream();
		
         int size = 2048;
         byte[] data = new byte[2048];
         
         while(true) 
         {
            size = zipInputStream.Read(data,0,data.Length);
            if(size > 0) 
            {
               memoryStream.Write(data,0,size);
            } 
            else 
            {
               break;
            }
         }
		
         zipInputStream.Close();
        
         memoryStream.Flush();         // flush it to make sure everything written
         memoryStream.Position = 0;    // reset stream position back to beginning

         return memoryStream;
      }

      //----------------------------------------------------------------------------
	}
}

//----------------------------------------------------------------------------
