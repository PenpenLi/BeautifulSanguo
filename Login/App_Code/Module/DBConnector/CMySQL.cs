using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
/// <summary>
/// CMySQL 的摘要描述
/// </summary>
public class CMySQL : ISQL
{
    // 連線字串
    string m_strConnect = "";
	public CMySQL(string strIP, string strDB, string strUser, string strPassword)
	{
        m_strConnect = string.Format("server={0};uid={1};pwd={2};database={3};charset=utf8", strIP, strUser, strPassword, strDB);
		MySqlConnection conn = new MySqlConnection(m_strConnect);
		conn.Open();
		conn.Close();
	}

    #region 連線開始關閉功能
    // 做開啟連線的動作
    private MySqlConnection OpenTheConnection()
    {
        MySqlConnection conn = new MySqlConnection(m_strConnect);
        try
        {
            conn.Open();
        }
        catch (MySql.Data.MySqlClient.MySqlException ex)
        {
            switch (ex.Number)
            {
                case 0:
                    Console.WriteLine("無法連線到資料庫.");
                    break;
                case 1045:
                    Console.WriteLine("使用者帳號或密碼錯誤,請再試一次.");
                    break;
            }
        }
        return conn;
    }

    // 做關閉連線的動作
    private void CloseTheConnection(MySqlConnection conn)
    {
        conn.Close();
    }
    #endregion

    // 做查詢
    public void DoCommand(string strCommand)
    {
        MySqlConnection conn = OpenTheConnection();
        MySqlCommand cmd = new MySqlCommand(strCommand, conn);
        int iResult = cmd.ExecuteNonQuery();
        CloseTheConnection(conn);
    }

    // 做連線
    public List<List<object>> DoQueryCommand(string strCommand)
    {
        List<List<object>> listResult = new List<List<object>>();
        // 開連線
        MySqlConnection conn = OpenTheConnection();
        MySqlCommand cmd = new MySqlCommand(strCommand, conn);
        MySqlDataReader myData = cmd.ExecuteReader();
        while (myData.Read())
        {
            List<object> listData = new List<object>();
            for (int Index = 0; Index < myData.VisibleFieldCount; Index++)
            {
                listData.Add(myData.GetValue(Index));
            }
            listResult.Add(listData);
        }
        // 關連線
        CloseTheConnection(conn);
        return listResult;
    }
}