//===============================================================================
// 
// MairSoft.Common
// 
// Copyright (c) 2006 Francis Mair (frank@mair.net.nz)
//
// Description:
// This class processes XML files using configuration items that are supplied. The configuration
// item is the XmlProcessConfig class which has a number of properties, including an XPath, an
// operand and an amount. This processor class will then, for each config item, iterate over all
// matching XPath nodes and update the element or attribute value by the change amount, using the
// specified change operand. (ie if change operand is 'addition' and the change amount is 4, then 
// the value at the specified XPath location will be increased by 4).
// The XmlProcessor class expects an input XmlDocument and a list of XmlProcessConfigs and will
// return a processed XmlDocument.
//
//===============================================================================

using System;
using System.Xml;
using FSuite.Utilities;

namespace MairSoft.Common.XML
{
    /// <summary>
    /// Type of change we are performing
    /// </summary>
    public enum ChangeType { Attribute, Element }
    
	/// <summary>
	/// Summary description for XmlProcessor.
	/// </summary>
	public class XmlProcessor
	{   
	    /// <summary>
	    /// Given an xml document and a list of processing config changes, we apply the changes
	    /// to the given xml document so that it is modified according to the specified changes.
	    /// </summary>
	    /// <param name="xmlDocument"></param>
	    /// <param name="xmlProcessConfigList"></param>
	    /// <returns></returns>
        public static void ApplyChanges(XmlDocument xmlDocument,XmlProcessConfigList xmlProcessConfigList)
        {
            // step through each processing config to find each change required
            foreach(XmlProcessConfig xmlProcessConfig in xmlProcessConfigList)
            {
                //_log.Debug("Loaded Process Config : " + xmlProcessConfig.ToString());

                // we need to know what attribute we are changing if we are requesting an attribute change
                string changeAttribute = string.Empty;
                int attributePos = xmlProcessConfig.ChangeLocationXPath.IndexOf('@');
                ChangeType changeType = (attributePos > 0) ? ChangeType.Attribute : ChangeType.Element;
                if(changeType == ChangeType.Attribute)
                {   int lastPos = xmlProcessConfig.ChangeLocationXPath.LastIndexOf(']');
                    changeAttribute = xmlProcessConfig.ChangeLocationXPath.Substring((attributePos+1),lastPos - (attributePos+1));
                }

                // select the nodes with the matching xpaths
                XmlElement documentElement = xmlDocument.DocumentElement;
                XmlNodeList currentNodeList = documentElement.SelectNodes(xmlProcessConfig.ChangeLocationXPath);

                //_log.Debug("Found " + currentNodeList.Count + " matching nodes");

                foreach(XmlNode currentNode in currentNodeList)
                {
                    if((currentNode is XmlElement) == false) continue;

                    // make clone of found node so we can modify it
                    XmlElement newNode = currentNode.CloneNode(true) as XmlElement;

                    // display node we found which we are updating
                    //_log.Debug("=======================================================");
                    //_log.Debug("Parent of matching node = " + currentNode.ParentNode.OuterXml);
            
                    // attribute change requested
                    if(changeType == ChangeType.Attribute)
                    {
                        // update new node contents with change to value
                        string valueStr = newNode.GetAttribute(changeAttribute);
                        decimal newValue = GetNewValue(valueStr,xmlProcessConfig);
                        newNode.SetAttribute(changeAttribute,newValue.ToString());                           
                        //_log.Debug("   Existing value = " + valueStr + ", Updated value = " + newValue.ToString());
                    }
                    // element change requested
                    else if(changeType == ChangeType.Element)
                    {                           
                        // update new node contents with change to value
                        string valueStr = newNode.InnerText;
                        decimal newValue = GetNewValue(valueStr,xmlProcessConfig);
                        newNode.InnerText = newValue.ToString();
                        //_log.Debug("   Existing value = " + valueStr + ", Updated value = " + newValue.ToString());
                    }

                    // use new value node for this valuation
                    XmlNode parentNode = currentNode.ParentNode;
                    parentNode.ReplaceChild(newNode,currentNode);
                }
            }
        }
	    
	    /// <summary>
	    /// Given a value string we apply the specified change that is provided by the
	    /// process config item.
	    /// </summary>
	    /// <param name="valueStr"></param>
	    /// <param name="xmlProcessConfig"></param>
	    private static decimal GetNewValue(string valueStr,XmlProcessConfig xmlProcessConfig)
	    {
            decimal newValue = (valueStr.Length > 0) ? Decimal.Parse(valueStr) : 0m;
	        
	        switch(xmlProcessConfig.ChangeOperator)
	        {
                case ChangeOperator.Addition:
                    newValue += xmlProcessConfig.ChangeAmount;
                    break;
                case ChangeOperator.Subtraction:
                    newValue -= xmlProcessConfig.ChangeAmount;
                    break;
                case ChangeOperator.Multiplication:
                    newValue *= xmlProcessConfig.ChangeAmount;
                    break;
                case ChangeOperator.Division:
                    newValue /= xmlProcessConfig.ChangeAmount;
                    break;	            
	        }
	        
	        return newValue;	        	        
	    }
	    
        #region Create Configs Methods

        /// <summary>
        /// This method can be called when we want to create our initial valuation
        /// test config list file. This file will be a sample which will need to be updated
        /// so that the valuation tests can take place.
        /// </summary>
        public static void CreateInitialConfig()
        {
            XmlProcessConfigList valuationTestConfigList = new XmlProcessConfigList();

            // add first sample config
            XmlProcessConfig xmlProcessConfig1 = new XmlProcessConfig();
            xmlProcessConfig1.ChangeLocationXPath = @"/MarketData/BondFloorVolatilities/BondFloorVolatility/DateVolatilityPairs/DateValuePair/Value";
            xmlProcessConfig1.ChangeAmount = 0.10m;
            xmlProcessConfig1.ChangeOperator = ChangeOperator.Division;            
            valuationTestConfigList.Add(xmlProcessConfig1);

            // add second sample config
            XmlProcessConfig xmlProcessConfig2 = new XmlProcessConfig();
            xmlProcessConfig2.ChangeLocationXPath = @"/MarketData/ProcessVolatility/ProcessVolatility/DateVolatilityPairs/DateValuePair/Value";
            xmlProcessConfig2.ChangeAmount = 0.10m;
            xmlProcessConfig2.ChangeOperator = ChangeOperator.Multiplication;            
            valuationTestConfigList.Add(xmlProcessConfig2);

            // now convert our list to an XML string and save to a file
            string xmlString = FSerialise.Serialise(valuationTestConfigList);
            FFile.Write("XmlProcessConfigList.xml",xmlString);
        }

        #endregion	    
	}
}

//===============================================================================

