using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class Config
{
	public Dictionary<string, string> m_dictInfo;
	public Config(Dictionary<string, string> dictInfo)
	{
		m_dictInfo = dictInfo;
	}

	public string Get(string strKey)
	{
		if (m_dictInfo.ContainsKey(strKey) == false)
			return "";
		return m_dictInfo[strKey];
	}
}

public class ConfigMgr
{
	public static Config LoadConfig(string strFilename)
	{
		// Read from file
		if (Utility.IsFileExist(strFilename) == false)
			return null;
		Dictionary<string, string> dictResult = new Dictionary<string, string>();
		System.IO.StreamReader sr = new StreamReader(strFilename);
		while (sr.Peek() > 0)
		{
			string strLine = sr.ReadLine();
			strLine = Utility.Strip(strLine);
			if (strLine.IndexOf("#") == 0)
				continue;
			if (strLine.IndexOf("=") == -1)
				continue;
			List<string> listToken = Utility.Split(strLine, "=");
			string strKey = Utility.Strip(listToken[0]);
			string strValue = Utility.Strip(listToken[1]);
			dictResult[strKey] = strValue;
		}
		sr.Close();
		// 設定過去
		Config Result = new Config(dictResult);
		return Result;
	}
}

