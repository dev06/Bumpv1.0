using UnityEngine;
using System.Collections;

public class Instantiator : MonoBehaviour {

	public GameObject Player;
	public GameObject AI;
	public GameObject GameCanvas;

	private CameraController _cameraController; 
	void Awake () {
		Init(); 
		InstantiatePlayer(); 
		//InstantiateAI(6); 
	}

	private void Init()
	{
		_cameraController = Camera.main.GetComponent<CameraController>(); 
	}

	private void InstantiatePlayer()
	{
		GameObject _playerClone = Instantiate(Player, Vector3.zero, Quaternion.identity) as GameObject;
		_playerClone.name = "Player"; 
		_playerClone.GetComponent<MovementHandler>().color = new Color(1,1,1,1); 
		_cameraController.targetTransforms.Add(_playerClone.transform); 
	}



	private void InstantiateAI(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			float offset = 10; 
			float x = Random.Range((-GameCanvas.GetComponent<RectTransform>().rect.width/2) + offset, (GameCanvas.GetComponent<RectTransform>().rect.width/2) - offset);
			float y = Random.Range((-GameCanvas.GetComponent<RectTransform>().rect.height/2) + offset, (GameCanvas.GetComponent<RectTransform>().rect.height/2) - offset);
			GameObject AI_Clone = Instantiate(AI, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
			AI_Clone.GetComponent<AIMovmentHandler>().target = GameObject.Find("Player"); 
			AI_Clone.GetComponent<AIMovmentHandler>()._color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f),Random.Range(0.0f, 1.0f)); 
			_cameraController.targetTransforms.Add(AI_Clone.transform); 
		}
	}	


	void Update()
	{
		if(Input.GetKeyDown(KeyCode.R))
		{
			InstantiateAI(1); 
		}
	}
}
