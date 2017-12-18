//===============================================================================
// 
// MairSoft Common
// 
// Copyright (c) Francis Mair (frank@mair.net.nz)
//
// Author:          Francis Mair  (frank@mair.net.nz)
// Date Created:    06/12/2006
//
// Description:
// Static utility methods for web crawling etc
//
//===============================================================================

using System.IO;
using System.Net;
using System.Xml;

namespace MairSoft.Common.Utilities
{
   /// <summary>
   /// Class definition
   /// </summary>
   public class WebUtils
   {
      /// <summary>
      /// Returns the specified url as an xml document
      /// </summary>
      /// <param name="url"></param>
      /// <returns></returns>
      public static XmlDocument GetWebPageAsXmlDocument(string url)
      {
         // prepare the web page we will be asking for
         HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

         // execute the request
         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
         StreamReader streamReader = new StreamReader(response.GetResponseStream());

         // create xml document from returned page stream
         XmlDocument xmlDocument = new XmlDocument();
         xmlDocument.Load(streamReader);

         streamReader.Close();

         return xmlDocument;
      }
   }
}

//===============================================================================
