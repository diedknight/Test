//===============================================================================
// 
// MairSoft.Common
// 
// Copyright (c) 2006 Francis Mair (frank@mair.net.nz)
//
// Description:
// This class contains parameters which specify what processing is required for an xml
// document. These config classes are then serialised as a list into a config file which
// is used by the XmlProcessor class to process an xml document. Each config item specifies
// the XPath of a location to modify, then two more parameters specify the modification 
// required and the type of operand.
// 
// Instructions:
// One of the parameters in this test config class is an XPath location which can 
// be specified using the following expression forms:
// XPATH EXPRESSIONS:
//    /catalog	 selects all the catalog elements
//    /catalog/cd	 selects all the cd elements of the catalog element	
//    /catalog/cd/price	 selects all the price elements of all the cd elements of the catalog element	
//    /catalog/cd[price>10.0]	 selects all the cd elements with price greater than 10.0	
//    starts with a slash(/)	 represents an absolute path to an element	
//    starts with two slashes(//)	 selects all elements that satisfy the criteria	
//    //cd	 selects all cd elements in the document	
//    /catalog/cd/title | /catalog/cd/artist	 selects all the title and artist elements of the cd elements of catalog	
//    //title | //artist	 selects all the title and artist elements in the document	
//    /catalog/cd/*	 selects all the child elements of all cd elements of the catalog element	
//    /catalog/*/price	 selects all the price elements that are grandchildren of catalog	
//    /*/*/price	 selects all price elements which have two ancestors	
//    //*	 selects all elements in the document	
//    /catalog/cd[1]	 selects the first cd child of catalog	
//    /catalog/cd[last()]	 selects the last cd child of catalog	
//    /catalog/cd[price]	 selects all the cd elements that have price	
//    /catalog/cd[price=10.90]	 selects cd elements with the price of 10.90	
//    /catalog/cd[price=10.90]/price	 selects all price elements with the price of 10.90	
//    //@country	 selects all "country" attributes	
//    //cd[@country]	 selects cd elements which have a "country" attribute	
//    //cd[@*]	 selects cd elements which have any attribute	
//    //cd[@country='UK']	 selects cd elements with "country" attribute equal to 'UK'	
//
//===============================================================================

using System;
using System.Collections;

namespace MairSoft.Common.XML
{
    /// <summary>
    /// Type of operation we want to perform
    /// </summary>
    public enum ChangeOperator { Addition, Subtraction, Multiplication, Division };
    
    /// <summary>
    /// Summary description for ValuationConfig.
    /// </summary>
    [Serializable]
    public class XmlProcessConfig
    {
        /// <summary>
        /// The location in the WorkUnit where we are applying the change.
        /// NOTE: The format is the following if you want an attribute modified:
        /// /WorkUnit//MarketData/Ir/YieldCurve/Instruments/Instrument[@value]
        /// AND: The format is the following if you want an element modified:
        /// /WorkUnit//MarketData/ProcessVolatility/ProcessVolatility/ProcessVolatility/DateVolatilityPairs/DateValuePair/Value
        /// SEE Instructions above for XPath expression possibilities
        /// </summary>
        private string _changeLocationXPath = string.Empty;

        /// <summary>
        /// Operation we want to perform
        /// </summary>
        private ChangeOperator _changeOperator = ChangeOperator.Multiplication;
        
        /// <summary>
        /// Amount of change we are applying
        /// </summary>
        private decimal _changeAmount = 0m;
                
        /// <summary>
        /// Constructor
        /// </summary>
        public XmlProcessConfig()
        {

        }

        /// <summary>
        /// ChangeLocationXPath
        /// </summary>
        public string ChangeLocationXPath
        {
            get { return _changeLocationXPath; }
            set { _changeLocationXPath = value; }
        }

        /// <summary>
        /// ChangeOperand
        /// </summary>
        public ChangeOperator ChangeOperator
        {
            get { return _changeOperator; }
            set { _changeOperator = value; }
        }
        
        /// <summary>
        /// ChangeAmount
        /// </summary>
        public decimal ChangeAmount
        {
            get { return _changeAmount; }
            set { _changeAmount = value; }
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "ChangeLocation: " + _changeLocationXPath + ", ChangeAmount: " + _changeAmount.ToString();
        }
    }

    /// <summary>
    /// List collection for our ValuationTestConfigList class
    /// </summary>
    [Serializable]
    public class XmlProcessConfigList : CollectionBase
    {
        #region List Methods

        /// <summary>
        /// Constructor
        /// </summary>
        public XmlProcessConfigList() : base() {}

        /// <summary>
        /// Indexer
        /// </summary>
        public XmlProcessConfig this[int index]
        {
            get { return List[index] as XmlProcessConfig; }
            set { List[index] = value; }
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="item"></param>
        public void Add(XmlProcessConfig item)
        {
            List.Add(item);
        }

        /// <summary>
        /// ToArray
        /// </summary>
        /// <returns></returns>
        public XmlProcessConfig[] ToArray()
        {
            XmlProcessConfig[] item = new XmlProcessConfig[this.Count];
            List.CopyTo(item,0);
            return item;		
        }

        #endregion
    }
}

//===============================================================================
