// Author : dandanshih
// Desc : 帳號相關的程式都需要放在這裏處理
// [problem] 可以使用 exception 來簡化修改
// Author : dnadanshih
// Date : 2014/5/9
// Desc : 這裏主要是放伍將相關的

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.Security;
using Newtonsoft.Json;

public partial class GameService : System.Web.Services.WebService
{
	// 取得玩家的伍將清單
	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public string Member_GetMember(string strJson)
	{
		return "";
	}
}

