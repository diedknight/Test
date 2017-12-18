//----------------------------------------------------------------------------
// FTriangle.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2001-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FTriangle item, which encapsulates a triangle in the generated mesh. 
// Each triangle consists of three points, with an x & y coordinate
// and a number of attributes per point (can be considered the height for
// example)

// REVISION HISTORY:
// Date           Author            Changes
// 07 Mar 2001    Francis Mair      1st implementation
// 29 May 2003    Francis Mair      C# conversion

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

   public class FTriangle
   {
      public enum TrianglePoints { POINTS = 3 };            // triangle has 3 corners
      
      public string Name;                                   // can give the triangle an identifying name

      public FPoint point0 = new FPoint();                  // x,y coord of triangle point 1
      public FPoint point1 = new FPoint();                  // x,y coord of triangle point 2
      public FPoint point2 = new FPoint();                  // x,y coord of triangle point 3

      public FList<FTriangleAttribute> point0AttributeList = new FList<FTriangleAttribute>();    // list of attributes for point 1
      public FList<FTriangleAttribute> point1AttributeList = new FList<FTriangleAttribute>();    // list of attributes for point 2
      public FList<FTriangleAttribute> point2AttributeList = new FList<FTriangleAttribute>();    // list of attributes for point 3

      double[] daXPts = new double[(int)TrianglePoints.POINTS];  // array of points for intersection calculations
      double[] daYPts = new double[(int)TrianglePoints.POINTS];  // array of points for intersection calculations

      //---------------------------------------------------------------------------------
      // Constructor

      public FTriangle()
      {

      }

      //---------------------------------------------------------------------------------
      // Copy Constructor.

      public FTriangle(FTriangle rhs)
      {
         point0 = rhs.point0;
         point1 = rhs.point1;
         point2 = rhs.point2;

         // do we need copy constructor???
         /*
         FTriangleAttributeIter attribute0Iter(rhs.point0AttributeList);
         while(attribute0Iter.Next() == TR_OK)                 // get next item (starting at beginning)
            point0AttributeList.Add(*(attribute0Iter.Data())); // pull out object from passed list, plonk into local list

         FTriangleAttributeIter attribute1Iter(rhs.point1AttributeList);
         while(attribute1Iter.Next() == TR_OK)                 // get next item (starting at beginning)
            point1AttributeList.Add(*(attribute1Iter.Data())); // pull out object from passed list, plonk into local list

         FTriangleAttributeIter attribute2Iter(rhs.point2AttributeList);
         while(attribute2Iter.Next() == TR_OK)                 // get next item (starting at beginning)
            point2AttributeList.Add(*(attribute2Iter.Data())); // pull out object from passed list, plonk into local list
         */
        
      }

      //---------------------------------------------------------------------------------
      // Method to allow easy addition of an attribute to one of the triangles corners
      // as defined by 'point'

      public void AddAttribute(int point,double attribute)
      {
         FTriangleAttribute triangleAttribute = new FTriangleAttribute();

         triangleAttribute.dAttribute = attribute;

         if(point == 0)
            point0AttributeList.Add(triangleAttribute);
         else if(point == 1)
            point1AttributeList.Add(triangleAttribute);
         else if(point == 2)
            point2AttributeList.Add(triangleAttribute);
      }

      //---------------------------------------------------------------------------------
      // Returns attribute contents for selected position in passed attributelist

      public double GetAttribute(FList<FTriangleAttribute> attributeList,int position)
      {
         int count=0;
         double dVal = 0.0;
      
         foreach(FTriangleAttribute attribute in attributeList)   // get next item (starting at beginning)
         {
            if(count++ == position)                          // have we found correct position
            {
               dVal = attribute.dAttribute;                  // assign contents to val
               break;
            }
         }

         return(dVal);
      }

      //---------------------------------------------------------------------------------
      // This method firstly checks to see if the passed point falls within this triangle.
      // If it does, then it will go through the 5 attribute types finding the attribute
      // at the specified point, and loading this info into the passed attribute list.
      // We find the attribute at the specified point, by making a plane, and then seeing
      // where a line going through the point (perpendicularly) cuts through the plane.

      public bool Intersection(double x,double y,FList<FTriangleAttribute> attributeList)
      {
         bool bInside = false;                         // assume point not inside triangle
         attributeList.Clear();
         double A,B,C,D;                               // plane variables

         daXPts[0] = point0.x;
         daYPts[0] = point0.y;                         // load the points now
         daXPts[1] = point1.x;
         daYPts[1] = point1.y;
         daXPts[2] = point2.x;     
         daYPts[2] = point2.y;

         if(FMath.PointInPolygon((int)TrianglePoints.POINTS,daXPts,daYPts,x,y))  // is our point within the triangle
         {
            bInside = true;      

            IEnumerator attribute0Iter = point0AttributeList.GetEnumerator();
            IEnumerator attribute1Iter = point1AttributeList.GetEnumerator();
            IEnumerator attribute2Iter = point2AttributeList.GetEnumerator();

            while(attribute0Iter.MoveNext() &&                   // all three must have attribute to continue
                  attribute1Iter.MoveNext() && attribute2Iter.MoveNext())
            {
               FTriangleAttribute attribute0 = (FTriangleAttribute)attribute0Iter.Current;  // get attribute contents
               FTriangleAttribute attribute1 = (FTriangleAttribute)attribute1Iter.Current;  // get attribute contents
               FTriangleAttribute attribute2 = (FTriangleAttribute)attribute2Iter.Current;  // get attribute contents
               double dAttr0 = attribute0.dAttribute;                   // assign contents to val
               double dAttr1 = attribute1.dAttribute;                   // assign contents to val
               double dAttr2 = attribute2.dAttribute;                   // assign contents to val
            
               FMath.GetPlaneEquation(point0.x,point0.y,dAttr0,
                                      point1.x,point1.y,dAttr1,     // now using my three corner points
                                      point2.x,point2.y,dAttr2,     // I need to make a plane
                                      out A,out B,out C,out D);     // returns plane equation coefficients 

               double dNewAttribute = FMath.PlanarIntersection(x,y,A,B,C,D); // where does point hit this plane?

               FTriangleAttribute newAttribute = new FTriangleAttribute();    // create new attribute object
               newAttribute.dAttribute = dNewAttribute;            // save data to object
               attributeList.Add(newAttribute);                    // save object to new attribute list
            }
         }

         return(bInside);
      }

      //---------------------------------------------------------------------------------
      // Outputs triangle as a string. We only show the second (RESIDENTIAL) attribute since this is 
      // all we are interested in at the moment.

      public override string ToString()
      {
         IEnumerator attribute0Iter = point0AttributeList.GetEnumerator();
         IEnumerator attribute1Iter = point1AttributeList.GetEnumerator();
         IEnumerator attribute2Iter = point2AttributeList.GetEnumerator();

         string intro = "Triangle coords and attributes :";
         string str0 = " (" + point0.x.ToString() + "," + point0.y.ToString() + ") - ";
         string str1 = " (" + point1.x.ToString() + "," + point1.y.ToString() + ") - ";
         string str2 = " (" + point2.x.ToString() + "," + point2.y.ToString() + ") - ";

         while(attribute0Iter.MoveNext() &&                   // all three must have attribute to continue
               attribute1Iter.MoveNext() && attribute2Iter.MoveNext())
         {
            FTriangleAttribute attribute0 = (FTriangleAttribute)attribute0Iter.Current;  // get attribute contents
            FTriangleAttribute attribute1 = (FTriangleAttribute)attribute1Iter.Current;  // get attribute contents
            FTriangleAttribute attribute2 = (FTriangleAttribute)attribute2Iter.Current;  // get attribute contents

            double dAttr0 = attribute0.dAttribute;                  // assign contents to val
            double dAttr1 = attribute1.dAttribute;                  // assign contents to val
            double dAttr2 = attribute2.dAttribute;                  // assign contents to val

            str0 += dAttr0.ToString() + ",";
            str1 += dAttr1.ToString() + ",";
            str2 += dAttr2.ToString() + ",";
         }

         string str = intro + str0 + str1 + str2;
         return(str);
      }
   }

   //---------------------------------------------------------------------------
   // Class definition for FTriangleAttribute

   public class FTriangleAttribute
   {
      public double dAttribute = 0;

      //---------------------------------------------------------------------------------
      // Constructor

      public FTriangleAttribute()
      {

      }

      //---------------------------------------------------------------------------------
      // Copy Constructor.

      public FTriangleAttribute(FTriangleAttribute rhs) 
      {
         dAttribute = rhs.dAttribute;        
      }
   }
}

//---------------------------------------------------------------------------------


