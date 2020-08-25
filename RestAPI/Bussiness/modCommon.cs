using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using log4net;
//////////////////////////
//using System;
using System.Globalization;
using System.ServiceModel.Channels;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;

namespace RestAPI.Bussiness
{
    #region constants
    static class Constants
    {
        public const string gc_SCHEMA_OBJMESSAGE_ROOT = "/ObjectMessage";
        public const string gc_SCHEMA_OBJMESSAGE_HEADER = "<ObjectMessage TXDATE='' TXNUM='' TXTIME='' TLID='' BRID='' LOCAL='' MSGTYPE='' OBJNAME='' ACTIONFLAG='' CMDINQUIRY='' CLAUSE='' FUNCTIONNAME='' AUTOID='' REFERENCE='' RESERVER='' IPADDRESS='' CMDTYPE='' PARENTOBJNAME='' PARENTCLAUSE='' GUID=''></ObjectMessage>";

        public const string gc_AtributeTXDATE = "TXDATE";
        public const string gc_AtributeTXTIME = "TXTIME";
        public const string gc_AtributeBRDATE = "BRDATE";
        public const string gc_AtributeBUSDATE = "BUSDATE";
        public const string gc_AtributeTXNUM = "TXNUM";
        public const string gc_AtributeLOCAL = "LOCAL";
        public const string gc_AtributePRETRAN = "PRETRAN";
        public const string gc_AtributeSTOREPROC = "STOREPROC";
        public const string gc_AtributePARAM_NAME = "PARAMNAME";
        public const string gc_AtributePARAM_VALUE = "PARAMVALUE";
        public const string gc_AtributePARAM_SIZE = "PARAMSIZE";
        public const string gc_AtributePARAM_TYPE = "PARAMTYPE";
        public const string gc_AtributeNUM_OF_PARAM = "NUM_OF_PARAM";
        public const string gc_AtributeTLID = "TLID";
        public const string gc_AtributeCMDID = "CMDID";
        public const string gc_AtributeGRPID = "GRPID";
        public const string gc_AtributeBRID = "BRID";
        public const string gc_AtributeIPADDRESS = "IPADDRESS";
        public const string gc_AtributeBANKACCOUNT = "BANKACCOUNT";
        public const string gc_AtributeBANKACCNAME = "BANKACCNAME";
        public const string gc_AtributeCMDTYPE = "CMDTYPE";
        public const string gc_AtributeWSNAME = "WSNAME";
        public const string gc_AtributeOFFID = "OFFID";
        public const string gc_AtributeCHKID = "CHKID";
        public const string gc_AtributeCHID = "CHID";
        public const string gc_AtributeTLID2 = "TLID2";
        public const string gc_AtributeBRID2 = "BRID2";
        public const string gc_AtributeIBT = "IBT";
        public const string gc_AtributeMSGAMT = "MSGAMT";
        public const string gc_AtributeMSGACCT = "MSGACCT";
        public const string gc_AtributeCHKTIME = "CHKTIME";
        public const string gc_AtributeOFFTIME = "OFFTIME";
        public const string gc_AtributePARENTOBJNAME = "PARENTOBJNAME";
        public const string gc_AtributePARENTCLAUSE = "PARENTCLAUSE";
        public const string gc_AtributeMSGTYPE = "MSGTYPE";
        public const string gc_AtributeTXACTION = "ACTIONFLAG";
        public const string gc_AtributeOBJNAME = "OBJNAME";
        public const string gc_AtributeFUNCNAME = "FUNCTIONNAME";
        public const string gc_AtributeACTFLAG = "ACTIONFLAG";
        public const string gc_AtributeCLAUSE = "CLAUSE";
        public const string gc_AtributeCMDINQUIRY = "CMDINQUIRY";
        public const string gc_AtributeAUTOID = "AUTOID";
        public const string gc_AtributeREFERENCE = "REFERENCE";
        public const string gc_AtributeRESERVER = "RESERVER";
        public const string gc_AtributeGUID= "GUID";

    }
    #endregion constants

    public static class modCommon
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string A(string logString, params object[] logParams)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(logString).Append(".:");
            foreach (object s in logParams)
                sb.Append(", [").Append(s).Append("]");
            return sb.ToString();
        }
        public static HttpResponseMessage CreateResponseAPI(HttpRequestMessage request, HttpStatusCode statusCode, object obj)
        {
            HttpResponseMessage responses = request.CreateResponse(statusCode, JObject.FromObject(obj));
            LogFullResponses(responses, statusCode);        
            return responses;
        }

        private static void LogFullResponses(HttpResponseMessage responses, HttpStatusCode statusCode)
        {
            StringBuilder preFixlogSession = new StringBuilder();
            preFixlogSession.Append("Responses");
            //preFixlogSession.AppendLine("===============================BEGIN");
            try
            {
                preFixlogSession.Append(" HttpStatusCode: " + statusCode);
                if (responses.Content != null)
                {
                    string v_strResponses = responses.Content.ReadAsStringAsync().Result;
                    if (Log.IsDebugEnabled)
                    {
                        preFixlogSession.Append("| Method: " + responses.RequestMessage.Method);
                    }
                    if (responses.RequestMessage.Method.ToString() != "GET" || !statusCode.Equals(HttpStatusCode.OK))
                    {
                        preFixlogSession.Append("| Message: " + v_strResponses);
                    }
                }
                //preFixlogSession.Append("===============================END");                
                Log.Info(preFixlogSession.ToString());
            }
            catch (Exception ex)
            {
                Log.Error(".:Get error when loggging Responses:." + preFixlogSession.ToString(), ex);
                //Log.Info(preFixlogSession + "======================END");
            }            
        }

        public static void LogFullRequest(HttpRequestMessage request)
        {

            StringBuilder preFixlogSession = new StringBuilder();
            preFixlogSession.Append("LogFullRequest");
            //preFixlogSession.Append("===============================BEGIN");
            try
            {
                if (Log.IsDebugEnabled)
                {
                    preFixlogSession.Append("| GUID:" + HttpContext.Current.Request.Headers["GUID"].ToString());
                }                    
                preFixlogSession.Append("| uri:" + request.RequestUri);
                preFixlogSession.Append("| method:" + request.Method);
                preFixlogSession.Append("| ClientIPAddress:" + ((HttpContextBase)request.Properties["MS_HttpContext"]).Request.UserHostAddress);
                if (request.Headers.Any())
                {                    
                    foreach (var header in request.Headers)
                    {
                        if(header.Key.ToUpper() != "AUTHORIZATION")
                        {
                            // 20200515 chi log forwarded-for theo yc ANTT
                            if (header.Key.ToUpper() != "X-FORWARDED-FOR")
                            {
                                if (Log.IsDebugEnabled)
                                {
                                    preFixlogSession.Append("| " + header.Key + ":" + header.Value.FirstOrDefault().ToString());
                                }
                            }
                            else
                                preFixlogSession.Append("| " + header.Key + ":" + header.Value.FirstOrDefault().ToString());
                        }                        
                    }
                }
                
                if (request.Content != null)
                {
                    string v_strBody = request.Content.ReadAsStringAsync().Result;
                    preFixlogSession.Append("| Body: " + v_strBody);
                }

                preFixlogSession.Append("===============================END");
                Log.Info(preFixlogSession.ToString());

            }
            catch (Exception ex)
            {
                Log.Error(".:Get error when loggging request:."+ (request!=null? request.RequestUri.ToString():"Request Null") + preFixlogSession.ToString(), ex);
                //Log.Info(preFixlogSession + "======================END");
            }
        }

        public static string GetClientIp()
        {
            foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork) // filter out ipv4
                {
                   return ipAddress.ToString();
                }
            }
            return null;
        }

        public static string getFilterSQL(HttpRequestMessage request, ref int pageindex, ref int pagesize)
        {
            string SQL = " 1=1 ";
            string v_strKey = "";
            string v_strValue = "";
            int queryCount = request.RequestUri.ParseQueryString().Count;
            for (int i = 0; i < queryCount; i++)
            {
                v_strKey = request.RequestUri.ParseQueryString().GetKey(i);
                v_strValue = request.RequestUri.ParseQueryString().GetValues(i)[0].ToString().Trim();

                if (v_strKey.ToUpper() == "PAGESIZE")
                    pagesize = int.Parse(v_strValue);
                if (v_strKey.ToUpper() == "PAGEINDEX")
                    pageindex = int.Parse(v_strValue);

                if (v_strKey.ToUpper() != "PAGESIZE" && v_strKey.ToUpper() != "PAGEINDEX")
                {
                    if (v_strKey.ToUpper().Contains("DATE"))
                    {
                        v_strValue = buildBussinessParam(v_strKey, v_strValue, true);
                        SQL = SQL + " AND (" + v_strValue + ") ";
                    }
                    else
                    {
                        v_strValue = buildBussinessParam(v_strKey, v_strValue, false);
                        SQL = SQL + " AND (" + v_strValue+") ";
                        //SQL = SQL + " AND " + v_strKey + " = '" + v_strValue + "' ";
                    }
                }
               
            }
            return SQL;
        }

        public static string getQueryValueByName(HttpRequestMessage request, string name)
        {
            string v_strKey = "";
            string v_strValue = "";
            int queryCount = request.RequestUri.ParseQueryString().Count;
            for (int i = 0; i < queryCount; i++)
            {
                v_strKey = request.RequestUri.ParseQueryString().GetKey(i);
                v_strValue = request.RequestUri.ParseQueryString().GetValues(i)[0].ToString().Trim();

                if (v_strKey.ToUpper() == name.ToUpper())
                    return v_strValue;
                if (v_strKey.ToUpper().Contains("DATE") && name.ToUpper() == "FROMDATE")
                    return getFromDate(v_strValue).Replace("-","/");
                if (v_strKey.ToUpper().Contains("DATE") && name.ToUpper() == "TODATE")
                    return getToDate(v_strValue).Replace("-", "/");

            }
            return "";
        }
        
        public static string getJsonValueByName(string request, string accountno, string name)
        {
            string v_strReturn = string.Empty;
            var jTran = JObject.Parse(request);
            try
            {
                var jKey = JObject.Parse(jTran["keys"].ToString());
                var jHeader = JObject.Parse(jTran["headers"].ToString());
                var jBody = JObject.Parse(jTran["data"].ToString());
            
                if (name.ToUpper() == "@ACCOUNTNO")
                {
                    return accountno;
                }
                if (name.ToUpper() == "@ORDERID")
                {
                    //v_strReturn = (string)jHeader[name];
                    v_strReturn = (string)jKey["orderID"];
                    return v_strReturn;
                }
                v_strReturn = (string)jBody[name];
            }
            catch (Exception ex)
            {
                Log.Error(A("getJsonValueByName", "request", request, "name", name, "exception", ex));
            }
            return v_strReturn;
        }


        public static string getValueFromKeys(string request, string accountno, string name)
        {
            string v_strReturn = string.Empty;
            var jTran = JObject.Parse(request);
            try
            {
                var jKey = JObject.Parse(jTran["keys"].ToString());

                if (name.ToUpper() == "@ACCOUNTNO")
                {
                    v_strReturn = accountno;
                }
                else
                {
                    v_strReturn = (string)jKey[name];
                }
            }
            catch (Exception ex)
            {
                Log.Error(A("getValueFromKeys", "request", request, "name", name, "exception", ex));
            }
            return v_strReturn;
        }

        public static string getJsonValueByIdx(string request, string nameLst, string name, int idx)
        {
            string v_strReturn = string.Empty;
            var jTran = JObject.Parse(request);
            var jBody = JObject.Parse(jTran["data"].ToString());
            
            try
            {
                v_strReturn = jBody[nameLst][idx][name].ToString();
            }
            catch (Exception ex)
            {
                Log.Error(A("getJsonValueByIdx", "request", request, "name", name, "exception", ex));
            }
            return v_strReturn;
        }

        public static string getFilterSQL_accounts(HttpRequestMessage request, ref string custodyID, ref string custID, ref int pageindex, ref int pagesize)
        {
            string SQL = " 1=1 ";
            string v_strKey = "";
            string v_strValue = "";
            int queryCount = request.RequestUri.ParseQueryString().Count;
            for (int i = 0; i < queryCount; i++)
            {
                v_strKey = request.RequestUri.ParseQueryString().GetKey(i);
                v_strValue = request.RequestUri.ParseQueryString().GetValues(i)[0].ToString().Trim();

                if (v_strKey.ToUpper() == "PAGESIZE")
                    pagesize = int.Parse(v_strValue);
                if (v_strKey.ToUpper() == "PAGEINDEX")
                    pageindex = int.Parse(v_strValue);
                if (v_strKey.ToUpper() == "CUSTODYID")
                    custodyID = v_strValue;
                if (v_strKey.ToUpper() == "CUSTID")
                    custID = v_strValue;

                if (v_strKey.ToUpper() != "PAGESIZE" && v_strKey.ToUpper() != "PAGEINDEX")
                {
                    if (v_strKey.ToUpper().Contains("DATE"))
                    {
                        v_strValue = buildBussinessParam(v_strKey, v_strValue, true);
                        SQL = SQL + " AND " + v_strValue;
                    }
                    else
                    {
                        v_strValue = buildBussinessParam(v_strKey, v_strValue, false);
                        SQL = SQL + " AND " + v_strValue;
                        //SQL = SQL + " AND " + v_strKey + " = '" + v_strValue + "' ";
                    }
                }
                
            }
            return SQL;
        } 

        public static string getFilterSQL_stockStatements(HttpRequestMessage request, ref string symbol, ref string transactionCode, 
                                                    ref string transationDate, ref int pageindex, ref int pagesize)
        {
            string SQL = " 1=1 ";
            string v_strKey = "";
            string v_strValue = "";

            int queryCount = request.RequestUri.ParseQueryString().Count;
            for (int i = 0; i < queryCount; i++)
            {
                v_strKey = request.RequestUri.ParseQueryString().GetKey(i);
                v_strValue = request.RequestUri.ParseQueryString().GetValues(i)[0].ToString().Trim();

                if (v_strKey.ToUpper() == "PAGESIZE")
                    pagesize = int.Parse(v_strValue);
                if (v_strKey.ToUpper() == "PAGEINDEX")
                    pageindex = int.Parse(v_strValue);

                if (v_strKey.ToUpper() != "PAGESIZE" && v_strKey.ToUpper() != "PAGEINDEX")
                {
                    if (v_strKey.ToUpper().Contains("DATE"))
                    {
                        v_strValue = buildBussinessParam(v_strKey, v_strValue, true);
                        SQL = SQL + " AND (" + v_strValue + ") ";
                    }
                    else
                    {
                        v_strValue = buildBussinessParam(v_strKey, v_strValue, false);
                        SQL = SQL + " AND (" + v_strValue + ") ";
                        //SQL = SQL + " AND " + v_strKey + " = '" + v_strValue + "' ";
                    }

                    if (v_strKey.ToUpper() == "TRANSATIONDATE")
                    {
                        transationDate = "("+ v_strValue + ")";
                    }
                    if (v_strKey.ToUpper() == "TRANSACTIONCODE")
                    {
                        transactionCode = "(" + v_strValue + ")";
                    }
                    if (v_strKey.ToUpper() == "SYMBOL")
                    {
                        symbol = "(" + v_strValue + ")";
                    }
                }

            }
            return SQL;
        }

        public static string buildTransMessage(HttpRequestMessage request, string value)
        {
            string v_strPath = request.RequestUri.LocalPath;
            string v_strHeader = "{";
            var v_dicHeader = new Dictionary<string,string>();
            foreach (var item in request.Headers.ToList())
            {
                v_dicHeader.Add(item.Key.ToLower().Replace("-", ""), item.Value.ToArray()[0].ToString());
            }
            if (!Log.IsDebugEnabled)
            {
                if (v_dicHeader.ContainsKey("authorization"))
                {
                    v_dicHeader["authorization"] = "authorization";
                }
            }
            v_strHeader = JsonConvert.SerializeObject(v_dicHeader);


            string v_strMethod = request.Method.ToString();
            string v_strData = request.Content.ReadAsStringAsync().Result;
            string v_strTxmessage = "{\"path\": \"" + v_strPath + "\",\"method\": \"" + v_strMethod + "\", \"headers\": " + v_strHeader;

            if (v_strData != string.Empty || v_strData != "")
            {
                v_strTxmessage = v_strTxmessage + ", \"data\":" + v_strData;
            }

            if (value == String.Empty || value == "")
            {
                v_strTxmessage = v_strTxmessage + "}";
            }
            else
            {
                v_strTxmessage = v_strTxmessage + ", \"keys\": " + value + "}";
            }
            return v_strTxmessage;
        }

        public static string buildTransMessage_Orders(HttpRequestMessage request, string value, string pv_timeType)
        {
            string v_strPath = request.RequestUri.LocalPath;
            string v_strHeader = "{";
            var v_dicHeader = new Dictionary<string, string>();
            foreach (var item in request.Headers.ToList())
            {
                v_dicHeader.Add(item.Key.ToLower().Replace("-", ""), item.Value.ToArray()[0].ToString());
            }
            if (!Log.IsDebugEnabled)
            {
                if (v_dicHeader.ContainsKey("authorization"))
                {
                    v_dicHeader["authorization"] = "authorization";
                }
            }
            v_strHeader = JsonConvert.SerializeObject(v_dicHeader);


            string v_strMethod = request.Method.ToString();
            string v_strData = request.Content.ReadAsStringAsync().Result;
            string v_strTxmessage = "{\"path\": \"" + v_strPath + "\",\"method\": \"" + v_strMethod + "\", \"headers\": " + v_strHeader;

            if (v_strData != string.Empty || v_strData != "")
            {
                v_strTxmessage = v_strTxmessage + ", \"data\":" + v_strData;
            }

            if (value == String.Empty || value == "")
            {
                v_strTxmessage = v_strTxmessage + "}";
            }
            else
            {
                string valueKeys = value.Replace("}", "");

                valueKeys += ", \"keysTimeType\":\""+pv_timeType+"\"";

                //<!--Co chan chia lenh khoi luong lon khong-->
                var disableDivideBigVolumnOrder = System.Configuration.ConfigurationManager.AppSettings["DisableDivideBigVolumnOrder"].ToString();
                valueKeys += ", \"disableDivideBigVolumnOrder\":\"" + disableDivideBigVolumnOrder + "\"";

                //-Co cho phep dat lenh truoc phien
                var checkSessionPlaceOrder = System.Configuration.ConfigurationManager.AppSettings["CheckSessionPlaceOrder"].ToString();
                valueKeys += ", \"checkSessionPlaceOrder\":\"" + checkSessionPlaceOrder + "\"";

                var checkOrderType = System.Configuration.ConfigurationManager.AppSettings["ORDERTYPE"].ToString();
                valueKeys += ", \"checkOrderType\":\"" + checkOrderType + "\"";



                valueKeys += "}";
                v_strTxmessage = v_strTxmessage + ", \"keys\": " + valueKeys + "}";
            }
            return v_strTxmessage;
        }

        public static string buildBussinessParam(string fieldName, string param, bool isDate = false)
        {
            try
            {
                if (param.Length == 0)
                {
                    return "";
                }
                string v_param = "", v_ret = "", v_value;
                string v_input = param;
                string v_tempParam = " {0} {1} '{2}' ";
                if (isDate)
                {
                    v_tempParam = " {0} {1} TO_DATE('{2}','dd/MM/rrrr') ";
                }
                while (true)
                {
                    try
                    {
                        if (v_input.ToUpper().Contains("LTE:") || v_input.ToUpper().Contains("LT:")
                        || v_input.ToUpper().Contains("GTE:") || v_input.ToUpper().Contains("GT:")
                        || v_input.ToUpper().Contains("EQ:"))
                        {
                            v_param = v_input.Substring(0, v_input.IndexOf(" "));
                            //v_param = v_input.Substring(0, param.IndexOf(" "));
                        }
                        else
                        {
                            v_param = v_input;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Log.Error("buildBussinessParam:", ex);
                        v_param = v_input;
                    }

                    if (v_param.ToUpper().Contains("LTE:") || v_param.ToUpper().Contains("LT:")
                        || v_param.ToUpper().Contains("GTE:") || v_param.ToUpper().Contains("GT:")
                        || v_param.ToUpper().Contains("EQ:"))
                    {
                        if (v_param.Substring(0, 3) == "lte")
                        {
                            v_value = v_param.Substring(4);
                            v_ret = v_ret + string.Format(v_tempParam, fieldName, "<=", v_value);
                        }
                        else if (v_param.Substring(0, 3) == "gte")
                        {
                            v_value = v_param.Substring(4);
                            v_ret = v_ret + string.Format(v_tempParam, fieldName, ">=", v_value);
                        }
                        else if (v_param.Substring(0, 2) == "lt")
                        {
                            v_value = v_param.Substring(3);
                            v_ret = v_ret + string.Format(v_tempParam, fieldName, "<", v_value);
                        }
                        else if (v_param.Substring(0, 2) == "gt")
                        {
                            v_value = v_param.Substring(3);
                            v_ret = v_ret + string.Format(v_tempParam, fieldName, ">", v_value);
                        }
                        else if (v_param.Substring(0, 2) == "eq")
                        {
                            v_value = v_param.Substring(3);
                            v_ret = v_ret + string.Format(v_tempParam, fieldName, "=", v_value);
                        }
                        else
                        {
                            v_ret = v_ret + string.Format(v_tempParam, fieldName, "=", v_param);
                        }
                    }
                    else
                    {
                        v_ret = v_ret + string.Format(v_tempParam, fieldName, "=", v_param);
                    }               

                    if (v_input.Trim().Length > v_param.Trim().Length)
                    {
                        v_input = v_input.Substring(v_param.Length).Trim();
                        if (v_input.ToUpper().IndexOf("AND") == 0)
                        {
                            v_input = v_input.Substring(3).Trim();
                            v_ret = v_ret + " AND ";
                        }
                        else if (v_input.ToUpper().IndexOf("OR") == 0)
                        {
                            v_input = v_input.Substring(2).Trim();
                            v_ret = v_ret + " OR ";
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                return v_ret;
            }
            catch (Exception ex)
            {
                Log.Error("buildBussinessParam:", ex);
                return "";
            }
        }


        public static string getFromDate(string param)
        {
            try
            {
                if (param.Length == 0)
                {
                    return "";
                }
                string v_param = "", v_value;
                string v_input = param;

                while (true)
                {
                    try
                    {
                        if (v_input.ToUpper().Contains("GTE:") || v_input.ToUpper().Contains("GT:")
                        || v_input.ToUpper().Contains("EQ:"))
                        {
                            v_param = v_input.Substring(0, v_input.IndexOf(" "));
                            //v_param = v_input.Substring(0, param.IndexOf(" "));
                        }
                        else
                        {
                            v_param = v_input;
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("getFromDate:", ex);
                        v_param = v_input;
                    }

                    if (v_param.ToUpper().Contains("GTE:")
                        || v_param.ToUpper().Contains("GT:")
                        || v_param.ToUpper().Contains("EQ:"))
                    {
                        if (v_param.Substring(0, 3) == "gte")
                        {
                            v_value = v_param.Substring(4);
                            return v_value;
                        }

                        else if (v_param.Substring(0, 2) == "gt")
                        {
                            v_value = v_param.Substring(3);
                            return v_value;
                        }
                        else if (v_param.Substring(0, 2) == "eq")
                        {
                            v_value = v_param.Substring(3);
                            return v_value;
                        }
                        else
                        {
                            v_value= v_param;
                        }
                    }
                    else
                    {
                        v_value = v_param;
                    }

                    if (v_input.Trim().Length > v_param.Trim().Length)
                    {
                        v_input = v_input.Substring(v_param.Length).Trim();
                        if (v_input.ToUpper().IndexOf("AND") == 0)
                        {
                            v_input = v_input.Substring(3).Trim();                            
                        }
                        else if (v_input.ToUpper().IndexOf("OR") == 0)
                        {
                            v_input = v_input.Substring(2).Trim();
                        }
                    }
                    else
                    {
                        break;
                    }

                }

                return v_value;


            }
            catch (Exception ex)
            {
                Log.Error("getFromDate:", ex);
                return "";
            }
        }

        public static string getToDate(string param)
        {
            try
            {
                if (param.Length == 0)
                {
                    return "";
                }
                string v_param = "", v_value;
                string v_input = param;
                while (true)
                {
                    try
                    {
                        if (v_input.ToUpper().Contains("LTE:") || v_input.ToUpper().Contains("LT:")
                        || v_input.ToUpper().Contains("EQ:"))
                        {
                            v_param = v_input.Substring(0, v_input.IndexOf(" "));
                            //v_param = v_input.Substring(0, param.IndexOf(" "));
                        }
                        else
                        {
                            v_param = v_input;
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("getToDate:", ex);
                        v_param = v_input;
                    }

                    if (v_param.ToUpper().Contains("LTE:")
                        || v_param.ToUpper().Contains("LT:")
                        || v_param.ToUpper().Contains("EQ:"))
                        {
                        if (v_param.Substring(0, 3) == "lte")
                        {
                            v_value = v_param.Substring(4);
                            return v_value;
                        }

                        else if (v_param.Substring(0, 2) == "lt")
                        {
                            v_value = v_param.Substring(3);
                            return v_value;
                        }
                        else if (v_param.Substring(0, 2) == "eq")
                        {
                            v_value = v_param.Substring(3);
                            return v_value;
                        }
                        else
                        {
                                v_value= v_param;
                        }
                    }
                    else
                    {
                            v_value = v_param;
                        }

                    if (v_input.Trim().Length > v_param.Trim().Length)
                    {
                        v_input = v_input.Substring(v_param.Length).Trim();
                        if (v_input.ToUpper().IndexOf("AND") == 0)
                        {
                            v_input = v_input.Substring(3).Trim();
                        }
                        else if (v_input.ToUpper().IndexOf("OR") == 0)
                        {
                            v_input = v_input.Substring(2).Trim();
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                return v_value;
            }
            catch (Exception ex)
            {
                Log.Error("getToDate:", ex);
                return "";
            }
        }
        public static string BuildXMLObjMsg(string pv_strTxDate = "", 
                                     string pv_strBranchId = "", 
                                     string pv_strTxTime = "", 
                                     string pv_strTellerId = "", 
                                     string pv_strLocal = "", 
                                     string pv_strMsgType = "", 
                                     string pv_strObjName = "", 
                                     string pv_strActionFlag = "", 
                                     string pv_strCmdInquiry = "", 
                                     string pv_strClause = "", 
                                     string pv_strFuncName = "", 
                                     string pv_strAutoId = "", 
                                     string pv_strTxNum = "", 
                                     string pv_strReference = "", 
                                     string pv_strReserver = "", 
                                     string pv_strIPAddress = "", 
                                     string pv_strCmdType = "T", 
                                     string pv_strPrarentObjName = "", 
                                     string pv_strParentClause = "")
        {
            System.Xml.XmlDocument XMLDocumentMessage = new System.Xml.XmlDocument();
            try
            {
                XMLDocumentMessage.LoadXml(Constants.gc_SCHEMA_OBJMESSAGE_HEADER);
                System.Xml.XmlNode XmlNodeMessage = XMLDocumentMessage.SelectSingleNode(Constants.gc_SCHEMA_OBJMESSAGE_ROOT);
                {
                    var withBlock = XmlNodeMessage;
                    withBlock.Attributes[Constants.gc_AtributeTXDATE].Value = pv_strTxDate;
                    withBlock.Attributes[Constants.gc_AtributeTXNUM].Value = pv_strTxNum;
                    withBlock.Attributes[Constants.gc_AtributeTXTIME].Value = pv_strTxTime;
                    withBlock.Attributes[Constants.gc_AtributeTLID].Value = pv_strTellerId;
                    withBlock.Attributes[Constants.gc_AtributeBRID].Value = pv_strBranchId;
                    withBlock.Attributes[Constants.gc_AtributeLOCAL].Value = pv_strLocal;
                    withBlock.Attributes[Constants.gc_AtributeMSGTYPE].Value = pv_strMsgType;
                    withBlock.Attributes[Constants.gc_AtributeOBJNAME].Value = pv_strObjName;
                    withBlock.Attributes[Constants.gc_AtributeACTFLAG].Value = pv_strActionFlag;
                    withBlock.Attributes[Constants.gc_AtributeCMDINQUIRY].Value = pv_strCmdInquiry;
                    withBlock.Attributes[Constants.gc_AtributeCLAUSE].Value = pv_strClause;
                    withBlock.Attributes[Constants.gc_AtributeFUNCNAME].Value = pv_strFuncName;
                    withBlock.Attributes[Constants.gc_AtributeAUTOID].Value = pv_strAutoId;
                    withBlock.Attributes[Constants.gc_AtributeREFERENCE].Value = pv_strReference;
                    withBlock.Attributes[Constants.gc_AtributeRESERVER].Value = pv_strReserver;
                    withBlock.Attributes[Constants.gc_AtributeIPADDRESS].Value = pv_strIPAddress;
                    withBlock.Attributes[Constants.gc_AtributeCMDTYPE].Value = pv_strCmdType;
                    withBlock.Attributes[Constants.gc_AtributePARENTOBJNAME].Value = pv_strPrarentObjName;
                    withBlock.Attributes[Constants.gc_AtributePARENTCLAUSE].Value = pv_strParentClause;
                    //20200505 bo sung guid
                    if (string.IsNullOrEmpty(HttpContext.Current.Request.Headers["GUID"]))
                    {
                        HttpContext.Current.Request.Headers.Add("GUID", Guid.NewGuid().ToString());
                    }
                    withBlock.Attributes[Constants.gc_AtributeGUID].Value = HttpContext.Current.Request.Headers["GUID"];
                }

                return XMLDocumentMessage.InnerXml.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                XMLDocumentMessage = null;
            }
        }

        public static string  formatDateString(string d)
        {
            try
            { 
                string result = "";
                try
                {
                    result = DateTime.Parse(d).ToString("dd/MM/yyyy");
                }
                catch
                {
                    result = d;
                }
                result = DateTime.ParseExact(result, "dd/MM/yyyy",null).ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz");
                return result;
            }
            catch(Exception ex)
            {
                //Log.Error("formatDateString: ", ex);
                return d;
            }
        }


        public static string getFilterSql_se(HttpRequestMessage request, string pv_name)
        {
            string v_strKey = "";
            string v_strValue = "";
            Char delimiter = '|';
            string[] v_arrKeypath;
            string[] v_arrValuepath;
            int queryCount = request.RequestUri.ParseQueryString().Count;
            string v_strReturn = "";
            for (int i = 0; i < queryCount; i++)
            {
                v_strKey = request.RequestUri.ParseQueryString().GetKey(i);
                v_arrKeypath = v_strKey.Split(delimiter);
                Log.Info("filter: v_strKey:" + v_strKey);
                if (v_strKey.ToUpper().Contains(pv_name))
                {
                    int total = v_arrKeypath.Length;
                    for (int j = 0; j < total; j++)
                    {
                        if (v_arrKeypath[j].ToUpper() == pv_name)
                        {
                            v_strValue = v_arrKeypath[j + 1];
                            return v_strValue;
                        }
                    }
                }
            }
            return "%";
        }

        public static string getConfigValue (string name, string defaultValue)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[name].ToString();
            }
            catch (Exception ex)
            {
                Log.Error("getConfigValue:.Error:.name=" + name, ex);
            }
            return defaultValue;
        }

        public static string getRequestHeaderValue(HttpRequestMessage request, string name)
        {
            try
            {
                return request.Headers.GetValues(name).ToArray()[0].ToString();
            }
            catch (Exception ex)
            {
                Log.Error("getRequestHeaderValue:.Error:.name=" + name, ex);
            }
            return String.Empty;
        }

        public static BoResponse getBoResponse(long error, string message = "")
        {
            BoResponse response = new BoResponse();
            try
            {
                response.s = error == 0 ? "ok" : error.ToString();
                response.errmsg = message;
                if (message == null || message.Length == 0)
                    response.errmsg = GetDataProcess.getErrmsg(error);
            }
            catch (Exception ex)
            {
                Log.Error("modCommon.getBoResponse:.error=" + error.ToString(), ex);
                response.errmsg = "System Error";
            }
            return response;
        }
        public static BoResponseWithData getBoResponseWithData(long error, object data, string message = "")
        {
            BoResponseWithData response = new BoResponseWithData();
            try
            {
                response.s = ((error == 0) ? "ok" : error.ToString());
                if (message != null && message.Length > 0)
                    response.errmsg = GetDataProcess.getErrmsg(error);
                response.d = data;
            }
            catch (Exception ex)
            {
                Log.Error("modCommon.getBoResponse:.error=" + error.ToString(), ex);
                response.errmsg = "System Error";
            }
            return response;
        }
    }



    
}
