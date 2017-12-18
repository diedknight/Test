//----------------------------------------------------------------------------
// FProfile.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 1999-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// FProfile is a framework class which allows the calling code to save and
// load settings easily and effectively. It uses an ascii based format, as per:

// [SECTION]
// KEY=VALUE
// KEY=VALUE
// etc...

// The file is broken into many uniquely named sections, with each section having
// unique key value pairs. Saving using the same section/key values will overwrite 
// the previous data value for this section/key.

// NOTE: This class does not currently have support for multiple usage on one file.
// ie. each instance will load the file into memory and act on the data in memory,
// only saving the data when requested, or upon destruction. Thus make sure you don't
// use two instances to modify the same file.

// REVISION HISTORY:
// Date           Author            Changes
// 28 Dec 1999    Francis Mair      1st implementation
// 25 May 2003    Francis Mair      C# conversion

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FProfile
   {
      private bool _dataChanged = false;       // loaded array has been updated
      private string _fileName;                // filename of loaded file
      private FSectionItemList _sectionList = new FSectionItemList();   // list of data items in sections

      //---------------------------------------------------------------------------------
      // Constructor. Constructs a FProfile object for an application.

      public FProfile()  
      {

      }   

      //---------------------------------------------------------------------------------
      // Constructor. Constructs a FProfile object for an application.
      // FileName specifies the name of the settings file to use.

      public FProfile(string sFileName)  
      {
         LoadFile(sFileName);                        // load the file now
      }   

      //---------------------------------------------------------------------------------
      // Constructor. Constructs a FProfile object for an application.
      // stream specifies the name of the settings file to use.

      public FProfile(Stream strm)  
      {
         LoadStream(strm);                           // load the stream now
      }   

      //-----------------------------------------------------------------------------
      // Destructor for FProfile class. 

      ~FProfile()
      {
         UpdateFile();                               // output data if necessary
      }

      //-----------------------------------------------------------------------------
      // Erases the data value associated with an settings file entry.
      // Call deleteKey to erase a data value associated with a settings file entry. 
      // Section is string containing the name of an settings file section, and key is 
      // a string containing the name of the key from for which to set a NULL value.
      // Note: Attempting to erase a data value in a non-existent section or attempting 
      // to erase data from a non-existent key are not errors. In these cases, deleteKey 
      // creates the section and key and sets its initial value to an empty value.

      public void DeleteKey(string section,string key)
      {
         this[section,key] = "";
      }

      //-----------------------------------------------------------------------------
      // Erases an entire section of an settings file.
      // Call eraseSection to remove a section, all its keys, and their data values 
      // from an settings file. section identifies the settings file section to remove. If a 
      // section cannot be removed, an exception is raised.

      public void EraseSection(string section)
      {
          int count = 0;
          foreach(FSectionItem sectionItem in _sectionList)
          {
              if(sectionItem._section == section)                   // found correct section item
              {
                  sectionItem._itemList.Clear();                    // delete contents of list
                  _sectionList.RemoveAt(count);                      // removes this section item
                  break;
              }
              count++;  
          }
      }

      //-----------------------------------------------------------------------------
      // Reads all the key names from a specified section of an settings file into a string list.
      // Call readSection to retrieve the names of all keys within a specified section 
      // of an settings file into a string list object.
      // section identifies the section from which to retrieve a list of key names. 
      // strings specifies the string object to hold the retrieved names. 

      public void ReadSection(string section,StringCollection strings)
      {
          strings.Clear();

          foreach(FSectionItem sectionItem in _sectionList)
          {
              if(sectionItem._section == section)                    // found correct section item
              {
                  foreach(FProfileItem profileItem in sectionItem._itemList)
                  {
                      strings.Add(profileItem._key);                 // add string for each key name
                  }
                  break;
              }
          }
      }

      //-----------------------------------------------------------------------------
      // Reads the names of all sections in an settings file into a string list.
      // Call readSections to retrieve the names of all sections in an settings file into 
      // a string list object.
      // Strings specifies the string object to hold the retrieved names.

      public void ReadSections(StringCollection strings)
      {
            strings.Clear();

            foreach(FSectionItem sectionItem in _sectionList)   // step through all section items
            {
                strings.Add(sectionItem._section);             // add string for section name
            }
      }

      //-----------------------------------------------------------------------------
      // Reads the values from all keys within a section of an settings file into a string list.
      // Call readSectionValues to read the values from all keys within a specified 
      // section of an settings file into a string list object. Section identifies the section 
      // in the file from which to read key values. Strings is the string list object into 
      // which to read the values. Strings specifies the string object to hold the retrieved names.

      public void ReadSectionValues(string section,StringCollection strings)
      {
          strings.Clear();

          foreach(FSectionItem sectionItem in _sectionList)
          {
            if(sectionItem._section == section)                    // found correct section item
            {
                foreach(FProfileItem profileItem in sectionItem._itemList)
                {
                  strings.Add(profileItem._value);               // add string for each value name
                }
                break;
            }
         }
      }

      //-----------------------------------------------------------------------------
      // Indicates whether a section exists in the settings file.
      // Use sectionExists to determine whether a section exists within the settings file 
      // specified in FileName. Section is the settings file section sectionExists determines 
      // the existence of. 
      // sectionExists returns a Boolean value that indicates whether the section in 
      // question exists.

      public bool SectionExists(string section)
      {
         bool found = false;

         foreach(FSectionItem sectionItem in _sectionList)
         {
            if(sectionItem._section == section)
            {  found = true;
               break;
            }
         }

         return(found);
      }

      //-----------------------------------------------------------------------------
      // Retrieves a Boolean value from an settings file. 
      // Call readBool to read a Boolean value from an settings file. Section identifies 
      // the section in the file that contains the desired key. key is the name of 
      // the key from which to retrieve the Boolean value. Default is the Boolean value 
      // to return if the:
      //·	Section does not exist.
      //·	Key does not exist.
      //·	Data value for the key is not assigned.

      public bool ReadBool(string section,string key,bool def)
      {
         string val = GetValue(section,key,def.ToString());
         return(bool.Parse(val));
      }

      //-----------------------------------------------------------------------------
      // Retrieves a date value from an settings file. 
      // Call ReadDate to read a date value from an settings file. Section identifies the 
      // section in the file that contains the desired key. key is the name of the key 
      // from which to retrieve the date value. Default is the date value to return if the:
      //.	Section does not exist.
      //·	Key does not exist.
      //·	Data value for the key is not assigned.
/*
      public DateTime ReadDate(const FString section,const FString key,FDate def)
      {
         FDate date; // (GetValue(section,key,def));
         return(date);
      }
*/
      //-----------------------------------------------------------------------------
      // Retrieves a double value from an settings file. 
      // Call ReadDouble to read a double value from an settings file. Section identifies 
      // the section in the file that contains the desired key. key is the name of 
      // the key from which to retrieve the float value. Default is the float value 
      // to return if the :
      //·	Section does not exist.
      //·	Key does not exist.
      //·	Data value for the key is not assigned.

      public double ReadDouble(string section,string key,double def)
      {
         string val = GetValue(section,key,def.ToString());
         return(double.Parse(val));
      }

      //-----------------------------------------------------------------------------
      // Retrieves a double value from an settings file. 
      // Call ReadDouble to read a double value from an settings file. Section identifies 
      // the section in the file that contains the desired key. key is the name of 
      // the key from which to retrieve the float value. Default is the float value 
      // to return if the :
      //·	Section does not exist.
      //·	Key does not exist.
      //·	Data value for the key is not assigned.

      public int ReadInt(string section,string key,int def)
      {
         string val = GetValue(section,key,def.ToString());
         return(int.Parse(val));
      }

      //-----------------------------------------------------------------------------
      // Retrieves a string value from an settings file. 
      // Call ReadString to read a string value from an settings file. Section identifies 
      // the section in the file that contains the desired key. key is the name of 
      // the key from which to retrieve the value. Default is the string value to 
      // return if the:
      //·	Section does not exist.
      //·	Key does not exist.
      //·	Data value for the key is not assigned.

      public string ReadString(string section,string key,string def)
      {
         return(GetValue(section,key,def));
      }

      //-----------------------------------------------------------------------------
      // Retrieves a time value from an settings file. 
      // Call ReadTime to read a date value from an settings file. Section identifies the 
      // section in the file that contains the desired key. key is the name of the key 
      // from which to retrieve the date value. Default is the date value to return if the:
      //.	Section does not exist.
      //·	Key does not exist.
      //·	Data value for the key is not assigned.
/*
      FTime ReadTime(const FString section,const FString key,FTime def)
      {
         FString timeStr = GetValue(section,key,"");
         if(timeStr.Length()==0)
            timeStr = FString((long)def.GetTime());

         FTime time((time_t)timeStr.AsInt());
         return(time);
      }
*/
      //-----------------------------------------------------------------------------
      // Writes a Boolean value to an settings file.
      // Call writeBool to write a Boolean value to an settings file. Section identifies 
      // the section in the file that contain the key to which to write. key is the 
      // name of the key for which to set a value. Value is the Boolean value to write.
      // Note: Attempting to write a data value to a non-existent section or attempting 
      // to write data to a non-existent key are not errors. In these cases, writeBool 
      // creates the section and key and sets its initial value to Value.

      public void WriteBool(string section,string key,bool val)
      {
         this[section,key] = val.ToString();
      }

      //-----------------------------------------------------------------------------
      // Writes a date value to an settings file.
      // Call writeDate to write a date value to an settings file. Section identifies the 
      // section in the file that contain the key to which to write. key is the name 
      // of the key for which to set a value. Value is the date value to write.
      // Note: Attempting to write a data value to a non-existent section or attempting 
      // to write data to a non-existent key are not errors. In these cases, writeDate 
      // creates the section and key and sets its initial value to Value.

      public void WriteDate(string section,string key,DateTime val)
      {
         this[section,key] = val.ToString();
      }

      //-----------------------------------------------------------------------------
      // Writes a double value to an settings file.
      // Call writeDouble to write a float value to an settings file. Section identifies the 
      // section in the file that contain the key to which to write. key is the name 
      // of the key for which to set a value. Value is the float value to write.
      // Note: Attempting to write a data value to a non-existent section or attempting 
      // to write data to a non-existent key are not errors. In these cases, writeDouble
      // creates the section and key and sets its initial value to Value.

      public void WriteDouble(string section,string key,double val)
      {
         this[section,key] = val.ToString();
      }

      //-----------------------------------------------------------------------------
      // Writes an integer value to an settings file.
      // Call writeInteger to write an integer value to an settings file. Section identifies 
      // the section in the file that contain the key to which to write. key is the 
      // name of the key for which to set a value. Value is the integer value to write.
      // Note: Attempting to write a data value to a non-existent section or attempting 
      // to write data to a non-existent key are not errors. In these cases, writeInteger 
      // creates the section and key and sets its initial value to Value.

      public void WriteInteger(string section,string key,int val)
      {
         this[section,key] = val.ToString();
      }

      //-----------------------------------------------------------------------------
      // Writes a string value to an settings file.
      // Call writeString to write a string value to an settings file. Section identifies 
      // the section in the file that contain the key to which to write. key is 
      // the name of the key for which to set a value. Value is the string value to write.
      // Note: Attempting to write a data value to a non-existent section or attempting 
      // to write data to a non-existent key are not errors. In these cases, writeString 
      // creates the section and key and sets its initial value to Value.

      public void WriteString(string section,string key,string val)
      {
         this[section,key] = val;
      }

      //-----------------------------------------------------------------------------
      // Writes a time value to an settings file.
      // Call writeTime to write a time value to an settings file. Section identifies the 
      // section in the file that contain the key to which to write. key is the name 
      // of the key for which to set a value. Value is the date value to write.
      // Note: Attempting to write a data value to a non-existent section or attempting 
      // to write data to a non-existent key are not errors. In these cases, writeTime
      // creates the section and key and sets its initial value to Value.

      public void WriteTime(string section,string key,DateTime val)
      {
         this[section,key] = val.ToString();
      }

      //-----------------------------------------------------------------------------
      // Indexer used for setting and getting our values

      public string this[string section,string key]
      {
         get
         {
            return(GetValue(section,key,""));
         }
         set
         {
            SetValue(section,key,value);
         }
      }

      //-----------------------------------------------------------------------------
      // Utility function used to access data members in our internal settings array

      private string GetValue(string section,string key,string def)
      {
         FProfileItem profileItem = GetItem(section,key,def);

         return(profileItem._value);
      }

      //-----------------------------------------------------------------------------
      // Utility function used to set data members in our internal settings array

      private bool SetValue(string section,string key,string val)
      {
         FProfileItem profileItem = GetItem(section,key,"");

         if(profileItem._value != val)
         {
            profileItem._value = val;
            _dataChanged = true;
         }

         return _dataChanged;
      }

      //-----------------------------------------------------------------------------
      // Pulls out the appropriate profileItem, or if one doesn't exist we create it
      // and add it in.

      private FProfileItem GetItem(string section,string key,string def)
      {
         foreach(FSectionItem sectionItem in _sectionList)
         {
            if(sectionItem._section == section)                    // found correct section item
            { 
               foreach(FProfileItem profileItem in sectionItem._itemList)
               {
                  if(profileItem._key == key)                      // found correct profile item
                  {
                     // found section and key, return item found
                     return(profileItem);
                  }
               }

               // key not found, so create new key and add to our list
               FProfileItem newProfileItem = new FProfileItem(key,def);
               sectionItem._itemList.Add(newProfileItem);  
               return(newProfileItem);
            }
         }

         // section not found, create new section and key
         FSectionItem newSectionItem = new FSectionItem(section);
         _sectionList.Add(newSectionItem);         
         FProfileItem addProfileItem = new FProfileItem(key,def);
         newSectionItem._itemList.Add(addProfileItem);          
         return(addProfileItem);
      }

      //-----------------------------------------------------------------------------
      // Loads all of the data in the file into our linked lists in memory to be 
      // managed internally. If we load a line which is not a comment and which
      // doesn't have an equal sign we automatically create a 'name' for it using
      // 'sectionname_datan' where n increases by one (starting at 0) for each next
      // line with no equals sign. This means we can dump large amounts of data into
      // a section with having any name part, and the load code will autogenerate name
      // parts so we can still access this data using this class.

      public bool LoadFile(string fileName)
      {
         //UpdateFile();                              // do we need to save previous data 
         _fileName = fileName;

         try
         {
            Stream stream = File.OpenRead(_fileName);   
         
            LoadStream(stream);
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Warning,"FProfile.LoadFile : " + ex.Message);
         }

         _dataChanged = false;                        // file loaded, no new data present
         return true;
      }

      //-----------------------------------------------------------------------------
      // Loads all of the data in the file into our linked lists in memory to be 
      // managed internally. If we load a line which is not a comment and which
      // doesn't have an equal sign we automatically create a 'name' for it using
      // 'sectionname_datan' where n increases by one (starting at 0) for each next
      // line with no equals sign. This means we can dump large amounts of data into
      // a section with having any name part, and the load code will autogenerate name
      // parts so we can still access this data using this class.

      private bool LoadStream(Stream strm)
      {
         int iEnd=0;
         _sectionList.Clear();                        // flush out existing data
         FSectionItem sectionItem = null;

         try
         {
            StreamReader streamReader = new StreamReader(strm);

            string sLine = null;

            while((sLine = streamReader.ReadLine()) != null)
            {
               if(sLine.Length == 0) continue;              // empty line, ignore
               if(sLine[0] == '#') continue;                // comment line, ignore

               if(sLine[0]=='[')                            // section found
               {
                  if((iEnd = sLine.IndexOf("]")) != -1)     // found section end
                  {            
                     sectionItem = new FSectionItem(sLine.Substring(1,iEnd-1));
                     _sectionList.Add(sectionItem);
                  }
               }
               else
               {
                  if((iEnd = sLine.IndexOf("=")) != -1)     // find equals sign
                  { 
                     string key = sLine.Substring(0,iEnd);  // grabbed key name
                     string val = sLine.Substring(iEnd+1);  // extract data

                     if(sectionItem != null) 
                        sectionItem._itemList.Add(new FProfileItem(key,val));
                  }
                  else                                      // no name part supplied, auto generate one
                  {  
                     if(sectionItem != null)                // need to have a previous section item
                     {
                        string key = sectionItem._section;
                        key += "_data";
                        key += sectionItem._lastUnnamed++;  // use last count for section & increment
               
                        sectionItem._itemList.Add(new FProfileItem(key,sLine));   // generated name, with line contents
                     }
                  }
               }
            }
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Warning,"FProfile.LoadStream : " + ex.Message);
         }
         return true;
      }

      //-----------------------------------------------------------------------------
      // Flushes buffered settings file data to disk.
      // Call UpdateFile to flush buffered reads from and writes to the settings file to 
      // disk. This function is automatically called when the object destructs.
      // First of all we create a temporary file in the same directory as the user wants
      // the final file. (Temporary file, so that original file is only interrupted by
      // the shortest possible time, ie copying temp file to orig file) (Same directory,
      // since we know this is a writable directory!). The temporary file is then loaded
      // with the new contents, and copied across to the original filename.

      public void UpdateFile()
      {
         if(_dataChanged == false) return;                           // nothing to do

         try
         {
            Stream stream = File.OpenWrite(_fileName);
            StreamWriter streamWriter = new StreamWriter(stream);

            foreach(FSectionItem sectionItem in _sectionList)
            {
               streamWriter.Write("\n[" + sectionItem._section + "]\n");   // output section name   

               foreach(FProfileItem profileItem in sectionItem._itemList)
               {
                  streamWriter.Write(profileItem._key + "=" + profileItem._value + "\n");
               }
            }
                           
            streamWriter.Close();
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Warning,"FProfile.UpdateFile : " + ex.Message);
         }

         _dataChanged = false;
      }

      //-----------------------------------------------------------------------------
      // Filename property

      public string Filename
      {
         get { return(_fileName); }
         set { _fileName = value; }
      }
   }

   //---------------------------------------------------------------------------
   //---------------------------------------------------------------------------
   // Class definition (FProfileItem)

   public class FProfileItem
   {
      public string _key;            // key name
      public string _value;          // data value

      //---------------------------------------------------------------------------
      // Constructor

      public FProfileItem(string key,string val) 
      { 
         _key = key; 
         _value = val; 
      }

      //---------------------------------------------------------------------------
      // Copy constructor

      public FProfileItem(FProfileItem rhs) 
      { 
         _key = rhs._key; 
         _value = rhs._value; 
      }
   }

   //---------------------------------------------------------------------------
   //---------------------------------------------------------------------------
   // Class definition (FProfileItemList)

   public class FProfileItemList : ArrayList
   {
      //---------------------------------------------------------------------------
      // Constructor
   
      public FProfileItemList()
      {

      }

      //------------------------------------------------------------------------------
      // Adds a FProfileItem to our list

      public FProfileItem Add(FProfileItem profileItem)
      {
         // Use base class to process actual collection operation

         try
         {                
            base.Add(profileItem);
         }
         catch(Exception ex)
         {
            Console.Write(ex.Message);

            //MessageBox.Show( _mainForm, ex.Message, "Exception during database read!" );
            //return;
         }

         return profileItem;
      }
   }

   //---------------------------------------------------------------------------
   //---------------------------------------------------------------------------
   // Class definition (FSectionItem)

   public class FSectionItem
   {
      public string _section;                 // section name
      public FProfileItemList _itemList = new FProfileItemList();      // list of profile items
      public int _lastUnnamed = 0;            // counter for items where we generate names (ie if only value is provided for the line)

      //---------------------------------------------------------------------------
      // Constructor

      public FSectionItem(string section) 
      { 
         _section = section; 
      }
      
      //---------------------------------------------------------------------------
      // Copy constructor

      public FSectionItem(FSectionItem rhs) 
      { 
         _section = rhs._section; 
         _lastUnnamed = 0; 

         foreach(FProfileItem profileItem in rhs._itemList)
         {
            _itemList.Add(new FProfileItem(profileItem));
         }
      }      
   }

   //---------------------------------------------------------------------------
   //---------------------------------------------------------------------------
   // Class definition (FSectionItemList)

   public class FSectionItemList : ArrayList
   {
      //---------------------------------------------------------------------------
      // Constructor

      public FSectionItemList()
      {

      }      
   }

   //---------------------------------------------------------------------------
   //---------------------------------------------------------------------------
   // Class definition for FProfileList (lists of profile objects)

   public class FProfileList : CollectionBase
   {
      //------------------------------------------------------------------------------
      // Constructor

      public FProfileList()
      {

      }
       
      //------------------------------------------------------------------------------
      // Add new item

      public void Add(FProfile profile)
      {         
         try
         {                
            List.Add(profile);                  // use base class to process actual collection operation
         }
         catch(Exception ex)
         {
            FLog.Write(FLog.Level.Info,"FProfileList.Add : " + ex.Message);
         }
      }
   }
}

//-----------------------------------------------------------------------------
