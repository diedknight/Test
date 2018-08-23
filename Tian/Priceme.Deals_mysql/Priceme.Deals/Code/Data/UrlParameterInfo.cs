using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UrlParameterInfo
/// </summary>
public class UrlParameterInfo
{
    public int c { get; set; }
    public string q { get; set; }
    public string m { get; set; }
    public string avs { get; set; }
    public string avsr { get; set; }
    public string swi { get; set; }
    public string pr { get; set; }
    public int pg { get; set; }
    public string sb { get; set; }
    //retailerIds
    public string pcsid { get; set; }
    public string adm { get; set; }
    public string dr { get; set; }
    public string v { get; set; }
    public string samp { get; set; }
    //page to
    public string pt { get; set; }
    public int ps { get; set; }
    
    public UrlParameterInfo()
    {
        c = 0;
        pg = 0;
        ps = 0;

        q = "";
        m = "";
        avs = "";
        avsr = "";
        swi = "";
        pr = "";
        sb = "";
        pcsid = "";
        adm = "";
        dr = "";
        v = "";
        samp = "";
        pt = "";
    }

    public static UrlParameterInfo CreateFromJson(string json)
    {
        try
        {
            UrlParameterInfo urlP = Newtonsoft.Json.JsonConvert.DeserializeObject<UrlParameterInfo>(json);
            return urlP;
        }
        catch(Exception ex)
        {
            return null;
        }
    }
}