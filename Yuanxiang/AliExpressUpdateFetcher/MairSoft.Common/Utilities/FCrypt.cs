//----------------------------------------------------------------------------
// FCrypt.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// This class gives us easy to use static methods to call the various
// crypto functions built into the NET framework.

// REVISION HISTORY:
// Date           Author            Changes
// 30 June 2003   Francis Mair      1st implementation

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FCrypt
   {
      /// <summary>
      /// Private constructor
      /// </summary>
      protected FCrypt()
      {
      }

      #region MD5 Methods

      //---------------------------------------------------------------------------------    
      // Create an md5 sum string of this string

      // This method provides a simple and fairly secure
      // algorithm to create a standard length hash value from a string.
      // It implements the "RSA Data Security, Inc. MD5 Message-Digest Algorithm"

      // TECHNICAL DESCRIPTION
      // The MD5 alogirthm takes as input a message of arbitrary length, and 
      // produces as output a 128-bit "fingerprint" or "message digest" of the 
      // input. It is conjectured that it is computationally infeasible to produce 
      // two messages having the same message digest, or to produce any message 
      // having a given prespecified target message digest. The MD5 algorithm is 
      // intended for digital signature applications, where a large file must be 
      // "compressed" in a secure manner before being encrypted with a private 
      // (secret) key under a public-key cryptosystem such as RSA. 

      public static string Md5Sum(string str)
      {
         // first we need to convert the string into bytes, which
         // means using a text encoder.
         Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

         // create a buffer large enough to hold the string
         byte[] unicodeText = new byte[str.Length * 2];
         enc.GetBytes(str.ToCharArray(),0,str.Length,unicodeText,0,true);

         // now that we have a byte array we can ask the CSP to hash it
         MD5 md5 = new MD5CryptoServiceProvider();
         byte[] result = md5.ComputeHash(unicodeText);

         // build the final string by converting each byte
         // into hex and appending it to a StringBuilder
         StringBuilder sb = new StringBuilder();
         for(int i=0;i<result.Length;i++)
         {
            sb.Append(result[i].ToString("X2"));
         }

         // return string
         return sb.ToString();
      }

      #endregion

      #region Memory Zeroing Methods

      /// <summary>
      /// Call this function to remove the key from memory after use for security
      /// </summary>
      /// <param name="Destination"></param>
      /// <param name="Length"></param>
      /// <returns></returns>
      [System.Runtime.InteropServices.DllImport("KERNEL32.DLL",EntryPoint = "RtlZeroMemory")]
      public static extern bool ZeroMemory(IntPtr Destination,int Length);

      #endregion

      #region Key Generation Methods

      /// <summary>
      /// Function to generate a 64 bit key
      /// </summary>
      /// <returns></returns>
      public static string GenerateKey()
      {
         // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
         DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();

         // Use the Automatically generated key for Encryption.
         return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
      }

      #endregion

      #region File Encryption / Decryption Methods

      /// <summary>
      /// DES encrypt a file 
      /// </summary>
      /// <param name="sInputFilename"></param>
      /// <param name="sOutputFilename"></param>
      /// <param name="sKey"></param>
      static void EncryptFile(string sInputFilename,string sOutputFilename,string sKey)
      {
         FileStream fsInput = new FileStream(sInputFilename,FileMode.Open,FileAccess.Read);
         FileStream fsEncrypted = new FileStream(sOutputFilename,FileMode.Create,FileAccess.Write);

         DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
         DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
         DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
         ICryptoTransform desencrypt = DES.CreateEncryptor();
         CryptoStream cryptostream = new CryptoStream(fsEncrypted,desencrypt,CryptoStreamMode.Write);

         byte[] bytearrayinput = new byte[fsInput.Length];
         fsInput.Read(bytearrayinput,0,bytearrayinput.Length);
         cryptostream.Write(bytearrayinput,0,bytearrayinput.Length);
         cryptostream.Close();
         fsInput.Close();
         fsEncrypted.Close();
      }

      /// <summary>
      /// DES decrypt a file
      /// </summary>
      /// <param name="sInputFilename"></param>
      /// <param name="sOutputFilename"></param>
      /// <param name="sKey"></param>
      static void DecryptFile(string sInputFilename,string sOutputFilename,string sKey)
      {
         DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
         //A 64 bit key and IV is required for this provider.
         //Set secret key For DES algorithm.
         DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
         //Set initialization vector.
         DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

         //Create a file stream to read the encrypted file back.
         FileStream fsread = new FileStream(sInputFilename,FileMode.Open,FileAccess.Read);
         //Create a DES decryptor from the DES instance.
         ICryptoTransform desdecrypt = DES.CreateDecryptor();
         //Create crypto stream set to read and do a
         //DES decryption transform on incoming bytes.
         CryptoStream cryptostreamDecr = new CryptoStream(fsread,desdecrypt,CryptoStreamMode.Read);
         //Print the contents of the decrypted file.
         StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
         fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
         fsDecrypted.Flush();
         fsDecrypted.Close();
      }

      #endregion

      #region String Encryption / Decryption Methods

      /// <summary>
      /// Standard DES encryption
      /// </summary>
      public static string Encrypt(string input,string key)
      {
         string output = string.Empty;

         try
         {
            // Note: The DES CryptoService only accepts certain key byte lengths
            // We are going to make things easy by insisting on an 8 byte legal key length
            string shortKey = key.Remove(8,key.Length - 8);
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(shortKey);
         
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
   
            //now encrypt the bytearray            
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,des.CreateEncryptor(keyBytes,keyBytes),CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cs);
            sw.Write(input);
            sw.Flush();
            cs.FlushFinalBlock();
            ms.Flush();
            
            // now return the byte array as a "safe for XMLDOM" Base64 String
            output = Convert.ToBase64String(ms.GetBuffer(),0,(int)ms.Length);
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Error,"FCrypt.Encrypt : " + ex.Message);
            return(ex.Message);               
         }

         return(output);
      }

      /// <summary>
      /// Standard DES decryption
      /// </summary>
      /// <param name="input"></param>
      /// <returns></returns>
      public static string Decrypt(string input,string key)
      {
         string output = string.Empty;
      
         try
         {
            // Note: The DES CryptoService only accepts certain key byte lengths
            // We are going to make things easy by insisting on an 8 byte legal key length
            string shortKey = key.Remove(8,key.Length - 8);
            byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(shortKey);
                     
            // we have a base 64 encoded string so first must decode to regular unencoded (encrypted) string
            byte[] inputByteArray = Convert.FromBase64String(input);
            
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // now decrypt the regular string
            MemoryStream ms = new MemoryStream(inputByteArray);
            CryptoStream cs = new CryptoStream(ms,des.CreateDecryptor(keyBytes,keyBytes),CryptoStreamMode.Read);

            StreamReader sr = new StreamReader(cs);

            output = sr.ReadToEnd();
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Error,"FCrypt.Decrypt : " + ex.Message);
            throw(new Exception(ex.Message));
         }         

         return(output);
      }

      #endregion
   }
}

//---------------------------------------------------------------------------------


