//===============================================================================
// 
// MairSoft.Common.Utilities
// 
// Copyright (c) 2006 Francis Mair (frank@mair.net.nz)
//
// Description:
// Provides static methods that can perform useful operations on our entities
//
//===============================================================================

using System;

namespace MairSoft.Common.Utilities
{
   class EntityUtils
   {
      /// <summary>
      /// Returns whether objectType is an instance of the specifed required type
      /// </summary>
      /// <param name="requiredType"></param>
      /// <param name="objectType"></param>
      /// <returns></returns>
      public static bool IsInstanceOfType(Type requiredType,Type objectType)
      {
         foreach(Type iface in objectType.GetInterfaces())
         {
            if(iface == requiredType)
               return true;
         }

         while(objectType != typeof(System.Object))
         {
            if(requiredType == objectType)
               return true;

            objectType = objectType.BaseType;
         }
         return false;
      }      
   }
}

//===============================================================================

