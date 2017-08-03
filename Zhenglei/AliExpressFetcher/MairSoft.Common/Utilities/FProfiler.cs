//----------------------------------------------------------------------------
// FProfiler.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2001-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// This class gives us profiling information when we need to know what is
// taking a long time and what isn't. We can create the class, then we
// call the set member for each instance in the function. And when finished
// we can do an output dump, which will give us the number of milliseconds
// difference between each set member call.

// REVISION HISTORY:
// Date          Author             Changes
// 14 Nov 2000   Francis Mair       1st implementation 
// 28 Oct 2004   Francis Mair       C# conversion

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

   public class FProfiler
   {
      // keep track of times
      private ArrayList setPointList = new ArrayList();
      //////////
      // Time structure
      //struct timeb    tstruct;
      //////////
      // List of times
      //double          *_times;
      //////////
      // List of time differences
      //double          *_diffs;
      //////////
      // Size of array
      //int             _size;
      //////////
      // Start time of profiling
      //double          _startTime;
      //////////
      // Number of diffs skipped
      //int             _skipped;

      //---------------------------------------------------------------------------------
      // Constructor

      public FProfiler()
      {
         //Init(100);
      }

      //---------------------------------------------------------------------------------
      // Constructor taking size

      public FProfiler(int size)
      {
         Init(size);
      }

      //----------------------------------------------------------------------------
      // Initialises our internal arrays

      private void Init(int size)
      {
         /*
         _size = size;
         _times = new double[size];
         _diffs = new double[size];
          
         for(int i=0; i<size; i++)           // clear the arrays
         {  _times[0] = 0.0;
            _diffs[0] = 0.0;
         }

         _startTime = getTime();             // start time
         _skipped = 0;                       // number of diffs skipped
         */
      }

      //----------------------------------------------------------------------------
      // Sets a time event. We need to specify which number event this is by passing
      // a value. Note that we need to have every set() point accessed in the code, 
      // otherwise we can't work out a valid difference. To handle this we keep track
      // of the lastPos (ie last entry) and unless we are on the next point after
      // the last entry we don't record a difference, since we cannot calculate it.
      // It may be possible to make the output data more complete if we keep track
      // of all differences, but then we may need more complex logic, and also a more
      // detailed output.

      public void Set(int pos)
      {
         /*
         if(pos>_size) return;               // exceeding internal arrays
          
         _times[pos] = getTime();

         if((pos >= 1) && (_times[pos - 1] > 0.0) && (_times[pos] > _times[pos - 1])) // can't have a difference for the first breakpoint
            _diffs[pos] += _times[pos] - _times[pos - 1];     // keep track of difference    
         else _skipped++;
         */
      }

      //----------------------------------------------------------------------------
      // Outputs an information table showing us the time differences between
      // the set breakpoints.

      public string Output()
      {
         string str = "";
         /*
         char caBuff[100];
         RWCString str;

         for(int i=1; i<_size; i++)
         { 
            if(_times[i] < 1) break;                // time will always be greater than 1, else not set

            sprintf(caBuff,"\nDiff betw. %d & %d = %.3f",i,i-1,_diffs[i]);
            str += caBuff;
         }

         double total = getTotalTime();
         sprintf(caBuff,"\nTotal time elapsed: %.3f",total);
         str += caBuff;
         sprintf(caBuff,"\nTotal diffs skipped: %d\n",_skipped);
         str += caBuff;
         */
         return(str);
      }

      //----------------------------------------------------------------------------
      // Returns the current time as a double (left of decimal = seconds & right of
      // decimal = milliseconds)

      private double GetTime()
      {
         double time = 0;
         /*
         ftime(&tstruct);                    // retrieve current time  (seconds & milliseconds)
          
         double time = tstruct.time;
         time += ((double)tstruct.millitm) / 1000.0;

         */
         return(time);
      }

      //----------------------------------------------------------------------------
      // Returns the total time spent in profiling.

      public double GetTotalTime()
      {
         return 0;
         //return(getTime() - _startTime);
      }

   }

   //---------------------------------------------------------------------------
   // Class definition for FPointList
/*
   public class FPointList : CollectionBase
   {
      //------------------------------------------------------------------------------
      // Constructor

      public FPointList()
      {

      }
         
      //------------------------------------------------------------------------------
      // Add new item

      public void Add(FPoint point)
      {         
         try
         {                
            List.Add(point);                  // use base class to process actual collection operation
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.INFO,"FPointList.Add : " + ex.Message);
         }
      }
   }
   */
}
//---------------------------------------------------------------------------------


