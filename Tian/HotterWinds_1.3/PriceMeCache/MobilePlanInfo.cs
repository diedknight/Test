using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PriceMeCache
{
    [Serializable]
    public class MobilePlanInfo
    {
        public int PhoneCount { get; set; }
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
        public int DataMB { get; set; }
        public int Minutes { get; set; }
        public int Texts { get; set; }
        //ContractType
        public int ContractTypeID { get; set; }
        public string ContractType { get; set; }
        //MobilePhone
        public int PhoneId { get; set; }
        public int PhoneProductID { get; set; }
        public string PhoneName { get; set; }
        public string PhoneImage { get; set; }
        public string PhoneDescription { get; set; }
        //CSK_Store_MobilePlanPhoneMap
        public int MapID { get; set; }
        public decimal UpfrontPrice { get; set; }
    }

    public class MobilePlanCarrier
    {
        public string ID { get; set; }
        public string CarrierName { get; set; }
    }
}
