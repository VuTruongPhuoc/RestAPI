using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Data;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using DataAccessLayer;

namespace RestAPI.Bussiness
{
    public class ErrorMapHepper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DataTable mv_dataTable { get; set; }
        private string FileName = "";
        private string Url = "";
        private string IDField;
        private string ErrorCodeField;
        private string ErrorMsgField;

        public ErrorMapHepper()
        {
            FileName = "ErrorMap.xml";
            Url = "~/Xmls";
            IDField = "ErrorID";
            ErrorCodeField = "ErrorCode";
            ErrorMsgField = "ErrorDesc";
        }
        public ErrorMapHepper(string fileName, string url, string IDFields, string codeField, string msgField)
        {
            this.FileName = fileName;
            this.Url = url;
            IDField = IDFields;
            ErrorCodeField = codeField;
            ErrorMsgField = msgField;
        }
        public bool initSource()
        {
            try
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("Begin init ErrorList");
                }
                if (mv_dataTable == null)
                    mv_dataTable = new DataTable();
                string strFileUrl = System.Web.Hosting.HostingEnvironment.MapPath(string.Format("{0}/{1}", this.Url, this.FileName));
                using (DataSet ds = new DataSet())
                {
                    ds.ReadXml(strFileUrl);
                    if (ds != null)
                    {
                        if (ds.Tables.Count > 0)
                        {

                            DataTable dt = ds.Tables[0].Copy();
                            dt.TableName = FileName;
                            mv_dataTable = dt;

                        }
                    }
                }
                if (Log.IsDebugEnabled)
                {
                    Log.Debug("End init ErrorList");
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error("init Error Exception:: " + ex.ToString());
                return false;
            }
        }

        public static HttpResponseMessage CreateResponseError(HttpRequestMessage request, object objResult, string errorType = "400")
        {
            try
            {
                switch ( errorType.Trim() )
                {
                    case "400":
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.BadRequest, objResult);
                    case "401":
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.Unauthorized, objResult);
                    case "404":
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.NotFound, objResult);
                    case "500":
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.InternalServerError, objResult);
                    case "501":
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.NotImplemented, objResult);
                    case "502":
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.BadGateway, objResult);
                    case "503":
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.ServiceUnavailable, objResult);
                    default:
                        return Bussiness.modCommon.CreateResponseAPI(request, HttpStatusCode.BadRequest, objResult);
                }
            }
            catch
            {
                return request.CreateResponse(HttpStatusCode.NotImplemented, JObject.FromObject(objResult));
            }
        }

        public BoResponse getResponsesForType(string errorCode,  string errorMsg, ref string errorType)
        {
            BoResponse ret = new BoResponse();
            try
            {
                int v_count;
                v_count = errorMsg.IndexOf('#');
                if(v_count > 0)
                {
                    string[] msgkey = errorMsg.Split('#');
                    errorType = msgkey[0];
                    ret = getResponse(errorCode, msgkey[1]);
                    return ret;
                }
                else
                {
                    errorType = "400";
                    ret = getResponse(errorCode, errorMsg);
                    return ret;
                }
            }
            catch(Exception ex)
            {
                ret.s = errorMsg;
                ret.errmsg = errorMsg;
                errorType = "400";
                return ret;
            }         
        }

        public BoResponse getResponse(string fdsErrorCode, string defMsg)
        {
            BoResponse ret = new BoResponse();
            try
            {
                if (mv_dataTable == null)
                    initSource();
                var v_drs = mv_dataTable.Select(string.Format("{0}='{1}'", IDField, fdsErrorCode));
                if (v_drs.Length > 0)
                {
                    ret.s = Convert.ToString(v_drs[0][ErrorCodeField]);
                    ret.errmsg = Convert.ToString(v_drs[0][ErrorMsgField]);
                    if (ret.s.IndexOf('#')>0)
                    {
                        ret.errmsg = ret.errmsg.Split('#')[1].ToString();
                    }
                }
                else
                {
                    ret.s = fdsErrorCode;
                    ret.errmsg = defMsg;
                }
                return ret;
            }
            catch (Exception ex)
            {
                ret.s = fdsErrorCode;
                ret.errmsg = defMsg;
                return ret;
            }
        }

        public static string getErrDesc(string errorCode, string defMsg)
        {
            string v_strSql = "SELECT errdesc FROM DEFERROR WHERE ERRNUM = " + errorCode + "";
            string v_strErrDesc = string.Empty;
            try
            {
                DataSet v_ds = null;
                DataAccess v_obj = new DataAccess();
                v_obj.NewDBInstance("@DIRECT_REPORT");
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSql);
                v_strErrDesc = defMsg;
                if (v_ds.Tables.Count > 0)
                {
                    v_strErrDesc = v_ds.Tables[0].Rows[0]["ERRDESC"].ToString();
                }
                return v_strErrDesc;
            }
            catch (Exception ex)
            {
                return defMsg;
            }
        }
    }
}