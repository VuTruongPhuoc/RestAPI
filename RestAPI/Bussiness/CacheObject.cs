using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DataAccessLayer;
using CommonLibrary;
using log4net;
using System.Xml;
using System.Web;
using System.Data;

public partial class CacheObject
{

    public static long CacheTimeOutInMs = 30000 * TimeSpan.TicksPerMillisecond;
    private DataAccess _HostDALObject;

    private DateTime _TxDate;
    private long _TxDateTick = 0;
    
    private long _SymbolListTick = 0;
    private IDictionary<string, DateTime> v_DateList;
    
    private string _MarketStatus;
    private long _MarketStatusTick = 0;
    
    private static readonly log4net.ILog _LogObj = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    

    public DataAccess HostDALObject
    {
        get
        {
            if (_HostDALObject == null)
            {
                // Khoi tao DAL
                _HostDALObject = new DataAccess();
                _HostDALObject.NewDBInstance("@DIRECT_HOST");
                _LogObj.Info("Create DAL object success! ");
            }
            return _HostDALObject;
        }
    }

    public static ILog Log
    {
        get
        {
            return _LogObj;
        }
    }


    public CacheObject()
    {
    }


    public string MapPath(string s)
    {
        return s.Replace("~/", AppDomain.CurrentDomain.BaseDirectory).Replace("/", @"\");
    }

}
