using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IMAPSyncWeb.Models;
using System.Reflection;
using IMAPSyncWeb.Code;
using System.Xml;


namespace IMAPSyncWeb.Areas.ImapSyncCommander.Controllers
{
    public class ImapSyncController : ApiController
    {
        #region Static Fct
        private static List<ApiModParamObject> GetAllProperiesOfObject(Type ObjectTp)
        {
            List<ApiModParamObject> result = new List<ApiModParamObject>();
            try
            {

                //object thisObject = Activator.CreateInstance(Type.GetType(ObjectTp.AssemblyQualifiedName)); 
                // get all public static properties of MyClass type
                PropertyInfo[] propertyInfos;
                propertyInfos = ObjectTp.GetProperties();//By default, it will return only public properties.
                // sort properties by name
                Array.Sort(propertyInfos,
                           (propertyInfo1, propertyInfo2) => propertyInfo1.Name.CompareTo(propertyInfo2.Name));

                // write property names

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    result.Add(new ApiModParamObject()
                    {
                        S_NAME = propertyInfo.Name,
                        S_TYPE = propertyInfo.PropertyType.FullName,
                        S_VALUE = null
                    });

                }

            }
            catch (Exception ex)
            {
                LogHelpers.WriteError(ex);
            }

            return result;
        }

        private static List<ApiModParamObject> GetAllProperiesOfObject(Type ObjectTp, object data)
        {
            List<ApiModParamObject> result = new List<ApiModParamObject>();
            try
            {

                //object thisObject = Activator.CreateInstance(Type.GetType(ObjectTp.AssemblyQualifiedName)); 
                // get all public static properties of MyClass type
                PropertyInfo[] propertyInfos;
                propertyInfos = ObjectTp.GetProperties();//By default, it will return only public properties.
                // sort properties by name
                Array.Sort(propertyInfos,
                           (propertyInfo1, propertyInfo2) => propertyInfo1.Name.CompareTo(propertyInfo2.Name));

                // write property names

                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    result.Add(new ApiModParamObject()
                    {
                        S_NAME = propertyInfo.Name,
                        S_TYPE = propertyInfo.PropertyType.FullName,
                        S_VALUE = propertyInfo.GetValue(data) == null ? "!!MISSING!!" : propertyInfo.GetValue(data).ToString()
                    });

                }

            }
            catch (Exception ex)
            {
                LogHelpers.WriteError(ex);
            }

            return result;
        }

        private static string ParamToString(List<ApiModParamObject> paramobject)
        {

            string ret = "";

            foreach (var item in paramobject)
            {
                ret += string.Format("Name:{0}, Type:{1}, Value:{2}\r\n", item.S_NAME, item.S_TYPE, item.S_VALUE);
            }

            return ret;

        }

        private static bool ValidProperiesOfObject(Type ObjectTp, object datas)
        {



            //object thisObject = Activator.CreateInstance(Type.GetType(ObjectTp.AssemblyQualifiedName)); 
            // get all public static properties of MyClass type
            PropertyInfo[] propertyInfos;
            propertyInfos = ObjectTp.GetProperties();//By default, it will return only public properties.
            // sort properties by name
            Array.Sort(propertyInfos,
                       (propertyInfo1, propertyInfo2) => propertyInfo1.Name.CompareTo(propertyInfo2.Name));

            // write property names

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                try
                {
                    if (propertyInfo.GetValue(datas) == null)
                        return false;
                }
                catch
                { return false; }
            }


            return true;
        }

        private static bool CallAndTry(string FunctName, Type ObjectTp, object datas, out ApiModResult result)
        {

            if (datas == null)
            {
                result = new ApiModResult() { Result = 0, Message = "Error : Bad call syntax, good syntax is", Param = GetAllProperiesOfObject(ObjectTp) };
                return false;
            }
            else
            {
                if (ValidProperiesOfObject(ObjectTp, datas))
                {
                    LogHelpers.WriteLog(FunctName, string.Format("Calling {1} with param : \r\n{0}", ParamToString(GetAllProperiesOfObject(ObjectTp, datas)), FunctName), LogHelpers.LogLevel.Info);
                    result = new ApiModResult() { Result = 1, Message = "Call Ok", Param = datas };
                    return true;
                }
                else
                {
                    LogHelpers.WriteLog(FunctName, string.Format("Error on calling {1} with param : \r\n{0}", ParamToString(GetAllProperiesOfObject(ObjectTp, datas)), FunctName), LogHelpers.LogLevel.Error);
                    result = new ApiModResult() { Result = 0, Message = string.Format("Error on calling {0}, Missing parametres or bad syntax", FunctName), Param = GetAllProperiesOfObject(ObjectTp, datas) };
                    return false;
                }
            }
        }

        private static List<ApiModHelpData> GetFunctionHelp()
        {

            List<ApiModHelpData> ret = new List<ApiModHelpData>();
            XmlDocument doc = new XmlDocument();
            doc.Load(System.Web.HttpContext.Current.Server.MapPath("~/Help/CloudVaultFiles.XML"));

            XmlNode root = doc.FirstChild.NextSibling;

            {
                if (root.HasChildNodes)
                    for (int i = 0; i < root.ChildNodes.Count; i++)
                    {
                        if (root.ChildNodes[i].Name == "members")
                        {
                            foreach (XmlNode ndmember in root.ChildNodes[i].ChildNodes)
                            {
                                if (ndmember.Attributes != null)
                                {
                                    string functname = ndmember.Attributes["name"].Value.Replace("M:ExchangeCenter.Controllers.ApiModController.", "").Replace("ExchangeCenter.Models.", "");
                                    string param = "";
                                    string sum = "";
                                    string returns = "";



                                    foreach (XmlNode nddata in ndmember.ChildNodes)
                                    {
                                        if (nddata.Name == "summary")
                                        {
                                            sum = nddata.InnerText;
                                        }
                                        if (nddata.Name == "param")
                                        {
                                            param = nddata.InnerText;
                                        }
                                        if (nddata.Name == "returns")
                                        {
                                            returns = nddata.InnerText;
                                        }
                                    }
                                    ret.Add(new ApiModHelpData() { Function = functname, Desc = sum, Param = param, Returns = returns });
                                }
                            }
                        }
                    }
            }

            return ret;
        }
        #endregion


        /// <summary>
        /// POST: api/ExCenter/EmptyFct : (Demo Function not Used)
        /// </summary>
        /// <param name="value">Type :ApiModAccountData, S_VALUE : Param Demo</param>
        /// <returns>ApiModResult : Not use</returns>
        [HttpPost]
        public ApiModResult EmptyFct([FromBody]ApiModSimpleData value)
        {
            ApiModResult resCR = new ApiModResult();
            if (!CallAndTry(System.Reflection.MethodBase.GetCurrentMethod().Name, value.GetType(), value, out resCR))
                return resCR;

            try
            {

                return new ApiModResult() { Result = 1, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - OK", Param = value };
            }
            catch (Exception ex)
            {
                //PSHelpers.DisposePS();
                LogHelpers.WriteError(ex);
                return new ApiModResult() { Result = 0, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - Error", Param = ex };
            }
        }


        /// <summary>
        /// POST: api/ExCenter/EmptyFct : (Demo Function not Used)
        /// </summary>
        /// <param name="value">Type :ApiModAccountData, S_VALUE : Param Demo</param>
        /// <returns>ApiModResult : Not use</returns>
        [HttpPost]
        public ApiModResult TestSSH([FromBody]ApiModSimpleData value)
        {
            ApiModResult resCR = new ApiModResult();
            if (!CallAndTry(System.Reflection.MethodBase.GetCurrentMethod().Name, value.GetType(), value, out resCR))
                return resCR;

            try
            {
                SSHHelpers.ConnectSSH();

                return new ApiModResult() { Result = 1, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - OK", Param = value };
            }
            catch (Exception ex)
            {
                //PSHelpers.DisposePS();
                LogHelpers.WriteError(ex);
                return new ApiModResult() { Result = 0, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - Error", Param = ex };
            }
        }

        /// <summary>
        /// POST: api/ExCenter/EmptyFct : (Demo Function not Used)
        /// </summary>
        /// <param name="value">Type :ApiModAccountData, S_VALUE : Param Demo</param>
        /// <returns>ApiModResult : Not use</returns>
        [HttpPost]
        public ApiModResult TestAccount([FromBody]ApiModSimpleData value)
        {
            ApiModResult resCR = new ApiModResult();
            if (!CallAndTry(System.Reflection.MethodBase.GetCurrentMethod().Name, value.GetType(), value, out resCR))
                return resCR;

            try
            {

                return new ApiModResult() { Result = 1, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - OK", Param = value };
            }
            catch (Exception ex)
            {
                //PSHelpers.DisposePS();
                LogHelpers.WriteError(ex);
                return new ApiModResult() { Result = 0, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - Error", Param = ex };
            }
        }

        /// <summary>
        /// POST: api/ExCenter/EmptyFct : (Demo Function not Used)
        /// </summary>
        /// <param name="value">Type :ApiModAccountData, S_VALUE : Param Demo</param>
        /// <returns>ApiModResult : Not use</returns>
        [HttpPost]
        public ApiModResult CreateSimpleJob([FromBody]ApiModSimpleData value)
        {
            ApiModResult resCR = new ApiModResult();
            if (!CallAndTry(System.Reflection.MethodBase.GetCurrentMethod().Name, value.GetType(), value, out resCR))
                return resCR;

            try
            {

                return new ApiModResult() { Result = 1, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - OK", Param = value };
            }
            catch (Exception ex)
            {
                //PSHelpers.DisposePS();
                LogHelpers.WriteError(ex);
                return new ApiModResult() { Result = 0, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - Error", Param = ex };
            }
        }

        /// <summary>
        /// POST: api/ExCenter/EmptyFct : (Demo Function not Used)
        /// </summary>
        /// <param name="value">Type :ApiModAccountData, S_VALUE : Param Demo</param>
        /// <returns>ApiModResult : Not use</returns>
        [HttpPost]
        public ApiModResult CreateBatchJob([FromBody]ApiModSimpleData value)
        {
            ApiModResult resCR = new ApiModResult();
            if (!CallAndTry(System.Reflection.MethodBase.GetCurrentMethod().Name, value.GetType(), value, out resCR))
                return resCR;

            try
            {

                return new ApiModResult() { Result = 1, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - OK", Param = value };
            }
            catch (Exception ex)
            {
                //PSHelpers.DisposePS();
                LogHelpers.WriteError(ex);
                return new ApiModResult() { Result = 0, Message = System.Reflection.MethodBase.GetCurrentMethod().Name + " - Error", Param = ex };
            }
        }

        /// <summary>
        /// POST : api/ExCenter/GetHelp : Get global help descriptor
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ApiModHelpData> GetHelp()
        {
            return GetFunctionHelp();
        }
    }
}
