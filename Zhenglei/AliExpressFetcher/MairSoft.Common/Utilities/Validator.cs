//===============================================================================
// 
// MairSoft.Common
// 
// Copyright (c) 2006 Francis Mair (frank@mair.net.nz)
//
// Description:
// This class allows us to keep all our validation code in one place
//
//===============================================================================

using System.Text.RegularExpressions;

namespace MairSoft.Common.Utilities
{
   class Validator
   {
      /// <summary>
      /// Returns T/F depending on whether passed string is a valid
      /// email address
      /// </summary>
      /// <param name="inputEmail"></param>
      /// <returns></returns>
      public static bool IsEmailValid(string inputEmail)
      {
         inputEmail = (inputEmail == null) ? string.Empty : inputEmail;

         if((inputEmail.Length == 0) || (inputEmail.Length > 200))
            return false;
         
         string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
               @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
               @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
         
         Regex re = new Regex(strRegex);
         
         if(re.IsMatch(inputEmail))
            return (true);
         else
            return (false);
      }      
   }
}

//===============================================================================
