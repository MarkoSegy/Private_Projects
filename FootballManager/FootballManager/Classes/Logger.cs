using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballManager.Classes
{
    public class Logger
    {
        //gibts auch von ms: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/w3c-logger/?view=aspnetcore-6.0

        static string path = @"c:\temp\weblogFootball.csv";
        public static string seperator = @";";
        static string headline = "date" + seperator + "clientIP" + seperator + "url" + seperator + "method" + seperator + "clientbrowser" + seperator + "guid";

        static Logger()
        {
            if (!System.IO.File.Exists(path))
            {
                AppendText(headline);
            }
        }

        public static void AppendText(string text)
        {
            //async await waer schneller
            text += Environment.NewLine;

            System.IO.File.AppendAllText(path, text);
        }
    }
}
