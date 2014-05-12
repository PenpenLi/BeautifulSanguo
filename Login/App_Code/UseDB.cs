// Author : dandanshih
// Desc: 所使用到的DB

// [History]
// 2014/5/5 加入 MSSQL 的連線版本

using System;
using System.Collections.Generic;

public class UseDB
{
    public static string WebPath = "";
	#region 做 DB 型別的判定
	static bool _IsMySQL = false;
	#endregion

	#region 取得 UID 的字串
	static string _MySQLID = ";SELECT LAST_INSERT_ID();";
	static string _MSSQLID = ";SELECT @@IDENTITY;";
	public static string GETID
	{
		get
		{
			if (_IsMySQL)
				return _MySQLID;
			else
				return _MSSQLID;
		}
	}
	#endregion

	#region 帳號 DB
	private static ISQL _AccountDB = null;
	public static ISQL AccountDB
    {
        get
        {
            if (_AccountDB == null)
            {
				try
				{
					_AccountDB = new CMySQL("127.0.0.1", "Sanguo", "sa", "koske1984");
				}
				catch
				{
					_AccountDB = new CMSSQL("db.08online.rd1.sgt", "51095", "Demo", "sa", "sqlgosmio2749");
					_IsMySQL = false;
				}
            }
            return _AccountDB;
        }
    }
    #endregion

    #region 遊戲 DB
	private static ISQL _GameDB = null;
	public static ISQL GameDB
    {
        get
        {
			try
			{
				_GameDB = new CMySQL("127.0.0.1", "Sanguo", "sa", "koske1984");
			}
			catch
			{
				_GameDB = new CMSSQL("db.08online.rd1.sgt", "51095", "Demo", "sa", "sqlgosmio2749");
				_IsMySQL = false;
			}
			return _GameDB;
        }
    }
    #endregion

    #region 遊戲 Log DB
	private static ISQL _GameLogDB = null;
	public static ISQL GameLogDB
    {
        get
        {
			try
			{
				_GameLogDB = new CMySQL("127.0.0.1", "Sanguo", "sa", "koske1984");
			}
			catch
			{
				_GameLogDB = new CMSSQL("db.08online.rd1.sgt", "51095", "Demo", "sa", "sqlgosmio2749");
				_IsMySQL = false;
			}
			return _GameLogDB;
        }
    }
    #endregion

}

