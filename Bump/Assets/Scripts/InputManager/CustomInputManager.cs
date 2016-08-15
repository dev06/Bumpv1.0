using UnityEngine;
using System.Collections;

public class CustomInputManager : MonoBehaviour
{
	public const int TotalButtons = 20;

	private string[] ControllerNames;
	private InputEvent.GameInputEventPress _inputEventPress;
	private InputEvent.GameInputEventHold _inputEventHold;

	public static int LIGHT_ATTACK;
	public static int BOOST;
	public static int CONFIRM;
	public static int SPAWN;
	public static int RESET;

	public bool isMappingController;

	private bool isControllerConnected;
	private bool isUsingController;
	private bool mappedController;

	private int[] _controllerIndex;
	private int _controllerCurrentIndex;



	void Awake()
	{
		IsControllerConnected();
		MapDefaultController(Constants.MAP_DEFAULT_CONTROLS);
	}

	void Start()
	{
		_controllerIndex = new int[TotalButtons];
	}

	void Update()
	{
		RegisterPressInput();
		RegisterHoldInput();
		if (Input.GetKeyDown(KeyCode.M))
		{
			isMappingController = true;
			Logger.Log("Mapping Controller");
		}
		if (isMappingController)
		{
			MapController(1);
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
			mappedController = true;
			isMappingController = false;
			LIGHT_ATTACK = _controllerIndex[0];
			BOOST = _controllerIndex[1];
			SPAWN = _controllerIndex[2];
			RESET = _controllerIndex[3];
			Logger.Log("LIGHT_ATTACK => " + LIGHT_ATTACK);
			Logger.Log("BOOST  => " + BOOST);
			Logger.Log("SPAWN => " + SPAWN);
			Logger.Log("RESET => " + RESET);

		}
	}

	private void RegisterHoldInput()
	{
		if (mappedController)
		{
			if (isUsingController)
			{
				if (Input.GetKey("joystick " + 1 + " button " + LIGHT_ATTACK))
				{
					_inputEventHold = InputEvent.GameInputEventHold.LIGHT_ATTACK;
				} else
				{
					_inputEventHold = InputEvent.GameInputEventHold.DEAD;
				}
			}
		}
	}
	private void RegisterPressInput()
	{
		if (mappedController)
		{
			if (isUsingController)
			{
				if (Input.GetKeyDown("joystick " + 1 + " button " + BOOST))
				{
					_inputEventPress = InputEvent.GameInputEventPress.BOOST;

				} else if (Input.GetKeyDown("joystick " + 1 + " button " + SPAWN))
				{
					_inputEventPress = InputEvent.GameInputEventPress.SPAWN;
				} else if (Input.GetKeyDown("joystick " + 1 + " button " + RESET))
				{
					_inputEventPress = InputEvent.GameInputEventPress.RESET;
				}
				else
				{
					_inputEventPress = InputEvent.GameInputEventPress.DEAD;
				}
			}
		}
	}





	private bool IsControllerConnected()
	{
		string[] joyStickNames = Input.GetJoystickNames();
		ControllerNames = new string[joyStickNames.Length];
		for (int i = 0; i < joyStickNames.Length; i++)
		{
			if (joyStickNames[i] != "")
			{
				isControllerConnected = true;
				isUsingController = true;
				ControllerNames[i] = joyStickNames[i];
				Logger.Log(joyStickNames[i] + " is connected", LoggerType.INFO);
			}
		}
		return isControllerConnected;
	}

	private void MapDefaultController(bool map)
	{
		if (map)
		{
			for (int i = 0; i < ControllerNames.Length; i++)
			{
				switch (ControllerNames[i])
				{
				case "Controller (Xbox 360 Wireless Receiver for Windows)":
				{
					Logger.Log("Controller Mapped For Xbox 360 / PS3", LoggerType.INFO);
					LIGHT_ATTACK = 2;
					BOOST = 0;
					mappedController = true;
					break;
				}

				case "ipega media gamepad controller":
				{
					Logger.Log("Controller Mapped For ipega media gamepad controller", LoggerType.INFO);
					LIGHT_ATTACK = 2;
					BOOST = 0;
					SPAWN = 7;
					RESET = 5;
					mappedController = true;
					break;
				}

				case "ipega Bluetooth Gamepad   ":
				{
					Logger.Log("Controller Mapped For ipega media gamepad controller", LoggerType.INFO);
					LIGHT_ATTACK = 3;
					BOOST = 0;
					SPAWN = 7;
					RESET = 5;
					mappedController = true;
					break;
				}
				}
			}
		}
	}


	public InputEvent.GameInputEventPress GetInputEventPress
	{
		get
		{
			return _inputEventPress;
		}
	}

	public InputEvent.GameInputEventHold GetInputEventHold
	{
		get
		{
			return _inputEventHold;
		}
	}

	public bool IsUsingController
	{
		get
		{
			return isUsingController;
		}
	}
}


public class InputEvent
{
	public enum GameInputEventPress
	{
		DEAD,
		BOOST,
		DOUBLE_BOOST,
		SPAWN,
		RESET,
	}

	public enum GameInputEventHold
	{
		DEAD,
		LIGHT_ATTACK,
	}
}
