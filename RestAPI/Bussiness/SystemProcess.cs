using System;
using RestAPI.Models;
using log4net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using CommonLibrary;
using System.Net.Sockets;
using System.Net;
using DataAccessLayer;
using System.Net.Http;
namespace RestAPI.Bussiness
{
    public static class SystemProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string ENPAY_SERVICE_URL = modCommond.GetConfigValue("ENPAY_SERVICE_URL", "");

        #region orders
        //huy lenh
        public static object HealthCheck()
        {
            string v_strSql = "select getcurrdate from dual";
            object v_strErrDesc = string.Empty;
            HealthCheck health = new HealthCheck();
            try
            {
                DataSet v_ds = null;

                v_ds = GetDataProcess.executeSQL(v_strSql);
                if (v_ds == null || v_ds.Tables.Count == 0 || v_ds.Tables[0].Rows.Count == 0 )
                {
                    health.dbReportStatus = "error";
                    health.errorCode = "500";
                }

                v_ds = null;
                v_ds = TransactionProcess.executeSQL(v_strSql);
                if (v_ds == null || v_ds.Tables.Count == 0 || v_ds.Tables[0].Rows.Count == 0)
                {
                    health.dbHostStatus = "error";
                    health.errorCode = "500";
                }

                if (!String.IsNullOrEmpty(ENPAY_SERVICE_URL))
                {
                    HttpClient client = new HttpClient();
                    try
                    {
                        HttpResponseMessage response = 
                            client.SendAsync(new HttpRequestMessage(HttpMethod.Get, ENPAY_SERVICE_URL + "/health")).Result;

                        if (!response.IsSuccessStatusCode)
                        {
                            health.enPayStatus = "error";
                            health.errorCode = "500";
                        }
                    }
                    catch (Exception e)
                    {
                        health.enPayStatus = "error";
                        health.errorCode = "500";
                    }
                    finally
                    {
                        client.Dispose();
                    }
                }
                return health;
            }
            catch (Exception ex)
            {
                Log.Error("HealthCheck:.Exception:." , ex);
                health.errorCode = "500";
                health.dbHostStatus = "error";
                health.dbReportStatus = "error";
                return health;
            }
        }
        #endregion

    }
}