using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

/// <summary>
/// WebPost 的摘要描述
/// </summary>
public class WebPost
{
    // 完成會呼叫的 FUNCTION 格式
    public delegate void CompleteCallback(int errorCode, object result, object userState);
    // 回應的相關資料
    private class CReqState
    {
        public HttpWebRequest request;
        public object userState=null;
        public event CompleteCallback callback;

        public void finishcallback(int errorCode, object result)
        {
			if (this.callback != null)
				this.callback(errorCode, result, userState);
			else
				System.Console.WriteLine(result.ToString());
        }
    }

    // Abort the request if the timer fires.
    private static void TimeoutCallback(object state, bool timedOut)
    {
        // 如果超時
        if (timedOut)
        {
            HttpWebRequest request = state as HttpWebRequest;
            if (request != null)
            {
                request.Abort();
            }
        }
    }

    // 要統一做處理
    static void FinishWebRequest(IAsyncResult result)
    {
        CReqState asyncState = (CReqState)result.AsyncState;
        HttpWebResponse cRsp = (HttpWebResponse)asyncState.request.EndGetResponse(result);
        StreamReader cStmRdr = new StreamReader(cRsp.GetResponseStream());
        string output = string.Empty;
        output = cStmRdr.ReadToEnd();
        cStmRdr.Close();
		asyncState.finishcallback(0, output);
    }

    public static void _PostHttp (string strConnectURL, string strContent, CompleteCallback callback=null)
    {
        // 產生連線
        HttpWebRequest cWebReq = (HttpWebRequest)WebRequest.Create(strConnectURL);
        // 設定連線內容
        cWebReq.AllowAutoRedirect = true;
        cWebReq.ServicePoint.Expect100Continue = true;
        cWebReq.Method = "POST";
        cWebReq.ContentLength = strContent.Length;
        cWebReq.ContentType = "application/json; charset=utf-8";
        // 寫入參數
        System.IO.StreamWriter sw = new System.IO.StreamWriter(cWebReq.GetRequestStream());
        sw.Write(strContent);
        sw.Close();
        CReqState myRequestState = new CReqState();
        myRequestState.request = cWebReq;
        if (callback != null)
            myRequestState.callback += callback;
        // 丟進去 Queue 裏等
        IAsyncResult result = cWebReq.BeginGetResponse(new AsyncCallback(FinishWebRequest), myRequestState);
        // 做等待的動作
        ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myRequestState, 5000, true);
        //try
        //{
        //    // 寫入參數
        //    System.IO.StreamWriter sw = new System.IO.StreamWriter(cWebReq.GetRequestStream());
        //    sw.Write(strContent);
        //    sw.Close();
        //    CReqState myRequestState = new CReqState();
        //    myRequestState.request = cWebReq;
        //    if (callback != null)
        //        myRequestState.callback += callback;
        //    // 丟進去 Queue 裏等
        //    IAsyncResult result = cWebReq.BeginGetResponse(new AsyncCallback(FinishWebRequest), myRequestState);
        //    // 做等待的動作
        //    ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), myRequestState, 1000, true);
        //}
        //catch (System.Net.WebException webexception)
        //{
        //    string strMsg = webexception.Status.ToString();
        //}
    }

    // 做 Post 上去的動作
    public static void PostHttp(string strURL, string strMethodName, Dictionary<string, object> dictArgs, CompleteCallback callback)
    {
        // 連線網址
        string strConnectURL = string.Format("{0}/{1}", strURL, strMethodName);
        // 參數內容
        Dictionary<string, object> dictResult = new Dictionary<string, object>();
        dictResult["jsonInfo"] = JsonConvert.SerializeObject (dictArgs);
		string strContent = JsonConvert.SerializeObject(dictResult);
        // 呼叫做處理
        _PostHttp(strConnectURL, strContent, callback);
    }
}
