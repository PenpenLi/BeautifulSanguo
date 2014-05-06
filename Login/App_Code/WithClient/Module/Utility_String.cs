// 撰寫歷史記錄的格式
// [時間] [修改人] [修改內容]
// [2014/03/04] [dandanshih] 修正 IsDigit 變成 INT 64 的格式
// [2014/03/04] [dandanshih] 新增 IsFloat 的格式

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
    #region DB 相關的操作
    public static string TranslateDBString(string strInput)
    {
        return strInput.Replace("'", "''");
    }

    // 取得時間
    public static string GetDBDateTime(System.DateTime? dtTime = null)
    {
        if (dtTime == null)
            dtTime = System.DateTime.Now;
        return string.Format("{0:yyyy-MM-dd hh:mm:ss}", dtTime);
    }
    #endregion

    // 取得檔案名稱
	public static string GetFilename (string strTotalFilename)
	{
		string strResult = "";
		for (int Index = strTotalFilename.Length - 1; Index > 0; Index--)
		{
			if (strTotalFilename[Index] == '/' || strTotalFilename[Index] == '\\')
				break;
			strResult += strTotalFilename[Index];
		}
		return Reverse(strResult) ;
	}

	// 取得檔案路徑
	public static string GetFilePath(string strTotalFilename)
	{
		return strTotalFilename.Substring(0, strTotalFilename.IndexOf(GetFilename(strTotalFilename)));
	}

	//--------------------------------------------------------
	// 字串處理
	//--------------------------------------------------------
	// 做打亂的動作
	public static List<T> SuffleList<T>(List<T> listInput)
	{
		Random random = new Random();
		List<T> listOutput = new List<T>();
		foreach (T Value in listInput)
		{
			listOutput.Insert(random.Next(listOutput.Count), Value);
		}
		return listOutput;
	}

	// 把 callstack 印出來
	public static string GetCallStack()
	{
		var l_CurrentStack = new System.Diagnostics.StackTrace(true);
		return l_CurrentStack.ToString();
	}

    //------------------------------------------------------------
	// 判定是不是都是數字
	public static bool IsDigit(char source)
	{
		string strResult = source.ToString();
		return IsDigit (strResult);
	}
	public static bool IsDigit(string source)
	{
		Int64 number;
		bool Res = Int64.TryParse(source, out number);
		return Res;
	}

    //------------------------------------------------------------
    // 判定是不是浮點數
    public static bool IsFloat(string strSource)
    {
        Double number;
        bool Res = Double.TryParse(strSource, out number);
        return Res;
    }
    //------------------------------------------------------------
    // 找尋字串, -1 為找不到
	public static int Find(string source, string findstring)
	{
		return source.IndexOf(findstring);
	}

	// 回頭找字串, -1 為找不到
	public static int rFind(string source, string findstring)
	{
		return source.LastIndexOf(findstring);
	}

	// 做字串的切割 token 動作 char[] seps = 	' ', '.', ','} 
	// 要支援 default value char[] = {'\n', '\t', ' '}
	public static List<string> Split(string source, char[] seps)
	{
		List<string> listResult = new List<string>();
		string[] res = source.Split(seps);
		for (int Index = 0; Index < res.Length; Index++)
		{
			listResult.Add(res[Index]);
		}
		return listResult;
	}
	public static List<string> Split(string source, string seps)
	{
		return Split(source, seps.ToCharArray());
	}

	// 做字串的去二邊動作
	public static string Strip(string source, char[] seps = null)
	{
		if (seps == null)
		{
			char[] defaultseps = { '\n', '\t', ' ' };
			return source.Trim(defaultseps);
		}
		else
			return source.Trim(seps);
	}

	public static string Strip(string source, string seps)
	{
		return Strip(source, seps.ToCharArray());
	}

	// 反轉字串
	public static string Reverse(string strInput)
	{
		char[] charArray = strInput.ToCharArray();
		Array.Reverse(charArray);
		return new string(charArray);
	}

	//-------------------------------------------------------------
	// 做字串的取代
	public static string Replace(string source, string discard, string need)
	{
		return source.Replace(discard, need);
	}

	// 子字串取得 (支援 -1 的取得 )
	public static string SubString(string source, int index, int counter = 0)
	{
		// 如果沒有指定數量就 copy 到底
		if (counter == 0)
			return source.Substring(index);
		// 處理 counter 負號的處理
		else if (counter < 0)
		{
			counter = source.Length + counter - index;
			if (counter < 0)
				counter = 0;
			return source.Substring(index, counter);
		}
		// 正常數量的處理
		else
			return source.Substring(index, counter);
	}

	// 做分割的動作
	public static List<string> Partition(string source, string token)
	{
		List<string> listResult = new List<string>();
		string strTmp = "";
		// 如果沒有 partition 的 token 就直接回傳結果
		int pos = source.IndexOf(token);
		if (pos == -1)
			return listResult;
		// 做字串的切割
		strTmp = SubString(source, 0, pos);	// 從開始 copy 到分隔
		listResult.Add(strTmp);
		strTmp = SubString(source, pos + token.Length);		// 從分隔 copy 到結尾
		listResult.Add(strTmp);
		return listResult;
	}
}
