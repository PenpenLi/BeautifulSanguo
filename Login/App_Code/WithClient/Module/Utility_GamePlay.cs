//-- 通用類 --//
using System;
using System.Collections.Generic;
//-- 動態取得屬性 --//
using System.Linq;
// 網頁類, 系統操作
using System.IO;
//-- 網頁類使用 --//
using System.Net;
using System.Text;

public partial class Utility
{
    public static string WebPath = "";

    public static string GetWebPath(string Filename)
    {
        return WebPath + "//" + Filename;
    }
}
