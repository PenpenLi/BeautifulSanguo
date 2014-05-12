// Author : dandanshih

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using System.Reflection;
using Newtonsoft.Json;

/// <summary>
/// WebService 的摘要描述
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
//[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// 若要允許使用 ASP.NET AJAX 從指令碼呼叫此 Web 服務，請取消註解下一行。
[System.Web.Script.Services.ScriptService]
public partial class GameService : System.Web.Services.WebService
{
    // 建構子的處理
    public GameService()
    {
        // 路徑
        Utility.WebPath = Server.MapPath(".");
        // 啟動 log4net
        System.IO.FileInfo FI = new System.IO.FileInfo(Server.MapPath (".") + "\\log4net.config");
        log4net.Config.XmlConfigurator.Configure(FI);
    }

    // 每一筆都需要寫 Log
    int ReportDBLog(string strMethodName, string strJosn, int PID=0)
    {
        strMethodName = Utility.TranslateDBString(strMethodName);
        strJosn = Utility.TranslateDBString(strJosn);
		string strCommand = string.Format("insert into L_WebMethod (MethodName, Args, PID) values ('{0}', '{1}', {2})"+UseDB.GETID, strMethodName, strJosn, PID);
        List<List<object>> listDBResult = UseDB.GameLogDB.DoQueryCommand(strCommand);
        int LogID = System.Convert.ToInt32(listDBResult[0][0]);
        return LogID;
    }

    #region 動態呼叫的入口

    // 統一的入口
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public string Web_Method(string strJson)
    {
		Dictionary<string, object> dictInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);
        Dictionary<string, object> dictResult = new Dictionary<string, object>();
        if (dictInfo.ContainsKey("MethodName") == false)
        {
			return JsonConvert.SerializeObject(dictResult);
        }
        // 做動態的呼叫
        return DynamicCallGameService(dictInfo["MethodName"].ToString(), dictInfo);
    }

    // 訂好格式
    string DynamicCallGameService(string strMethodName, Dictionary<string, object> dictInfo)
    {
        Type callMethod = typeof(GameService);
        return callMethod.InvokeMember(strMethodName, BindingFlags.InvokeMethod, null, this, new object[] { dictInfo }).ToString();
    }

    #endregion

	#region 統一的錯誤處理
	// 統一的錯誤處理
	string ReportTheResult(Dictionary<string, object> dictResult, ErrorID IErrorID, int LogID)
	{
		return JsonConvert.SerializeObject (ReportTheResultDict(dictResult, IErrorID, LogID));
	}

	Dictionary<string, object> ReportTheResultDict(Dictionary<string, object> dictResult, ErrorID IErrorID, int LogID)
    {
        dictResult["Result"] = IErrorID;
		ReportDBLog(IDMap.GetEnumAttribute(IErrorID), JsonConvert.SerializeObject (IErrorID), LogID);
        return dictResult;
    }
	#endregion

	#region 統一的參數處理
	// 取得 session Key
	string GetSessionKey(string Account)
	{
		// 以後再來想怎麼編碼
		return "SessionKey:" + Account;
	}

	// 利用 Session Key 取得帳號資料
	Dictionary<string, object> GetAccountInfoBySessionKey(string SessionKey)
	{
		Dictionary<string, object> dictResult = new Dictionary<string, object>();
		string strCommand = "";
		List<List<object>> listDBResult = null;
		// 從 DB 取得玩家資料
		strCommand = string.Format("select AccountID, Account, PlayerID, UpdateDate, IsStop, StopDate from a_account where SessionKey='{0}'", SessionKey);
		listDBResult = UseDB.GameDB.DoQueryCommand(strCommand);
		if (listDBResult.Count == 0)
		{
			ReportDBLog(JsonConvert.SerializeObject(ErrorID.SessionError), IDMap.GetEnumAttribute(ErrorID.SessionError));
			return null;
		}
		// 把資料一個一個存起來
		dictResult["AccountID"] = listDBResult[0][0];
		dictResult["Account"] = listDBResult[0][1];
		dictResult["PlayerID"] = listDBResult[0][2];
		dictResult["UpdateDate"] = listDBResult[0][3];
		dictResult["IsStop"] = listDBResult[0][4];
		dictResult["StopDate"] = listDBResult[0][5];

		return dictResult;
	}

	// 做輸出的處理 : Input : Args info, output --> account info
	Dictionary<string, object> PaserArgs(string strJson, int LogID, out Dictionary<string, object> dictInfo)
	{
		Dictionary<string, object> dictResult = new Dictionary<string, object>();
		// 處理參數
		dictInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);
		if (dictInfo == null)
		{
			return ReportTheResultDict(dictResult, ErrorID.SessionError, LogID);
		}
		// 處理 session key
		string SessionKey = dictInfo["SessionKey"].ToString();

		// 先轉 Session Key
		dictResult = GetAccountInfoBySessionKey(SessionKey);
		if (dictResult == null)
		{
			return ReportTheResultDict(dictResult, ErrorID.SessionError, LogID);
		}
		// 檢查是否有被停權
		int IsStop = System.Convert.ToInt32(dictResult["IsStop"]);
		if (IsStop != 0)
		{
			return ReportTheResultDict(dictResult, ErrorID.Account_Stoped, LogID);
		}
		// [Todo] 還要檢查 SessionKey 有沒有過期

		// 把資料存過去
		dictInfo = dictInfo.Union(dictResult).ToDictionary(k => k.Key, v => v.Value);

		return null;
	}
	#endregion

}
