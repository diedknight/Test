//----------------------------------------------------------------------------
// FEmbeddedResource.cs
//----------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2004 Francis Mair (frank@mair.net.nz)

// DESCRIPTION:   
// FEmbeddedResource class.  Handles our loading from embedded resources

// REVISION HISTORY:
// Date             Author             Changes
// 23 Jan 2004      Francis Mair       1st implementation

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition

	public class FEmbeddedResource
	{
      //---------------------------------------------------------------------------
      // Constructor

		public FEmbeddedResource()
		{

      }

      //------------------------------------------------------------------------------
      // Creates an XML embedded resource file using the passed object. Saves the file
      // in the current (unless specified) working directory.

      public static void CreateResource(object obj,string filename)
      {
         XmlSerializer serializer = new XmlSerializer(obj.GetType());

         TextWriter writer = new StreamWriter(filename);

         serializer.Serialize(writer,obj);

         writer.Close();
      }

      //------------------------------------------------------------------------------
      // Loads in object from embedded resource. Type is the object we want to create.
      // use 'object.GetType()' or typeof(FParameters) to get the System.Type value.

      public static object LoadObject(string name,Type type)
      {
         // resources are named using a fully qualified name.
         Assembly asm = Assembly.GetCallingAssembly();
         string nm = asm.GetName().Name;
         Stream strm = asm.GetManifestResourceStream(nm + "." + name);

         XmlTextReader reader = new XmlTextReader(strm);

         // use the Deserialize method to restore the object's state with data from the XML document stream
         XmlSerializer serializer = new XmlSerializer(type);
         object obj = serializer.Deserialize(reader);

         return obj;
      }

      //------------------------------------------------------------------------------
      // Loads in compressed object from embedded resource. Type is the object we want to create.
      // use 'object.GetType()' or typeof(FParameters) to get the System.Type value.

      public static object LoadCompressedObject(string name,Type type,string password)
      {
         // resources are named using a fully qualified name.
         Assembly asm = Assembly.GetCallingAssembly();
         string nm = asm.GetName().Name;
         Stream strm = asm.GetManifestResourceStream(nm + "." + name);

         // decompress stream now
         Stream unzippedStream = FZip.Unzip(strm,password);

         XmlTextReader reader = new XmlTextReader(unzippedStream);       

         // use the Deserialize method to restore the object's state with data from the XML document stream
         XmlSerializer serializer = new XmlSerializer(type);
         object obj = serializer.Deserialize(reader);

         unzippedStream.Close();
         strm.Close();
         return obj;
      }

      //------------------------------------------------------------------------------
      // Read in embedded resource. Make sure resource has "embedded resource" as it's build type.

      public static string GetResourceAsString(string name)
      {
         string data = "";

         // resources are named using a fully qualified name.
         Assembly asm = Assembly.GetCallingAssembly();
         string nm = asm.GetName().Name;
         Stream strm = asm.GetManifestResourceStream(nm + "." + name);

         // read the contents of the embedded file.
         if(strm != null)
         {
            StreamReader reader = new StreamReader(strm);
            data = reader.ReadToEnd();
         }

         return(data);
      }

      //------------------------------------------------------------------------------
      // Read in embedded resource. Make sure resource has "embedded resource" as it's build type.

      public static Stream GetResourceAsStream(string name)
      {
         Stream stream = null;

         // resources are named using a fully qualified name.
         Assembly asm = Assembly.GetCallingAssembly();
         string nm = asm.GetName().Name;

         stream = asm.GetManifestResourceStream(nm + "." + name);

         return stream;
      }

      //------------------------------------------------------------------------------
      // Read in a compressed embedded resource. 
      // Make sure resource has "embedded resource" as it's build type.
      // A compressed resource is one that has been zipped up. We can also specify
      // a password for additional security.

      public static Stream GetCompressedResourceAsStream(string name,string password)
      {
         Stream stream = null;

         // resources are named using a fully qualified name.
         Assembly asm = Assembly.GetCallingAssembly();
         string nm = asm.GetName().Name;

         stream = asm.GetManifestResourceStream(nm + "." + name);

         // decompress stream now
         Stream unzippedStream = FZip.Unzip(stream,password);

         return unzippedStream;
      }

      //------------------------------------------------------------------------------
      // Read in embedded resource name. Make sure resource has "embedded resource" as it's build type.
      // Decompresses and unencrypts using similar code to the above method. The reason we don't
      // just call the above method is because then the calling assembly becomes this assembly
      // and we can't then locate the resource.

      public static string GetCompressedResourceAsString(string name,string password)
      {
         string data = "";
         Stream stream = null;

         // resources are named using a fully qualified name.
         Assembly asm = Assembly.GetCallingAssembly();
         string nm = asm.GetName().Name;

         stream = asm.GetManifestResourceStream(nm + "." + name);

         // decompress stream now
         Stream unzippedStream = FZip.Unzip(stream,password);

         // read the contents of the embedded file.
         if(unzippedStream != null)
         {
            StreamReader reader = new StreamReader(unzippedStream);
            data = reader.ReadToEnd();
         }

         return (data);
      }

      //------------------------------------------------------------------------------
      // Returns a list of strings for filenames in the specified embedded resource 
      // directory.

      public static FStringList GetResources(string dir)
      {
         FStringList stringList = new FStringList();

         // get the calling assembly
         Assembly asm = Assembly.GetCallingAssembly();
         string nm = asm.GetName().Name;

         string path = nm + "." + dir + ".";

         // enumerate the assembly's manifest resources
         foreach(string resourceName in asm.GetManifestResourceNames()) 
         {
            if(resourceName.StartsWith(path))
            {
               string name = resourceName.Remove(0,path.Length);

               stringList.Add(name);
            }
         }

         return stringList;
      }
	}
}

//---------------------------------------------------------------------------
