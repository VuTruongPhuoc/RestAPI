using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestAPI.Models
{

    #region execution
    public class Execution
    {
        public string id = "";
        public string instrument = "";
        public long price = 0;
        public string time = "";
        public long qty = 0;
        public string side = "";
    }
    #endregion

    #region orders
    public class orders
    {
        public string id = "";
        public string instrument = "";
        public long qty = 0;
        public string side = "";
        public string type = "";
        public string timeInForce = "";
        public long filledqty = 0;
        public double avgprice = 0;
        public long limitprice = 0;
        public long stopprice = 0;
        public string parentid = "";
        public string parenttype = "";
        public string duration = "";
        public string status = "";
        public string txdate = "";
        public string lastModified = "";
        public string createdDate = "";
        public string channel = "";
        public string maker = "";
    }
    public class ordersInfo
    {
        public string lastModified = "";
        public string action = "";
        public string orderId = "";
        public long matchQtty = 0;
        public long matchPrice = 0;
        public long quoteQtty = 0;
        public long quotePrice = 0;
        public string channel = "";
        public string maker = "";
    }
    #endregion

    #region ordersHistory
    public class ordersHistory
        {
        public string id = "";
        public string instrument = "";
        public long qty = 0;
        public string side = "";
        public string type = "";
        public long filledqty = 0;
        public long avgprice = 0;
        public long limitprice = 0;
        public long stopprice = 0;
        public string parentid = "";
        public string parenttype = "";
        public string duration = "";
        public string status = "";
        public long lastModified = 0;
        public string channel = "";
        public string maker = "";
    }
    #endregion

    #region "Account"
    public class Account
    {
        public string id = "";
        public string name = "";
        public string currency = "";
        public string accountType = "";
        public string accountTypeName = "";
        public string accountTypeBriefName = "";
        public string openDate = "";
    }
    #endregion

    public class PPSE
    {
        public long ppse = 0;
        public long maxQtty = 0;
        public long trade = 0;
    }

    public class summaryAccount
    {
        public long totalCash = 0;
        public long availableCash = 0;
        public long termDeposit = 0;
        public long depositInterest = 0; 
        public long receivingt0 = 0;
        public long receivingt1 = 0;
        public long receivingt2 = 0;
        public long stockValue = 0;
        public long marginStockValue = 0;
        public long nonMarginStockValue = 0;
        public long dealFinanceStockValue = 0;
        public long totalDebt = 0;
        public long securedAmount = 0;
        //public long trfbuyamt = 0;
        public long marginDebt = 0;
        public long t0Debt = 0;
        public long advancedAmount = 0;
        public long dealFinancingDebt = 0;
        //public long tddebtamt = 0;
        public long stockDepositFee = 0;
        public long netassetvalue = 0;
        //public long requiredmarginamt = 0;
        public long marginableValue = 0;
        //public long sesecured_buy = 0;
        //public long accountvalue = 0;
        //public long qttyamt = 0;
        //public long mrcrlimit = 0;
        //public long bankavlbal = 0;
        //public long debtamt = 0;
        //public long advancemaxamtfee = 0;
        public long receivingamt = 0;
        public long purchasingPower = 0;
        public double marginrate = 0;
        //public long holdbalance = 0;
        //public string bankinqirytime = "";
        public long caReceiving = 0;
        public long blockedAmount = 0;
        public long smsFee = 0;
        public long maxLoanLimit = 0;
        public long withdrawableCash = 0;
        public long collateralValue = 0;
        public long orderSecured = 0;
        public long marginCallAmount = 0;
        public long ppwithdraw = 0;
    }

    public class loanInfo //Thong tin mon vay DNS.2022.01.1.02
    {
        public string custodCode = "";
        public string subAccountId = "";
        public string lnTypeId = "";
        public string lnAccountnNo = "";
        public long id = 0;
        public string releaseDate = "";
        public string t1 = "";
        public string dueDate = "";
        public double unduePrincipal = 0;
        public double duePrincipal = 0;
        public double overduePrincipal = 0;
        public double undueInterest = 0;
        public double dueInterest = 0;
        public double overdueInterest = 0;
        public double interestOnOverduePrincipal = 0;
        public double undueFee = 0;
        public double dueFee = 0;
        public double overdueFee = 0;
        public double feeintovdacr = 0;
        public double interestRate1 = 0;
        public double interestRate2 = 0;
        public double interestRate3 = 0;
        public double feeRate1 = 0;
        public double feeRate2 = 0;
        public double feeRate3 = 0;
        public long extendTimes = 0;
    }

    public class securitiesPortfolio
    {
        public string custodycd = "";
        public string accountid = "";
        public string symbol = "";
        public long totalQuantity = 0;
        public long tradeQuantity = 0;
        public long blockedQuantity = 0;
        //public long vsdmortgage = 0;
        public long mortgageQuantity = 0;
        public long restrictedQuantity = 0;
        public long receivingRightQuantity = 0;
        public long receivingt0 = 0;
        public long receivingt1 = 0;
        public long receivingt2 = 0;
        public double costprice = 0;
        public long initialValue = 0;
        public long marketPrice = 0;
        public long stockValue = 0;
        //public long marginratio = 0;
        //public long requiredmarginamt = 0;
        //public long marginamt = 0;
        public long unrealizedProfit = 0;
        public string unrealizedProfitRate = "";
        //public string issell = "";
        public long pendingToWithdraw = 0;
    }

    public class WithdrawalTransactionInfo : WithdrawalTransaction
    {
        public string enpayTransId;
        public string status;
        public string code;
        public string message;
    }

    public class WithdrawalTransaction
    {
        public string benAccNo;
        public string benAccType;
        public string benName;
        public string desc;
        public int amt;
        public string benBankName;
        public string benBankCode;
        public string benBankBranchName;
        public string benBankBranchCode;
        public string benBankNapasCode;
        public string srcTransId;
    }
}

