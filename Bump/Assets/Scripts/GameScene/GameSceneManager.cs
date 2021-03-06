﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameSceneManager : MonoBehaviour {


    public static int GAME_TIME = 0;
    public static int TOTAL_PLAYERS = 0;
    public static List<EntityMovementHandler> Players = new List<EntityMovementHandler>();

    public CameraController _cameraController;
    public static bool isControllerConnected;
    public Text fpsText;

    private CustomInputManager _customInputManager;
    private GameObject GameCanvas;
    private GameObject _environment;
    private int frames = 60;

    private int hello;
    void Awake () {

        Init();
        //IsControlledConnected();
        LoadEnvironment();
    }

    void Init()
    {
        GameCanvas = GameObject.FindWithTag("Canvas/GameCanvas");
        _environment = GameObject.FindWithTag("Environment");
        _customInputManager = GetComponent<CustomInputManager>();
        _cameraController = Camera.main.GetComponent<CameraController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update () {

        //TODO temp

        GAME_TIME = (int)Time.realtimeSinceStartup;
        frames++;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Application.isEditor == false)
            {
                Application.Quit();
            } else
            {
                //EditorApplication.isPlaying = false;
            }
        }


        if (frames % 60 == 0)
        {
            fpsText.text = "" + (int)(1.0f / Time.deltaTime);

            frames = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AIMovementHandler.TOGGLE = !AIMovementHandler.TOGGLE;
        }


        if (CustomInputManager.GetInputEventPress == InputEvent.GameInputEventPress.RESET || Input.GetKeyDown(KeyCode.K))
        {
            ResetGameManager();
            Logger.Log("Game Reseted!", LoggerType.EVENT);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



    bool IsControlledConnected()
    {
        string[] joyStickNames = Input.GetJoystickNames();

        for (int i = 0; i < joyStickNames.Length; i++)
        {
            if (joyStickNames[i] != "")
            {
                isControllerConnected = true;
                Debug.Log(joyStickNames[i] + " is connected");
            }
        }
        return isControllerConnected;
    }

    public void LoadEnvironment()
    {
        GameObject environment = Instantiate(Resources.Load("Environment/Frozen"), Vector3.zero, Quaternion.identity) as GameObject;
        environment.name = "Frozen";
        RectTransform _rTransform = environment.GetComponent<RectTransform>();
        environment.transform.parent = GameCanvas.transform;
        _rTransform.position = new Vector3(0, 0, 1);
        _rTransform.localScale = new Vector3(1, 1, 0);
        _rTransform.offsetMin = new Vector2(0, 0);
        _rTransform.offsetMax = new Vector2(0, 0);
    }


    private void ResetGameManager()
    {
        TOTAL_PLAYERS = 0;
        Players.Clear();
        GAME_TIME = 0;
        isControllerConnected = false;

    }


    public GameObject GetGameCanvas {
        get { return GameCanvas; }
    }


    public CustomInputManager CustomInputManager
    {
        get  {return _customInputManager; }
    }
}
