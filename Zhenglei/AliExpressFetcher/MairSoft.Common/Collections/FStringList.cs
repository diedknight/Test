//----------------------------------------------------------------------------
// FStringList.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2004 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FStringList encapsulates a list of stringd

// REVISION HISTORY:
// Date          Author            Changes
// 26 Jan 2004   Francis Mair      1st implementation

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.Collections;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FStringList : CollectionBase
   {
      //------------------------------------------------------------------------------
      // Constructor

      public FStringList()
      {

      }
         
      //------------------------------------------------------------------------------
      // Add new item

      public void Add(string str)
      {         
         try
         {                
            List.Add(str);                  // use base class to process actual collection operation
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Info,"FStringList.Add : " + ex.Message);
         }
      }

      //------------------------------------------------------------------------------
      // Does list contain string

      public bool Contains(string str)
      {
         return List.Contains(str);
      }

      //------------------------------------------------------------------------------
      // Index operator

      public string this[int pos]
      {
         get { return (string)List[pos]; }
      }
   }
}

//---------------------------------------------------------------------------------


