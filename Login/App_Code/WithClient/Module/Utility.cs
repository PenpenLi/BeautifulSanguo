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
	//--------------------------------------------------------
	// 網頁類的操作
	//--------------------------------------------------------
	// 從網頁抓取資料
	public static string HttpGet(string strURL, string strEncode = "big5")
	{
		// 產生連線
		WebRequest WebConnect = WebRequest.Create(strURL);
		// 取得資料
		Stream objStream = WebConnect.GetResponse().GetResponseStream();
		System.Text.Encoding encode = System.Text.Encoding.GetEncoding(strEncode);
		// 讀出資料
		StreamReader objReader = new StreamReader(objStream, encode);
		// 記下結果
		string strOut = objReader.ReadToEnd();
		// 清除資料
		objReader.Dispose();
		objStream.Dispose();
		WebConnect = null;
		// 傳回結果
		return strOut;
	}
}
