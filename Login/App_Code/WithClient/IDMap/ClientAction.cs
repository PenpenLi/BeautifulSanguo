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

    #region 流程類

    // 切換到新手流程
    public static void ToNewPlayer(Dictionary<string, object> dictResult)
    {
        AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.ToNewPlayer, ""));
    }

    // 切換到登入流程
    public static void ToLogin(Dictionary<string, object> dictResult)
    {
        AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.ToLogin, ""));
    }

    #endregion

    #region Player 類

    // 更新遊戲幣
    public static void MoneyUpdate(Dictionary<string, object> dictResult, object Money)
    {
        AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Playe_Money, Money));
    }

	// 更新商城幣
    public static void CoinUpdate(Dictionary<string, object> dictResult, object Coin)
    {
        AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_Coin, Coin));
    }

    // 更新 LV
    public static void LVUpdate(Dictionary<string, object> dictResult, object LV)
    {
        AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_LV, LV));
    }

    // 更新 Exp
    public static void ExpUpdate (Dictionary<string, object> dictResult, object Exp)
    {
        AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_Exp, Exp));
    }

    // 更新 Name
    public static void NameUpdate(Dictionary<string, object> dictResult, object Name)
    {
        AddClientAction(dictResult, new KeyValuePair<ClientActionID, object>(ClientActionID.Player_Name, Name));
    }

    #endregion

}