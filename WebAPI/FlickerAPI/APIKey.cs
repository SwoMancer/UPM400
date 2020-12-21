using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace FlickerAPI
{
    public class APIKey
    {
        public string Key { get; set; }
        public APIKey()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"APIKey.txt");
            string[] files = File.ReadAllLines(path);

            try
            {
                this.Key = files[0];
            }
            catch
            {
                throw new Exception("Can not find APIKey.txt in folder \"" + path + "\"");
            }
        }
    }
}
