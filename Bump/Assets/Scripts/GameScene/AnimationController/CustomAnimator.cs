using UnityEngine;
using System.Collections;

public class CustomAnimator : MonoBehaviour {

	#region--- PUBLIC MEMEBERS----
	public Sprite[] sprites;

	[HideInInspector]
	public bool _bumperActive;
	[HideInInspector]
	public bool _triggerHit;
	[HideInInspector]
	public bool reverse;
	[HideInInspector]
	public int index = 0;
	#endregion--- PUBLIC MEMEBERS----

	private bool _animating = false; 

	void Update(){

	}

	public void  AnimateBumper(float rate)
	{

		_animating = true; 
		if (reverse == false)
		{
			index += (int)rate;
			_bumperActive = true; 
		}
		else
		{
			if (index > 0)
			{
				index -= (int)rate;
				_bumperActive = true; 
			} else
			{
				reverse = false;
				_triggerHit = false;
				_bumperActive = false;
				_animating = false; 
			}
		}


		index = index % sprites.Length;
		if (index < 0)
		{
			index = 0;
		}
		GetComponent<SpriteRenderer>().sprite = sprites[index];


		if (index >= sprites.Length - 1)
		{

			reverse = true;

		}
	}

	public void ResetIndex(){
		index = 0; 
		GetComponent<SpriteRenderer>().sprite = sprites[0];
	}
}
