// Author : dandanshih
// Date : 2014/5/12
// Desc : Config 表單

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class ConfigTable : Singleton<ConfigTable>
{
    private ConfigTable()
    {
        LoadFromFile();
    }

    Dictionary<string, string> m_dictConfigMap = new Dictionary<string, string>();
    // 先從資料表讀進來
    void LoadFromFile()
    {
        // 清掉設定檔
        m_dictConfigMap.Clear();
        string strTableName = "config";
        StaticTable Table = StaticTableMgr.ReadTable(strTableName);
        // 一個一個值讀進來
        List<string> listKey = Table.GetKeys();
        foreach (var Key in listKey)
        {
            string strKey = Table.Get(Key, "ID");
            string strValue = Table.Get(Key, "Value");
            m_dictConfigMap[strKey] = strValue;
        }
        // 把 Cache 給清掉
        StaticTableMgr.ClearCache(strTableName);
    }

    // 取值
    public string Get(string strKey)
    {
        if (m_dictConfigMap.ContainsKey(strKey) == false)
            return "";
        return m_dictConfigMap[strKey];
    }
}


