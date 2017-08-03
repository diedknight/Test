//----------------------------------------------------------------------------
// FFile.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2006 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// File class for simplifying common file operations.

// REVISION HISTORY:
// Date          Author            Changes
// 14 Feb 2006   Francis Mair      1st implementation

//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FSuite.Utilities
{
   /// <summary>
   /// Class definition
   /// </summary>
   public class FFile
   {
      /// <summary>
      /// Write the passed string to the specified filename
      /// </summary>
      public static void Write(string fileName,string data)
      {
         // delete the file if it exists.
         if(File.Exists(fileName)) 
            File.Delete(fileName);

         // 0pen the stream and read it back.
         using(FileStream fileStream = File.OpenWrite(fileName)) 
         {
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.Write(data);
            streamWriter.Close();
         }
      }

      /// <summary>
      /// Reads the passed filename and returns as a string
      /// </summary>
      /// <param name="fileName"></param>
      public static string Read(string fileName)
      {
         StringBuilder fileContents = new StringBuilder();

         // open the stream and read it back.
         using(StreamReader streamReader = File.OpenText(fileName))
         {
            string line = null;
            while((line = streamReader.ReadLine()) != null)
            {
               fileContents.Append(line);
            }
         }

         return fileContents.ToString();
      }

      //------------------------------------------------------------------------------
      // Recursive file copying routine. Pass in a source directory, and a destination
      // directory, plus whether you want the file copy to be recursive or not.

      public static void FileCopy(string srcdir,string destdir,bool recursive)
      {
         DirectoryInfo dir;
         FileInfo[] files;
         DirectoryInfo[] dirs;
         string tmppath;

         // determine if the destination directory exists, if not create it
         if(!Directory.Exists(destdir))
         {
            Directory.CreateDirectory(destdir);
         }

         dir = new DirectoryInfo(srcdir);

         // if the source dir doesn't exist, throw
         if(!dir.Exists)
         {
            throw new ArgumentException("source dir doesn't exist -> " + srcdir);
         }

         //get all files in the current dir
         files = dir.GetFiles();

         //loop through each file
         foreach(FileInfo file in files)
         {
            //create the path to where this file should be in destdir
            tmppath = Path.Combine(destdir,file.Name);

            try
            {
               //copy file to dest dir
               file.CopyTo(tmppath,false);      // throws exception if file already exists
            }
            catch(Exception /*ex*/)
            {
               // ignore since this is ok
            }
         }

         //cleanup
         files = null;

         //if not recursive, all work is done
         if(!recursive)
         {
            return;
         }

         //otherwise, get dirs
         dirs = dir.GetDirectories();

         //loop through each sub directory in the current dir
         foreach(DirectoryInfo subdir in dirs)
         {
            //create the path to the directory in destdir
            tmppath = Path.Combine(destdir,subdir.Name);

            //recursively call this function over and over again
            //with each new dir.
            FileCopy(subdir.FullName,tmppath,recursive);
         }

         dirs = null;                  // cleanup
         dir = null;
      }
   }
}

//----------------------------------------------------------------------------
