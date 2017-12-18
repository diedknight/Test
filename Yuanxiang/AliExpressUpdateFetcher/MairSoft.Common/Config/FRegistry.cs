//----------------------------------------------------------------------------
// FRegistry.cs
//----------------------------------------------------------------------------
//
// COPYRIGHT:
// Copyright (C) 2003 Francis Mair (frank@mair.net.nz)
//
// DESCRIPTION:
// FRegistry class, allows us to quickly access and set values in the
// registry.
//
// EXAMPLE USAGE:
//
// FRegistry registry = new FRegistry();
//
// try
// { // WRITE PART
//   registry.OpenKey("HKEY_CURRENT_USER","Software",true);  // open software section
//   registry.CreateKey("CompanyName",true,true);            // create company name
//   registry.SetValue("value1","test String");              // save string
//   registry.SetValue("value2",2);                          // save int
//   // READ PART     
//   registry.OpenKey("HKEY_CURRENT_USER","Software",false); // open read only
//   registry.OpenKeyFromCurrentKey("CompanyName",false);    // go to company name
//   string strTest1 = (string)registry.GetValue("Value1");  // get string
//   int nTest2 = (int)registry.GetValue("Value2");          // get int
// }
// catch(Exception ex)
// {
//   MessageBox.Show(ex.Message);
// }        
//
// REVISION HISTORY:
// Date             Author                                 Changes
// 30 Jun 2003      Francis Mair (frank@mair.net.nz)       1st implementation
//----------------------------------------------------------------------------

//----------------------------------------------------------------------------
// Using

using System;
using System.IO;
using System.Security;
using System.Text;

using Microsoft.Win32;

//----------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //------------------------------------------------------------------------------
   // Class definition
   
   public class FRegistry
	{
      private RegistryKey currentKey;     // The current registry key
		private RegistryKey previousKey;    // keep track of the previous key

      //------------------------------------------------------------------------------
      // Constructor
 
		public FRegistry()
		{
			currentKey = null;
			previousKey = null;
		}

      //------------------------------------------------------------------------------
      // Current key property

      public RegistryKey GetCurrentKey
      {
         get { return currentKey; }
      }

      //------------------------------------------------------------------------------
		// Open the registry key in read only or write mode
		// <param name="registryKey">Key to open as a string "HKEY_CURRENT_USER", "HKEY_LOCAL_MACHINE"
		// "HKEY_USERS" HKEY_CLASSES_ROOT and HKEY_CURRENT_CONFIG ommitted intentionally, if
		// you need them add them </param>
		// <param name="key">key to open</param>
		// <param name="writeable">writeable equals false for readonly</param>

		public void OpenKey(string registryKey,string key,bool writeable)
		{		
			switch(registryKey.ToString())
			{
				case "HKEY_CURRENT_USER": 
               currentKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey( key, writeable ); 
               break;
				case "HKEY_LOCAL_MACHINE": 
               currentKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey( key, writeable ); 
               break;
				case "HKEY_USERS": 
               currentKey = Microsoft.Win32.Registry.Users.OpenSubKey( key, writeable ); 
               break;
				default: 
				   throw(new Exception("Invalid registry key")); 
			}
      }

      //------------------------------------------------------------------------------
      // Allow the opening of a key once the current key has been set
		// <param name="key">name of the key to open</param>
		// <param name="writeable">writeable equals false for readonly</param>

		public void OpenKeyFromCurrentKey(string key,bool writeable)
		{
			if(currentKey == null)
            throw(new Exception("The current key is invalid when calling OpenKeyFromCurrentKey"));

         previousKey = currentKey;

   		currentKey = currentKey.OpenSubKey(key,writeable);

			if(currentKey == null)
            throw(new Exception("Unable to open the specified key " + key + " currentKey is still valid"));
		}

      //------------------------------------------------------------------------------
		// Set a value on the current key
		// <param name="name">name of the value to be set</param>
		// <param name="value">value to set</param>
		// <returns>true on success</returns>

		public void SetValue(string name,object value)
		{
			if(currentKey == null)
            throw(new Exception("The current key was invalid when calling SetValue"));

   		currentKey.SetValue(name,value);
		}

      //------------------------------------------------------------------------------
		// Get a value from the registry

		public object GetValue(string name)
		{
			return currentKey.GetValue(name);
		}

      //------------------------------------------------------------------------------
		/// As a previous key is being kept allow for an easy step back

      public void RevertToPrevious()
		{
			if(previousKey != null)
			{
				currentKey = previousKey;
			}
			else
			{
            throw(new Exception("Unable to revert to the previous key as it equals null"));
			}
		}

      //------------------------------------------------------------------------------
		// Delete the current key and optionally move back to the previous key 
		// <param name="moveBack">true to revert to the previous key</param>
		// <param name="key">Name of the subkey to delete</param>
		// <param name="deleteTree">delete the entire subtree from the specified key</param>
		// <returns>returns success of deletion</returns>

		public void DeleteKey(bool moveBack,string key,bool deleteTree)
		{
			if(moveBack == true)
				RevertToPrevious();

			if(deleteTree == false)
  				  currentKey.DeleteSubKey(key,true);
			else currentKey.DeleteSubKeyTree(key);
		}

      //------------------------------------------------------------------------------
		// Delete the root key from the main registry key
		// <param name="registryKey">Key to delete from as a string "HKEY_CURRENT_USER", "HKEY_LOCAL_MACHINE"
		// "HKEY_USERS" HKEY_CLASSES_ROOT and HKEY_CURRENT_CONFIG ommitted intentionally, if
		// you need them add them</param>
		// <param name="key">name of the key to delete</param>
		// <param name="deleteTree">Delete the entire subtree</param>

		public void DeleteRootKey(string registryKey,string key,bool deleteTree)
		{
			switch(registryKey.ToString())
			{
				case "HKEY_CURRENT_USER": 
               if(deleteTree)
                     Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(key); 
               else Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(key); 
               break;
				case "HKEY_LOCAL_MACHINE": 
               if(deleteTree)
                    Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(key); 
               else Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(key); 
               break;
				case "HKEY_USERS": 
               if(deleteTree)
                    Microsoft.Win32.Registry.Users.DeleteSubKeyTree(key); 
               else Microsoft.Win32.Registry.Users.DeleteSubKey(key); 
               break;
				default: 
               throw(new Exception("Invalid registry key")); 
			}
		}

      //------------------------------------------------------------------------------
		// Create key version for initial start up of project
		// Allows the creation of a key from the root registry key
		// eg HKEY_CURRENT_USER\YOUR COMPANY NAME
		/// </summary>
		/// <param name="registryKey">Key to open as a string "HKEY_CURRENT_USER", "HKEY_LOCAL_MACHINE"
		/// "HKEY_USERS" HKEY_CLASSES_ROOT and HKEY_CURRENT_CONFIG ommitted intentionally, if
		/// you need them add them </param>
		/// <param name="key">name of the key to create</param>
		/// <returns>true on success</returns>

		public void CreateKey(string registryKey,string key)
		{
			switch(registryKey.ToString())
			{
				case "HKEY_CURRENT_USER": 
               currentKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(key); 
               break;
				case "HKEY_LOCAL_MACHINE": 
               currentKey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(key); 
               break;
				case "HKEY_USERS": 
               currentKey = Microsoft.Win32.Registry.Users.CreateSubKey(key); 
               break;
				default: 
               throw(new Exception("Invalid registry key")); 
			}
		}

      //------------------------------------------------------------------------------
		// Create a new registry key
		/// <param name="key">name of the key to be created</param>
		/// <param name="moveToKey"/>open the key after creating it</param>
		/// <param name="writeable">write permission on the key after it is open</param>

      public void CreateKey(string key,bool moveToKey,bool writeable)
		{
			if(currentKey == null)
            throw(new Exception("Need to open a key before one can be created from it"));

         previousKey = currentKey;

   		currentKey.CreateSubKey(key);

			if(moveToKey == true)
				OpenKeyFromCurrentKey(key,writeable);
		}

      //------------------------------------------------------------------------------
		// Close the current key, also allow you to optionally close the previous key
		// Returns true on success

		public void Close(bool closePrevious)
		{
			// check previous first
			if(closePrevious == true)
			{
				if(previousKey != null)
				{
					previousKey.Close();
					previousKey = null;
				}
			}

			if(currentKey != null)
			{
				currentKey.Close();
				currentKey = null;
			}
			else
            throw(new Exception("Can't delete a registry when none have been opened"));
		}

      //------------------------------------------------------------------------------
		// GetSubKeyNames is the only function that doesn't return a bool
		// this is due to the fact that it would be more irritating (ie more function calls)
		// to get the data
		// Rreturns a string array of the subkeys off the currentkey

		public string[] GetSubKeyNames()
		{
			string[] subKeys = null;

			subKeys = currentKey.GetSubKeyNames();

			return subKeys;
		}	
	}
}

//------------------------------------------------------------------------------
