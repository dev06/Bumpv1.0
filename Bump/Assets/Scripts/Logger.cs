using UnityEngine;
using System.Collections;

public class Logger : MonoBehaviour {

	private static bool printLog = true;

	public static void Log(Vector3 content)
	{
		if (printLog) {
			Debug.Log("<color=blue>" + content + "</color>");
		}

	}



	public static void Log(object content)
	{
		if(printLog)
		{
			Debug.Log("<color=red>" + content + "</color>");
		}
	}
}
