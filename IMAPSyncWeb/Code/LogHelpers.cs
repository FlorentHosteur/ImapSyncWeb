using IMAPSyncWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMAPSyncWeb.Code
{
    public static class LogHelpers
    {

        public enum LogLevel { Error = 0, Warning = 1, Info = 2, Debug = 3 }
        public static void WriteError(Exception ex)
        {
            string message = ex.Message;
            while (ex.InnerException != null)
            {
                message += "Message\r\n" +
                    ex.Message.ToString() + "\r\n\r\n";
                message += "Source\r\n" +
                    ex.Source + "\r\n\r\n";
                message += "Target site\r\n" +
                    ex.TargetSite.ToString() + "\r\n\r\n";
                message += "Stack trace\r\n" +
                    ex.StackTrace + "\r\n\r\n";
                message += "ToString()\r\n\r\n" +
                    ex.ToString();

                // Assign the next InnerException
                // to catch the details of that exception as well
                ex = ex.InnerException;
            }

            WriteLog(ex.Source, message, LogLevel.Error);
        }

        public static void WriteLog(string source, string message, LogLevel logType)
        {
            //using (ExchangeCenterDBDataContext dc = new ExchangeCenterDBDataContext(DataHelpers._ConnexionString))
            //{
            //    t_log newlog = new t_log() { D_DATELOG = DateTime.Now, I_LVL = (short)logType, S_MESSAGE = source + " - " + message };
            //    dc.t_logs.InsertOnSubmit(newlog);
            //    dc.SubmitChanges();
            //}
        }


        public static List<ApiModLogData> GetLastLogError(int NumberToRet)
        {
            List<ApiModLogData> ret = new List<ApiModLogData>();
            //using (ExchangeCenterDBDataContext dc = new ExchangeCenterDBDataContext(DataHelpers._ConnexionString))
            //{
            //    var logret = dc.t_logs.Where(l => l.I_LVL == 0).OrderByDescending(l => l.D_DATELOG).Take(NumberToRet);
            //    foreach (var item in logret)
            //    {
            //        ret.Add(new Models.ApiModLogData() { D_DATELOG = item.D_DATELOG, S_TYPE = Enum.GetName(typeof(LogLevel), item.I_LVL), S_MESSAGE = item.S_MESSAGE });
            //    }

            //}
            return ret;
        }

        public static List<ApiModLogData> GetLastLog(int NumberToRet)
        {
            List<ApiModLogData> ret = new List<ApiModLogData>();
            //using (ExchangeCenterDBDataContext dc = new ExchangeCenterDBDataContext(DataHelpers._ConnexionString))
            //{
            //    var logret = dc.t_logs.OrderByDescending(l => l.D_DATELOG).Take(NumberToRet);
            //    foreach (var item in logret)
            //    {
            //        ret.Add(new Models.ApiModLogData() { D_DATELOG = item.D_DATELOG, S_TYPE = Enum.GetName(typeof(LogLevel), item.I_LVL), S_MESSAGE = item.S_MESSAGE });
            //    }

            //}
            return ret;
        }
    }
}