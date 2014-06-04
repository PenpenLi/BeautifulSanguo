// Author : dandanshih
// Desc : 使用定義

using System;

[AttributeUsage(AttributeTargets.Field)]
class ErrorIDMapAttribute : Attribute
{
    public string Description { get; set; }

    public ErrorIDMapAttribute(string Description)
    {
        this.Description = Description;
    }

    public override string ToString()
    {
        return this.Description.ToString();
    }
}

/// <summary>
/// ErrorIDMap 的摘要描述
/// </summary>
public enum ErrorID : int
{

    #region 共用的區域

    [ErrorIDMapAttribute("成功")]
    Success = 0,
    [ErrorIDMapAttribute("參數 Json 解不開")]
    Json_Format_Error = 1,
    [ErrorIDMapAttribute("沒有帶 SessionKey")]
    No_SessionKey = 2,
    [ErrorIDMapAttribute("Session Key 錯誤, 根本不存在")]
    SessionError = 3,
    [ErrorIDMapAttribute("Session Key 逾時")]
    SessionTimeout = 4,
	[ErrorIDMapAttribute("帳號被停權中")]
	Account_Stoped = 5,
	[ErrorIDMapAttribute("NPC ID 錯誤")]
	NPC_ID_Error = 6,
	[ErrorIDMapAttribute("Item ID 錯誤")]
	Item_Not_Exist = 7,

    #endregion

    #region Agent_Account_Create 的錯誤

    [ErrorIDMapAttribute("帳號重覆")]
    Agent_Account_Create_Duplicate_Account = 101,

    #endregion

    [ErrorIDMapAttribute("沒有帳號資料")]
    Check_Account_No_Account = 10001,
    [ErrorIDMapAttribute("沒有密碼資料")]
    Check_Account_No_Password = 10002,
    [ErrorIDMapAttribute("帳號錯誤")]
    Check_Account_No_Such_Account = 10003,
    [ErrorIDMapAttribute("密碼錯誤")]
    Check_Account_No_Such_Password = 10004,

    [ErrorIDMapAttribute("己有角色存在")]
    Account_CreatePlayer_Exist_Player = 10101,

    [ErrorIDMapAttribute("沒有角色編號")]
    Player_GetAttr_No_Player_ID = 10201,

	// Create NPC Error
	[ErrorIDMapAttribute("產生 NPC 的參數出錯")]
	NPC_Create_Args_Error = 10301,

	[ErrorIDMapAttribute("想產道具沒有 ItemID 輸入")]
	Item_CreateItem_No_ItemID = 10401,
	[ErrorIDMapAttribute("產道具輸入負數或是0")]
	Item_CreateItem_Number_Negative = 10402,

}

