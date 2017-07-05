using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserInfo
/// </summary>
public class UserInfo
{
	public UserInfo()
	{
		
	}

    string userName;

    public string UserName
    {
        get { return userName; }
        set { userName = value; }
    }
    string email;

    public string Email
    {
        get { return email; }
        set { email = value; }
    }
    string firstName;

    public string FirstName
    {
        get { return firstName; }
        set { firstName = value; }
    }
    string lastName;

    public string LastName
    {
        get { return lastName; }
        set { lastName = value; }
    }
    string location;

    public string Location
    {
        get { return location; }
        set { location = value; }
    }
}