using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PriceMeEventLog
{
    public static class SystemEventController
    {
        private static Regex applicationVirtualPathRegex = new Regex(" Application Virtual Path: (?<applicationVirtualPath>[^\\n^\\r]+)\\r\\n", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex requestURLRegex = new Regex(" Request URL: (?<requestURL>[^\\n^\\r]+)\\r\\n", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex exceptionMessageRegex = new Regex(" Exception message: (?<exceptionMessage>[^\\n^\\r]+)\\r\\n", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex exceptionMessageRegex2 = new Regex(" Exception message: (?<exceptionMessage>[^\\n]+)\\n", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex exceptionTypeRegex = new Regex(" Exception type: (?<exceptionType>[^\\n^\\r]+)\\r\\n", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex userIpRegex = new Regex(" User host address: (?<ip>[^\\n^\\r]+)\\r\\n", RegexOptions.Compiled | RegexOptions.Multiline);

        public static List<WebEventInfo> GetWebEventInfoList(int eventID, int hours)
        {
            Dictionary<string, WebEventInfo> dictionary = new Dictionary<string, WebEventInfo>();
            EventLog eventLog = new EventLog();
            eventLog.Log = "Application";
            EventLogEntryCollection entries = eventLog.Entries;
            DateTime dateTime = DateTime.Now.AddHours(-hours);
            var enumerator = entries.GetEnumerator();
            {
                while (enumerator.MoveNext())
                {
                    EventLogEntry eventLogEntry = (EventLogEntry)enumerator.Current;
                    if (eventLogEntry.TimeGenerated > dateTime)
                    {
                        if (eventLogEntry.EventID == eventID)
                        {
                            WebEventInfo webEventInfo = GetWebEventInfo(eventLogEntry.Message);
                            if (webEventInfo != null)
                            {
                                if (dictionary.ContainsKey(webEventInfo.RequestURL))
                                {
                                    dictionary[webEventInfo.RequestURL].EventCount++;
                                    dictionary[webEventInfo.RequestURL].EventDateTimes.Add(eventLogEntry.TimeGenerated);
                                }
                                else
                                {
                                    webEventInfo.EventDateTimes.Add(eventLogEntry.TimeGenerated);
                                    webEventInfo.EventCount = 1;
                                    dictionary.Add(webEventInfo.RequestURL, webEventInfo);
                                }
                            }
                        }
                    }
                }
            }
            return Enumerable.ToList<WebEventInfo>(dictionary.Values);
        }

        private static WebEventInfo GetWebEventInfo(string message)
        {
            WebEventInfo webEventInfo = new WebEventInfo();
            Match match = SystemEventController.applicationVirtualPathRegex.Match(message);
            WebEventInfo result;
            if (!match.Success)
            {
                result = null;
            }
            else
            {
                webEventInfo.ApplicationVirtualPath = match.Groups["applicationVirtualPath"].Value.Trim();
                match = requestURLRegex.Match(message);
                if (!match.Success)
                {
                    result = null;
                }
                else
                {
                    webEventInfo.RequestURL = match.Groups["requestURL"].Value.Trim();
                    
                    match = exceptionMessageRegex2.Match(message);
                    if (!match.Success)
                    {
                        result = null;
                    }
                    else
                    {
                        webEventInfo.ExceptionMessage = match.Groups["exceptionMessage"].Value.Trim();
                        match = exceptionTypeRegex.Match(message);
                        if (!match.Success)
                        {
                            result = null;
                        }
                        else
                        {
                            webEventInfo.ExceptionType = match.Groups["exceptionType"].Value.Trim();
                            int num = message.IndexOf("Stack trace:") + 12;
                            int num2 = message.IndexOf("Custom event details:");
                            if (num < 0 || num2 < 0 || num2 < num)
                            {
                                result = null;
                            }
                            else
                            {
                                webEventInfo.StackTrace = message.Substring(num, num2 - num);
                                match = userIpRegex.Match(message);
                                if (!match.Success)
                                {
                                    result = null;
                                }
                                else
                                {
                                    webEventInfo.Ip = match.Groups["ip"].Value.Trim();
                                    result = webEventInfo;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
