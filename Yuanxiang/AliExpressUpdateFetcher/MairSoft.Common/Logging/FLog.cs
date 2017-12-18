//----------------------------------------------------------------------------
// FLog.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 1999-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// Logging class

// REVISION HISTORY:
// Date           Author            Changes
// 04 Aug 1999    Francis Mair      1st implementation
// 18 May 2003    Francis Mair      C# conversion
//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.IO;
using System.Threading;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FLog
   {
      //---------------------------------------------------------------------------------
      // Default log file (needs to go first)

      static readonly string LOGFILE = "logfile.txt";     // logfile output

      //---------------------------------------------------------------------------------
      // Static initialisations

      static FLog logInstance = new FLog();       // create now

      //---------------------------------------------------------------------------------
      // Log level string map and levels

      string[] LogLevelText = { "-","FTL","ERR","WRN","INF","DTL","DBG","TRC"};  

      public enum Level { None, Fatal, Error, Warning, Info, Detail, Debug, Trace };

      //---------------------------------------------------------------------------------
      // Member variables

      private Level eLevel;                       // current log level
      private string  sFilename;                  // name of logfile

      //---------------------------------------------------------------------------------
      // Constructor

      private FLog()
      {
         //putenv("TZ=NZL-12");                 // NZ time zone is -12 (ie 12 hours ahead)
         //tzset();

         eLevel = Level.Info;                   // log level set to info

         sFilename = LOGFILE;                   // assume default log file name
      }

      //--------------------------------------------------------------------------
      // Returns a reference to the current logging object singleton, creating it
      // if necessary.

      public static FLog GetInstance()
      {         
         return(logInstance);
      }

      //---------------------------------------------------------------------------------
      // Static log write method. Call this on it's own to output a line to the log file.
      // This static function also creates the log if required.

      public static void Write(Level level,string message)
      {
         FLog.GetInstance().Output(level,message);
      }

      //---------------------------------------------------------------------------------
      // Static log write method. Call this on it's own to output a line to the log file.
      // This static function also creates the log if required. This version of the Write
      // method also outputs an exception trace if possible.

      public static void Write(Level level,string message,Exception exception)
      {
         FLog.GetInstance().Output(level,message + " : " + exception.ToString());
      }

      //---------------------------------------------------------------------------------
      // Log output function. This writes the current message (in the global buffer) into
      // the log file, prepended with the current time and log level string.
      // Here is an example output log file line:
      // 05May2003 06:49:11 INF FCompute::Calculate : Loaded curve parameters.

      private void Output(Level level,string sText)
      {
         if(level > eLevel) return;             // passed log level too high

         Monitor.Enter(logInstance);            // block other threads from doing this at the same time

         DateTime dateTime = DateTime.Now;

         string sTimeStr = dateTime.ToString("ddMMMyyyy HH:mm:ss");
         string sLogLevel = LogLevelText[(int)level];
         string sOutput = sTimeStr + " " + sLogLevel + " " + sText;

         StreamWriter streamWriter;
         streamWriter = File.AppendText(sFilename);
         streamWriter.WriteLine(sOutput);
         streamWriter.Close();

         Monitor.Exit(logInstance);          // release now so other threads can access _log
      }

      //---------------------------------------------------------------------------------
      // Deletes the log file

      public void Delete()
      {
         File.Delete(sFilename);               // delete logfile
      }

      //---------------------------------------------------------------------------------
      // Log level property

      public Level LogLevel
      {
         get { return(eLevel); }
         set { eLevel = value; }
      }
   }
}
//---------------------------------------------------------------------------------

