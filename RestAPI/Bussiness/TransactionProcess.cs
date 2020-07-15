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
        public static long doTransaction(string pv_strProcessId,ref StoreParameter[] pv_keyField, int returnIndex)
        {
            string preFixlogSession = "doTransaction_" + pv_strProcessId + ": ";
            long v_lngErrorCode = 0;
            try
            {
                Log.Info(preFixlogSession + "======================BEGIN");
                //1.ghi khoa duy nhat cua request de tranh trung
                if (!isHostActive())
                {
                    Log.Info(preFixlogSession + "isHostActive: false");
                    Log.Info(preFixlogSession + "======================END");
                    return -100023; // ma loi dinh nghia trong deferror: he thong khong active
                }

                // for BO process
                using (TransactionScope tran = new TransactionScope())
                {
                    //gọi packages thuc hien api 
                    try
                    {
                        v_lngErrorCode = callDBTransaction(pv_strProcessId,ref pv_keyField, returnIndex);
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
                return v_lngErrorCode;
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

        public static long callDBTransaction(string pv_strProcessID,ref StoreParameter[] pv_keyField, int returnIndex)
        {
            string preFixlogSession = "callDBTransaction_" + pv_strProcessID + ": ";
            try
            {
                Log.Info(preFixlogSession + "======================BEGIN");
                DataAccess v_DataAccess = new DataAccess();
                v_DataAccess.NewDBInstance("@DIRECT_HOST");
                

                string v_strStoredName = pv_strProcessID;

                string v_lngErrorCode = v_DataAccess.ExecuteOracleStored(v_strStoredName, ref pv_keyField, returnIndex);
                if (v_lngErrorCode == null)
                    v_lngErrorCode = "0";

                Log.Info(preFixlogSession + "======================END");
                return Int64.Parse(v_lngErrorCode);
            }
            catch (Exception ex)
            {
                Log.Error(preFixlogSession, ex);
                Log.Info(preFixlogSession + "======================END");
                return -1;
            }
        }

        #endregion
           
        public static DataSet call_prc_DBTransactions(string pv_strMethod, List<KeyField> pv_keyField)
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