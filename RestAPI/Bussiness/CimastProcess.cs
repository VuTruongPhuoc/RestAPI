using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.WebPages;
using CommonLibrary;
using log4net;
using Newtonsoft.Json.Linq;
using RestAPI.Models;

namespace RestAPI.Bussiness
{
    public class CimastProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string COMMAND_GET_CIMAST = "getallcimast";
        public const string COMMAND_UPDATE_ADDMONEY = "suathemtiencimast";
        public const string COMMAND_UPDATE_SUBTRACTMONEY = "suatrutiencimast";

        public static object getAllCimast()
        {
            try
            {
                List<KeyField> keyField = new List<KeyField>();

                KeyField field = new KeyField();

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_CIMAST, keyField);

                Cimast[] cimast = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    cimast = new Cimast[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        cimast[i] = new Cimast()
                        {
                            afacctno = ds.Tables[0].Rows[i]["AFACCTNO"].ToString(),
                            acctno = ds.Tables[0].Rows[i]["ACCTNO"].ToString(),
                            balance = Convert.ToDouble(ds.Tables[0].Rows[i]["BALANCE"].ToString()),
                            pp = Convert.ToDouble(ds.Tables[0].Rows[i]["PP"].ToString()),
                            cidepofeeacr = Convert.ToDouble(ds.Tables[0].Rows[i]["CIDEPOFEEACR"].ToString()),
                            depofeeamt = Convert.ToDouble(Convert.ToDouble(ds.Tables[0].Rows[i]["DEPOFEEAMT"]).ToString()),
                            currentdebt = Convert.ToDouble(ds.Tables[0].Rows[i]["CURRENTDEBT"].ToString()),
                            lastchange = Convert.ToDateTime(ds.Tables[0].Rows[i]["LASTCHANGE"]).ToString("yyyy-MM-dd"),
                            status = ds.Tables[0].Rows[i]["STATUS"].ToString()
                        };
                    }
                }

                return new list() { s = "ok", d = cimast };
            }
            catch (Exception ex)
            {
                Log.Error("getcfmast ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
        public static object updateAddBalance(string strRequest, string afacctno,double money, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[4];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_afacctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = afacctno;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_money";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = money;
                v_objParam.ParamSize = money.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[3] = v_objParam;

                long returnErr = TransactionProcess.doTransaction(COMMAND_UPDATE_ADDMONEY, ref v_arrParam, 2);
                string v_strerrorMessage = (string)v_arrParam[3].ParamValue;
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("afmast:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }

        public static object updateSubtractBalance(string strRequest, string afacctno, double money, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                double depofeeamt = 0;
                string lastchange = "";
                if (request.TryGetValue("depofeeamt", out jToken))
                    depofeeamt = Convert.ToDouble(jToken.ToString());
                if (request.TryGetValue("lastchange", out jToken))
                    lastchange = jToken.ToString();
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[6];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_afacctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = afacctno;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_depofeeamt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = depofeeamt;
                v_objParam.ParamSize = depofeeamt.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_lastchange";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = lastchange.AsDateTime();
                v_objParam.ParamSize = lastchange.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.DateTime").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_money";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = money;
                v_objParam.ParamSize = money.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[5] = v_objParam;

                long returnErr = TransactionProcess.doTransaction(COMMAND_UPDATE_SUBTRACTMONEY, ref v_arrParam,4);
                string v_strerrorMessage = (string)v_arrParam[5].ParamValue;
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("afmast:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
    }
}