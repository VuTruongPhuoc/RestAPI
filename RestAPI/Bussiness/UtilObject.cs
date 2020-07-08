using DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using CommonLibrary;
//using BPSUtils;
//using Microsoft.VisualBasic.CompilerServices;

public class UtilsObject
{
    private static CacheObject _CacheHelper = new CacheObject();
    private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    public static CacheObject CacheHelper
    {
        get
        {
            return _CacheHelper;
        }
    }

    private static Dictionary<string, XmlDocument> mv_apFile = new Dictionary<string, XmlDocument>();
    public static string GetCommandString(string pv_strFilePath, string pv_strCommandName)
    {
        // Dim v_apFile As Hashtable
        XmlDocument v_xmlDoc;
        string v_strReturn = "";
        // If Not _CacheHelper.Application("FileCache") Is Nothing Then
        // v_apFile = CType(_CacheHelper.Application("FileCache"), Hashtable)
        // Else
        // v_apFile = New Hashtable
        // End If
        if (mv_apFile.ContainsKey(pv_strFilePath.ToUpper().Trim()))
            v_xmlDoc = mv_apFile[pv_strFilePath.ToUpper().Trim()];
        else
        {
            v_xmlDoc = new XmlDocument();
            v_xmlDoc.Load(_CacheHelper.MapPath(pv_strFilePath.ToUpper().Trim()));
            if (!(v_xmlDoc == null))
                mv_apFile.Add(pv_strFilePath.ToUpper().Trim(), v_xmlDoc);
        }

        if (!(v_xmlDoc == null))
        {
            var v_xmlNode = v_xmlDoc.DocumentElement.SelectSingleNode("/root/TableInfo[QueryKey='" + pv_strCommandName.ToUpper().Trim() + "']/QueryValue");
            if (!(v_xmlNode == null))
                v_strReturn = v_xmlNode.InnerXml.Replace("<![CDATA[", "").Replace("]]>", "");
        }

        return v_strReturn;
    }

    public static DataSet getDataFromBOReturnDataSet(string pv_funcname, string pv_modulename, string[] pv_str, object[] pv_objects)
    {
        string v_strSQL = string.Empty;
        CommonLibrary.ReportParameters[] v_rptParameters = new CommonLibrary.ReportParameters[pv_str.Length];
        try
        {
            v_strSQL = UtilsObject.GetCommandString(string.Format("{0}{1}.xml", "~/Xmls/", pv_modulename), pv_funcname);
            if (Log.IsDebugEnabled)
                Log.Debug(string.Format("getDataFromBOReturnDataSet:{0}:{1}; {2}:{3};Sql:{4}", pv_funcname, pv_modulename, string.Join(",", pv_str), string.Join(",",pv_objects), v_strSQL));

            for (int keyindex = 0; keyindex < pv_str.Length; keyindex++)
            {
                CommonLibrary.ReportParameters v_rptParameter = new CommonLibrary.ReportParameters();
                v_rptParameter.ParamName = pv_str[keyindex];
                v_rptParameter.ParamValue = pv_objects[keyindex];
                if (pv_objects[keyindex].GetType() == typeof(int))
                {
                    v_rptParameter.ParamType = typeof(int).ToString();
                }
                else if(pv_objects[keyindex].GetType() == typeof(double))
                {
                    v_rptParameter.ParamType = typeof(double).ToString();
                }
                else if (pv_objects[keyindex].GetType() == typeof(DateTime))
                {
                    v_rptParameter.ParamType = typeof(DateTime).ToString();
                }
                else
                {
                    v_rptParameter.ParamType = typeof(string).ToString();
                    v_rptParameter.ParamSize = pv_objects[keyindex].ToString().Length;
                }

                v_rptParameters[keyindex] = v_rptParameter;
            }          

            DataSet v_ds = null;
            DataAccess v_obj = new DataAccess();
            v_obj.NewDBInstance("@DIRECT_HOST");

            if(v_rptParameters.Length > 0)
            {
                v_ds = v_obj.ExecuteSQLParametersReturnDataset(v_strSQL, v_rptParameters);
            }
            else
            {
                v_ds = v_obj.ExecuteSQLReturnDataset(CommandType.Text, v_strSQL);
            }

            if  (v_ds.Tables[0].Rows.Count > 0)
            {
                v_ds.Tables[0].TableName = pv_funcname;
            }else
            {
                return null;
            }
            
            return v_ds;
        }
        catch (Exception ex)
        {
            Log.Error(string.Format("getDataFromBOReturnDataSet.:Exception: [{0},{1}] => [{2}]:[{3}] =>[{4}]", pv_modulename, pv_funcname, string.Join(",", pv_str), string.Join(",", pv_objects), v_strSQL), ex);
            return null;
        }
    }


    //public static DataTable GetTableConfig(string pv_strFilePath, string pv_strTableName)
    //{
    //    // Dim v_apFile As Hashtable
    //    XmlDocument v_xmlDoc;
    //    string v_strReturn = "";
    //    DataSet v_ds = null;
    //    DataTable v_dt = null;
    //    // If Not _CacheHelper.Application("FileCache") Is Nothing Then
    //    // v_apFile = CType(_CacheHelper.Application("FileCache"), Hashtable)
    //    // Else
    //    // v_apFile = New Hashtable
    //    // End If
    //    if (mv_apFile.ContainsKey(pv_strFilePath.ToUpper().Trim()))
    //        v_xmlDoc = mv_apFile[pv_strFilePath.ToUpper().Trim()];
    //    else
    //    {
    //        v_xmlDoc = new XmlDocument();
    //        v_xmlDoc.Load(_CacheHelper.MapPath(pv_strFilePath.ToUpper().Trim()));
    //        if (!(v_xmlDoc == null))
    //            mv_apFile.Add(pv_strFilePath.ToUpper().Trim(), v_xmlDoc);
    //    }

    //    if (!(v_xmlDoc == null))
    //    {
    //        v_ds = new DataSet();
    //        var v_xmlreader = new XmlTextReader(new StringReader(v_xmlDoc.OuterXml));
    //        v_ds.ReadXml(v_xmlreader);
    //        if (!(v_ds == null))
    //        {
    //            if (v_ds.Tables.Contains(pv_strTableName))
    //                v_dt = v_ds.Tables(pv_strTableName);
    //        }
    //    }

    //    return v_dt;
    //}




    public static DataTable XmlToTable(XmlNodeList pv_NodeList)
    {
        DataTable v_dt = null;
        if (!(pv_NodeList == null))
        {
            if (pv_NodeList.Count > 0)
            {
                v_dt = new DataTable(pv_NodeList[0].Name);
                for (int i = 0, loopTo = pv_NodeList.Count - 1; i <= loopTo; i++)
                {
                    // Tao cau truc bang
                    if (i == 0)
                    {
                        var v_fchild = pv_NodeList[i].ChildNodes;
                        if (!(v_fchild == null))
                        {
                            for (int j = 0, loopTo1 = v_fchild.Count - 1; j <= loopTo1; j++)
                            {
                                if (!v_dt.Columns.Contains(v_fchild[j].Attributes["name"].Value))
                                    v_dt.Columns.Add(v_fchild[j].Attributes["name"].Value, Type.GetType(v_fchild[j].Attributes["type"].Value));
                            }
                        }
                    }
                    // End
                    DataRow v_dr = v_dt.NewRow();

                    var v_child = pv_NodeList[i].ChildNodes;
                    if (!(v_child == null))
                    {
                        for (int j = 0, loopTo2 = v_child.Count - 1; j <= loopTo2; j++)
                        {
                            if ((v_child[j].Attributes["type"].Value ?? "") == "System.DateTime")
                            {
                                if (v_child[j].InnerXml.Length > 0)
                                    v_dr[v_child[j].Attributes["name"].Value] = DateTime.ParseExact(v_child[j].InnerXml, "dd/MM/yyyy", null);
                                else
                                    v_dr[v_child[j].Attributes["name"].Value] = DBNull.Value;
                            }
                            else
                                v_dr[v_child[j].Attributes["name"].Value] = v_child[j].InnerXml;
                        }
                    }

                    v_dt.Rows.Add(v_dr);
                }
            }
        }
        return v_dt;
    }
    
}
