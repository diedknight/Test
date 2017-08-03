//----------------------------------------------------------------------------
// COPYRIGHT:
// Copyright (C) 2006 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// Class which handles the loading and creation of UserLicense files. 

// REVISION HISTORY:
// Date           Author            Changes
// 02 Apr 2005    Francis Mair      1st implementation
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using FSuite.Utilities;
using System.IO;
using System.Security.Principal;

namespace FSuite.Licensing
{
   /// <summary>
   /// Types of license validity
   /// </summary>
   public enum LicenseType { Valid, NoLicenseFile, InvalidLicense, InvalidUser, ExpiredLicense };

   /// <summary>
   /// Class definition
   /// </summary>
   public class UserLicenseManager
   {
      /// <summary>
      /// Constructor
      /// </summary>
      protected UserLicenseManager()
      {
      }

      /// <summary>
      /// Given a filepath and password we load the specified license file
      /// and return whether we are allowed to proceed or not. Every time we check the
      /// license we also update it. We return the license validity. We also
      /// check the date of the last load to see if the current date time is earlier
      /// in which case we reject the license, also we can check the dates of any
      /// created settings files to see if they are later than the current date time.
      /// SUGGESTION. Copy the license file to another name, ie FControls.dll then if this
      /// file exists use this version, but make sure we can still send new license updates...
      /// Maybe we could even put something in the registry or in isolated storage...
      /// </summary>
      /// <param name="filepath"></param>
      /// <param name="password"></param>
      /// <param name="settingsDirectory"></param>
      public static LicenseType IsLicenseValid(string filepath,string password,string settingsDirectory)
      {
         LicenseType licenseType = LicenseType.InvalidLicense;

         try
         {
            // try loading license now
            UserLicense userLicense = LoadLicenseFile(filepath,password);

            // get license type now
            licenseType = IsLicenseValid(userLicense);
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Error,ex.ToString());
         }

         return licenseType;
      }
         
      /// <summary>
      /// This method simply returns the license validity type for the passed
      /// license file
      /// </summary>
      /// <param name="userLicense"></param>
      public static LicenseType IsLicenseValid(UserLicense userLicense)
      {
         LicenseType licenseType = LicenseType.InvalidLicense;

         try
         {
            if(userLicense == null) 
            { 
               licenseType = LicenseType.NoLicenseFile;     // no license file found
            }
            else if((userLicense.LoginName.Length > 0) &&
                    (userLicense.LoginName != WindowsIdentity.GetCurrent().Name))
            {
               licenseType = LicenseType.InvalidUser;       // user is invalid
            }
            else if(userLicense.ExpiryDate < DateTime.Now)
            {
               licenseType = LicenseType.ExpiredLicense;    // license expired
            }
            else if(userLicense.ExpiryDate < userLicense.LastLoadDate)
            {
               licenseType = LicenseType.ExpiredLicense;    // license expired due to PC date modification
            }
            else
            {
               licenseType = LicenseType.Valid;             // must be a valid license
            }
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Error,ex.ToString());
         }

         return licenseType;
      }

      /// <summary>
      /// Allows us to create a new license file and saves it out
      /// to disk encrypted using the specified password
      /// </summary>
      /// <param name="filepath"></param>
      /// <param name="password"></param>
      /// <param name="userLicense"></param>
      public static void SaveLicenseFile(string filepath,string password,UserLicense userLicense)
      {
         // convert license to an xml string and then encrypt it
         string xmlLicense = FSerialise.Serialise(userLicense);
         string encryptedLicense = FCrypt.Encrypt(xmlLicense,password);

         // save the encrypted string to the file location
         FFile.Write(filepath,encryptedLicense);
      }

      /// <summary>
      /// Load the license file if possible
      /// </summary>
      /// <param name="filepath"></param>
      /// <param name="password"></param>
      /// <returns></returns>
      public static UserLicense LoadLicenseFile(string filepath,string password)
      {
         // if file doesn't exist nothing we can do
         if(!File.Exists(filepath)) return null;

         // load, unencrypt and deserialize
         string encryptedLicense = FFile.Read(filepath);
         string xmlLicense = FCrypt.Decrypt(encryptedLicense,password);
         UserLicense userLicense = FSerialise.Deserialise(xmlLicense,typeof(UserLicense)) as UserLicense;

         // every time we load it we also bring forward the last load date if possible
         if((userLicense != null) && (userLicense.LastLoadDate < DateTime.Now))
         {  userLicense.LastLoadDate = DateTime.Now;
            userLicense.UsageCount = userLicense.UsageCount + 1;
            SaveLicenseFile(filepath,password,userLicense);
         }

         // return license file now
         return userLicense;
      }
   }
}

//---------------------------------------------------------------------------
