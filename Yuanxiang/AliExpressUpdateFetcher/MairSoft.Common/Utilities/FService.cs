//----------------------------------------------------------------------------
// FService.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2001-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FService class, allows you to control services on the specified workstation.

// REVISION HISTORY:
// Date          Author            Changes
// 19 July 2003  Francis Mair      1st implementation

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.ServiceProcess;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FService
   {
      //---------------------------------------------------------------------------------
      // Constructor

      public FService()
      {

      }

      //---------------------------------------------------------------------------------
      // Do a platform check. only win nt, 2000, xp supported for controlling services

      static public bool Available()
      {
         OperatingSystem os = Environment.OSVersion;
         
         if(os.Platform != PlatformID.Win32NT)
            return(false);

         return(true);
      }

      //---------------------------------------------------------------------------------
      // Starts the specified service. Returns true if service started successfully.

      static public bool Start(string serviceName)
      {
         if(Available() == false) return(false);            // services not available

         bool serviceStarted = false;
         TimeSpan timeout = new TimeSpan(0,0,30);

         // create instance of service controller
         ServiceController sc = new ServiceController(serviceName);

         // check the status of the service
         if(sc.Status == ServiceControllerStatus.Running)
            return(true);                                   // already running

         if(sc.Status == ServiceControllerStatus.Stopped)
         {            
            sc.Start();                                     // start service

            try
            {
               // wait 30 seconds for service to start
               sc.WaitForStatus(ServiceControllerStatus.Running,timeout);

               serviceStarted = true;
            }
            catch(System.ServiceProcess.TimeoutException)
            {
               serviceStarted = false;
            }
         }
         
         sc.Close();                                        // start service
         sc = null;

         return(serviceStarted);
      }

      //---------------------------------------------------------------------------------
      // Stops the specified service. Returns true if service stopped successfully.

      static public bool Stop(string serviceName)
      {
         if(Available() == false) return(false);         // services not available

         bool serviceStopped = false;
         TimeSpan timeout = new TimeSpan(0,0,30);

         // create instance of service controller
         ServiceController sc = new ServiceController(serviceName);

         // check the status of the service
         if(sc.Status == ServiceControllerStatus.Stopped)
            return(true);                                   // already stopped

         // check the status of the service
         if(sc.Status == ServiceControllerStatus.Running)
         {
            // make sure the service is stoppable
            if(sc.CanStop)
            {
               // stop service
               sc.Stop();

               try
               {
                  // wait 30 seconds for service to stop
                  sc.WaitForStatus(ServiceControllerStatus.Stopped,timeout);

                  serviceStopped = true;
               }
               catch(System.ServiceProcess.TimeoutException)
               {
                  serviceStopped = false;          // service did not stop in specified time
               }
            }
            else
            {
               serviceStopped = false;             // service cannot be stopped
            }
         }

         
         sc.Close();                               // cleanup
         sc = null;

         return(serviceStopped);
      }
   }
}
//---------------------------------------------------------------------------------


