// Author : dandanshih
// Date : 2014/5/5

using System;
using System.Collections.Generic;

// 取得 Client 的行為
public class ClientAction
{
    //-------------------------------------------------------------
    // 做 Client 的行為定義
    static void AddClientAction(Dictionary<string, object> dictResult, object oClientAction)
    {
        if (dictResult.ContainsKey("ClientAction") == false)
        {
            dictResult["ClientAction"] = new List<object>();
        }
        List<object> listClientAction = dictResult["ClientAction"] as List<object>;
        listClientAction.Add(oClientAction);
    }
    //-------------------------------------------------------------
	// 做呼叫的動作
	public static void AddClientAction(Dictionary<string, object> dictResult, string ActionID, object Args = null)
	{
		// 先取得 Client Action
		Type eType = typeof(ClientActionID);
		System.Reflection.FieldInfo FieldInfo = eType.GetField(ActionID);
		object data = FieldInfo.GetRawConstantValue();
		ClientActionID CAID = (ClientActionID)data;
		// 加入 Client Action
		AddClientAction(dictResult, CAID, Args);
	}
	public static void AddClientAction(Dictionary<string, object> dictResult, ClientActionID CAID, object Args = null)
	{
		// 加入 Client Action
		AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(CAID, Args));
	}

	//#region 流程類

	//// 切換到新手流程
	//public static void ToNewPlayer(Dictionary<string, object> dictResult)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.ToNewPlayer, ""));
	//}

	//// 切換到登入流程
	//public static void ToLogin(Dictionary<string, object> dictResult)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.ToLogin, ""));
	//}

	//#endregion

	//#region Player 類

	//// 更新遊戲幣
	//public static void MoneyUpdate(Dictionary<string, object> dictResult, object Money)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Playe_Money, Money));
	//}

	//// 更新商城幣
	//public static void CoinUpdate(Dictionary<string, object> dictResult, object Coin)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_Coin, Coin));
	//}

	//// 更新 LV
	//public static void LVUpdate(Dictionary<string, object> dictResult, object LV)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_LV, LV));
	//}

	//// 更新 Exp
	//public static void ExpUpdate (Dictionary<string, object> dictResult, object Exp)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_Exp, Exp));
	//}

	//// 更新 Name
	//public static void NameUpdate(Dictionary<string, object> dictResult, object Name)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_Name, Name));
	//}

	//#endregion

	//// 更新 Client 端的 NPC
	//public static void UpdateNPC(Dictionary<string, object> dictResult, object NPC)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.NPC_Update, NPC));
	//}

	//// 更新 NPC 的順序
	//public static void UpdateNPCPos(Dictionary<string, object> dictResult, object NPCPos)
	//{
	//	AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.NPC_Update, NPCPos));
	//}

}
