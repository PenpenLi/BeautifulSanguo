using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public interface ISQL
{
	// 做查詢
	void DoCommand(string strCommand);

	// 做連線
	List<List<object>> DoQueryCommand(string strCommand);
}
