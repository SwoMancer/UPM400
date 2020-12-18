using System;
using System.Collections.Generic;
using BitsLibrary;
using BitsLibrary.Logs;

namespace TestaAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");

            CTypeErrorLog errorLog = new CTypeErrorLog();

            errorLog.Id = -1;
            errorLog.Message = "This is a test";
            errorLog.StackTrace = "StackTrace";
            errorLog.Source = "Source";
            errorLog.HelpLink = "HelpLink";
            errorLog.HResult = -1;
            errorLog.TargetSiteName = "TargetSiteName";
            errorLog.HowIsThis = "Test konsole";
            errorLog.Datum = DateTime.Now;

            Answer answer = Log.Add(errorLog);
            if (answer.Json is null)
                answer.Json = "null";

            Console.WriteLine(answer.IsASuccess + " " + answer.Json.ToString());

            Console.WriteLine("End...");
        }
    }
}
