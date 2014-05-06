using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// MSSQL 的摘要描述
/// </summary>
public class CMSSQL : ISQL
{
    private string m_DB;

    public CMSSQL(string strIP, string strPort, string strDB, string strAccount, string strPassword)
    {
        m_DB = string.Format("Data Source={0},{1};Initial Catalog={2};Persist Security Info=True;User ID={3};Password={4}; Max Pool Size=500", strIP, strPort, strDB, strAccount, strPassword);
    }

    #region 給外部呼叫的 API

    // 做送出 Command 的動作
    public void DoCommand(string strCommand)
    {
        // open the connect
        SqlConnection sqlCon = new SqlConnection(m_DB);
        sqlCon.Open();
        // open a command
        SqlCommand sqlCmd = new SqlCommand(strCommand, sqlCon);
        sqlCmd.ExecuteNonQuery();
        // 做好處理
        sqlCmd.Dispose();
        sqlCon.Close();
    }

    // 做查詢相關的 Command
    public List<List<object>> DoQueryCommand(string strCommand)
    {
        List<List<object>> listResult = new List<List<object>>();
        // open the connect
        SqlConnection sqlCon = new SqlConnection(m_DB);
        sqlCon.Open();
        // open a command
        SqlCommand sqlCmd = new SqlCommand(strCommand, sqlCon);
        // read the date
        using (SqlDataReader reader = sqlCmd.ExecuteReader())
        {
            while (reader.Read())
            {
                List<object> listRow = new List<object>();
                for (int Index = 0; Index < reader.FieldCount; Index++)
                {
                    object oTmp = reader.GetValue(Index);
                    // 特別處理時間相關的東西
                    if (oTmp.GetType() == typeof(System.DateTime))
                        listRow.Add(System.Convert.ToDateTime(oTmp).ToString("yyyy-MM-dd hh:mm:ss"));
                    // 其他轉成時間
                    else
                        listRow.Add(reader.GetValue(Index).ToString());
                }
                listResult.Add(listRow);
            }
        }
        sqlCmd.Dispose();
        sqlCon.Close();
        return listResult;
    }

    #endregion
}

