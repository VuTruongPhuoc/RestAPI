using CommonLibrary;
using log4net;
using Newtonsoft.Json.Linq;
using RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.WebPages;

namespace RestAPI.Bussiness
{
    public class CfmastProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string COMMAND_GET_ALLCFMAST = "GetAllCfmast";
        private const string COMMAND_GET_CFMAST = "GetCfmastByCustid";
        private const string COMMAND_POST_CFMAST = "ThemCFMAST";
        private const string COMMAND_PUT_CFMAST = "SuaCFMAST";
        private const string COMMAND_DELETE_CFMAST = "XoaCFMAST";
        private const string COMMAND_GET_MAXCUSTID = "GetMaxCustidbyCfmast";

        #region getall
        public static object getAllCfmasts()
        {
            try
            {
                List<KeyField> keyField = new List<KeyField>();

                KeyField field = new KeyField();

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_ALLCFMAST, keyField);

                Cfmast[] cfmast = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cfmast = new Cfmast[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cfmast[i] = new Cfmast()
                        {
                            custid = ds.Tables[0].Rows[i]["CUSTID"].ToString(),
                            fullname = ds.Tables[0].Rows[i]["FULLNAME"].ToString(),
                            custodycd = ds.Tables[0].Rows[i]["CUSTODYCD"].ToString(),
                            idtype = ds.Tables[0].Rows[i]["IDTYPE"].ToString(),
                            idcode = ds.Tables[0].Rows[i]["IDCODE"].ToString(),
                            iddate = Convert.ToDateTime(ds.Tables[0].Rows[i]["IDDATE"]).ToString("yyyy-MM-dd"),
                            address = ds.Tables[0].Rows[i]["ADDRESS"].ToString(),
                            phone = ds.Tables[0].Rows[i]["PHONE"].ToString(),
                            mobile = ds.Tables[0].Rows[i]["MOBILE"].ToString(),
                            email = ds.Tables[0].Rows[i]["EMAIL"].ToString()
                        };
                    }
                }

                return new list() { s = "ok", d = cfmast };
            }
            catch (Exception ex)
            {
                Log.Error("getcfmast ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
        #endregion
        public static object getCfmastByCustId(string custid)
        {
            try
            {
                List<KeyField> keyField = new List<KeyField>();

                KeyField field = new KeyField();
                field.keyName = "p_custid";
                field.keyValue = custid;
                field.keyType = "VARCHAR2";
                keyField.Add(field);

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_CFMAST, keyField);

                Cfmast[] cfmasts = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cfmasts = new Cfmast[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cfmasts[i] = new Cfmast()
                        {
                            custid = ds.Tables[0].Rows[i]["CUSTID"].ToString().Trim(),
                            fullname = ds.Tables[0].Rows[i]["FULLNAME"].ToString(),
                            custodycd = ds.Tables[0].Rows[i]["CUSTODYCD"].ToString(),
                            idtype = ds.Tables[0].Rows[i]["IDTYPE"].ToString(),
                            idcode = ds.Tables[0].Rows[i]["IDCODE"].ToString(),
                            iddate = Convert.ToDateTime(ds.Tables[0].Rows[i]["IDDATE"]).ToString("yyyy-MM-dd"),
                            address = ds.Tables[0].Rows[i]["ADDRESS"].ToString(),
                            phone = ds.Tables[0].Rows[i]["PHONE"].ToString(),
                            mobile = ds.Tables[0].Rows[i]["MOBILE"].ToString(),
                            email = ds.Tables[0].Rows[i]["EMAIL"].ToString()
                        };
                    }
                }
                return new list() { s = "ok", d = cfmasts };
            }
            catch (Exception ex)
            {
                Log.Error("getCfmasts:.custid=" + custid + ":.", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

        public static object postCfmast(string strRequest, string p_ipAddress)
        {
            string logStr = "postCfmast";
            Log.Info(logStr + "strRequest" + strRequest);
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string CustId = "", FullName = "", CustOdycd = "", Idtype = "", Idcode = "", Iddate = "", Address = "", Phone = "", Mobile = "", Email = "";
                if (request.TryGetValue("custid", out jToken))
                    CustId = jToken.ToString();
                if (request.TryGetValue("fullname", out jToken))
                    FullName = jToken.ToString();
                if (request.TryGetValue("custodycd", out jToken))
                    CustOdycd = jToken.ToString();
                if (request.TryGetValue("idtype", out jToken))
                    Idtype = jToken.ToString();
                if (request.TryGetValue("idcode", out jToken))
                    Idcode = jToken.ToString();
                if (request.TryGetValue("iddate", out jToken))
                    Iddate = jToken.ToString();
                if (request.TryGetValue("address", out jToken))
                    Address = jToken.ToString();
                if (request.TryGetValue("phone", out jToken))
                    Phone = jToken.ToString();
                if (request.TryGetValue("mobile", out jToken))
                    Mobile = jToken.ToString();
                if (request.TryGetValue("email", out jToken))
                    Email = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[12];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = CustId;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_fullname";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = FullName;
                v_objParam.ParamSize = FullName.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = CustOdycd;
                v_objParam.ParamSize = CustOdycd.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idtype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Idtype;
                v_objParam.ParamSize = Idtype.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idcode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Idcode;
                v_objParam.ParamSize = Idcode.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_iddate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Iddate.AsDateTime();
                v_objParam.ParamSize = Iddate.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.DateTime").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_address";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Address;
                v_objParam.ParamSize = Address.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_phone";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Phone;
                v_objParam.ParamSize = Phone.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_mobile";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Mobile;
                v_objParam.ParamSize = Mobile.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_email";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Email;
                v_objParam.ParamSize = Email.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";

                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";

                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                long returnErr = TransactionProcess.doTransaction(COMMAND_POST_CFMAST, ref v_arrParam, 10);
                string v_strerrorMessage = (string)v_arrParam[11].ParamValue;
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error("postCfmmast " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object putCfmast(string strRequest, string custid, string p_ipAddress)
        {
            try
            {
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                JObject request = JObject.Parse(strRequest);
                JToken jToken;

                string FullName = "", CustOdycd = "", Idtype = "", Idcode = "", Iddate = "", Address = "", Phone = "", Mobile = "", Email = "";
                if (request.TryGetValue("fullname", out jToken))
                    FullName = jToken.ToString();
                if (request.TryGetValue("custodycd", out jToken))
                    CustOdycd = jToken.ToString();
                if (request.TryGetValue("idtype", out jToken))
                    Idtype = jToken.ToString();
                if (request.TryGetValue("idcode", out jToken))
                    Idcode = jToken.ToString();
                if (request.TryGetValue("iddate", out jToken))
                    Iddate = jToken.ToString();
                if (request.TryGetValue("address", out jToken))
                    Address = jToken.ToString();
                if (request.TryGetValue("phone", out jToken))
                    Phone = jToken.ToString();
                if (request.TryGetValue("mobile", out jToken))
                    Mobile = jToken.ToString();
                if (request.TryGetValue("email", out jToken))
                    Email = jToken.ToString();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[12];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custid;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_fullname";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = FullName;
                v_objParam.ParamSize = FullName.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custodycd";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = CustOdycd;
                v_objParam.ParamSize = CustOdycd.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idtype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Idtype;
                v_objParam.ParamSize = Idtype.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_idcode";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Idcode;
                v_objParam.ParamSize = Idcode.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_iddate";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Iddate.AsDateTime();
                v_objParam.ParamSize = Iddate.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.DateTime").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_address";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Address;
                v_objParam.ParamSize = Address.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_phone";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Phone;
                v_objParam.ParamSize = Phone.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_mobile";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Mobile;
                v_objParam.ParamSize = Mobile.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_email";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = Email;
                v_objParam.ParamSize = Email.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;

                long returnErr = TransactionProcess.doTransaction(COMMAND_PUT_CFMAST, ref v_arrParam, 10);
                string v_strerrorMessage = (string)v_arrParam[11].ParamValue;
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("cfmast:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        public static object deleteCfmast(string strRequest, string acctno, string p_ipAddress)
        {
            try
            {

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[3];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = acctno;
                v_objParam.ParamSize = acctno.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;


                long returnErr = TransactionProcess.doTransaction(COMMAND_DELETE_CFMAST, ref v_arrParam, 1);
                string v_strerrorMessage = (string)v_arrParam[2].ParamValue;

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("deleteCfmast: ", ex);
                return 1;
            }
        }

        public static object getMaxCustidByCfmast()
        {
            try
            {

                List<KeyField> keyField = new List<KeyField>();

                KeyField field = new KeyField();

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_MAXCUSTID, keyField);

                MaxCustIdCfmast[] custid = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    custid = new[] { new MaxCustIdCfmast(ds.Tables[0].Rows[0]["MAXCUSTID"].ToString()) };
                }

                return new list() { s = "ok", d = custid };
            }
            catch (Exception ex)
            {
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
    }

}