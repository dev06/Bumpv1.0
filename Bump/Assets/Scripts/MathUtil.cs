using UnityEngine;
using System.Collections;

public class MathUtil {


	public static int RangeBetweenTwo(int x, int y)
	{
		return (Random.Range(0,1) == 0) ? x : y; 
	}

	public static float Round(float value)
	{
		return Mathf.Round(value * 100.0f)/100f; 
	}
}
