using System;
using System.Collections.Generic;
using System.Text;

namespace BitsLibrary.Logs
{
    public class CTypeErrorLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string HelpLink { get; set; }
        public int HResult { get; set; }
        public string TargetSiteName { get; set; }
        public string HowIsThis { get; set; }
        public DateTime Datum { get; set; }

        public string ToJSONString()
        {
            string json = "{";

            json += "\"Id\":" + this.Id + ",";
            json += "\"Message\":\"" + this.Message + "\",";
            json += "\"StackTrace\":\"" + this.StackTrace + "\",";
            json += "\"Source\":\"" + this.Source + "\",";
            json += "\"HelpLink\":\"" + this.HelpLink + "\",";
            json += "\"HResult\":" + this.HResult + ",";
            json += "\"TargetSiteName\":\"" + this.TargetSiteName + "\",";
            json += "\"HowIsThis\":\"" + this.HowIsThis + "\",";
            json += "\"Datum\":\"" + GetLocalTime() + "\"";

            json += "}";

            return json;
        }

        private string GetLocalTime()
        {
            return "" + StringTime(4, this.Datum.Year) + "-" + StringTime(2, this.Datum.Month) + 
                "-" + StringTime(2, this.Datum.Day) + "T" + StringTime(2, this.Datum.Hour) + 
                ":" + StringTime(2, this.Datum.Minute) + ":" + StringTime(2, this.Datum.Second);
        }
        private static string StringTime(int length, int input)
        {
            if (length < input.ToString().ToCharArray().Length)
                return string.Empty;

            if (length == input.ToString().ToCharArray().Length)
                return input.ToString();

            string text = "";
            int timeLength = input.ToString().ToCharArray().Length;

            while (length != timeLength)
            {
                text += "0";
                timeLength++;
            }
            text += input.ToString();

            return text;
        }

        public Dictionary<string, string> ToDictionary()
        {
            var value = new Dictionary<string, string>
            {
                {"Id", this.Id.ToString()},
                {"Message",this.Message},
                {"StackTrace",this.StackTrace},
                {"Source",this.Source},
                {"HelpLink",this.HelpLink},
                {"HResult",this.HResult.ToString()},
                {"TargetSiteName",this.TargetSiteName},
                {"HowIsThis",this.HowIsThis},
                {"Datum",GetLocalTime()}
            };
            return value;
        }
        public static CTypeErrorLog ExceptionToCTypeErrorLog(Exception exception, string howIsThis)
        {
            CTypeErrorLog errorLog = new CTypeErrorLog();

            errorLog.Message = exception.Message;
            errorLog.StackTrace = exception.StackTrace;
            errorLog.Source = exception.Source;
            errorLog.HelpLink = exception.HelpLink;
            errorLog.HResult = exception.HResult;
            errorLog.TargetSiteName = exception.TargetSite.Name;
            errorLog.HowIsThis = howIsThis;
            errorLog.Datum = DateTime.Now;

            return errorLog;
        }
    }
}