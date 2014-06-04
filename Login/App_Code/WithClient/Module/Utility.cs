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
	// 把二個 Dictionary Copy 在一起
	//--------------------------------------------------------
	public static Dictionary<string, T> DictUnionDict<T> (Dictionary<string, T> dict0, Dictionary<string, T> dict1)
	{
		return dict0.Union(dict1).ToDictionary(k => k.Key, v => v.Value);
	}
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

    // 取得 Random 值
    public static System.Random GetRandom()
    {
        // 用GUID的HashCode來當亂數種子帶入, 在極短的時間內一樣是無法有效的取得不同的亂數種子
        return new Random(System.Guid.NewGuid().GetHashCode());
    }

    // 做亂數取選一個的動作
    public static T GetRange<T>(List<T> listInput)
    {
        if (listInput.Count == 0)
            return default(T);
        Random RandomSeed = GetRandom();
        int Index = RandomSeed.Next(0, listInput.Count);
        return listInput[Index];
    }

}
