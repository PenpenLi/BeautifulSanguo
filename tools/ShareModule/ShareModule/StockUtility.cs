// 撰寫歷史記錄的格式
// [時間] [修改人] [修改內容]
// [2014/02/26] [dandanshih] 加入網路 Log 下載的功能
// [2014/03/04] [dandanshih] 加入分析了漲跌幅的數字
// [2014/03/05] [dandanshih] 修正沒有漲跌幅會當的問題

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StockUtility
{
	// 計算手續費
	public static int GetCharge(double SellPrice, double Number = 1.0, double Cheap = 0.5)
	{
		double dResult = SellPrice * 1.425 * Cheap * Number;
		int iResult = System.Convert.ToInt32(dResult);
		iResult = Math.Max(iResult, 20);
		return iResult;
	}

	// 計算抽的稅
	public static int GetTax(double SellPrice, double Number = 1.0)
	{
		return System.Convert.ToInt32(SellPrice * 3 * Number + 0.5);
	}

	// 計算營收多少
	public static int GetStockResult(double BuyPrice, double SellPrice, double Number = 1, double Cheap = 0.5)
	{
		// 買的手續費
		int BuyCharge = GetCharge(BuyPrice, Number);
		// 賣的手續費
		int SellCharge = GetCharge(SellPrice, Number);
		// 抽的稅金
		int Tax = GetTax(SellPrice, Number);
		int iTotalPay = BuyCharge + SellCharge + Tax;
		// 計算結果
		int iDiff = System.Convert.ToInt32((SellPrice - BuyPrice) * 1000 * Number - iTotalPay);
		// 傳回結果
		return iDiff;
	}

	// 取得 Html Token
	static List<string> ChangeHtmlToken(string strLine)
	{
		List<string> listResult = new List<string>();
		strLine = Utility.Strip(strLine);
		string strTmp = "";
		foreach (char Word in strLine)
		{
			// Start
			if (Word == '<')
			{
				if (strTmp != "")
				{
					listResult.Add(strTmp);
				}
				strTmp = "";
				strTmp += Word;
				continue;
			}
			// End
			if (Word == '>')
			{
				strTmp += Word;
				listResult.Add(strTmp);
				strTmp = "";
				continue;
			}
			// Normal
			strTmp += Word;
		}
		if (strTmp != "")
		{
			listResult.Add(strTmp);
			strTmp = "";
		}
		return listResult;
	}

	// 從網路取得股價
	public static Dictionary<string, object> GetStockPriceFromHttp (object StockID)
	{
		string strStockID = StockID.ToString();
        // 建立目錄
        Utility.MakeDir("HttpTmp");
        // 建出檔案名稱
        string strLogName = "HttpTmp/" + strStockID + ".tmp";
        Utility.ResetFile(strLogName);
		// Wait
		System.Threading.Thread.Sleep(300);
		// Get from Network
		string strTargetURL = "http://tw.stock.yahoo.com/q/q?s=" + strStockID;
		string strData = Utility.HttpGet(strTargetURL);
        Utility.WriteFile(strLogName, strData);
		// Parser
		// 做檔案處理
		string strStartToken = "/pf/pfsel?stocklist=" + strStockID;
		string strEndToken = "/q/ts?s=" + strStockID;
		// 開檔
		System.Text.Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
        System.IO.StreamReader File = new System.IO.StreamReader(strLogName, encode);
		string strLine = "";
		List<string> listToken = null;
		while ((strLine = File.ReadLine()) != null)
		{
			strLine = Utility.Strip(strLine);
			if (strLine.Length < 3)
				continue;
			if (strLine.Contains(strEndToken) == true)
				break;
			if (strLine.Contains(strStartToken) == true)
			{
				listToken = new List<string>();
				continue;
			}
			// 還沒有使用到的 Token 也不需要使用
			if (listToken == null)
				continue;
			// 切開看看
			List<string> listHtmlToken = ChangeHtmlToken(strLine);
			foreach (string HtmlToken in listHtmlToken)
			{
				if (Utility.IsDigit(HtmlToken[0]) == true)
				{
					listToken.Add(HtmlToken);
				}
				else if (HtmlToken.Contains("▽") == true)
				{
					listToken.Add(HtmlToken);
				}
				else if (HtmlToken.Contains("△") == true)
				{
					listToken.Add(HtmlToken);
				}
				// 漲/跌停處理
				else if (HtmlToken.Contains("▲") == true)
				{
					listToken.Add(HtmlToken);
				}
				else if (HtmlToken.Contains("▼") == true)
				{
					listToken.Add(HtmlToken);
				}
				// 沒交易處理
				else if (HtmlToken.Contains("－") == true)
				{
					listToken.Add(HtmlToken);
				}
			}
		}
		File.Close();
		// 把資料存出去
		Dictionary<string, object> dictResult = new Dictionary<string, object>();
		dictResult["UpdateTie"] = listToken[0];	// 最後更新時間
		dictResult["NowPrice"] = listToken[1];	// 可能是 －
		dictResult["Result"] = listToken[4];	// 要處理－▼▲▽△
		dictResult["Number"] = listToken[5];	// 要處理 1,200 這種
        // 處理 Result 變成純數字
        string strToken = listToken[4];
        if (strToken.IndexOf("－") != -1)
        {
            dictResult["ResultNumber"] = 0;
        }
        else if (strToken.IndexOf("△") != -1)
        {
            dictResult["ResultNumber"] = Utility.SubString(strToken, 1);
        }
        else if (strToken.IndexOf("▲") != -1)
        {
            dictResult["ResultNumber"] = Utility.SubString(strToken, 1);
        }
        else if (strToken.IndexOf("▽") != -1)
        {
            dictResult["ResultNumber"] = "-" + Utility.SubString(strToken, 1);
        }
        else if (strToken.IndexOf("▼") != -1)
        {
            dictResult["ResultNumber"] = "-" + Utility.SubString(strToken, 1);
        }
        else
        {
            dictResult["ResultNumber"] = 0;
        }

		return dictResult;
	}
}
