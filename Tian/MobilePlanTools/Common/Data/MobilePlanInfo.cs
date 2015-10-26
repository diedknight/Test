using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Data;

namespace Common.Data
{
    public class MobilePlanInfo : ICrawlDataInfo<MobilePlanInfo>
    {
        //Carrier
        public string CarrierID { get; set; }
        public string CarrierName { get; set; }
        public string CarrierLogo { get; set; }
        public string CarrierURl { get; set; }
        //MobilePlan
        public string MobilePlanID { get; set; }
        public string MobilePlanName { get; set; }
        public string MobilePlanURL { get; set; }
        public string PlanDescription { get; set; }
        public decimal Price { get; set; }
        public string DataMB { get; set; }
        public int Minutes { get; set; }
        public int Texts { get; set; }
        public int plus { get; set; }

        public string ContractType { get; set; }


        public decimal CallRate { get; set; }
        public decimal DataRate { get; set; }
        public decimal TextCostPer { get; set; }

        public List<MobilePhoneInfo> Phones { get; set; }

        public string ToTextLine()
        {
            string textLine = CarrierName + "\t" + MobilePlanName
                + "\t" + MobilePlanURL + "\t" + Price.ToString("0.00")
                + "\t" + DataMB + "\t" + Minutes + "\t" + Texts + "\t" + (Phones == null ? 0 : Phones.Count);
            return textLine;
        }

        public string WritePhones()
        {
            StringBuilder textLine = new StringBuilder();
            if (Phones == null || Phones.Count == 0) return "";
            foreach (var phone in Phones)
            {
                textLine.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\r\n",
                    this.MobilePlanName, phone.PhoneName, phone.ContractTypeID, phone.UpfrontPrice.ToString("0.00"), phone.phoneURL);
            }
            return textLine.ToString();
        }

        public MobilePlanInfo SetInfoFromTextLine(string textLine)
        {
            string[] info = textLine.Split('\t');
            if (info.Length == 10)
            {
                
                return this;
            }
            else
            {
                Console.WriteLine("(MobilePlanInfo)Invaild line : " + textLine);
                return null;
            }
        }

        public void CopyTo(MobilePlanInfo copyto)
        {
            copyto.CarrierID = this.CarrierID;
            copyto.CarrierName = this.CarrierName;
            copyto.CarrierLogo = this.CarrierLogo;
            copyto.CarrierURl = this.CarrierURl;
            //MobilePlan
            copyto.MobilePlanID = this.MobilePlanID;
            copyto.MobilePlanName = this.MobilePlanName;
            copyto.MobilePlanURL = this.MobilePlanURL;
            copyto.PlanDescription = this.PlanDescription;
            copyto.Price = this.Price;
            copyto.DataMB = this.DataMB;
            copyto.Minutes = this.Minutes;
            copyto.Texts = this.Texts;
            copyto.plus = this.plus;
        }
    }

    public class MobilePhoneInfo
    {
        //ContractType
        public int ContractTypeID { get; set; }
        public string ContractType { get; set; }
        //MobilePhone
        public int PhoneId { get; set; }
        public int PhoneProductId { get; set; }
        public int ManufacturerID { get; set; }
        public string PhoneName { get; set; }
        public string PhoneImage { get; set; }
        public string ManufacturerName { get; set; }
        public string PhoneDescription { get; set; }
        //CSK_Store_MobilePlanPhoneMap
        public decimal UpfrontPrice { get; set; }
        public string phoneURL { get; set; }
    }
}
