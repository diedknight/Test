using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prt = PriceMe.RichAttributeDisplayTool;

namespace PriceMe.RichAttributeDisplayTool.Process
{
    class Work
    {
        public Work() { 
            
        }

        static List<Prt.RichClass.AttributeDisplayType> Adt=new List<RichClass.AttributeDisplayType>();

        static List<Prt.RichClass.ProductValue> AttList = new List<RichClass.ProductValue>();


        public static void StartWork()
        {

            ProcessData pd = new ProcessData();

            pd.Rank += new DataRank().GetRank;

            pd.Log += LogTxt.LogWrite;

            pd.Process();

        }


    }
}
