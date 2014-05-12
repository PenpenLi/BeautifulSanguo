// Author : dandanshih
// Date : 2014/5/12

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// 做 Json 的動作
using Newtonsoft.Json;
// 做 IO 的動作
using System.IO;

// 單一表單
public class StaticTable
{
	#region 變數區
	// 表單資料
	Dictionary<string, Dictionary<string, string>> m_dictData = new Dictionary<string, Dictionary<string, string>>();
	// 檔案名稱
	string m_strFilename="";
	// 欄位資料
	List<string> m_listColumn = new List<string>();
	// key 資料
	List<string> m_listKey = new List<string>();
	#endregion
	// 建構子
	public StaticTable(string strFilename)
	{
		LoadFromFile(strFilename);
	}

	// 從檔案把資料做載入的動作
	void LoadFromFile(string strFilename)
	{
		// 做開檔動作
		string strContents = File.ReadAllText(strFilename);
		// 記錄檔案名稱
		m_strFilename = strFilename;
		// 資料清除
		m_dictData.Clear();
		m_listColumn.Clear();
		m_listKey.Clear();
		// 記錄表單名稱
		List<object> listData = JsonConvert.DeserializeObject<List<object>>(strContents);
		m_listKey = JsonConvert.DeserializeObject<List<string>>(listData[0].ToString());
		m_listColumn = JsonConvert.DeserializeObject<List<string>>(listData[1].ToString());
		m_dictData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>> (listData[2].ToString());
	}

	// 取得欄位名稱
	public List<string> GetColumns()
	{
		return m_listColumn;
	}

	// 取得 Key
	public List<string> GetKeys()
	{
		return m_listKey;
	}

	// 利用 Key / Value 取得資料
	public string Get (string strKey, string strColumn)
	{
		if (m_dictData.ContainsKey(strKey) == false)
			return "";
		if (m_dictData[strKey].ContainsKey(strColumn) == false)
			return "";
		return m_dictData[strKey][strColumn];
	}
}

// 表單管理器
public class StaticTableMgr
{
	// 對應表
	public static Dictionary<string, StaticTable> m_dictMap = new Dictionary<string, StaticTable>();

	// 讀檔案
	public static StaticTable ReadTable(string strFilename)
	{
		if (m_dictMap.ContainsKey(strFilename) == true)
		{
			return m_dictMap[strFilename];
		}
		StaticTable Table = new StaticTable(strFilename);
		m_dictMap[strFilename] = Table;
		return m_dictMap[strFilename];
	}

	// 把 Cache 清掉
	public static void ClearCache(string strTableName="")
	{
		if (strTableName == "")
		{
			m_dictMap.Clear();
		}
		else
		{
			if (m_dictMap.ContainsKey(strTableName) == false)
				return;
			m_dictMap.Remove(strTableName);
		}
	}
}
