using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;


namespace Commerce.Common
{

    public enum AttributeType
    {
        SingleSelection,
        MultipleSelection,
        UserInput
    }

    public enum AdBoxPlacement
    {
        Right,
        Center,
        Left
    }

    public enum ShippingEstimate : int
    {
        OneToTwoDays = 1,
        ThreeToFiveDays = 2,
        OneToTwoWeeks = 3
    }

    public enum ShippingType : int
    {
        NormalShipping = 1,
        AirOnly = 2,
        DownloadOnly = 3
    }
    public enum ProductType : int
    {
        DownloadableSoftware = 1,
        Books = 2,
        ElectronicGoods = 3,
        SoftwareShipOnly = 4
    }

    //public enum ProductStatus : int
    //{
    //    Active = 1,
    //    New = 2,
    //    OnBackorder = 3,
    //    TopSeller = 4,
    //    Featured = 5,
    //    Discontinued = 6,
    //    NoLongerAvailable = 7,
    //    FutureRelease = 8,
    //    Inactive = 99
    //}

    //public enum RetailerStatus : int
    //{
    //    ActiveNonPaying = 1,
    //    ActiveAffiliate = 2,
    //    Inactive = 99
    //}

    /// <summary>
    /// The type of payment for an order
    /// </summary>
    public enum TransactionType : int
    {
        CreditCardPayment = 1,
        PayPalPayment = 2,
        Refund = 3,
        PurchaseOrder = 4,
        OfflineCreditCardPayment = 5
    }
    /// <summary>
    /// Credit Cards the site accepts. Alter as needed
    /// </summary>
    [Flags, Serializable]
    public enum CreditCardType
    {
        MasterCard,
        VISA,
        Amex,
        Discover
    }

    /// <summary>
    /// The Order Status
    /// </summary>
    public enum OrderStatus : int
    {
        NotProcessed = 9999,
        ReceivedAwaitingPayment = 1,
        ReceivedPaymentProcessingOrder = 10,
        GatheringItemsFromInventory = 20,
        AwatingShipmentToCustomer = 30,
        DelayedItemsNotAvailable = 50,
        ShippedToCustomer = 100,
        DelayedReroutingShipping = 60,
        DelayedCustomerRequest = 70,
        DelayedOrderUnderReview = 80,
        OrderCancelledPriorToShipping = 99,
        OrderRefunded = 200
    }
    public enum USState
    {
        /// AK
        [Description("AK")]
        AK = 1
            /// AL
    ,
        [Description("AL")]
        AL = 2
            /// AR
    ,
        [Description("AR")]
        AR = 3
            /// AZ
    ,
        [Description("AZ")]
        AZ = 4
            /// CA
    ,
        [Description("CA")]
        CA = 5
            /// CO
    ,
        [Description("CO")]
        CO = 6
            /// CT
    ,
        [Description("CT")]
        CT = 7
            /// DC
    ,
        [Description("DC")]
        DC = 8
            /// DE
    ,
        [Description("DE")]
        DE = 9
            /// FL
    ,
        [Description("FL")]
        FL = 10
            /// GA
    ,
        [Description("GA")]
        GA = 11
            /// HI
    ,
        [Description("HI")]
        HI = 12
            /// IA
    ,
        [Description("IA")]
        IA = 13
            /// ID
    ,
        [Description("ID")]
        ID = 14
            /// IL
    ,
        [Description("IL")]
        IL = 15
            /// IN
    ,
        [Description("IN")]
        IN = 16
            /// KS
    ,
        [Description("KS")]
        KS = 17
            /// KY
    ,
        [Description("KY")]
        KY = 18
            /// LA
    ,
        [Description("LA")]
        LA = 19
            /// MA
    ,
        [Description("MA")]
        MA = 20
            /// MD
    ,
        [Description("MD")]
        MD = 21
            /// ME
    ,
        [Description("ME")]
        ME = 22
            /// MI
    ,
        [Description("MI")]
        MI = 23
            /// MN
    ,
        [Description("MN")]
        MN = 24
            /// MO
    ,
        [Description("MO")]
        MO = 25
            /// MS
    ,
        [Description("MS")]
        MS = 26
            /// MT
    ,
        [Description("MT")]
        MT = 27
            /// NC
    ,
        [Description("NC")]
        NC = 28
            /// ND
    ,
        [Description("ND")]
        ND = 29
            /// NE
    ,
        [Description("NE")]
        NE = 30
            /// NH
    ,
        [Description("NH")]
        NH = 31
            /// NJ
    ,
        [Description("NJ")]
        NJ = 32
            /// NM
    ,
        [Description("NM")]
        NM = 33
            /// NV
    ,
        [Description("NV")]
        NV = 34
            /// NY
    ,
        [Description("NY")]
        NY = 35
            /// OH
    ,
        [Description("OH")]
        OH = 36
            /// OK
    ,
        [Description("OK")]
        OK = 37
            /// OR
    ,
        [Description("OR")]
        OR = 38
            /// PA
    ,
        [Description("PA")]
        PA = 39
            /// RI
    ,
        [Description("RI")]
        RI = 40
            /// SC
    ,
        [Description("SC")]
        SC = 41
            /// SD
    ,
        [Description("SD")]
        SD = 42
            /// TN
    ,
        [Description("TN")]
        TN = 43
            /// TX
    ,
        [Description("TX")]
        TX = 44
            /// UT
    ,
        [Description("UT")]
        UT = 45
            /// VA
    ,
        [Description("VA")]
        VA = 46
            /// VT
    ,
        [Description("VT")]
        VT = 47
            /// WA
    ,
        [Description("WA")]
        WA = 48
            /// WI
    ,
        [Description("WI")]
        WI = 49
            /// WV
    ,
        [Description("WV")]
        WV = 50
            /// WY
    ,
        [Description("WY")]
        WY = 51
    }

    public enum CurrencyCode
    {
        [Description("Afghanistan afghani")]
        AFA = 1,
        [Description("Albanian lek")]
        ALL = 2,
        [Description("Algerian dinar")]
        DZD = 3,
        [Description("Angolan kwanza reajustado")]
        AOR = 4,
        [Description("Argentine peso")]
        ARS = 5,
        [Description("Armenian dram")]
        AMD = 6,
        [Description("Aruban guilder")]
        AWG = 7,
        [Description("Australian dollar")]
        AUD = 8,
        [Description("Azerbaijanian new manat")]
        AZN = 9,
        [Description("Bahamian dollar")]
        BSD = 10,
        [Description("Bahraini dinar")]
        BHD = 11,
        [Description("Bangladeshi taka")]
        BDT = 12,
        [Description("Barbados dollar")]
        BBD = 13,
        [Description("Belarusian ruble")]
        BYR = 14,
        [Description("Belize dollar")]
        BZD = 15,
        [Description("Bermudian dollar")]
        BMD = 16,
        [Description("Bhutan ngultrum")]
        BTN = 17,
        [Description("Bolivian boliviano")]
        BOB = 18,
        [Description("Botswana pula")]
        BWP = 19,
        [Description("Brazilian real")]
        BRL = 20,
        [Description("British pound")]
        GBP = 21,
        [Description("Brunei dollar")]
        BND = 22,
        [Description("Bulgarian lev")]
        BGN = 23,
        [Description("Burundi franc")]
        BIF = 24,
        [Description("Cambodian riel")]
        KHR = 25,
        [Description("Canadian dollar")]
        CAD = 26,
        [Description("Cape Verde escudo")]
        CVE = 27,
        [Description("Cayman Islands dollar")]
        KYD = 28,
        [Description("CFA franc BCEAO")]
        XOF = 29,
        [Description("CFA franc BEAC")]
        XAF = 30,
        [Description("CFP franc")]
        XPF = 31,
        [Description("Chilean peso")]
        CLP = 32,
        [Description("Chinese yuan renminbi")]
        CNY = 33,
        [Description("Colombian peso")]
        COP = 34,
        [Description("Comoros franc")]
        KMF = 35,
        [Description("Congolese franc")]
        CDF = 36,
        [Description("Costa Rican colon")]
        CRC = 37,
        [Description("Croatian kuna")]
        HRK = 38,
        [Description("Cuban peso")]
        CUP = 39,
        [Description("Cypriot pound")]
        CYP = 40,
        [Description("Czech koruna")]
        CZK = 41,
        [Description("Danish krone")]
        DKK = 42,
        [Description("Djibouti franc")]
        DJF = 43,
        [Description("Dominican peso")]
        DOP = 44,
        [Description("East Caribbean dollar")]
        XCD = 45,
        [Description("Egyptian pound")]
        EGP = 46,
        [Description("El Salvador colon")]
        SVC = 47,
        [Description("Eritrean nakfa")]
        ERN = 48,
        [Description("Estonian kroon")]
        EEK = 49,
        [Description("Ethiopian birr")]
        ETB = 50,
        [Description("EU euro")]
        EUR = 51,
        [Description("Falkland Islands pound")]
        FKP = 52,
        [Description("Fiji dollar")]
        FJD = 53,
        [Description("Gambian dalasi")]
        GMD = 54,
        [Description("Georgian lari")]
        GEL = 55,
        [Description("Ghanaian cedi")]
        GHC = 56,
        [Description("Gibraltar pound")]
        GIP = 57,
        [Description("Gold (ounce)")]
        XAU = 58,
        [Description("Gold franc")]
        XFO = 59,
        [Description("Guatemalan quetzal")]
        GTQ = 60,
        [Description("Guinean franc")]
        GNF = 61,
        [Description("Guyana dollar")]
        GYD = 62,
        [Description("Haitian gourde")]
        HTG = 63,
        [Description("Honduran lempira")]
        HNL = 64,
        [Description("Hong Kong SAR dollar")]
        HKD = 65,
        [Description("Hungarian forint")]
        HUF = 66,
        [Description("Icelandic krona")]
        ISK = 67,
        [Description("IMF special drawing right")]
        XDR = 68,
        [Description("Indian rupee")]
        INR = 69,
        [Description("Indonesian rupiah")]
        IDR = 70,
        [Description("Iranian rial")]
        IRR = 71,
        [Description("Iraqi dinar")]
        IQD = 72,
        [Description("Israeli new shekel")]
        ILS = 73,
        [Description("Jamaican dollar")]
        JMD = 74,
        [Description("Japanese yen")]
        JPY = 75,
        [Description("Jordanian dinar")]
        JOD = 76,
        [Description("Kazakh tenge")]
        KZT = 77,
        [Description("Kenyan shilling")]
        KES = 78,
        [Description("Kuwaiti dinar")]
        KWD = 79,
        [Description("Kyrgyz som")]
        KGS = 80,
        [Description("Lao kip")]
        LAK = 81,
        [Description("Latvian lats")]
        LVL = 82,
        [Description("Lebanese pound")]
        LBP = 83,
        [Description("Lesotho loti")]
        LSL = 84,
        [Description("Liberian dollar")]
        LRD = 85,
        [Description("Libyan dinar")]
        LYD = 86,
        [Description("Lithuanian litas")]
        LTL = 87,
        [Description("Macao SAR pataca")]
        MOP = 88,
        [Description("Macedonian denar")]
        MKD = 89,
        [Description("Malagasy ariary")]
        MGA = 90,
        [Description("Malawi kwacha")]
        MWK = 91,
        [Description("Malaysian ringgit")]
        MYR = 92,
        [Description("Maldivian rufiyaa")]
        MVR = 93,
        [Description("Maltese lira")]
        MTL = 94,
        [Description("Mauritanian ouguiya")]
        MRO = 95,
        [Description("Mauritius rupee")]
        MUR = 96,
        [Description("Mexican peso")]
        MXN = 97,
        [Description("Moldovan leu")]
        MDL = 98,
        [Description("Mongolian tugrik")]
        MNT = 99,
        [Description("Moroccan dirham")]
        MAD = 100,
        [Description("Mozambique new metical")]
        MZN = 101,
        [Description("Myanmar kyat")]
        MMK = 102,
        [Description("Namibian dollar")]
        NAD = 103,
        [Description("Nepalese rupee")]
        NPR = 104,
        [Description("Netherlands Antillian guilder")]
        ANG = 105,
        [Description("New Zealand dollar")]
        NZD = 106,
        [Description("Nicaraguan cordoba oro")]
        NIO = 107,
        [Description("Nigerian naira")]
        NGN = 108,
        [Description("North Korean won")]
        KPW = 109,
        [Description("Norwegian krone")]
        NOK = 110,
        [Description("Omani rial")]
        OMR = 111,
        [Description("Pakistani rupee")]
        PKR = 112,
        [Description("Palladium (ounce)")]
        XPD = 113,
        [Description("Panamanian balboa")]
        PAB = 114,
        [Description("Papua New Guinea kina")]
        PGK = 115,
        [Description("Paraguayan guarani")]
        PYG = 116,
        [Description("Peruvian nuevo sol")]
        PEN = 117,
        [Description("Philippine peso")]
        PHP = 118,
        [Description("Platinum (ounce)")]
        XPT = 119,
        [Description("Polish zloty")]
        PLN = 120,
        [Description("Qatari rial")]
        QAR = 121,
        [Description("Romanian new leu")]
        RON = 122,
        [Description("Russian ruble")]
        RUB = 123,
        [Description("Rwandan franc")]
        RWF = 124,
        [Description("Saint Helena pound")]
        SHP = 125,
        [Description("Samoan tala")]
        WST = 126,
        [Description("Sao Tome and Principe dobra")]
        STD = 127,
        [Description("Saudi riyal")]
        SAR = 128,
        [Description("Serbian dinar")]
        CSD = 129,
        [Description("Seychelles rupee")]
        SCR = 130,
        [Description("Sierra Leone leone")]
        SLL = 131,
        [Description("Silver (ounce)")]
        XAG = 132,
        [Description("Singapore dollar")]
        SGD = 133,
        [Description("Slovak koruna")]
        SKK = 134,
        [Description("Slovenian tolar")]
        SIT = 135,
        [Description("Solomon Islands dollar")]
        SBD = 136,
        [Description("Somali shilling")]
        SOS = 137,
        [Description("South African rand")]
        ZAR = 138,
        [Description("South Korean won")]
        KRW = 139,
        [Description("Sri Lanka rupee")]
        LKR = 140,
        [Description("Sudanese dinar")]
        SDD = 141,
        [Description("Suriname dollar")]
        SRD = 142,
        [Description("Swaziland lilangeni")]
        SZL = 143,
        [Description("Swedish krona")]
        SEK = 144,
        [Description("Swiss franc")]
        CHF = 145,
        [Description("Syrian pound")]
        SYP = 146,
        [Description("Taiwan New dollar")]
        TWD = 147,
        [Description("Tajik somoni")]
        TJS = 148,
        [Description("Tanzanian shilling")]
        TZS = 149,
        [Description("Thai baht")]
        THB = 150,
        [Description("Tongan pa'anga")]
        TOP = 151,
        [Description("Trinidad and Tobago dollar")]
        TTD = 152,
        [Description("Tunisian dinar")]
        TND = 153,
        [Description("Turkish lira")]
        TRY = 154,
        [Description("Turkmen manat")]
        TMM = 155,
        [Description("UAE dirham")]
        AED = 156,
        [Description("Uganda new shilling")]
        UGX = 157,
        [Description("UIC franc")]
        XFU = 158,
        [Description("Ukrainian hryvnia")]
        UAH = 159,
        [Description("Uruguayan peso uruguayo")]
        UYU = 160,
        [Description("US dollar")]
        USD = 161,
        [Description("Uzbekistani sum")]
        UZS = 162,
        [Description("Vanuatu vatu")]
        VUV = 163,
        [Description("Venezuelan bolivar")]
        VEB = 164,
        [Description("Vietnamese dong")]
        VND = 165,
        [Description("Yemeni rial")]
        YER = 166,
        [Description("Zambian kwacha")]
        ZMK = 167,
        [Description("Zimbabwe dollar")]
        ZWD = 168
    }


    public enum Country
    {
        [Description("Afghanistan")]
        AF = 1,
        [Description("Albania")]
        AL = 2,
        [Description("Algeria")]
        DZ = 3,
        [Description("American Samoa")]
        AS = 4,
        [Description("Andorra")]
        AD = 5,
        [Description("Angola")]
        AO = 6,
        [Description("Anguilla")]
        AI = 7,
        [Description("Antarctica")]
        AQ = 8,
        [Description("Antigua and Barbuda")]
        AG = 9,
        [Description("Argentina")]
        AR = 10,
        [Description("Armenia")]
        AM = 11,
        [Description("Aruba")]
        AW = 12,
        [Description("Australia")]
        AU = 13,
        [Description("Austria")]
        AT = 14,
        [Description("Azerbaijan")]
        AZ = 15,
        [Description("Bahamas")]
        BS = 16,
        [Description("Bahrain")]
        BH = 17,
        [Description("Bangladesh")]
        BD = 18,
        [Description("Barbados")]
        BB = 19,
        [Description("Belarus")]
        BY = 20,
        [Description("Belgium")]
        BE = 21,
        [Description("Belize")]
        BZ = 22,
        [Description("Benin")]
        BJ = 23,
        [Description("Bermuda")]
        BM = 24,
        [Description("Bhutan")]
        BT = 25,
        [Description("Bolivia")]
        BO = 26,
        [Description("Bosnia and Herzegovina")]
        BA = 27,
        [Description("Botswana")]
        BW = 28,
        [Description("Bouvet Island")]
        BV = 29,
        [Description("Brazil")]
        BR = 30,
        [Description("British Indian Ocean Territory")]
        IO = 31,
        [Description("British Virgin Islands")]
        VG = 32,
        [Description("Brunei Darussalam")]
        BN = 33,
        [Description("Bulgaria")]
        BG = 34,
        [Description("Burkina Faso")]
        BF = 35,
        [Description("Burundi")]
        BI = 36,
        [Description("Cambodia")]
        KH = 37,
        [Description("Cameroon")]
        CM = 38,
        [Description("Canada")]
        CA = 39,
        [Description("Cape Verde")]
        CV = 40,
        [Description("Cayman Islands")]
        KY = 41,
        [Description("Central African Republic")]
        CF = 42,
        [Description("Chad")]
        TD = 43,
        [Description("Chile")]
        CL = 44,
        [Description("China")]
        CN = 45,
        [Description("Christmas Island")]
        CX = 46,
        [Description("Cocos")]
        CC = 47,
        [Description("Colombia")]
        CO = 48,
        [Description("Comoros")]
        KM = 49,
        [Description("Congo")]
        CG = 50,
        [Description("Cook Islands")]
        CK = 51,
        [Description("Costa Rica")]
        CR = 52,
        [Description("Croatia")]
        HR = 53,
        [Description("Cuba")]
        CU = 54,
        [Description("Cyprus")]
        CY = 55,
        [Description("Czech Republic")]
        CZ = 56,
        [Description("Denmark")]
        DK = 57,
        [Description("Djibouti")]
        DJ = 58,
        [Description("Dominica")]
        DM = 59,
        [Description("Dominican Republic")]
        DO = 60,
        [Description("East Timor")]
        TP = 61,
        [Description("Ecuador")]
        EC = 62,
        [Description("Egypt")]
        EG = 63,
        [Description("El Salvador")]
        SV = 64,
        [Description("Equatorial Guinea")]
        GQ = 65,
        [Description("Eritrea")]
        ER = 66,
        [Description("Estonia")]
        EE = 67,
        [Description("Ethiopia")]
        ET = 68,
        [Description("Falkland Islands")]
        FK = 69,
        [Description("Faroe Islands")]
        FO = 70,
        [Description("Fiji")]
        FJ = 71,
        [Description("Finland")]
        FI = 72,
        [Description("France")]
        FR = 73,
        [Description("French Guiana")]
        GF = 74,
        [Description("French Polynesia")]
        PF = 75,
        [Description("French Southern Territories")]
        TF = 76,
        [Description("Gabon")]
        GA = 77,
        [Description("Gambia")]
        GM = 78,
        [Description("Georgia")]
        GE = 79,
        [Description("Germany")]
        DE = 80,
        [Description("Ghana")]
        GH = 81,
        [Description("Gibraltar")]
        GI = 82,
        [Description("Greece")]
        GR = 83,
        [Description("Greenland")]
        GL = 84,
        [Description("Grenada")]
        GD = 85,
        [Description("Guadeloupe")]
        GP = 86,
        [Description("Guam")]
        GU = 87,
        [Description("Guatemala")]
        GT = 88,
        [Description("Guinea")]
        GN = 89,
        [Description("Guinea-Bissau")]
        GW = 90,
        [Description("Guyana")]
        GY = 91,
        [Description("Haiti")]
        HT = 92,
        [Description("Heard and McDonald Islands")]
        HM = 93,
        [Description("Honduras")]
        HN = 94,
        [Description("Hong Kong")]
        HK = 95,
        [Description("Hungary")]
        HU = 96,
        [Description("Iceland")]
        IS = 97,
        [Description("India")]
        IN = 98,
        [Description("Indonesia")]
        ID = 99,
        [Description("Iran")]
        IR = 100,
        [Description("Iraq")]
        IQ = 101,
        [Description("Ireland")]
        IE = 102,
        [Description("Israel")]
        IL = 103,
        [Description("Italy")]
        IT = 104,
        [Description("Ivory Coast")]
        CI = 105,
        [Description("Jamaica")]
        JM = 106,
        [Description("Japan")]
        JP = 107,
        [Description("Jordan")]
        JO = 108,
        [Description("Kazakhstan")]
        KZ = 109,
        [Description("Kenya")]
        KE = 110,
        [Description("Kiribati")]
        KI = 111,
        [Description("Kuwait")]
        KW = 112,
        [Description("Kyrgyzstan")]
        KG = 113,
        [Description("Laos")]
        LA = 114,
        [Description("Latvia")]
        LV = 115,
        [Description("Lebanon")]
        LB = 116,
        [Description("Lesotho")]
        LS = 117,
        [Description("Liberia")]
        LR = 118,
        [Description("Libya")]
        LY = 119,
        [Description("Liechtenstein")]
        LI = 120,
        [Description("Lithuania")]
        LT = 121,
        [Description("Luxembourg")]
        LU = 122,
        [Description("Macau")]
        MO = 123,
        [Description("Macedonia")]
        MK = 124,
        [Description("Madagascar")]
        MG = 125,
        [Description("Malawi")]
        MW = 126,
        [Description("Malaysia")]
        MY = 127,
        [Description("Maldives")]
        MV = 128,
        [Description("Mali")]
        ML = 129,
        [Description("Malta")]
        MT = 130,
        [Description("Marshall Islands")]
        MH = 131,
        [Description("Martinique")]
        MQ = 132,
        [Description("Mauritania")]
        MR = 133,
        [Description("Mauritius")]
        MU = 134,
        [Description("Mayotte")]
        YT = 135,
        [Description("Mexico")]
        MX = 136,
        [Description("Micronesia")]
        FM = 137,
        [Description("Moldova")]
        MD = 138,
        [Description("Monaco")]
        MC = 139,
        [Description("Mongolia")]
        MN = 140,
        [Description("Montserrat")]
        MS = 141,
        [Description("Morocco")]
        MA = 142,
        [Description("Mozambique")]
        MZ = 143,
        [Description("Myanmar")]
        MM = 144,
        [Description("Namibia")]
        NA = 145,
        [Description("Nauru")]
        NR = 146,
        [Description("Nepal")]
        NP = 147,
        [Description("Netherlands")]
        NL = 148,
        [Description("Netherlands Antilles")]
        AN = 149,
        [Description("New Caledonia")]
        NC = 150,
        [Description("New Zealand")]
        NZ = 151,
        [Description("Nicaragua")]
        NI = 152,
        [Description("Niger")]
        NE = 153,
        [Description("Nigeria")]
        NG = 154,
        [Description("Niue")]
        NU = 155,
        [Description("Norfolk Island")]
        NF = 156,
        [Description("North Korea")]
        KP = 157,
        [Description("Northern Mariana Islands")]
        MP = 158,
        [Description("Norway")]
        NO = 159,
        [Description("Oman")]
        OM = 160,
        [Description("Pakistan")]
        PK = 161,
        [Description("Palau")]
        PW = 162,
        [Description("Panama")]
        PA = 163,
        [Description("Papua New Guinea")]
        PG = 164,
        [Description("Paraguay")]
        PY = 165,
        [Description("Peru")]
        PE = 166,
        [Description("Philippines")]
        PH = 167,
        [Description("Pitcairn")]
        PN = 168,
        [Description("Poland")]
        PL = 169,
        [Description("Portugal")]
        PT = 170,
        [Description("Puerto Rico")]
        PR = 171,
        [Description("Qatar")]
        QA = 172,
        [Description("Reunion")]
        RE = 173,
        [Description("Romania")]
        RO = 174,
        [Description("Russian Federation")]
        RU = 175,
        [Description("Rwanda")]
        RW = 176,
        [Description("S. Georgia and S. Sandwich Islands")]
        GS = 177,
        [Description("Saint Kitts and Nevis")]
        KN = 178,
        [Description("Saint Lucia")]
        LC = 179,
        [Description("Saint Vincent and The Grenadines")]
        VC = 180,
        [Description("Samoa")]
        WS = 181,
        [Description("San Marino")]
        SM = 182,
        [Description("Sao Tome and Principe")]
        ST = 183,
        [Description("Saudi Arabia")]
        SA = 184,
        [Description("Senegal")]
        SN = 185,
        [Description("Seychelles")]
        SC = 186,
        [Description("Sierra Leone")]
        SL = 187,
        [Description("Singapore")]
        SG = 188,
        [Description("Slovakia")]
        SK = 189,
        [Description("Slovenia")]
        SI = 190,
        [Description("Solomon Islands")]
        SB = 191,
        [Description("Somalia")]
        SO = 192,
        [Description("South Africa")]
        ZA = 193,
        [Description("South Korea")]
        KR = 194,
        [Description("Soviet Union")]
        SU = 195,
        [Description("Spain")]
        ES = 196,
        [Description("Sri Lanka")]
        LK = 197,
        [Description("St. Helena")]
        SH = 198,
        [Description("St. Pierre and Miquelon")]
        PM = 199,
        [Description("Sudan")]
        SD = 200,
        [Description("Suriname")]
        SR = 201,
        [Description("Svalbard and Jan Mayen Islands")]
        SJ = 202,
        [Description("Swaziland")]
        SZ = 203,
        [Description("Sweden")]
        SE = 204,
        [Description("Switzerland")]
        CH = 205,
        [Description("Syria")]
        SY = 206,
        [Description("Taiwan")]
        TW = 207,
        [Description("Tajikistan")]
        TJ = 208,
        [Description("Tanzania")]
        TZ = 209,
        [Description("Thailand")]
        TH = 210,
        [Description("Togo")]
        TG = 211,
        [Description("Tokelau")]
        TK = 212,
        [Description("Tonga")]
        TO = 213,
        [Description("Trinidad and Tobago")]
        TT = 214,
        [Description("Tunisia")]
        TN = 215,
        [Description("Turkey")]
        TR = 216,
        [Description("Turkmenistan")]
        TM = 217,
        [Description("Turks and Caicos Islands")]
        TC = 218,
        [Description("Tuvalu")]
        TV = 219,
        [Description("Uganda")]
        UG = 220,
        [Description("Ukraine")]
        UA = 221,
        [Description("United Arab Emirates")]
        AE = 222,
        [Description("United Kingdom")]
        GB = 223,
        [Description("United States")]
        US = 224,
        [Description("Uruguay")]
        UY = 225,
        [Description("US Minor Outlying Islands")]
        UM = 226,
        [Description("US Virgin Islands")]
        VI = 227,
        [Description("Uzbekistan")]
        UZ = 228,
        [Description("Vanuatu")]
        VU = 229,
        [Description("Venezuela")]
        VE = 230,
        [Description("Viet Nam")]
        VN = 231,
        [Description("Wallis and Futuna Islands")]
        WF = 232,
        [Description("Western Sahara")]
        EH = 233,
        [Description("Yemen")]
        YE = 234,
        [Description("Yugoslavia")]
        YU = 235,
        [Description("Zaire")]
        ZR = 236,
        [Description("Zambia")]
        ZM = 237,
        [Description("Zimbabwe")]
        ZW = 238
    }


}
