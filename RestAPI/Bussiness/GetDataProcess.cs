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
        
        public static string getErrmsg(long errorCode, string lang = "VI")
        {
            string errorMsg = string.Empty;
            try
            {
                if (errorCode == 0)
                    errorMsg = "Success";
                else if (errorCode == -1 || errorCode == 1)
                    errorMsg = "System Error";
                else
                {
                    string v_strSQL = "SELECT DECODE(:p_lang,'VI',errdesc,en_errdesc) ERRDESC FROM deferror where errnum = :p_error";
                    ReportParameters[] arrayParam = new ReportParameters[2];
                    arrayParam[0] = new ReportParameters() { ParamName = "p_lang", ParamValue = lang, ParamSize = lang.Length, ParamType = Type.GetType("System.String").Name };
                    arrayParam[1] = new ReportParameters() { ParamName = "p_error", ParamValue = errorCode, ParamSize = errorCode.ToString().Length, ParamType = Type.GetType("System.String").Name };


                    DataAccess v_obj = new DataAccess(gc_DBModule);
                    DataSet ds = v_obj.ExecuteSQLParametersReturnDataset(v_strSQL, arrayParam);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                            errorMsg = ds.Tables[0].Rows[0]["ERRDESC"].ToString();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return "Undefined Error!";
            }
            return errorMsg;
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
                v_DataAccess.NewDBInstance("@DIRECT_HOST");

                ReportParameters v_objRptParam = new ReportParameters();
                ReportParameters[] v_arrRptPara = new ReportParameters[length];

                string v_strStoredName = pv_strMethod;

                for (int i = 0; i < pv_keyField.Count(); i++)
                {
                    v_objRptParam = new ReportParameters();
                    v_objRptParam.ParamName = pv_keyField[i].keyName;
                    v_objRptParam.ParamDescription = "1";
                    v_objRptParam.ParamValue = pv_keyField[i].keyValue;
                    if (String.IsNullOrEmpty(pv_keyField[i].keyValue))
                        v_objRptParam.ParamSize = 0;
                    else
                        v_objRptParam.ParamSize =  pv_keyField[i].keyValue.Length;
                    if (pv_keyField[i].keyType.ToUpper() == "VARCHAR2")
                        v_objRptParam.ParamType = Type.GetType("System.String").Name;
                    else
                        v_objRptParam.ParamType = Type.GetType("System.Double").Name;

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

        public static DataSet executeSQL(string pv_strSSQL)
        {
            try
            {
                DataAccess v_DataAccess = new DataAccess(gc_DBModule);

                return v_DataAccess.ExecuteSQLReturnDataset(CommandType.Text,pv_strSSQL);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}