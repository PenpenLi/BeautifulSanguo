// Author : dandanshih
// Date : 2014/5/12
// Desc : NPC 表單

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ItemTable : Singleton<ItemTable>
{
	private ItemTable()
	{
		LoadFromFile();
	}

	Dictionary<string, Dictionary<string, string>> m_dictItem = null;

	// 先從資料表讀進來
	void LoadFromFile()
	{
		if (m_dictItem == null)
			m_dictItem = new Dictionary<string, Dictionary<string, string>>();
		// 做清空的動作
		m_dictItem.Clear();
		List<string> listTable = new List<string> { "item_gem", "item_general"};
		foreach (string strTableName in listTable)
		{
			StaticTable Table = StaticTableMgr.ReadTable(strTableName);
			// 一張一張做載入的動作
			m_dictItem = Utility.DictUnionDict<Dictionary<string, string>>(m_dictItem, Table.GetData());
			// 把 Cache 給清掉
			StaticTableMgr.ClearCache(strTableName);
		}
	}

	// 取得資料
	public Dictionary<string, string> Get(object oKey)
	{
		string strKey = oKey.ToString();
		if (HasKey(oKey) == false)
			return null;
		return m_dictItem[strKey];
	}

	public bool HasKey(object oKey)
	{
		return m_dictItem.ContainsKey(oKey.ToString());
	}

	// 取得 NPC 列表
	public List<string> GetKey()
	{
		return new List<string>(m_dictItem.Keys);
	}
}
