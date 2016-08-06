using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameSceneManager : MonoBehaviour {


    public static bool isControllerConnected;
    public Text fpsText;
    private GameObject GameCanvas;
    private int frames = 60;

    private int hello;
    void Awake () {

        Init();
        IsControlledConnected();
        LoadEnvironment();


    }

    void Init()
    {
        GameCanvas = GameObject.FindWithTag("Canvas/GameCanvas");
    }


    void Update () {

        //TODO temp
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        frames++;
        if (frames % 60 == 0)
        {
            fpsText.text = "" + (int)(1.0f / Time.deltaTime);

            frames = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AIMovmentHandler.TOGGLE = !AIMovmentHandler.TOGGLE;
        }



        // if (mapping)
        // {
        //     MapController();
        // } else {
        //     if (Input.GetKeyDown("joystick 1 button " + hello)) {
        //         Logger.Log("Hello World!");
        //     }
        // }



    }

    // bool mapping = true;
    // void MapController()
    // {
    //     for (int i = 0; i < 20; i++) {
    //         if (Input.GetAxis("Z") == 0)            {
    //             if (Input.GetKeyDown("joystick 1 button " + i)) {
    //                 hello = i;
    //                 Logger.Log("Map To => " + i);
    //             }
    //         }else{
    //             Logger.Log("Press choose another button");
    //         }

    //     }



    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         mapping = false;
    //     }
    // }

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
        _rTransform.position = new Vector3(0,0,1); 
        _rTransform.localScale = new Vector3(1, 1, 0);
        _rTransform.offsetMin = new Vector2(0, 0);
        _rTransform.offsetMax = new Vector2(0, 0);
    }



}
