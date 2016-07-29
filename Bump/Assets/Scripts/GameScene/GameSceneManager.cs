using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameSceneManager : MonoBehaviour {


    public static bool isControllerConnected;

    public Text fpsText;



    private int frames = 60;


    void Awake () {

        IsControlledConnected(); 
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


        //Debug.Log(Input.IsJoystickPreconfigured(Input.GetJoystickNames()[0]));
    }

    /// <summary>
    /// Checks to see if any controllers are connected
    /// </summary>
    /// <returns></returns>
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
}
