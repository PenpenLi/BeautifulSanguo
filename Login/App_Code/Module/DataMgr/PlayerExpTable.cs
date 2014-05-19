// Author : dandanshih
// Date : 2014/5/12
// Desc : 玩家經驗值表單

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class PlayerExpTable : Singleton<PlayerExpTable>
{
	private PlayerExpTable()
	{
		LoadFromFile();
	}

	// 玩家等級和經驗值的對應表
	Dictionary<string, int> m_dictLevelExp = new Dictionary<string, int>();
	// 先從資料表讀進來
	void LoadFromFile()
	{
		StaticTable Table = StaticTableMgr.ReadTable("levelexp");
		List<string> listKeys = Table.GetKeys();
		foreach (string strKey in listKeys)
		{
			m_dictLevelExp[strKey] = System.Convert.ToInt32(Table.Get(strKey, "Exp"));
		}
		// 把 Cache 給清掉
		StaticTableMgr.ClearCache("levelexp");
	}

	// 取得該等級需求的經驗值
	public int GetExpByLV(object LV)
	{
		string strLV = LV.ToString();
		// 找不到該等級的經驗
		if (m_dictLevelExp.ContainsKey(strLV) == false)
		{
			return -1;
		}
		return m_dictLevelExp[strLV];
	}
}
