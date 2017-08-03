//----------------------------------------------------------------------------
// FSerialise.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2006 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// Utility class to perform serialisation and deserialisation operations. There
// is also a method for cloning objects using serialisation.

// REVISION HISTORY:
// Date          Author            Changes
// 12 Feb 2006   Francis Mair      1st implementation

//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace FSuite.Utilities
{
   public class FSerialise
   {   
      /// <summary>
      /// Serialises an object to an xml string
      /// </summary>
      /// <param name="obj"></param>
      /// <returns></returns>
      public static string Serialise(object obj)
		{
			string xmlString = null;
         StringBuilder stringBuilder = new StringBuilder();
         StringWriter stringWriter = new StringWriter(stringBuilder);

         XmlSerializer formatter = new XmlSerializer(obj.GetType());
         formatter.Serialize(stringWriter,obj);
         xmlString = stringBuilder.ToString();

         return xmlString;
		}

      /// <summary>
      /// Deserialises an xml string to the specified type
      /// </summary>
      /// <param name="xml"></param>
      /// <param name="type"></param>
      /// <returns></returns>
		public static object Deserialise(string xml,Type type)
		{
			StringReader stringReader = new StringReader(xml);
         object obj = null;

			try
			{
             XmlSerializer formatter = new XmlSerializer(type);
             obj = formatter.Deserialize(stringReader);
			}
			finally
			{
            stringReader.Close();
			}

         return obj;
		}

      /// <summary>
      /// This method allows us to output the specified entity to
      /// the specified filename. This is useful for debugging purposes.
      /// </summary>
      /// <param name="entity"></param>
      /// <param name="filename"></param>
      public static void SerialiseToFile(object entity,string filename)
      {
         // serialize entity to xml file
         XmlSerializer serializer = new XmlSerializer(entity.GetType());
         TextWriter textWriter = new StreamWriter(filename);
         serializer.Serialize(textWriter,entity);
         textWriter.Close();        
      }

      /// <summary>
      /// Returns an object from a filename
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="objectType"></param>
      /// <returns></returns>
      public static object DeserialiseFromFile(string fileName,Type objectType)
      {
         object result = null;
         string filePath = Path.GetFullPath(fileName);
         using(FileStream fs = new FileStream(filePath,FileMode.Open,FileAccess.Read))
         {
            XmlSerializer x = new XmlSerializer(objectType);
            result = x.Deserialize(fs);
         }
         return result;
      }      
    			
		/// <summary>
		/// Clones the specified object.
		/// </summary>
      /// <param name="obj">The object to clone.</param>
		/// <returns>The cloned object.</returns>
		public static object CloneObject(object obj)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter(); 
			MemoryStream memoryStream = new MemoryStream();

         binaryFormatter.Serialize(memoryStream,obj);
         memoryStream.Seek(0,SeekOrigin.Begin);

         return binaryFormatter.Deserialize(memoryStream); 
		}

      /// <summary>
      /// Returns a list of all the current portable alpha types
      /// that we may want to serialize.
      /// </summary>
      //public static Type[] GetAllSerialisableTypes()
      //{
      //   Type[] typeArray = new Type[]
      //       { typeof(TRS), typeof(TRSAssetLeg), typeof(TRSFundingLeg) };

      //   return typeArray;
      //}
	}
}

//===============================================================================
