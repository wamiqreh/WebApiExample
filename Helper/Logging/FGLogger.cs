#region Modification History
// Created By:  Mirza Fahad Ali Baig
// Created On:  27th May, 2013
// Description: 
// ****************************** Modifications *********************************

// ******************************************************************************
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using log4net;
using log4net.Config;
using System.Diagnostics;
using System.Reflection;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: log4net.Config.Repository()]

namespace Helper
{
    public static class FGLogger
    {
        #region DATA_MEMBERS
        private static readonly ILog logger = LogManager.GetLogger(typeof(FGLogger));
        #endregion

        #region FUNCTIONS
        public static void PrintInfo(string info)
        {
            try
            {
                if (logger.IsInfoEnabled)
                {
                    logger.Info(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@Info:" + " " + info + "\n");
                }

            }
            catch (Exception exception)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@LogError:" + " " + exception.StackTrace.ToString() + "\n");
                }
            }

        }

        public static void PrintInfo(string line, params object[] list)
        {
            try
            {
                if (logger.IsInfoEnabled)
                {
                    logger.InfoFormat(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@Info:" + " " + line, list);
                }

            }
            catch (Exception exception)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@LogError:" + " " + exception.StackTrace.ToString() + "\n");
                }
            }

        }

        public static void PrintDebug(string debug)
        {

            try
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@Debug:" + " " + debug + "\n");
                }

            }
            catch (Exception exception)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@LogError:" + " " + exception.StackTrace.ToString() + "\n");
                }
            }
        }

        public static void PrintError(Exception error)
        {
            try
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@Error Message:" + " " + error.Message.ToString() + "\n");
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@Error Trace:" + " " + error.StackTrace.ToString() + "\n");
                }

            }
            catch (Exception exception)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@LogError:" + " " + exception.StackTrace.ToString() + "\n");
                }
            }
        }

        public static void PrintError(string line, params object[] list)
        {
            try
            {
                string category = "@@ERROR: ";
                if (logger.IsErrorEnabled)
                {
                    logger.ErrorFormat(category + string.Format("{0:M/d/yyyy}", System.DateTime.Now) + " " + line, list);
                }

            }
            catch (Exception exception)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@LogError:" + " " + exception.StackTrace.ToString() + "\n");
                }
            }
        }

        public static void PrintLoggingDetails()
        {
            try
            {
                if (logger.IsInfoEnabled)
                    logger.Info("@@ IsInfoEnabled = TRUE.");
                if (logger.IsWarnEnabled)
                    logger.Warn("@@ IsWarnEnabled = TRUE.");
                if (logger.IsFatalEnabled)
                    logger.Fatal("@@ IsFatalEnabled = TRUE.");
                if (logger.IsDebugEnabled)
                    logger.Debug("@@ IsDebugEnabled = TRUE.");
                if (logger.IsErrorEnabled)
                    logger.Error("@@ IsErrorEnabled = TRUE.");

            }
            catch (Exception exception)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error(String.Format("{0:M/d/yyyy}", System.DateTime.Now) + " @@LogError:" + " " + exception.StackTrace.ToString() + "\n");
                }
            }

        }

        #endregion
    }
}
