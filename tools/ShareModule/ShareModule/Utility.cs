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

    public static string GetNewLine()
    {
        return Environment.NewLine;
    }
    //--------------------------------------------------------
	// 動態取得屬性
	//--------------------------------------------------------
	// 取得屬性
	public static object GetValueByName(object Source, string Name)
	{
		return Source.GetType().GetProperties().Single(pi => pi.Name == Name).GetValue(Source, null);
	}

	// 設定屬性
	public static void SetValueByName(object Source, string Name, object Value)
	{
		Source.GetType().GetProperties().Single(pi => pi.Name == Name).SetValue(Source, Value, null);
	}

	//--------------------------------------------------------
	// 資料結構的屬性 Cpoy(第一層)
	//--------------------------------------------------------
	// 從資料結構屬性的 Copy
	public static void DeepClone<T>(T Target, T Source, string strIgnore)
	{
		List<string> listArgs = new List<string>();
		listArgs.Add(strIgnore);
		DeepClone<T>(Target, Source, listArgs);
	}
	public static void DeepClone<T>(T Target, T Source, List<string> listIgnore = null)
	{
		if (listIgnore == null)
		{
			listIgnore = new List<string>();
		}
		// 取得所有的屬性
		Type allType = Source.GetType();
		foreach (var prop in allType.GetProperties())
		{
			// 不是屬性就不做 copy
			if (prop.MemberType != System.Reflection.MemberTypes.Property)
				continue;
			// 不能讀也不 Copy
			if (prop.CanRead == false)
				continue;
			if (prop.CanWrite == false)
				continue;
			if (prop.IsSpecialName == true)
				continue;
			string strName = prop.Name;
			// 如果不需要 Copy 也不做 Copy
			if (listIgnore.Contains(strName) == true)
				continue;
			// 把資料寫過去
			object Value = prop.GetValue(Source, null);
			if (Value == null)
				continue;
			SetValueByName(Target, strName, Value);
		}
	}

	//--------------------------------------------------------
	// 檔案系統類的操作
	//--------------------------------------------------------
    public static void CopyFile(string strSource, string strDest)
    {
        System.IO.File.Copy(strSource, strDest, true);
    }
	// Check the File Exits
	public static bool IsFileExist(string strFilename)
	{
		return System.IO.File.Exists(strFilename);
	}

	// 建立目錄
	public static bool MakeDir(string DirPath)
	{
		if (Directory.Exists(DirPath) == true)
			return true;
		DirectoryInfo DirInfo = Directory.CreateDirectory(DirPath);
		return true;
	}

    public static string ReadFile(string strFilename, Encoding eEncode = null)
    {
        if (IsFileExist(strFilename) == false)
            return "";
        if (eEncode == null)
            eEncode = Encoding.ASCII;
        FileStream file = new FileStream(strFilename, FileMode.Open, FileAccess.Read);
        StreamReader sr = new StreamReader(file, eEncode);
        string strResult = sr.ReadToEnd();
        sr.Close ();
        file.Close ();
        return strResult;
    }

	// 寫入檔案
    public static void WriteFile(string strFilename, string Data, bool IsReset = false)
	{
        if (IsReset == true)
            ResetFile(strFilename);
        FileStream file = new FileStream(strFilename, FileMode.Append);
		StreamWriter sw = new StreamWriter(file);
		sw.WriteLine(Data);
		sw.Close();
		file.Close();
	}

    // 清除檔案內容
    public static void ResetFile(string strFilename)
    {
        FileStream file = new FileStream(strFilename, FileMode.Create);
        file.Close();
    }

    // 利用系統開檔
    public static void SystemOpenFile(string strFilename)
    {
        System.Diagnostics.Process.Start(strFilename);
    }

	//--------------------------------------------------------
	// 網頁類的操作
	//--------------------------------------------------------
	// 從網頁抓取資料
	public static string HttpGet(string strURL, string strEncode = "big5")
	{
        strURL = Utility.Strip(strURL);
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

    // 更新次數
    static int m_UpdateTime = 300;
    public static void HttpToFile(string strURL, string strFilename, bool IsOverWrite=true)
    {
        if (IsOverWrite == false)
        {
            if (IsFileExist(strFilename) == true)
                return;
        }
        // 休息一下
        System.Threading.Thread.Sleep(m_UpdateTime);
        // Get from http
        string strData = HttpGet(strURL);
        // write it to the file
        WriteFile(strFilename, strData);
    }
}
