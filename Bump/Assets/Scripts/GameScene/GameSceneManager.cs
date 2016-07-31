using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameSceneManager : MonoBehaviour {


    public static bool isControllerConnected;
    public Text fpsText;
    private GameObject GameCanvas;
    private int frames = 60;


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
    }



}
