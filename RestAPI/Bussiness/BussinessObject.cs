using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Bussiness
{
    public class BussinessObject
    {
    }

    public class list
    {
        public string s = "";
        public object[] d;
    }

    public class BoResponse
    {
        public string s = "";
        public string errmsg = "";
    }
    public class BoResponseWithData
    {
        public string s = "";
        public string errmsg = "";
        public object d = null;
    }
    public class orderResponse
    {
        public string orderid = "";
    }
    public class idResponse
    {
        public string id = "";
    }

    public class KeyField
    {
        public string keyName = "";
        public string keyValue = "";
        public string keyType = "";   //VARCHAR2 OR NUMBER
    }

    public class HealthCheck
    {
        public string errorCode = "200";
        public string dbHostStatus = "ok";
        public string dbReportStatus = "ok";
        //public string enPayStatus = "ok";
    }

    public class loanpaymentResponse
    {
        public double overdueFee = 0;
        public double dueFee = 0;
        public double undueFee = 0;
        public double interestOnOverduePrincipal = 0;
        public double overdueInterest = 0;
        public double dueInterest = 0;
        public double undueInterest = 0;
        public double overduePrincipal = 0;
        public double duePrincipal = 0;
        public double unduePrincipal = 0;
        public double feeOnOverduePrincipal = 0;
        public string isSettled;

    }
    public class loanrate
    {
        public double newundueInterest = 0;
        public double newdueInterest = 0;
        public double newoverdueInterest = 0;
        public double newinterestOnOverduePrincipal = 0;
        public double newundueFee = 0;
        public double newdueFee = 0;
        public double newoverdueFee = 0;
        public double newfeeOnOverduePrincipal = 0;

    }
}