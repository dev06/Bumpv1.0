using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour {

	private static bool printAll = true;
	private static bool printLog = true;
	private static bool printEvent = true;
	private static bool printTask = true;
	public static void Log(Vector3 content)
	{
		if (printAll) {
			Debug.Log("<color=blue>" + content + "</color>");
		}
	}



	public static void Log(object o)
	{
		if (printAll)
		{

			Debug.Log(LoggerType.LOG + " => <color=red>" + o + "</color>");

		}
	}

	public static void Log(object o, LoggerType type)
	{
		if (printAll)
		{
			if (type == LoggerType.LOG && printLog)
			{
				Debug.Log(type + " => <color=red>" + o + "</color>");
			} else if (type == LoggerType.EVENT  && printEvent)
			{
				Debug.Log(type + " => <color=blue>" + o + "</color>");
			} else if (type == LoggerType.TASK && printTask)
			{
				Debug.Log(type + " => <color=green>" + o + "</color>");
			}
		}
	}



}


public enum LoggerType
{
	LOG,
	TASK,
	EVENT,
}







