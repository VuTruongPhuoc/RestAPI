using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using CommonLibrary;
using System.Web.WebPages;
using log4net;
using Newtonsoft.Json.Linq;
using RestAPI.Models;

namespace RestAPI.Bussiness
{
    public class AfmastProcess
    {
        public static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public const string COMMAND_GET_ALLAFMAST = "GetAllAfmast";
        public const string COMMAND_GET_AFMAST = "GetAfmastbyAcctno";
        public const string COMMAND_POST_AFMAST = "ThemAFMAST";
        public const string COMMAND_PUT_AFMAST = "SuaAFMAST";
        public const string COMMMAN_DELETE_AFMAST = "XoaAFMAST";
        public const string COMMAND_GET_MAXACCTNO = "getmaxacctnobyafmast";

        public static object getAllAfmasts()
        {
            try
            {
                List<KeyField> keyField = new List<KeyField>();

                DataSet ds = null;

                ds = GetDataProcess.executeGetData(COMMAND_GET_ALLAFMAST, keyField);

                Afmast[] afmast = null;

                if (ds == null)
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    afmast = new Afmast[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        afmast[i] = new Afmast()
                        {
                            custid = ds.Tables[0].Rows[i]["CUSTID"].ToString(),
                            acctno = ds.Tables[0].Rows[i]["ACCTNO"].ToString(),
                            martype = ds.Tables[0].Rows[i]["MARTYPE"].ToString(),
                            mrcrlimitmax = ds.Tables[0].Rows[i]["MRCRLIMITMAX"].ToString(),
                        };
                    }
                }
                return new list() { s = "ok", d = afmast };
            }catch (Exception ex)
            {
                Log.Error("afmast:" + ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }            
        }
        public static object getAfmastByAcctno(string acctno)
        {
            try
            {
                List<KeyField> keyField = new List<KeyField>();
                KeyField field = new KeyField();
                field.keyName = "p_acctno";
                field.keyValue = acctno;
                field.keyType = "varchar2";
                keyField.Add(field);

                DataSet ds = null;

                ds = GetDataProcess.executeGetData(COMMAND_GET_AFMAST, keyField);

                Afmast[] afmast = null;

                if (ds == null)
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    afmast = new Afmast[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        afmast[i] = new Afmast()
                        {
                            custid = ds.Tables[0].Rows[i]["CUSTID"].ToString(),
                            acctno = ds.Tables[0].Rows[i]["ACCTNO"].ToString(),
                            martype = ds.Tables[0].Rows[i]["MARTYPE"].ToString(),
                            mrcrlimitmax = ds.Tables[0].Rows[i]["MRCRLIMITMAX"].ToString(),
                        };
                    }
                }
                return new list() { s = "ok", d = afmast };
            }
            catch (Exception ex)
            {
                Log.Error("afmast: ", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }


        }
        public static object postAfmast(string strRequest, string p_ipAddress)
        {
            string logStr = "postafmast";
            Log.Info(logStr + "strRequest" + strRequest);
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string custid = "", acctno = "", martype = "",  afacctno = "", lastchange = DateTime.Now.ToString(), status = "N";
                double mrcrlimitmax = 0, balance = 20000000, pp = 0, cidepofeeacr = 0, depofeeamt = 0, currentdebt = 0;
                if (request.TryGetValue("custid", out jToken))
                    custid = jToken.ToString();
                if (request.TryGetValue("acctno", out jToken))
                    acctno = jToken.ToString();
                if (request.TryGetValue("martype", out jToken))
                    martype = jToken.ToString();
                if (request.TryGetValue("mrcrlimitmax", out jToken))
                    mrcrlimitmax = Convert.ToDouble(jToken.ToString());
                afacctno = acctno;
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[14];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custid;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = acctno;
                v_objParam.ParamSize = acctno.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_martype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = martype;
                v_objParam.ParamSize = martype.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_mrcrlimitmax";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = mrcrlimitmax;
                v_objParam.ParamSize = mrcrlimitmax.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[3] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_afacctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = afacctno;
                v_objParam.ParamSize = afacctno.Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_balance";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = balance;
                v_objParam.ParamSize = balance.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[5] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_pp";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = pp;
                v_objParam.ParamSize = pp.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[6] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_cidepofeeacr";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = cidepofeeacr;
                v_objParam.ParamSize = cidepofeeacr.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[7] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_depofeeamt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = depofeeamt;
                v_objParam.ParamSize = depofeeamt.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[8] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_currentdebt";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = currentdebt;
                v_objParam.ParamSize = currentdebt.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[9] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_lastchange";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = lastchange.AsDateTime();
                v_objParam.ParamSize = lastchange.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.DateTime").Name;
                v_arrParam[10] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_status";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = status;
                v_objParam.ParamSize = status.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[11] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_code";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[12] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_err_param";
                v_objParam.ParamDirection = "2";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[13] = v_objParam;

                long returnErr = TransactionProcess.doTransaction(COMMAND_POST_AFMAST, ref v_arrParam, 12);
                string v_strerrorMessage = (string)v_arrParam[13].ParamValue;
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);
            }
            catch (Exception ex)
            {
                Log.Error("postAfmmast " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        public static object putAfmast(string strRequest, string acctno, string p_ipAddress)
        {
            try
            {
                JObject request = JObject.Parse(strRequest);
                JToken jToken;
                string custid = "", martype = "";
                double mrcrlimitmax = 0;
                if (request.TryGetValue("custid", out jToken))
                    custid = jToken.ToString();
                if (request.TryGetValue("martype", out jToken))
                    martype = jToken.ToString();
                if (request.TryGetValue("mrcrlimitmax", out jToken))
                    mrcrlimitmax = Convert.ToDouble(jToken.ToString());
                
                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[6];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custid";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = custid;
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_acctno";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = acctno;
                v_objParam.ParamSize = acctno.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_martype";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = martype;
                v_objParam.ParamSize = martype.ToString().Length;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_mrcrlimitmax";
                v_objParam.ParamDirection = "1";
                v_objParam.ParamValue = mrcrlimitmax;
                v_objParam.ParamSize = mrcrlimitmax.ToString().Length;
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

                long returnErr = TransactionProcess.doTransaction(COMMAND_PUT_AFMAST, ref v_arrParam, 4);
                string v_strerrorMessage = (string)v_arrParam[5].ParamValue;
                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("afmast:.strRequest: " + strRequest, ex);
                return modCommon.getBoResponse(400, "Bad Request");
            }
        }
        public static object deleteAfmast(string strRequest, string acctno, string p_ipAddress)
        {
            try
            {

                string ipAddress = p_ipAddress;
                if (p_ipAddress == null || p_ipAddress.Length == 0)
                    ipAddress = modCommon.GetClientIp();

                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[3];

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_custid";
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


                long returnErr = TransactionProcess.doTransaction(COMMMAN_DELETE_AFMAST, ref v_arrParam, 1);
                string v_strerrorMessage = (string)v_arrParam[2].ParamValue;

                return modCommon.getBoResponse(returnErr, v_strerrorMessage);

            }
            catch (Exception ex)
            {
                Log.Error("deleteAfmast: ", ex);
                return 1;
            }
        }
        public static object getMaxAcctno()
        {
            try
            {

                List<KeyField> keyField = new List<KeyField>();

                KeyField field = new KeyField();

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_MAXACCTNO, keyField);

                MaxAcctnoAfmast[] acctno = null;
                if (ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    acctno = new[] { new MaxAcctnoAfmast(ds.Tables[0].Rows[0]["MAXACCTNO"].ToString()) };
                }

                return new list() { s = "ok", d = acctno };
            }
            catch (Exception ex)
            {
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }

    }
}