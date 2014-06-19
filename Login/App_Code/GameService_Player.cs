// Author : dandanshih
// Desc : 帳號相關的程式都需要放在這裏處理
// [problem] 可以使用 exception 來簡化修改

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
    #region 取得玩家屬性

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public string Test_Player_GetAttr()
    {
        Dictionary<string, object> dictResult = new Dictionary<string, object>();
		dictResult["SessionKey"] = "SessionKey:dandan";
        return Player_GetAttr(JsonConvert.SerializeObject(dictResult));
    }

    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public string Player_GetAttr(string strJson)
    {
		// 先寫一筆 Log
        int LogID = ReportDBLog("Player_GetAttr", strJson);
        Dictionary<string, object> dictResult = null;

        // 先解析資料
		Dictionary<string, object> dictInfo = new Dictionary<string, object>();
		dictResult = PaserArgs(strJson, LogID, out dictInfo);
		if (dictResult != null)
		{
			return JsonConvert.SerializeObject(dictResult);
		}
		dictResult = new Dictionary<string, object>();

		// 取得帳號分析
		int PlayerID = System.Convert.ToInt32(dictInfo["PlayerID"]);
        if (PlayerID == 0)
        {
            return ReportTheResult(dictResult, ErrorID.Player_GetAttr_No_Player_ID, LogID);
        }

		// 取得資料
		Dictionary<string, object> dictPlayerAttr = _GetPlayerAttrFromDB(PlayerID);
		//ClientAction.NameUpdate(dictResult, dictPlayerAttr["PlayerName"]);
		ClientAction.AddClientAction(dictResult, ClientActionID.Player_Name, dictPlayerAttr["PlayerName"]);
		//ClientAction.MoneyUpdate(dictResult, dictPlayerAttr["Money"]);
        ClientAction.AddClientAction(dictResult, ClientActionID.Game_Money, dictPlayerAttr["Money"]);
		//ClientAction.CoinUpdate(dictResult, dictPlayerAttr["Coin"]);
		ClientAction.AddClientAction(dictResult, ClientActionID.Game_Coin, dictPlayerAttr["Coin"]);
		//ClientAction.LVUpdate(dictResult, dictPlayerAttr["LV"]);
		ClientAction.AddClientAction(dictResult, ClientActionID.Player_LV, dictPlayerAttr["LV"]);
		//ClientAction.ExpUpdate(dictResult, dictPlayerAttr["Exp"]);
		ClientAction.AddClientAction(dictResult, ClientActionID.Player_Exp, dictPlayerAttr["Exp"]);

        return ReportTheResult(dictResult, ErrorID.Success, LogID);
    }

    #endregion

	#region 玩家經驗值
	// 做加經驗值的動作
	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Test_AddPlayerExp()
	{
		Dictionary<string, object> dictResult = new Dictionary<string, object>();
		dictResult["SessionKey"] = "SessionKey:dandan";
		string strJson = JsonConvert.SerializeObject(dictResult);
		Dictionary<string, object> dictInfo = new Dictionary<string, object>();
		dictResult = PaserArgs(strJson, 0, out dictInfo);
		return _AddPlayerExp(dictInfo, 10);
	}

	string _AddPlayerExp(Dictionary<string, object> dictInfo, int AddExp)
	{
		Dictionary<string, object> dictResult = new Dictionary<string, object>();
		// 取得玩家編號
		int PlayerID = System.Convert.ToInt32(dictInfo["PlayerID"]);
		// 取得玩家資料
		Dictionary<string, object> dictPlayer = _GetPlayerAttrFromDB(PlayerID);
		// 取得玩家等級經驗值
		int LV = System.Convert.ToInt32 ( dictPlayer["LV"].ToString() );
		int Exp = System.Convert.ToInt32(dictPlayer["Exp"]);
		dictResult["LV"] = LV;
		dictResult["Exp"] = Exp;
        dictResult["AddExp"] = AddExp;
		// 取得先等級需求的 Exp
		int NeedExp = PlayerExpTable.instance().GetExpByLV(LV);
		dictResult["NeedExp"] = NeedExp;
		// 先做 Exp 的加上
        Exp += AddExp;
        if (Exp >= NeedExp)
        {
            LV += 1;
            Exp -= NeedExp;
        }
        dictResult["NewLV"] = LV;
        dictResult["NewExp"] = Exp;
        // 更新 DB
        string strCommand = string.Format("update a_member set LV={0}, Exp={1} where PlayerID = {2}", LV, Exp, PlayerID);
        dictResult["strCommand"] = strCommand;
        UseDB.GameDB.DoCommand(strCommand);
		// 傳回結果
		return JsonConvert.SerializeObject(dictResult);
	}

	#endregion

	#region 玩家 NPC

	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Test_Player_GetNPC()
	{
		Dictionary<string, object> dictResult = new Dictionary<string, object>();
		dictResult["SessionKey"] = "SessionKey:dandan";
		return Player_GetNPC(JsonConvert.SerializeObject (dictResult));
	}

	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Player_GetNPC(string strJson)
	{
		// 先寫一筆 Log
		int LogID = ReportDBLog("Player_GetAttr", strJson);
		Dictionary<string, object> dictResult = null;

		// 先解析資料
		Dictionary<string, object> dictInfo = new Dictionary<string, object>();
		dictResult = PaserArgs(strJson, LogID, out dictInfo);
		if (dictResult != null)
		{
			return JsonConvert.SerializeObject(dictResult);
		}
		dictResult = new Dictionary<string, object>();

		// 取得帳號分析
		int PlayerID = System.Convert.ToInt32(dictInfo["PlayerID"]);
		if (PlayerID == 0)
		{
			return ReportTheResult(dictResult, ErrorID.Player_GetAttr_No_Player_ID, LogID);
		}

		// 取得所有的玩家身上的 NPC 資料
		string strCommand = string.Format("select ID, NPCID, PlayerID, LV, Exp from a_npc where PlayerID = {0}", PlayerID);
		List<List<object>> listDBREsult = UseDB.GameDB.DoQueryCommand(strCommand);
		List<string> listID = new List<string>();
		for (int Index = 0; Index < listDBREsult.Count; Index++)
		{
			// 取得資料
			Dictionary<string, object> dictData = new Dictionary<string, object>();
			CopyDBListToDict(dictData, listDBREsult, Index, "ID", "NPCID", "PlayerID", "LV", "Exp");
			ClientAction.AddClientAction(dictResult, ClientActionID.NPC_Update, dictData);
			// 記錄 ID
			listID.Add(listDBREsult[Index][0].ToString());
		}
		// 更新位置
		ClientAction.AddClientAction(dictResult, ClientActionID.NPC_POS, listID);
		// 回傳結果
		return ReportTheResult(dictResult, ErrorID.Success, LogID);
	}

	#endregion
}
