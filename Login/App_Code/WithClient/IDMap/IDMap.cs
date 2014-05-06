using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// 取得描述
public class IDMap
{
    // 利用 ID 去取得描述的中文字
    public static string GetEnumAttribute(ErrorID reportType)
    {
        var members = typeof(ErrorID).GetMember(reportType.ToString());
        var attributes = members[0].GetCustomAttributes(typeof(ErrorIDMapAttribute), false);
        var description = ((ErrorIDMapAttribute)attributes[0]).Description;
        return description;
    }

    // 利用 ID 去取得描述的中文字
    public static string GetEnumAttribute(ClientActionID reportType)
    {
        var members = typeof(ClientActionID).GetMember(reportType.ToString());
        var attributes = members[0].GetCustomAttributes(typeof(ClientActionIDMapAttribute), false);
        var description = ((ClientActionIDMapAttribute)attributes[0]).Description;
        return description;
    }
}
