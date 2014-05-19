// Author : dandanshih
// Date : 2014/5/12
// Desc : NPC 表單

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class NPCTable : Singleton<NPCTable>
{
    private NPCTable()
    {
        LoadFromFile();
    }

    Dictionary<string, Dictionary<string, string>> m_dictNPC = null;

    // 先從資料表讀進來
    void LoadFromFile()
    {
        string strTableName = "npc";
        StaticTable Table = StaticTableMgr.ReadTable(strTableName);
        m_dictNPC = Table.GetData();
        // 把 Cache 給清掉
        StaticTableMgr.ClearCache(strTableName);
    }

	public bool HasKey(object oKey)
	{
		return m_dictNPC.ContainsKey(oKey.ToString());
	}
	
	// 取得 NPC 列表
	public List<string> GetKey()
	{
		return new List<string>(m_dictNPC.Keys);
	}
}
