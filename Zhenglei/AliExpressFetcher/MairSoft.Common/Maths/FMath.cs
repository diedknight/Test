//----------------------------------------------------------------------------
// FMath.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 1999-2003 Francis Mair (frank@mair.net.nz)
// DESCRIPTION   
// Utility class for mathematical functions

// REVISION HISTORY:
// Date              Author            Changes
// 05 Nov 1999       Francis Mair      1st implementation
// 29 May 2003       Francis Mair      C# conversion
// 17 Apr 2004       Francis Mair      Added linear interpolation

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

   public class FMath
   {
      //---------------------------------------------------------------------------
      // Constructor

      private FMath()
      {

      }

      //---------------------------------------------------------------------------------
      // Method to return T/F if a point lies within a polygon
      //
      // SOURCE:
      // The definitive reference is "Point in Polygon Strategies" by
      // Eric Haines [Gems IV]  pp. 24-46.
      // The code in the Sedgewick book Algorithms (2nd Edition, p.354) fails
      // under certain circumstances.  See 
      // http://condor.informatik.Uni-Oldenburg.DE/~stueker/graphic/index.html
      // for a discussion.
      //
      // DESCRIPTION:
      // The essence of the ray-crossing method is as follows.
      // Think of standing inside a field with a fence representing the polygon. 
      // Then walk north. If you have to jump the fence you know you are now 
      // outside the poly. If you have to cross again you know you are now 
      // inside again; i.e., if you were inside the field to start with, the 
      // total number of fence jumps you would make will be odd, whereas if you 
      // were ouside the jumps will be even.
      //
      // The code below is from Wm. Randolph Franklin <wrf@ecse.rpi.edu>
      // (see URL below) with some minor modifications for speed.  It returns 
      // 1 for strictly interior points, 0 for strictly exterior, and 0 or 1 
      // for points on the boundary.  The boundary behavior is complex but 
      // determined; in particular, for a partition of a region into polygons, 
      // each point is "in" exactly one polygon.  
      // (See p.243 of [O'Rourke (C)] for a discussion of boundary behavior.)
      //
      // The code may be further accelerated, at some loss in clarity, by
      // avoiding the central computation when the inequality can be deduced,
      // and by replacing the division by a multiplication for those processors
      // with slow divides.  For code that distinguishes strictly interior
      // points from those on the boundary, see [O'Rourke (C)] pp. 239-245.
      //
      // FRANKS QUICK DESCRIPTION
      // Basically you give this function a list of sequentially connected
      // points (ie the polygon), and your point which you want to know if it
      // is inside or not. Then you check whether each polygon line segment
      // intersects a line heading north from the point. If you got an odd
      // number of intersections you were inside to start with!

      // References:
      // Franklin's code: 
      // http://www.ecse.rpi.edu/Homepages/wrf/geom/pnpoly.html
      // [Gems IV]  pp. 24-46
      // [O'Rourke (C)] Sec. 7.4.
      // [Glassner:RayTracing]

      public static bool PointInPolygon(int npol,double[] xp,double[] yp,double x,double y)
      {
         int i,j;
         bool c = false;
         for(i = 0, j = npol-1; i < npol; j = i++) 
         {  if((((yp[i]<=y) && (y<yp[j])) ||
               ((yp[j]<=y) && (y<yp[i]))) &&
               (x < (xp[j] - xp[i]) * (y - yp[i]) / (yp[j] - yp[i]) + xp[i]))      
               c = !c;
         }
         return c;
      }

      //---------------------------------------------------------------------------------
      // Passed three points in space (x,y,z coords), this function will return the 4 
      // values the uniquely identify the plane that passes through these three points.
      // The plane equation is Ax + By + Cz + D = 0
      // The last four parameters are defined as 'out' which is the same as 'ref' but they
      // don't need to be defined before this method is called.

      public static bool GetPlaneEquation(double x1,double y1,double z1,
                                    double x2,double y2,double z2,
                                    double x3,double y3,double z3,
                                    out double A,out double B,out double C,out double D)
      {
         A = y1*(z2-z3) + y2*(z3-z1) + y3*(z1-z2);
         B = z1*(x2-x3) + z2*(x3-x1) + z3*(x1-x2);
         C = x1*(y2-y3) + x2*(y3-y1) + x3*(y1-y2);
         D = x1*(y2*z3 - y3*z2) + x2*(y3*z1 - y1*z3) + x3*(y1*z2 - y2*z1);
         D *= -1;

         return true;
      }

      //---------------------------------------------------------------------------------
      // This function is passed two points which make up a line, and 4 variables which
      // define a plane (these can be deduced from the 'GetPlaneEquation' function. We
      // then return T/F depending on whether the line intersects through the plane. 
      // Currently we assume the two passed points are the end points of the line and
      // we check to see if the plane crosses between these two points. We could also
      // modify the function to assume the line goes off to infinity in both directions
      // through this point if we wanted to.

      public static bool LineIntersectsPlane(double x1,double y1,double z1,
                                       double x2,double y2,double z2,             
                                       double A,double B,double C,double D)
      {
         bool intersects = false;

         double num = A*x1 + B*y1 + C*z1 + D;
         double den = A*(x1-x2) + B*(y1-y2) + C*(z1-z2);

         if(den == 0) return(false);         // denominator is 0 means line is parallel to plane (ie no intersection possible, unless line is on the plane!)

         double u = num / den;               // u is '0' at point 1 and '1' at point 2

         if(0 <= u && u <= 1)                // if u between 0 and 1, then intersection occurs between point 1 and 2
            intersects = true;

         return intersects;
      }

      //---------------------------------------------------------------------------------
      // This function is passed an x & y coordinate of a line, and we want to know 
      // where this line cuts through the plane. (ie the z coordinate).

      public static double PlanarIntersection(double x,double y,
                                              double A,double B,double C,double D)
      {
         double z = 0.0;

         double num = A*x + B*y + D;
         double den = -1 * C;

         if(den == 0.0) return(z);

         z = num / den;

         return z;
      }

      //---------------------------------------------------------------------------------
      // This function is passed two x & y coordinates, and we return the distance
      // between the two points.
      // 
      // Distance Formula:
      //
      // Given the two points (x1, y1) and (x2, y2), the distance between these points 
      // is given by the formula:
      //
      // distance = sqrt( (x2 - x1)2 + (y2 - y1)2 )

      public static double PlanarDistance(double x1,double y1,double x2,double y2)
      {
         double distance = 0.0;

         distance = Math.Sqrt( Math.Pow( (x2 - x1) , 2) + Math.Pow( (y2 - y1), 2) );

         return distance;
      }

      //---------------------------------------------------------------------------------
      // Linear Interpolation. 
      //
      // This function is passed two x & y coordinations, and a further x coordinate.
      // We then return the corresponding y coordinate using linear interpolation.
      // 
      // Linear Interpolation Formula:
      // 
      // Given the two points (x1, y1) and (x2, y2), and a further x coordinate we
      // can find the corresponding x using:
      //
      // y = y1 + ((((y2 - y1) * (x - x1)) / (x2 - x1)))

      public static double LinearInterpolation(double x1,double y1,double x2,double y2,double x)
      {
         double y = 0.0;

         y = y1 + ((((y2 - y1) * (x - x1)) / (x2 - x1)));

         return y;
      }
   }
}

//---------------------------------------------------------------------------------

