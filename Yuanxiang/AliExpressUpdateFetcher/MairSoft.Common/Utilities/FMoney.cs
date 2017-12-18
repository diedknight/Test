//----------------------------------------------------------------------------
// FMoney.cs
//---------------------------------------------------------------------------

// COPYRIGHT:
// Copyright (C) 2001-2003 Francis Mair (frank@mair.net.nz)

// DESCRIPTION   
// Money formatting class class

// REVISION HISTORY:
// Date           Author            Changes
// 23 May 2001    Francis Mair      1st implementation
// 29 May 2003    Francis Mair      C# conversion

//---------------------------------------------------------------------------

//---------------------------------------------------------------------------
// Using

using System;

//---------------------------------------------------------------------------
// Namespace

namespace FSuite
{
   //---------------------------------------------------------------------------
   // Class definition		

   public class FMoney
   {
      string sCurrency = "";                // currency string
      string sDelimiter = "";               // delimiter of money output
      string sPostFix = "";                 // postfix

      double dAmount = 0;                   // amount of money
      double dFactor = 1;                   // factor amount
      bool bDecimals = true;                // show decimals

      //---------------------------------------------------------------------------------
      // Constructor

      public FMoney()
      {

      }

      //---------------------------------------------------------------------------------
      // Constructor which allows passing of amount

      public FMoney(double dAmt)
      {
         dAmount = dAmt;
         dFactor = 1;
      }

      //---------------------------------------------------------------------------------
      // Constructor taking four params

      public FMoney(string delimiter,double factor,string currency,string postfix)
      {
         sDelimiter = delimiter;
         dFactor = factor;
         sCurrency = currency;
         sPostFix = postfix;
      }

      //---------------------------------------------------------------------------------
      // Returns money as string

      public override string ToString()
      {
         return(Convert());
      }

      //---------------------------------------------------------------------------------
      // Returns passed money as string

      public string ToString(double dAmt)
      {
         dAmount = dAmt;
         return(Convert());
      }

      //---------------------------------------------------------------------------------
      // Converts our internal data to a string for display. For example if the 
      // if currency was set to 'CHF', the delimiter set to ', and the factor was 1,000, 
      // then an amount of 15455666.0 would be returned as  "CHF 15'455.666"

      string Convert()
      {
         string sMoney = "";
         
         if(sCurrency.Length > 0)
         {  sMoney += sCurrency;                            // start with currency string
            sMoney += " ";          
         }

         double dFactoredAmt = dAmount / dFactor;           // take factor into account

         if(sDelimiter.Length > 0)                          // do we have a delimiter
         {
            // note that we use the operating system defined delimiter here (settable via control panel?)
            if(bDecimals == false)
                 sMoney += dFactoredAmt.ToString("N0"); 
            else if(dFactor == 1)                           // with no factoring, only show two decimals, ie cents
                 sMoney += dFactoredAmt.ToString("N2");     // N3 = use thousand separator, and two decimals
            else sMoney += dFactoredAmt.ToString("N3");     // N3 = use thousand separator, and three decimals
         }
         else                                               // no delimiter
         {
            if(bDecimals == false)
                 sMoney += dFactoredAmt.ToString("F0"); 
            else if(dFactor == 1)                           // with no factoring, only show two decimals, ie cents
                 sMoney += dFactoredAmt.ToString("F2");     // F2 = floating point with two decimals
            else sMoney += dFactoredAmt.ToString("F3");     // F3 = floating point with three decimals
         }


         /* CODE FOR MANUALLY INSERTING OUR OWN DEFINED DELIMITER
         string sAmount(dFactoredAmt,"%.3f");              // convert amount to string

         if(sDelimiter.Length())                            // do we have a delimiter
         {
            int iDecimal = sAmount.Find('.');                  // find decimal point
            if(iDecimal == -1) iDecimal = sAmount.Length();    // if none exists go to end

            int offset = 0;
            for(int i=iDecimal; i>1; i--)
            {
               offset++;         
               if(offset < 3) continue;
               sAmount.Insert(i-1,sDelimiter.c_str());
               offset=0;
            } 
         }

         sMoney += sAmount;                                 // add the amount to the string
         */

         if(sPostFix.Length > 0)                            // do we have a postfix
         {  sMoney += " ";
            sMoney += sPostFix;                             // add the postfix
         }

         return(sMoney);
      }

      //---------------------------------------------------------------------------------
      // Sets the factor amount. ie if we are showing the amount in millions, set to 1,000,000

      public double Factor
      {
         get { return(dFactor); }
         set { dFactor = value; }
      }

      //---------------------------------------------------------------------------------
      // Sets the currency string we want to use. (ie 'CHF') gets prepended to output.

      public string Currency
      {
         get { return(sCurrency); }
         set { sCurrency = value; }
      }

      //---------------------------------------------------------------------------------
      // Sets the delimiter string we want to use. (ie ' or ,) for European style money
      // output. If you don't want one, then don't set it.

      public string Delimiter
      {
         get { return(sDelimiter); }
         set { sDelimiter = value; }
      }

      //---------------------------------------------------------------------------------
      // Allows us to turn decimals on/off

      public bool Decimals
      {
         get { return(bDecimals); }
         set { bDecimals = value; }
      }

      //---------------------------------------------------------------------------------
      // Sets a postfix string we want to use. (ie 'thousands') gets postpended to output.

      public string PostFix
      {
         get { return(sPostFix); }
         set { sPostFix = value; }
      }
   }
}
//---------------------------------------------------------------------------------

