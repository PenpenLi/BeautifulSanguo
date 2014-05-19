// Author : dandanshih
// Desc : 使用定義

using System;

[AttributeUsage(AttributeTargets.Field)]
class ClientActionIDMapAttribute : Attribute
{
    public string Description { get; set; }

    public ClientActionIDMapAttribute(string Description)
    {
        this.Description = Description;
    }

    public override string ToString()
    {
        return this.Description.ToString();
    }
}

/// <summary>
/// 定義一些常使用的 Client ID 的對應表
/// </summary>
public enum ClientActionID : int
{
    #region 流程控制
    [ClientActionIDMapAttribute("切換到新角介面")]
    ToNewPlayer = 1,
    [ClientActionIDMapAttribute("切換到登入介面")]
    ToLogin = 2,
    #endregion

    #region PlayerAttr
    [ClientActionIDMapAttribute("更新 Money")]
    Playe_Money = 101,
    [ClientActionIDMapAttribute("更新 Coin")]
    Player_Coin = 102,
    [ClientActionIDMapAttribute("更新 LV")]
    Player_LV = 103,
    [ClientActionIDMapAttribute("更新 Exp")]
    Player_Exp = 104,
    [ClientActionIDMapAttribute("更新玩家姓名")]
    Player_Name = 105,
    #endregion

	// 更新 NPC 資料
	[ClientActionIDMapAttribute("NPC資料更新")]
	NPC_Update = 201,
	// 更新 NPC 的順序
	[ClientActionIDMapAttribute("NPC資料順序")]
	NPC_POS = 202,
	// 刪除 NPC 資料
	[ClientActionIDMapAttribute("刪除 NPC 資料")]
	NPC_Delete = 203,
}

