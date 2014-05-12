using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using NPOI;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class ExcelExUtility
{
	// 產生一個 Font
	public static IFont CreateFontFrom(IWorkbook WorkBook, IFont Font)
	{
		IFont Result = WorkBook.CreateFont();
		Utility.DeepClone<IFont>(Result, Font, new List<string> (){"Index"});
		return Result;
	}

	public static ICellStyle CreateCellStyle (IWorkbook WorkBook, ICellStyle CellStyle)
	{
		ICellStyle Result = WorkBook.CreateCellStyle();
		Utility.DeepClone<ICellStyle>(Result, CellStyle, new List<string>() { "Index" });
		return Result;
	}
}
// 常用的常數
public partial class Const
{
	public static int MaxRow = 65536;
	public static int MaxColumn = 256;
}

// Excel 的欄位屬性
public enum ExcelProperty : int
{
	// 顏色
	Font_Color = 1,
	// 改變數值
	Change_Value = 2,
}

// Excel 可以使用的顏色
public class ExcelColor
{
	public enum eColor : int
	{
		// 紅色
		Color_Red = 1,
		// 綠色
		Color_Green = 2,
		// 黑色
		Color_Black = 3,
		// 白色
		Color_White = 4,
		// 黃色
		Color_Yellow = 5,
		// 藍色
		Color_Blue = 6,
	}
	public static short GetColor(object Value)
	{
		return GetColor((eColor)Value);
	}

	public static short GetColor(eColor Color)
	{
		// 紅
		if (Color == eColor.Color_Red)
		{
			return (short)HSSFColor.RED.index;
		}
		// 綠
		if (Color == eColor.Color_Green)
		{
			return (short)HSSFColor.GREEN.index;
		}
		// 黑
		if (Color == eColor.Color_Black)
		{
			return (short)HSSFColor.BLACK.index;
		}
		// 白
		if (Color == eColor.Color_White)
		{
			return (short)HSSFColor.WHITE.index;
		}
		// 藍
		if (Color == eColor.Color_Blue)
		{
			return (short)HSSFColor.BLUE.index;
		}
		// 黃
		if (Color == eColor.Color_Yellow)
		{
			return (short)HSSFColor.YELLOW.index;
		}
		return 0;
	}
}
