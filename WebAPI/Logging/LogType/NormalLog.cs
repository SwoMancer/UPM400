using System;
using System.Collections.Generic;
using System.Text;

namespace Logging.LogType
{
    public class NormalLog : CTypeErrorLog
    {
        public NormalLog(string error)
        {
            this.Datum = DateTime.Now;
            this.Id = 0;
            this.HelpLink = string.Empty;
            this.HowIsThis = "FoodAPI Publish";
            this.HResult = 0;
            this.Message = error;
            this.Source = string.Empty;
            this.StackTrace = string.Empty;
            this.TargetSiteName = string.Empty;
        }
        public CTypeErrorLog ToCTypeErrorLog()
        {
            CTypeErrorLog cTypeErrorLog = new CTypeErrorLog();

            cTypeErrorLog.Datum = this.Datum;
            cTypeErrorLog.Id = this.Id;
            cTypeErrorLog.HelpLink = this.HelpLink;
            cTypeErrorLog.HowIsThis = this.HowIsThis;
            cTypeErrorLog.HResult = this.HResult;
            cTypeErrorLog.Message = this.Message;
            cTypeErrorLog.Source = this.Source;
            cTypeErrorLog.StackTrace = this.StackTrace;
            cTypeErrorLog.TargetSiteName = this.TargetSiteName;

            return cTypeErrorLog;
        }
    }
}
