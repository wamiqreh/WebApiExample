using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Common.Utilities
{
    public static class Logger
    {
        public static void CreateLog(string msg)
        {
            try
            {


                string filepath = "sad";
                if (!string.IsNullOrEmpty(filepath))
                {
                    filepath = HttpContext.Current.Server.MapPath("~/Logs");
                    //else

                    //    filepath = AppDomain.CurrentDomain.BaseDirectory;
                    string log_text = DateTime.Now.ToString() + Environment.NewLine + msg + Environment.NewLine;
                    log_text = log_text + "*********************************************" + Environment.NewLine;

                    string file_name = "Log-" + DateTime.Now.ToString("dd-MMM-yyyy") + ".txt";
                    if (!Directory.Exists(filepath))
                        Directory.CreateDirectory(filepath);

                    if (Directory.Exists(filepath))
                    {
                        //filepath = @"D:\FourgenSuppApps\Logs\";
                        string logFilePath = Path.Combine(filepath, file_name);
                        //logFilePath = @"D:\FourgenSuppApps\Logs\" + file_name;
                        File.AppendAllText(logFilePath, log_text);
                    }
                   
                }
               
            }
            catch (Exception ex)
            {

            }

        }
    }
}