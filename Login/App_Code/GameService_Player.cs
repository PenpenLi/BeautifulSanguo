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
        return Player_GetAttr(JsonConvert.SerializeObject(dictResult));
    }
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public string Player_GetAttr(string strJson)
    {
        // 先寫一筆 Log
        int LogID = ReportDBLog("Player_GetAttr", strJson);
        Dictionary<string, object> dictResult = new Dictionary<string, object>();
        string strCommand = "";
        List<List<object>> listDBResult = null;
        // 先解析資料
        //Dictionary<string, object> dictInfo = Json.Deserialize(strJson) as Dictionary<string, object>;
		Dictionary<string, object> dictInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);
        if (dictInfo == null)
        {
            return ReportTheResult(dictResult, ErrorID.SessionError, LogID);
        }
        string SessionKey = dictInfo["SessionKey"].ToString();

        // 先轉 Session Key
        Dictionary<string, object> dictAccount = GetAccountInfoBySessionKey(SessionKey);
        if (dictAccount == null)
        {
            return ReportTheResult(dictResult, ErrorID.SessionError, LogID);
        }
        int PlayerID = System.Convert.ToInt32(dictAccount["PlayerID"]);
        if (PlayerID == 0)
        {
            return ReportTheResult(dictResult, ErrorID.Player_GetAttr_No_Player_ID, LogID);
        }

        return ReportTheResult(dictResult, ErrorID.Success, LogID);
    }

    #endregion
}
