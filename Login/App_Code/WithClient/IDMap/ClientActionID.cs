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
    [ClientActionIDMapAttribute("切換到新角介面")]
    ToNewPlayer = 1,
    [ClientActionIDMapAttribute("切換到登入介面")]
    ToLogin = 2,
}

