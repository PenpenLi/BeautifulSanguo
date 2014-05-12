// 使用來做呼叫的東西
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// use Sqlite
using System.Data;  // 不引用會出現 <錯誤 CS0012: 型別 'System.Data.Common.DbConnection' 是定義在未參考的組件中>
using System.Data.SQLite;

// SQLite 工具集
public class SQLiteUtility
{
    // 做跑的結果
    public static void DB_SendCommand(string strDB, string strCommand)
    {
        // new the connection
        SQLiteConnection objConn = new SQLiteConnection(strDB);
        objConn.Open();
        // new a command
        SQLiteCommand objCmd = objConn.CreateCommand();
        objCmd.CommandText = strCommand;
        objCmd.ExecuteNonQuery();
        objCmd = null;
        // finish
        objConn.Close();
        objConn = null;
    }

    // 做查詢動作
    public static List<List<object>> DB_SendQueryCommand(string strDB, string strCommand)
    {
        List<List<object>> listResult = new List<List<object>>();
        // new the connection
        SQLiteConnection objConn = new SQLiteConnection(strDB);
        objConn.Open();
        // new a command
        SQLiteCommand objCmd = objConn.CreateCommand();
        objCmd.CommandText = strCommand;
        SQLiteDataReader Data = objCmd.ExecuteReader();
        while (Data.Read())
        {
            List<object> listDBRow = new List<object>();
            for (int Index = 0; Index < Data.FieldCount; Index++)
            {
                listDBRow.Add(Data.GetValue(Index).ToString());
            }
            listResult.Add(listDBRow);
        }
        Data.Close();
        Data = null;
        // finish
        objConn.Close();
        objConn = null;
        return listResult;
    }
}
