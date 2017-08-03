//----------------------------------------------------------------------------
// FPoint.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2001-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FPoint item, which encapsulates a point on a 2d coordinate surface,
// ie, (x,y)

// REVISION HISTORY:
// Date          Author            Changes
// 28 May 2001   Francis Mair      1st implementation
// 29 May 2003   Francis Mair      C# conversion

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

   public class FPoint
   {
      public double x = 0;
      public double y = 0;

      //---------------------------------------------------------------------------------
      // Constructor

      public FPoint()
      {

      }

      //---------------------------------------------------------------------------------
      // Constructor with x,y passed

      public FPoint(double dX,double dY)
      {
         x = dX;
         y = dY;
      }

      //---------------------------------------------------------------------------------
      // Copy Constructor.

      public FPoint(FPoint rhs) 
      {
         x = rhs.x;
         y = rhs.y;        
      }
   }
}
//---------------------------------------------------------------------------------


