using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ForumInfo
/// </summary>
public class ForumInfo
{
    public int TID { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public DateTime PostDate { get; set; }
    public string PostDateInfo { get; set; }
    public string PosterName { get; set; }
    public bool isTopic { get; set; }
}