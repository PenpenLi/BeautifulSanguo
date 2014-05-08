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
		string strCommand = "";
		List<List<object>> listDBResult = null;

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
		strCommand = string.Format("select PlayerName, Money, Coin from a_member where PlayerID = {0}", PlayerID);
		listDBResult = UseDB.GameDB.DoQueryCommand(strCommand);
		dictResult["PlayerName"] = listDBResult[0][0];
		dictResult["Money"] = listDBResult[0][1];
		dictResult["Coin"] = listDBResult[0][2];

        return ReportTheResult(dictResult, ErrorID.Success, LogID);
    }

    #endregion
}
