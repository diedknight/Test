//----------------------------------------------------------------------------
// FLocale.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2005 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FLocale class. This class allows us to easily change the locale for
// the current thread. Locales are important when we have international
// users of our program. For instance, if we load ini and xml files that
// the application accesses, with numbers with decimal points, then the
// user runs the application in a German locale where decimal points mean
// something else (commas are used) then the numbers returned will be
// erroneous, ie
// string strVal = "0.02766"
// double dVal = double.Parse(strVal);
// with en-US or en-NZ locales, dVal = 0.02766
// with de-DE locale, dVal = 2766
// Changing locales needs to be done on a per thread basis, you cannot
// change the default locale for the entire process.

// REVISION HISTORY:
// Date          Author            Changes
// 16 Jan 2005   Francis Mair      1st implementation

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.Threading;
using System.Globalization;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FLocale
   {
      public enum Culture { enUS, enNZ, deDE };

      //---------------------------------------------------------------------------------
      // Constructor

      private FLocale()
      {

      }

      //---------------------------------------------------------------------------------
      // Static method to change the locale for the current thread

      public static void SetCulture(Culture culture)
      {
         string cultureName = "";

         if(culture == Culture.enUS)
         {
            cultureName = "en-US";
         }
         else if(culture == Culture.enNZ)
         {
            cultureName = "en-NZ";
         }
         else if(culture == Culture.deDE)
         {
            cultureName = "de-DE";
         }

         if(cultureName.Length > 0)
         {  CultureInfo cultureInfo = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
         }
         else throw new Exception("Culture not currently supported");
      }
   }
}

//---------------------------------------------------------------------------------


