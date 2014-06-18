// 撰寫歷史記錄的格式
// [時間] [修改人] [修改內容]
// [2014/01/24] [dandanshih] 只有修改資料才會寫入
// [2014/03/04] [dandanshih] 底層判定數字, 浮點數的處理
// [2014/03/05] [dandanshih] 新增公式的讀取
// [2014/03/12] [dandanshih] 修正 CreateCellStyle 和 CreateFont 的寫法

//#define BACK_UP

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

// Excel 相關的 API 處理
using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

// 導入新的 Json 壓法
using Newtonsoft.Json;

// 單一個 Excel 資料
public class SingleExcelEx
{
	// 取得關鍵字
	public static string GetKeyWord(string strRow, string strColumn)
	{
		return string.Format("{0}_{1}", strRow, strColumn);
	}
	// Excel Name
	string m_ExcelName = "";
	// Sheet Name
	string m_SheetName = "";
	//--------------------------------------------------------
	// 欄位資料
	List<string> m_listColumn = new List<string>();
	// 取得欄位名稱
	public List<string> GetColumns()
	{
		return m_listColumn;
	}
	//--------------------------------------------------------
	// key 資料
	List<string> m_listKey = new List<string>();
	// 取得 Key
	public List<string> GetKeys()
	{
		return m_listKey;
	}

	//--------------------------------------------------------
	// 表單資料
	Dictionary<string, Dictionary<string, string>> m_dictData = new Dictionary<string, Dictionary<string, string>>();
	// 建構子
	public SingleExcelEx(string ExcelFilename, string SheetName)
	{
		LoadFromExcelSheet(ExcelFilename, SheetName);
	}
	// 做載入表單的動作
	public void LoadFromExcelSheet(string ExcelFilename, string SheetName)
	{
		m_ExcelName = ExcelFilename;
		m_SheetName = SheetName;
		// 清除資料
		m_dictData.Clear();
		m_listColumn.Clear();
		m_listKey.Clear();
		// 取得資料
		using (FileStream fs = new FileStream(ExcelFilename, FileMode.Open))
		{
			// 取得該 Excel 的資料流
			IWorkbook WorkBook = new HSSFWorkbook(fs);
			// 取得 Excel 的該頁
			ISheet Sheet = WorkBook.GetSheet(SheetName);
			// Copy the ColumnName
			IRow FirstRow = Sheet.GetRow(0);
			// 取得欄位名稱
			for (int Index = 0; Index < Const.MaxColumn; Index++)
			{
				if (FirstRow.GetCell(Index) == null)
					break;
				string strCellData = FirstRow.GetCell(Index).StringCellValue;
				if (strCellData == "")
					break;
				m_listColumn.Add(strCellData);
			}

			// Build the Map : 第一列(column), 第二列(註解), 第三列(區隔色)
			for (int Index = 3; Index < Const.MaxRow; Index++)
			{
				IRow Row = Sheet.GetRow(Index);
				if (Row == null)
					break;
				if (Row.GetCell(0) == null)
					break;
				// 建立 KeyMap
				string strKey = Row.Cells[0].ToString();
				if (strKey == "")
					break;
				m_listKey.Add(strKey);
				// 把資料做轉換對應
				Dictionary<string, string> dictInfo = new Dictionary<string, string>();
				for (int ColumnIndex = 0; ColumnIndex < m_listColumn.Count; ColumnIndex++)
				{
					string strCellData = "";
					ICell Cell = Row.GetCell(ColumnIndex);
					if (Cell != null)
					{
                        // [2014/03/05] [dandanshih] 新增公式的讀取
                        // 如果是公式, 而且公式計算後是數值的話
                        if (Cell.CellType == CellType.FORMULA && Cell.CachedFormulaResultType == CellType.NUMERIC)
                        {
                            // 就去抓數值欄位
                            strCellData = Cell.NumericCellValue.ToString();
                        }
                        else
    						strCellData = Cell.ToString();
					}
					dictInfo[m_listColumn[ColumnIndex]] = strCellData;
				}
				m_dictData[strKey] = dictInfo;
			}

			// 清除
			WorkBook = null;
			Sheet = null;
		}
	}

	//---------------------------------------------------------------
	// 取得欄位
	public string Get(string strKey, string strColumn)
	{
		if (m_dictData.ContainsKey(strKey) == false)
			return "";
		if (m_dictData[strKey].ContainsKey(strColumn) == false)
			return "";
		return m_dictData[strKey][strColumn];
	}
	public int GetByInt(string strKey, string strColumn)
	{
		string strValue = Get(strKey, strColumn);
		if (strValue == "")
			return 0;
		return System.Convert.ToInt32(strValue);
	}
	public double GetByDouble(string strKey, string strColumn)
	{
		string strValue = Get(strKey, strColumn);
		if (strValue == "")
			return 0;
		return System.Convert.ToDouble(strValue);
	}

	//---------------------------------------------------------------
	// 做塞入的動作
	public void Add(string strKey)
	{
		Dictionary<string, string> dictResult = new Dictionary<string, string>();
		foreach (string strColumn in m_listColumn)
		{
			dictResult[strColumn] = "";
		}
		m_dictData[strKey] = dictResult;
		// set the Key
		Set(strKey, m_listColumn[0], strKey, true);
		// keep the Key
		m_listKey.Add(strKey);
	}

	// 設定欄位
	public void Set(string strKey, string strColumn, object strValue, bool IsAddKey=false)
	{
        // 如果不是加 KEY 而且不在 KEY 列表中, 就算是錯誤
		if (IsAddKey == false && m_listKey.Contains(strKey) == false)
			return;
		if (m_listColumn.Contains(strColumn) == false)
			return;
		m_dictData[strKey][strColumn] = strValue.ToString();
		SetProperty(strKey, strColumn, ExcelProperty.Change_Value, strValue);
	}

	// 設定屬性
	Dictionary<string, Dictionary<ExcelProperty, object>> m_dictProperty = new Dictionary<string, Dictionary<ExcelProperty, object>>();
	public void SetProperty(string strKey, string strColumn, ExcelProperty Data, object Value)
	{
		string strKeyWord = GetKeyWord(strKey, strColumn);
		if (m_dictProperty.ContainsKey(strKeyWord) == false)
		{
			m_dictProperty[strKeyWord] = new Dictionary<ExcelProperty, object>();
		}
		m_dictProperty[strKeyWord][Data] = Value;
	}

	// 根據資料做存檔的動作
	public void SaveToJson(string strPath = "")
	{
        string strFilename = "";
        if (strPath == "")
        {
            strFilename = Utility.GetFilePath(m_ExcelName) + "../Login/GameData/" + m_SheetName + ".txt";
        }
        else
        {
            strFilename = Utility.GetFilePath(m_ExcelName) + strPath + "/" + m_SheetName + ".txt";
        }
		// 轉存成 Json 的格式
		List<object> listResult = new List<object>();
		listResult.Add(m_listKey);
		listResult.Add(m_listColumn);
		listResult.Add(m_dictData);
		// 做寫檔的動作
		FileStream file = new FileStream(strFilename, FileMode.Create);//產生檔案
		StreamWriter File = new StreamWriter(file, Encoding.ASCII);
		File.Write(JsonConvert.SerializeObject(listResult));
		File.Close();
		file.Close();
	}
//	public void Save(bool IsUseBackup=true)
	public void Save()
	{
		if (m_dictProperty.Count == 0)
			return;
		// 重新再做一次載入
		ExcelExMgr.instance().ReloadAll();
		using (FileStream fs = new FileStream(m_ExcelName, FileMode.Open))
		{
			IWorkbook WorkBook = new HSSFWorkbook(fs);
			fs.Close();
			ISheet Sheet = WorkBook.GetSheet(m_SheetName);
			// 把資料寫回去
			for (int RowIndex = 0; RowIndex < m_listKey.Count; RowIndex++)
			{
				int RealRowIndex = RowIndex + 3;
				string strKey = m_listKey[RowIndex];
				IRow Row = Sheet.GetRow(RealRowIndex);
				if (Row == null)
				{
					Row = Sheet.CreateRow(RealRowIndex);
				}
				for (int ColumnIndex = 0; ColumnIndex < m_listColumn.Count; ColumnIndex++)
				{
					string strColumn = m_listColumn[ColumnIndex];
					string strKeyWord = GetKeyWord (strKey, strColumn);
					// 如果沒有相關資料也不需要變更
					if (m_dictProperty.ContainsKey(strKeyWord) == false)
						continue;
					// 如果是空的就先產生
					ICell Cell = Row.GetCell(ColumnIndex);
					if (Cell == null)
					{
						Cell = Row.CreateCell(ColumnIndex);
					}
					// 設定數值
                    string strValue = m_dictData[strKey][strColumn].ToString();
                    // 數字
                    if (Utility.IsDigit(strValue) == true)
                    {
                        Row.GetCell(ColumnIndex).SetCellValue(System.Convert.ToInt32 (strValue));
                    }
                    // 浮點數
                    else if (Utility.IsFloat (strValue) == true)
                    {
                        Row.GetCell(ColumnIndex).SetCellValue(System.Convert.ToDouble(strValue));
                    }
                    //// 浮點數
                    //else if (Utility.IsPercentFloat(strValue) == true)
                    //{
                    //    Row.GetCell(ColumnIndex).SetCellValue(System.Convert.ToDouble(strValue));
                    //}
                    else
                    {
                        Row.GetCell(ColumnIndex).SetCellValue(strValue);
                    }
					// 設定屬性
					SetPerpertyToExcel(strKey, strColumn, Cell, WorkBook); 
				}
			}
			string strOutName = m_ExcelName;
#if BACK_UP
			strOutName += ".auto.xls";
#endif
			FileStream file = new FileStream(strOutName, FileMode.Create);//產生檔案
			WorkBook.Write(file);
			file.Close();
			// 清除
			WorkBook = null;
			Sheet = null;
		}
		// 清除掉屬性設定
		m_dictProperty.Clear();
	}

    // 取得 Cell 的文字顏色
    private short GetCellFontColor(IWorkbook WorkBook, ICell Cell)
    {
        return Cell.CellStyle.GetFont(WorkBook).Color;
    }

    // 取得相似的 CellStyle 來做使用
    // [problem] 應該還是會爆吧?!
    private ICellStyle GetCellStyleByFontColor(IWorkbook WorkBook, ICell Cell, short sColor)
    {
        ////-------------------------------
        //// 找舊的
        //for (short Index = 0; Index < 255; Index++)
        //{
        //    ICellStyle iCellStyle = WorkBook.GetCellStyleAt(Index);
        //    IFont iFont = iCellStyle.GetFont(WorkBook);
        //    if (sColor == iFont.Color)
        //        return iCellStyle;
        //}
        ////-------------------------------
        //// 產生新的 CellStyle
        //ICellStyle CellStyle = ExcelExUtility.CreateCellStyle(WorkBook, Cell.CellStyle);
        //// 產生新的 Font
        //IFont font = ExcelExUtility.CreateFontFrom(WorkBook, Cell.CellStyle.GetFont(WorkBook));
        //// 設定 Font
        //font.Color = sColor;
        //CellStyle.SetFont(font);
        //// 傳回結果
        //return CellStyle;
        //-------------------------------
        // 產生新的 CellStyle
        ICellStyle CellStyle = ExcelExUtility.CreateCellStyle(WorkBook, Cell.CellStyle);
        // 產生新的 Font
        IFont font = WorkBook.FindFont(400, sColor, 240, "新細明體", false, false, 0, 0);
        if (font == null)
        {
            font = ExcelExUtility.CreateFontFrom(WorkBook, Cell.CellStyle.GetFont(WorkBook));
            font.Color = sColor;
        }
        // 設定 Font
        CellStyle.SetFont(font);
        // 傳回結果
        return CellStyle;
    }

    // 把屬性設定到 Excel 上面
    private void SetPerpertyToExcel(string strKey, string strColumn, ICell Cell, IWorkbook WorkBook)
    {
        string strKeyWord = GetKeyWord(strKey, strColumn);
        // 沒有就不需要做設定
        if (m_dictProperty.ContainsKey(strKeyWord) == false)
            return;
        // 把屬性一個一個設定
        Dictionary<ExcelProperty, object> dictProperty = m_dictProperty[strKeyWord];
        if (dictProperty.ContainsKey(ExcelProperty.Font_Color))
        {
            // 取得顏色
            short sColor = ExcelColor.GetColor(dictProperty[ExcelProperty.Font_Color]);
            // 如果同色就不需要改變
            short sTargetColor = GetCellFontColor (WorkBook, Cell);
            if (sColor == sTargetColor)
                return;
            // 找看看有沒有舊的可以用
            ICellStyle CellStyle = GetCellStyleByFontColor(WorkBook, Cell, sColor);
            Cell.CellStyle = CellStyle;
        }
    }

}

// Excel 管理器
public class ExcelExMgr : Singleton <ExcelExMgr>
{
	// 管理有開啟過的 Excel 表單
	private Dictionary<string, SingleExcelEx> m_dictMap = new Dictionary<string, SingleExcelEx>();
	// 建構子的功能
	private ExcelExMgr()
	{
	}

	public void ReloadAll()
	{
		m_dictMap.Clear();
	}

	// 取得所有的 SheetName
	public List<string> GetAllSheetName(string ExcelFilename)
	{
		// Get All Sheet Name
		FileStream fs = new FileStream(ExcelFilename, FileMode.Open);
		// 取得該 Excel 的資料流
		IWorkbook WorkBook = new HSSFWorkbook(fs);
		List<string> listSheetName = new List<string>();
		for (int Index = 0; Index < WorkBook.NumberOfSheets; Index++)
		{
			listSheetName.Add(WorkBook.GetSheetName(Index));
		}
		fs.Close();
		WorkBook = null;
		return listSheetName;
	}


	public Dictionary<string, SingleExcelEx> GetExcelSheet(string ExcelFilename)
	{
		Dictionary<string, SingleExcelEx> dictResult = new Dictionary<string, SingleExcelEx>();
		// return the result
		List<string> listSheetName = GetAllSheetName(ExcelFilename);
		for (int Index = 0; Index < listSheetName.Count; Index++)
		{
			string strSheetName = listSheetName[Index];
			dictResult[strSheetName] = GetExcelSheet(ExcelFilename, strSheetName);
		}
		return dictResult;
	}

	// 取得資料表
	public SingleExcelEx GetExcelSheet(string ExcelFilename, string SheetName)
	{
		// 判定檔案是否存在
		if (Utility.IsFileExist(ExcelFilename) == false)
			return null;
		// 判定檔案類型是否正確
		if (ExcelFilename.Contains(".xls") == false)
			return null;
		// 找詢記憶體
		string strKeyWord = string.Format("{0}_{1}", ExcelFilename, SheetName);
		if (m_dictMap.ContainsKey(strKeyWord) == false)
		{
			SingleExcelEx Result = new SingleExcelEx(ExcelFilename, SheetName);
			m_dictMap[strKeyWord] = Result;
		}
		return m_dictMap[strKeyWord];
	}
}
