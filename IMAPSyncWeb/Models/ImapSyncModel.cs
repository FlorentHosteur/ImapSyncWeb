using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMAPSyncWeb.Models
{
    /// <summary>
    /// Default API Result (Param should contains Error or Data)
    /// </summary>
    public class ApiModResult
    {
        /// <summary>
        /// Default Result 0 = ERROR, 1 = OK
        /// </summary>
        public int Result { get; set; }

        /// <summary>
        /// Default Message String
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Multiple Complexe Type Return
        /// </summary>
        public object Param { get; set; }

        //public string GetDebugData()
        //{
        //    return string.Format("Result : {0} - Message : {1}", this.Result, this.Message);
        //}
    }

    /// <summary>
    /// Help Data Model
    ///string Function { get; set; }
    ///string Desc { get; set; }
    ///string Param { get; set; }
    ///string Returns { get; set; }
    ///</summary>
    public class ApiModHelpData
    {
        public string Function { get; set; }
        public string Desc { get; set; }
        public string Param { get; set; }
        public string Returns { get; set; }
    }


    /// <summary>
    /// Default POST Type
    /// string S_VALUE { get; set; }
    /// </summary>
    public class ApiModSimpleData
    {
        public string S_VALUE { get; set; }
    }

    /// <summary>
    /// POST Type for default
    /// string S_NAME { get; set; }
    /// string S_TYPE { get; set; }
    /// string S_VALUE { get; set; }
    /// </summary>
    public class ApiModParamObject
    {
        public string S_NAME { get; set; }
        public string S_TYPE { get; set; }
        public string S_VALUE { get; set; }
    }

    /// <summary>
    /// POST Type for Log
    /// string D_DATELOG { get; set; }
    /// string S_TYPE { get; set; }
    /// string S_MESSAGE { get; set; }
    /// </summary>
    public class ApiModLogData
    {
        public DateTime D_DATELOG { get; set; }
        public string S_TYPE { get; set; }
        public string S_MESSAGE { get; set; }
    }




}