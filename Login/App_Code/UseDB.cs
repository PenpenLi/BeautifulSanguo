// Author : dandanshih
// Desc: 所使用到的DB

using System;
using System.Collections.Generic;

public class UseDB
{
    #region 帳號 DB
    private static CMySQL _AccountDB = null;
    public static CMySQL AccountDB
    {
        get
        {
            if (_AccountDB == null)
            {
                _AccountDB = new CMySQL("127.0.0.1", "Sanguo", "sa", "koske1984");
            }
            return _AccountDB;
        }
    }
    #endregion

    #region 遊戲 DB
    private static CMySQL _GameDB = null;
    public static CMySQL GameDB
    {
        get
        {
            if (_GameDB == null)
            {
                _GameDB = new CMySQL("127.0.0.1", "Sanguo", "sa", "koske1984");
            }
            return _GameDB;
        }
    }
    #endregion

    #region 遊戲 Log DB
    private static CMySQL _GameLogDB = null;
    public static CMySQL GameLogDB
    {
        get
        {
            if (_GameLogDB == null)
            {
                _GameLogDB = new CMySQL("127.0.0.1", "Sanguo", "sa", "koske1984");
            }
            return _GameLogDB;
        }
    }
    #endregion

}

