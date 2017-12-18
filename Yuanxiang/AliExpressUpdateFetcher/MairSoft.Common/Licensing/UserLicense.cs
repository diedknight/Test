//----------------------------------------------------------------------------
// COPYRIGHT:
// Copyright (C) 2006 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// Class which contains license details. Each user has to have this file
// in the exe run directory and the UserLicenseManager loads it to see if
// the file is valid etc. We also keep the date of last use in the file
// etc to try and stop license hacking, etc.
// NOTE: We serialize DateTimes as string since different locales cause
// different styles of serialization and if we save as a string we can perform
// the conversion ourselves.

// REVISION HISTORY:
// Date           Author            Changes
// 02 Apr 2005    Francis Mair      1st implementation
//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite.Licensing
{
   /// <summary>
   /// UserLicense class
   /// </summary>
   [Serializable]
   public class UserLicense
   {
      // USER RESTRICTIONS - CAN SET ONE OR MORE OF THESE
      // name of user this license applies to, ie DELL/Frank
      private string _loginName = string.Empty;
      // hard drive volume id
      private string _volumeID = string.Empty;
      // MAC address of network card
      private string _macAddress = string.Empty;

      // TIME RESTRICTION - IF REQUIRED
      // date which this license expires at
      private DateTime _expiryDate = DateTime.MaxValue;

      // DEMO MODE RESTRICTION - IF REQUIRED
      // whether we want to run in demo mode
      private bool _demoMode = false;

      // USEAGE INFORMATION - STATISTICS ONLY
      // date which this license file was last loaded
      private DateTime _lastLoadDate = DateTime.MinValue;
      // the number of times the license has been loaded
      private int _usageCount = 0;

      // APPLICATION SPECIFIC INFORMATION
      private string _applicationData = string.Empty;

      /// <summary>
      /// Constructor
      /// </summary>
      public UserLicense()
      {
      }

      /// <summary>
      /// Constructor taking
      /// </summary>
      /// <param name="loginName"></param>
      /// <param name="expiryDate"></param>
      public UserLicense(string loginName,DateTime expiryDate)
      {
         _loginName = loginName;
         _expiryDate = expiryDate;
      }

      /// <summary>
      /// Constructor taking
      /// </summary>
      /// <param name="loginName"></param>
      /// <param name="expiryDate"></param>
      /// <param name="applicationData"></param>
      public UserLicense(string loginName,DateTime expiryDate,string applicationData)
      {
         _loginName = loginName;
         _expiryDate = expiryDate;
         _applicationData = applicationData;
      }

      /// <summary>
      /// LoginName
      /// </summary>
      public string LoginName
      {
         get { return _loginName; }
         set { _loginName = value; }
      }

      /// <summary>
      /// ExpiryDate. Not serialized so we can control
      /// serialization format.
      /// </summary>
      [XmlIgnore]
      public DateTime ExpiryDate
      {
         get { return _expiryDate; }
         set { _expiryDate = value; }
      }

      /// <summary>
      /// Property to handling serialization of our expiry date
      /// in such a way that it is region independent.
      /// </summary>
      public long ExpiryDateString
      {
         get { return _expiryDate.Ticks; }
         set { _expiryDate = new DateTime(value); }
      }

      /// <summary>
      /// LastLoadDate
      /// </summary>
      [XmlIgnore]
      public DateTime LastLoadDate
      {
         get { return _lastLoadDate; }
         set { _lastLoadDate = value; }
      }

      /// <summary>
      /// Property to handling serialization of our last load date
      /// in such a way that it is region independent.
      /// </summary>
      public long LastLoadDateString
      {
         get { return _lastLoadDate.Ticks; }
         set { _lastLoadDate = new DateTime(value); }
      }

      /// <summary>
      /// UsageCount
      /// </summary>
      public int UsageCount
      {
         get { return _usageCount; }
         set { _usageCount = value; }
      }

      /// <summary>
      /// VolumeID
      /// </summary>
      public string VolumeID
      {
         get { return _volumeID; }
         set { _volumeID = value; }
      }

      /// <summary>
      /// MacAddress
      /// </summary>
      public string MacAddress
      {
         get { return _macAddress; }
         set { _macAddress = value; }
      }

      /// <summary>
      /// DemoMode
      /// </summary>
      public bool DemoMode
      {
         get { return _demoMode; }
         set { _demoMode = value; }
      }

      /// <summary>
      /// Application specific data
      /// </summary>
      public string ApplicationData
      {
         get { return _applicationData; }
         set { _applicationData = value; }
      }
   }
}

//---------------------------------------------------------------------------