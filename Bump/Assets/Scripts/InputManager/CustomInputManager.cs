using UnityEngine;
using System.Collections;

public class CustomInputManager : MonoBehaviour
{
	public const int TotalButtons = 20;

	public static int LIGHT_ATTACK;
	public static int BOOST;
	public static int CONFIRM;

	public bool isMappingController;


	private int[] _controllerIndex;
	private int _controllerCurrentIndex;

	void Start()
	{
		_controllerIndex = new int[TotalButtons];
		
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			isMappingController = true; 
			Logger.Log("Mapping Controller"); 
		}
		if (isMappingController)
		{	
			MapController(1);
		}

		if(Input.GetKeyDown("joystick 1 button " + LIGHT_ATTACK))
		{
			Logger.Log("Light attack pressed!"); 
		}
	}

	private void MapController(int controllerIndex)
	{
		
		for (int i = 0; i < TotalButtons; i++)
		{
			if (Input.GetAxis("Z") == 0)
			{
				if (Input.GetKeyDown("joystick " + controllerIndex + " button " + i))
				{
					_controllerIndex[_controllerCurrentIndex] = i;
					_controllerCurrentIndex++;
				}
			} else {
				Logger.Log(@"Please choose a ""Button"" and not a ""Trigger""");
			}
		}

		if (Input.GetKeyDown(KeyCode.G))
		{

			isMappingController = false;
			LIGHT_ATTACK = _controllerIndex[0];
			BOOST = _controllerIndex[1];
			Logger.Log("LIGHT_ATTACK => " + LIGHT_ATTACK);
			Logger.Log("BOOST  => " + BOOST);
		}

	}
}
