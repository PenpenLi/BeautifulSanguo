// Author : dandanshih

using System;
using System.Collections.Generic;
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
        System.IO.FileInfo FI = new System.IO.FileInfo(Server.MapPath (".") + "\\log4net.config");
        log4net.Config.XmlConfigurator.Configure(FI);
        log4net.LogManager.GetLogger("GameService").ErrorFormat("Test log4net");
    }

    // 做 Client 的行為定義
    void AddClientAction(Dictionary<string, object> dictResult, object oClientAction)
    {
        if (dictResult.ContainsKey("ClientAction") == false)
        {
            dictResult["ClientAction"] = new List<object>();
        }
        List<object> listClientAction = dictResult["ClientAction"] as List<object>;
        listClientAction.Add(oClientAction);
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
//        Dictionary<string, object> dictInfo = Json.Deserialize(jsonInfo) as Dictionary<string, object>;
		Dictionary<string, object> dictInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(strJson);
        Dictionary<string, object> dictResult = new Dictionary<string, object>();
        if (dictInfo.ContainsKey("MethodName") == false)
        {
//            return Json.Serialize (dictResult);
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

    // 統一的錯誤處理
    string ReportTheResult(Dictionary<string, object> dictResult, ErrorID IErrorID, int LogID)
    {
        dictResult["Result"] = IErrorID;
		//ReportDBLog(IDMap.GetEnumAttribute(IErrorID), Json.Serialize(IErrorID), LogID);
		//return Json.Serialize(dictResult);
		ReportDBLog(IDMap.GetEnumAttribute(IErrorID), JsonConvert.SerializeObject (IErrorID), LogID);
        return JsonConvert.SerializeObject(dictResult);
    }

	//// 做輸出的處理
	//Dictionary<string, object> PaserArgs(string strJson, out Dictionary<string, object> dictInfo)
	//{
	//}

}
