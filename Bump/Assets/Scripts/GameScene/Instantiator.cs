using UnityEngine;
using System.Collections;

public class Instantiator : MonoBehaviour {

	private GameObject Player;
	private GameObject AI;
	private GameObject EntityIconPrefab;

	public GameObject GameCanvas;
	public GameObject UICanvas;

	private CameraController _cameraController;
	void Awake () {
		Init();
		InstantiatePlayer();
		//InstantiateAI(6);
	}

	private void Init()
	{
		_cameraController = Camera.main.GetComponent<CameraController>();
		Player = (GameObject)Resources.Load("Prefabs/Player");
		AI = (GameObject)Resources.Load("Prefabs/Bot");
		EntityIconPrefab = (GameObject)Resources.Load("Prefabs/EntityIcon");

	}

	private void InstantiatePlayer()
	{
		GameObject _playerClone = Instantiate(Player, Vector3.zero, Quaternion.identity) as GameObject;
		_playerClone.name = "Player";
		_playerClone.GetComponent<EntityMovementHandler>().Color = new Color(1, 1, 1, 1);
		_cameraController.targetTransforms.Add(_playerClone.transform);
		InstantiateEntityIcon(_playerClone.GetComponent<MovementHandler>());
		GameSceneManager.TOTAL_PLAYERS++;
		GameSceneManager.Players.Add(_playerClone.GetComponent<MovementHandler>()); 
	}

	private void InstantiateAI(int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			float offset = 10;
			float x = Random.Range((-GameCanvas.GetComponent<RectTransform>().rect.width / 2) + offset, (GameCanvas.GetComponent<RectTransform>().rect.width / 2) - offset);
			float y = Random.Range((-GameCanvas.GetComponent<RectTransform>().rect.height / 2) + offset, (GameCanvas.GetComponent<RectTransform>().rect.height / 2) - offset);
			GameObject AI_Clone = Instantiate(AI, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
			AI_Clone.GetComponent<AIMovementHandler>().target = GameObject.Find("Player");
			AI_Clone.GetComponent<EntityMovementHandler>().Color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
			_cameraController.targetTransforms.Add(AI_Clone.transform);
			InstantiateEntityIcon(AI_Clone.GetComponent<AIMovementHandler>());
			GameSceneManager.TOTAL_PLAYERS++;
			GameSceneManager.Players.Add(AI_Clone.GetComponent<AIMovementHandler>()); 
		}
	}

	private void InstantiateEntityIcon(EntityMovementHandler enm)
	{
		GameObject _entityIcon = Instantiate(EntityIconPrefab, Vector2.zero, Quaternion.identity) as GameObject;
		_entityIcon.GetComponent<EntityIcon>().Target = enm;
		_entityIcon.transform.parent = UICanvas.transform.FindChild("Background").transform;
		RectTransform _rTransform = _entityIcon.GetComponent<RectTransform>();
		//_rTransform.localScale = new Vector3(.547f, .547f, 1);
		_rTransform.anchoredPosition = new Vector2((GameSceneManager.TOTAL_PLAYERS * 150.0f) + 50, 0);
	}



	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			InstantiateAI(1);
		}
	}
}
