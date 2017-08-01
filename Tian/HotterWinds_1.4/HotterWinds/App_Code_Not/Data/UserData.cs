using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserData
/// </summary>
[Serializable]
public class UserData
{
	public UserData()
	{
		//
		// TODO: Add constructor logic here
		//
	}


    public string name { get; set; }
    public string pass { get; set; }

    public string email { get; set; }

    public string parseid { get; set; }

    public string userid { get; set; }

    public int countryid { get; set; }

    public bool sex { get; set; }

    public bool isapproveemail { get; set; }

    public DateTime? createon { get; set; }

    public string firstname { get; set; }
    public string lastname { get; set; }

    public int logintype { get; set; }


}
