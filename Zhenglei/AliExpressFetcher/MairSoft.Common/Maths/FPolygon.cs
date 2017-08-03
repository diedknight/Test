//----------------------------------------------------------------------------
// FPolygon.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2004 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FPolygon item, which encapsulates a polygon. That is a shape with an 
// set number of sides. We represent this by having a list of x,y coordinates
// which we assume are joined in the order they are presented to us.

// REVISION HISTORY:
// Date           Author            Changes
// 18 Apr 2004    Francis Mair      1st implementation

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.Collections;
using FSuite.Generics;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FPolygon
   {
      public FList<FPoint> pointList = new FList<FPoint>();       // list of points making up this polygon

      //---------------------------------------------------------------------------------
      // Constructor

      public FPolygon()
      {

      }

      //---------------------------------------------------------------------------------
      // Copy Constructor.

      protected FPolygon(FPolygon rhs)
      {
         // not implemented        
      }

      //---------------------------------------------------------------------------------
      // Method to allow easy addition of a new point to our polygon

      public void AddPoint(FPoint point)
      {
         pointList.Add(point);
      }
   }
}

//---------------------------------------------------------------------------------
//---------------------------------------------------------------------------------


