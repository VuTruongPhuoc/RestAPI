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
    public static class GetDataProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string gc_DBModule = "@DIRECT_REPORT";
        
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
        
        public static DataSet executeGetData(string pv_strMethod, List<KeyField> pv_keyField)
        {
            string preFixlogSession = "executeGetData" + pv_strMethod + ": ";
            try
            {
                Log.Info(preFixlogSession + "======================BEGIN");

                int length = pv_keyField.Count();

                DataSet v_ds = null;
                DataAccess v_DataAccess = new DataAccess(gc_DBModule);
                //v_DataAccess.NewDBInstance("@DIRECT_HOST");

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