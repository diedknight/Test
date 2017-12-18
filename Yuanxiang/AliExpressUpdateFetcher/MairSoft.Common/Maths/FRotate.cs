//----------------------------------------------------------------------------
// FRotate.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2001-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FRotate class, handles rotation. We set a rotate point (on a 2d place)
// and an angle of rotation, then we can feed this object other points on the
// plane, and it will give us the rotated new position. Positive angle is
// anti-clockwise rotation.

// REVISION HISTORY:
// Date           Author            Changes
// 25 Apr 2001    Francis Mair      1st implementation
// 29 May 2003    Francis Mair      C# conversion

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FRotate
   {
      //---------------------------------------------------------------------------
      // Number definitions

      const double PI = 3.141592653589793238462643383279502884197169399375105820974944592308;

      //---------------------------------------------------------------------------------

      double _xorigin = 0;
      double _yorigin = 0;
      double _angle = 0;
      double _radians = 0;

      bool _initialised = false;

      //---------------------------------------------------------------------------------
      // Constructor

      public FRotate()
      {

      }

      //---------------------------------------------------------------------------------
      // Load the point that serves as our origin, also the angle that we want to rotate
      // by. This angle is positive as it rotates anti-clockwise.

      public void Init(double xorigin,double yorigin,double angle)
      {  
         _xorigin = xorigin;
         _yorigin = yorigin;
         _angle = angle;                           // set angle in degrees
         _radians = _angle / (180.0 / PI);         // set angle in radians (used by cos() & sin())

         _initialised = true;
      }

      //---------------------------------------------------------------------------------
      // Pass a point in to be rotated. The first x & y are the original point locations.
      // The second point is it's new location, after being rotated around the origin
      // point, using the set angle.

      public void Rotate(double x,double y,out double newX,out double newY)
      {
         newX = x;                              // initialse return variables, in case angle is 0 degrees
         newY = y;

         if((_initialised == false) || (_angle == 0.0)) return;   // no use rotating if not initialised

         double cos0 = Math.Cos(_radians);           // get cosine/sine
         double sin0 = Math.Sin(_radians);

         double xdiff = x - _xorigin;           // get diffs
         double ydiff = y - _yorigin;

         newX = (cos0 * xdiff) - (sin0 * ydiff) + _xorigin;       // perform rotation calculation
         newY = (sin0 * xdiff) + (cos0 * ydiff) + _yorigin;
      }
   }
}
//---------------------------------------------------------------------------------


