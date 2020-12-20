using Logging.LogType;
using System;
using System.Collections.Generic;
using System.Text;

using System.Net.Http;
using System.Web.Http;
using System.Linq;
//using System.Web.Http.Description;

namespace Logging
{
    public static class Misc
    {
        public static CTypeErrorLog ModelStateLog(System.Web.Http.ModelBinding.ModelStateDictionary modelState)
        {
            var errorList = modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );

            Dictionary<string, string[]>.ValueCollection vc = errorList.Values;
            Dictionary<string, string[]>.KeyCollection kc = errorList.Keys;

            errorList = modelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.Exception.Message).ToArray()
            );

            Dictionary<string, string[]>.ValueCollection vec = errorList.Values;

            List<string> keys = new List<string>();
            List<string[]> values = new List<string[]>();
            List<string[]> valueExceptions = new List<string[]>();


            foreach (string[] value in vc)
                values.Add(value);

            foreach (string key in kc)
                keys.Add(key);

            foreach (string[] valueE in vec)
                valueExceptions.Add(valueE);

            string text = string.Empty;
            for (int i = 0; i < keys.Count; i++)
            {
                text += keys[i] + " : [";

                text += "ErrorMessage : [";
                foreach (string value in values[i])
                {
                    text += "\"" + value + "\", ";
                }
                text += "], ";
                text += "Exception.Message : [";
                foreach (string valueException in valueExceptions[i])
                {
                    text += "\"" + valueException + "\", ";
                }
                text += "]";
                text += "], ";
            }

            return(new NormalLog(text).ToCTypeErrorLog());
        }
    }
}
