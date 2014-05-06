// Author : dandanshih
// Date : 2014/5/5

using System;
using System.Collections.Generic;

// 取得 Client 的行為
public class ClientAction
{
    // 切換到新手流程
    public static object ToNewPlayer()
    {
        return new KeyValuePair<ClientActionID, string>(ClientActionID.ToNewPlayer, "");
    }

    // 切換到登入流程
    public static object ToLogin()
    {
        return new KeyValuePair<ClientActionID, string>(ClientActionID.ToLogin, "");
    }
}