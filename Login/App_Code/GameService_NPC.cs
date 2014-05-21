// Author : dandanshih
// Desc : NPC 相關的都存放在這裏

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
	#region 產生NPC

	// 測試產生一隻 NPC
	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Test_CreateNPC()
	{
		Dictionary<string, object> dictResult = new Dictionary<string, object>();
		dictResult["SessionKey"] = "SessionKey:dandan";
		dictResult["PlayerID"] = 1;
		dictResult["NPCID"] = 100;
		return JsonConvert.SerializeObject (_NPC_CreateNPC(dictResult));
	}

	// 產生一隻 NPC 在玩家身上
	Dictionary<string, object> _NPC_CreateNPC(object PlayerID, object NPCID, object LV)
	{
		Dictionary<string, object> dictInfo = new Dictionary<string, object>();
		dictInfo["PlayerID"] = PlayerID;
		dictInfo["NPCID"] = NPCID;
		dictInfo["LV"] = LV;
		return _NPC_CreateNPC(dictInfo);
	}

	Dictionary<string, object> _NPC_CreateNPC (Dictionary<string, object> dictInfo)
	{
		//---------------------------------------------------------
		// 先寫一筆 Log
		int LogID = ReportDBLog("_NPC_CreateNPC", JsonConvert.SerializeObject (dictInfo));
		Dictionary<string, object> dictResult = null;
		// 先解析資料
		dictResult = new Dictionary<string, object>();
		//---------------------------------------------------------
		// 取得所需資料
		if (dictInfo.ContainsKey("PlayerID") == false)
		{
			return ReportTheResultDict(dictResult, ErrorID.NPC_Create_Args_Error, LogID);
		}
		int PlayerID = System.Convert.ToInt32(dictInfo["PlayerID"]);
		if (dictInfo.ContainsKey("NPCID") == false)
		{
			return ReportTheResultDict(dictResult, ErrorID.NPC_Create_Args_Error, LogID);
		}
		int NPCID = System.Convert.ToInt32(dictInfo["NPCID"]);
		//---------------------------------------------------------
		// 非所需要參數
		int LV = 1;
		if (dictInfo.ContainsKey("LV") == true)
		{
			LV = System.Convert.ToInt32(dictInfo["LV"]);
		}
		//---------------------------------------------------------
		// 檢查資料正確性
		if (NPCTable.instance().HasKey(NPCID) == false)
		{
			return ReportTheResultDict(dictResult, ErrorID.NPC_ID_Error, LogID);
		}

		//---------------------------------------------------------
		// 產生結果
		string strCommand = string.Format("insert into a_npc (NPCID, PlayerID, LV) values ({0}, {1}, {2})" + UseDB.GETID, NPCID, PlayerID, LV);
		List<List<object>> listDBResult = UseDB.GameDB.DoQueryCommand(strCommand);
		int NPCDBID = System.Convert.ToInt32(listDBResult[0][0]);
		Dictionary<string, object> dictNPC = _GetNPCAttrFromDB (NPCDBID);

		//---------------------------------------------------------
		// 更新 Client
		ClientAction.AddClientAction(dictResult, ClientActionID.NPC_Update, dictNPC);

		// 回報結果
		return ReportTheResultDict(dictResult, ErrorID.Success, LogID);
	}

	#endregion

	#region 升級 NPC LV

	#endregion

	#region 升級 NPC degree

	#endregion

	#region 刪除 NPC

	#endregion

}
