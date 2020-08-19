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
namespace RestAPI.Bussiness
{
    public static class SymtemProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region orders
        //huy lenh
        public static object HealthCheck(string strRequest)
        {
            string v_strSql = "select getcurrdate from dual";
            object v_strErrDesc = string.Empty;
            try
            {
                DataSet v_ds = null;
                DataAccess v_obj = new DataAccess();
                DataAccess v_obj1 = new DataAccess();
                v_obj.NewDBInstance("@DIRECT_REPORT");
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSql);

                v_obj1.NewDBInstance("@DIRECT_HOST");
                v_ds = v_obj1.ExecuteSQLReturnDataset(CommandType.Text, v_strSql);

                if (v_ds.Tables.Count > 0)
                {
                   v_strErrDesc = new BoResponse() { s = "200", errmsg = "ok" };
                }
                return v_strErrDesc;
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return new BoResponse() { s = "500", errmsg = "connection error" } ;
            }
        }
        #endregion

    }
}