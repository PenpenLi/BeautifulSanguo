// 作者:石凌霖
// 日期:2013/1/10
// Desc:所有的 singleton instance
using System;
using System.Reflection;

//-----------------------------------------------------------------
// Use like this
/*
public class Highlander : Singleton<Highlander>
{
	private Highlander()
	{
		Console.WriteLine("There can be only one...");
	}
}
*/

//-----------------------------------------------------------------
// singleton 的實體
public class Singleton<T> where T:class
{
	//-------------------------------------------------------------
	// singleton 的實體
	private static T m_Instance;
	// 做 thread 的 lock 機制
	private static object m_InitLock = new object ();
	
	//-------------------------------------------------------------
	// 取得 instance 的 function
	public static T instance ()
	{
		if (m_Instance == null)
		{
			createInstance ();
		}
		return m_Instance;
	}
	
	//-------------------------------------------------------------
	// 產生實體的動作
	private static void createInstance ()
	{
		lock (m_InitLock)
		{
			if (m_Instance == null)
			{
				Type t = typeof (T);
				
				// Ensure there are no public constructors...
				ConstructorInfo[] ctors = t.GetConstructors ();
				if (ctors.Length > 0)
				{
					throw new InvalidOperationException (String.Format ("{0} has at least one accesible ctor making it impossible to enforce singleton behaviour", t.Name));
				}
				
				// Create an m_Instance via the private constructor
				m_Instance = (T)Activator.CreateInstance (t, true);
			}
		}
	}
	//-------------------------------------------------------------
}