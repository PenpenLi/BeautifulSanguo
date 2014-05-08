// Author : dandanshih
// Desc : 帳號相關的程式都需要放在這裏處理
// Agent 類的盡量都不使用 Json 格式做撰寫

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using Newtonsoft.Json;
/// <summary>
/// WebService 的摘要描述
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
public partial class GameService : System.Web.Services.WebService
{
	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Agent_Player_Stop(string AccountName, int IsStop)
	{
		// 先寫一筆 Log 進去
		int LogID = ReportDBLog("Agent_Player_Stop", string.Format("{0},{1}", AccountName, IsStop));
		// 先找看看有沒有該玩家

		// 對該玩家做停權的行為
		string strCommand = string.Format("update a_account set IsStop = {0}, StopDate = '{1}' where Account = '{2}'", IsStop, Utility.GetDBDateTime(), AccountName);
		UseDB.GameDB.DoCommand(strCommand);
		return "Agent_Player_Stop : IsStop : " + IsStop.ToString();
	}

	// 產生帳號的功能
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public string Agent_Account_Create(string strAccount, string strPassword)
    {
        // 先寫一筆 Log 進去
        int LogID = ReportDBLog("Agent_Account_Create", string.Format("{0},{1}", strAccount, strPassword));
        // 開始處理
        string strCommand = "";
        List<List<object>> listDBResult = null;
        Dictionary<string, object> dictResult = new Dictionary<string,object>();
        // 先檢查帳號是否存在
        strCommand = string.Format("select count(*) from a_account where Account = '{0}'", strAccount);
        listDBResult = UseDB.AccountDB.DoQueryCommand(strCommand);
        int iNumber = System.Convert.ToInt32(listDBResult[0][0]);
        if (iNumber != 0)
        {
            dictResult["Result"] = ErrorID.Agent_Account_Create_Duplicate_Account;
            ReportDBLog ("Agent_Account_Create Duplication Account", JsonConvert.SerializeObject (dictResult), LogID);
            return JsonConvert.SerializeObject (dictResult);
        }
        // 做塞入帳號的動作
		string strSessionKey = GetSessionKey(strAccount);
		strCommand = string.Format("insert into a_account (Account, Password, SessionKey) values ('{0}', '{1}', '{2}')", strAccount, strPassword, strSessionKey);
        UseDB.AccountDB.DoCommand(strCommand);

        // 傳回完成
        dictResult["Result"] = ErrorID.Success;
		dictResult["SessionKey"] = strSessionKey;
        ReportDBLog ("Agent_Account_Create Success", JsonConvert.SerializeObject (dictResult), LogID);
        return JsonConvert.SerializeObject (dictResult);
    }
}
