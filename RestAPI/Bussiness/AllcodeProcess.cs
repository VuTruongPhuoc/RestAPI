using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using log4net;
using RestAPI.Models;

namespace RestAPI.Bussiness
{
    public class AllcodeProcess
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string COMMAND_GET_ALLCODE = "getAllcode";

        public static object getAllCodes()
        {
            try
            {
                List<KeyField> keyField = new List<KeyField>();

                DataSet ds = null;
                ds = GetDataProcess.executeGetData(COMMAND_GET_ALLCODE, keyField);

                AllCode[] allcode = null;
                if(ds == null)
                {
                    return new ErrorMapHepper().getResponse("500", "bad request!");
                }
                else if(ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    allcode = new AllCode[ds.Tables[0].Rows.Count];
                    for ( int i = 0 ; i < ds.Tables[0].Rows.Count; i++)
                    {
                        allcode[i] = new AllCode()
                        {
                            cdtype = ds.Tables[0].Rows[i]["CDTYPE"].ToString(),
                            cdname = ds.Tables[0].Rows[i]["CDNAME"].ToString(),
                            cdval = ds.Tables[0].Rows[i]["CDVAL"].ToString(),
                            cdcontent = ds.Tables[0].Rows[i]["CDCONTENT"].ToString(),
                            lstodr = Convert.ToInt64(ds.Tables[0].Rows[i]["LSTODR"].ToString()),
                            cduser = ds.Tables[0].Rows[i]["CDUSER"].ToString(),
                            en_content = ds.Tables[0].Rows[i]["EN_CONTENT"].ToString(),
                            chstatus = ds.Tables[0].Rows[i]["CHSTATUS"].ToString(),
                        };
                    }
                }  
                return new list() { s = "ok", d = allcode };
            }
            catch(Exception ex)
            {
                Log.Error("getAllcode", ex);
                return new ErrorMapHepper().getResponse("400", "bad request!");
            }
        }
    }
}