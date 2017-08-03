//----------------------------------------------------------------------------
// FWindow.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2005 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FWindow class. This class handles anything to do with windowing or
// windows and forms. Currently we are only using it to save and restore
// the window location and size from a user specified profile (config.ini)
// file.

// REVISION HISTORY:
// Date          Author            Changes
// 16 Jan 2005   Francis Mair      1st implementation

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.Drawing;
using System.Windows;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FWindow
   {
      //---------------------------------------------------------------------------------
      // Constructor

      private FWindow()
      {

      }

      //---------------------------------------------------------------------------------
      // Static method to save the current window settings

      public static void SaveLayout(System.Windows.Forms.Form window,FProfile profile)
      {
         // save window height and width
         profile.WriteString("Display","WindowHeight",window.Size.Height.ToString());
         profile.WriteString("Display","WindowWidth",window.Size.Width.ToString());

         // save window start location
         profile.WriteString("Display","WindowX",window.Location.X.ToString());
         profile.WriteString("Display","WindowY",window.Location.Y.ToString());
      }

      //---------------------------------------------------------------------------------
      // Static method to load the current window settings

      public static void LoadLayout(System.Windows.Forms.Form window,FProfile profile)
      {
         int windowHeight = profile.ReadInt("Display","WindowHeight",0);
         int windowWidth = profile.ReadInt("Display","WindowWidth",0);

         int windowX = profile.ReadInt("Display","WindowX",0);
         int windowY = profile.ReadInt("Display","WindowY",0);

         // have we already pre-saved a layout
         if(windowHeight > 0 && windowWidth > 0)
         {
            window.Size = new Size(windowWidth,windowHeight);
            window.Location = new Point(windowX,windowY);
         }
      }
   }
}

//---------------------------------------------------------------------------------


