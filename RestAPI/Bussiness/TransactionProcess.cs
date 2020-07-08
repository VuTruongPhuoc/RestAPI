using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using log4net;
using DataAccessLayer;
using System.Data;
using CommonLibrary;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;

namespace RestAPI.Bussiness
{
    public static class TransactionProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Boolean isHostActive()
        {
            try
            {
                string v_strSQL = "SELECT cspks_system.fn_get_sysvar('SYSTEM', 'HOSTATUS') HostStatus FROM DUAL";
                string v_hostStatus = "0";
                DataAccess v_obj = new DataAccess();
                v_obj.NewDBInstance("@DIRECT_HOST");
                DataSet ds = new DataSet();
                ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        v_hostStatus = ds.Tables[0].Rows[0]["HOSTSTATUS"].ToString();
                }
                if (v_hostStatus.ToUpper() == "1")
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Log.Error("isHostActive", ex);
                return false;
            }
        }

        public static string getErrmsg(long errorCode, string errorType = "400", string lang = "VI")
        {
            try
            {   
                if(errorCode == -1)
                {
                    return "500#System error!";
                }else if (errorCode == 400)
                {
                    return "400#bad request!";
                }

                string v_strSQL = "SELECT pck_api_common.fn_get_errorObject('"+ errorCode + "', '" + lang + "', '" + errorType + "') ERRDESC FROM DUAL";
                string errorMsg = string.Empty;
                DataAccess v_obj = new DataAccess();
                v_obj.NewDBInstance("@DIRECT_HOST");
                DataSet ds = new DataSet();
                ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        errorMsg = ds.Tables[0].Rows[0]["ERRDESC"].ToString();
                }
                return errorMsg;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return "500#Undefined Error!";
            }
        }

        #region doTransaction
        public static long doTransaction(string pv_strProcessId, string pv_strTxMessage, ref string returnKey, string pv_strMethod/*, HttpRequestMessage pv_request*/ )
        {
            string preFixlogSession = "doTransaction_" + pv_strProcessId + ": ";
            long v_lngErrorCode = 0;
            try
            {
                Log.Info(preFixlogSession + "======================BEGIN");
                //1.ghi khoa duy nhat cua request de tranh trung
                if (!isHostActive())
                {
                    returnKey = getErrmsg(-100023, "400");
                    Log.Info(preFixlogSession + "isHostActive: false");
                    Log.Info(preFixlogSession + "======================END");
                    return -100023; // ma loi dinh nghia trong deferror: he thong khong active
                }

                var jTran = JObject.Parse(pv_strTxMessage);
                var jHeaders = JObject.Parse(jTran["headers"].ToString());

                // for BO process
                using (TransactionScope tran = new TransactionScope())
                {
                    //gọi packages thuc hien api 
                    try
                    {
                        v_lngErrorCode = callDBTransaction(pv_strProcessId, pv_strTxMessage, ref returnKey, pv_strMethod);
                        Log.Info(preFixlogSession + "callDBTransaction.v_lngErrorCode: " + v_lngErrorCode.ToString());
                        if (v_lngErrorCode == 0)
                            tran.Complete();
                     
                    }
                    catch (Exception ex)
                    {
                        tran.Dispose();
                        Log.Error(preFixlogSession, ex);
                        Log.Info(preFixlogSession + "======================END");
                        return 400;
                    }
                }

                Log.Info(preFixlogSession + "======================END");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Error(preFixlogSession, ex);
                Log.Info(preFixlogSession + "======================END");
                return 400;
            }
        }

        public static string GetTimespanSequence()
        {
            return string.Format("{0}{1}{2}{3}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
        }

        public static long callDBTransaction(string pv_strProcessID, string pv_strTxMessage, ref string returnKey, string pv_strMethod)
        {
            string preFixlogSession = "callDBTransaction_" + pv_strProcessID + ": ";
            try
            {
                Log.Info(preFixlogSession + "======================BEGIN");
                DataAccess v_DataAccess = new DataAccess();
                v_DataAccess.NewDBInstance("@DIRECT_HOST");
                StoreParameter v_objParam = new StoreParameter();
                StoreParameter[] v_arrParam = new StoreParameter[5];


                string v_strStoredName = "pck_api_" + pv_strProcessID + "." + pv_strMethod;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "return";
                v_objParam.ParamValue = 0;
                v_objParam.ParamDirection = "6"; // ParameterDirection.ReturnValue.ToString();
                v_objParam.ParamSize = 100;
                v_objParam.ParamType = Type.GetType("System.Double").Name;
                v_arrParam[0] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "pv_strTxMessage";
                v_objParam.ParamValue = pv_strTxMessage;
                v_objParam.ParamDirection = "1"; // ParameterDirection.Input.ToString();
                v_objParam.ParamSize = 32000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[1] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_wsname";
                v_objParam.ParamDirection = "1"; // ParameterDirection.Input.ToString();
                v_objParam.ParamValue = System.Net.Dns.GetHostName();
                v_objParam.ParamSize = 200;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[2] = v_objParam;

                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_ws_ipaddress";
                v_objParam.ParamDirection = "1"; // ParameterDirection.Input.ToString();
                v_objParam.ParamType = Type.GetType("System.String").Name;
                foreach (IPAddress ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
                {
                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork) // filter out ipv4
                    {
                        v_objParam.ParamValue = ipAddress.ToString();
                    }
                }
                v_arrParam[3] = v_objParam;


                v_objParam = new StoreParameter();
                v_objParam.ParamName = "p_out_key";
                v_objParam.ParamDirection = "3"; //ParameterDirection.InputOutput.ToString();
                v_objParam.ParamValue = "";
                v_objParam.ParamSize = 4000;
                v_objParam.ParamType = Type.GetType("System.String").Name;
                v_arrParam[4] = v_objParam;



                returnKey = v_DataAccess.ExecuteOracleStored(v_strStoredName, ref v_arrParam, 4);
                long v_lngErrorCode = long.Parse(v_arrParam[0].ParamValue.ToString());

                Log.Info(preFixlogSession + "======================END");
                return v_lngErrorCode;
            }
            catch (Exception ex)
            {
                Log.Error(preFixlogSession, ex);
                Log.Info(preFixlogSession + "======================END");
                return -1;
            }
        }

        #endregion
               
        public static DataSet call_prc_DBTransactions(string pv_strMethod, List<keyField> pv_keyField)
        {
            string preFixlogSession = "call_prc_DBTransactions" + pv_strMethod + ": ";
            try
            {
                Log.Info(preFixlogSession + "======================BEGIN");

                int length = pv_keyField.Count();

                DataSet v_ds = null;
                DataAccess v_DataAccess = new DataAccess();
                v_DataAccess.NewDBInstance("@DIRECT_HOST");

                ReportParameters v_objRptParam = new ReportParameters();
                ReportParameters[] v_arrRptPara = new ReportParameters[length];

                string v_strStoredName = pv_strMethod;

                for (int i = 0; i < pv_keyField.Count(); i++)
                {
                    v_objRptParam = new ReportParameters();
                    v_objRptParam.ParamName = pv_keyField[i].keyName;
                    v_objRptParam.ParamValue = pv_keyField[i].keyValue;
                    v_objRptParam.ParamSize = 32000;

                    if (pv_keyField[i].keyType.ToUpper() == "VARCHAR2")
                    {
                        v_objRptParam.ParamType = Type.GetType("System.String").Name;
                    }
                    else
                    {
                        v_objRptParam.ParamType = Type.GetType("System.Double").Name;
                    }

                    v_arrRptPara[i] = v_objRptParam;

                }

                v_ds = v_DataAccess.ExecuteStoredReturnDataset(v_strStoredName, v_arrRptPara);

                Log.Info(preFixlogSession + "======================END");
                return v_ds;
            }
            catch (Exception ex)
            {
                Log.Error(preFixlogSession, ex);
                Log.Info(preFixlogSession + "======================END");
                return null;
            }
        }
        

    }
}