// Author : dandanshih
// 道具

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
	// 測試生成道具
	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Test_Item_CreateItem()
	{
		return Item_CreateItem("");
	}

	// 生成道具
	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Item_CreateItem(string strJson)
	{
		// 先寫一筆 Log
		int LogID = ReportDBLog("Item_CreateItem", strJson);
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

		// 檢查參數
		if (dictInfo.ContainsKey("ItemID") == false)
		{
			return ReportTheResult(dictResult, ErrorID.Item_CreateItem_No_ItemID, LogID);
		}
		int ItemID = System.Convert.ToInt32(dictInfo["ItemID"]);
		int iNumber = 1;
		if (dictInfo.ContainsKey("Number") == true)
		{
			iNumber = System.Convert.ToInt32(dictInfo["Number"]);
		}
		if (iNumber <= 0)
		{
			return ReportTheResult(dictResult, ErrorID.Item_CreateItem_Number_Negative, LogID);
		}

		// 檢查道具是否存在
		if (ItemTable.instance().HasKey(ItemID) == false)
		{
			return ReportTheResult(dictResult, ErrorID.Item_Not_Exist, LogID);
		}

		// 取得最大的數量
		int MaxNumber = 1;
		Dictionary<string, string> dictItemInfo = ItemTable.instance().Get(ItemID);
		if (dictItemInfo.ContainsKey("MaxNumber") == true)
		{
			MaxNumber = System.Convert.ToInt32(dictItemInfo["MaxNumber"]);
		}

		// 統一都走可以堆疊的路
		List<List<object>> listDBResult = null;
		string strCommand = string.Format("select ItemUID, PlayerID, ItemID, Number from a_backpack where PlayerID={0} and ItemID={1}", PlayerID, ItemID);
		listDBResult = UseDB.GameDB.DoQueryCommand(strCommand);
		// 直接更新的數量
		Dictionary<int, int> dictUpdateNumber = new Dictionary<int, int>();
		// 剩下要直接產生的數量
		int LeaveNumber = iNumber;
		for (int Index = 0; Index < listDBResult.Count; Index++)
		{
			// 如果最大數量就不需要再堆上去
			int ItemNumber = System.Convert.ToInt32(listDBResult[Index][3]);
			if (ItemNumber >= MaxNumber)
				continue;
			// 把東西堆滿
			int ItemUID = System.Convert.ToInt32(listDBResult[Index][0]);
			// 如果還不夠
			if (ItemNumber + LeaveNumber > MaxNumber)
			{
				
			}
		}

		// 回寫成功的結果
		return ReportTheResult(dictResult, ErrorID.Success, LogID);
	}
	
	// [勿直接呼叫] 產生數量
	Dictionary<string, object> _CreateItemToPlayer (int PlayerID, int ItemID, int iNumber)
	{
		return null;
	}

	// [勿直接呼叫] 更新數量
	Dictionary<string, object> _UpdateItemToPlayer (int PlayerID, int ItemDBID, int ItemID, int iNumber)
	{
		return null;
	}

}
