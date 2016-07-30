using UnityEngine;
using System.Collections.Generic;
using System.Collections; 
public class CameraController : MonoBehaviour {

	public List<Transform> targetTransforms;
	private Camera _camera;
	private float _cameraPositionX;
	private float _cameraPositionY;
	private float _cameraDistance;
	private bool _damp;
	private float _dampVelocityX = 0.0f;
	private float _dampVelocityY = 0.0f;
	private float _dampOrthoSize = 0.0f;
	public float DampSmoothTimePosition = .1f;
	public float DampSmoothTimeZoom = 0.1F;

	private bool _isDamping;
	public float ZoomOffset = 5.0f;
	void Start () {
		_camera = GetComponent<Camera>();
		for (int i = 0; i < GameObject.FindGameObjectsWithTag("Entity/GameEntity").Length; i++)
		{
			targetTransforms.Add(GameObject.FindGameObjectsWithTag("Entity/GameEntity")[i].transform);
		}

	}

	void Update ()
	{
		if (targetTransforms.Count > 0)
		{
			float _camX;
			float _camY;
			float _cameraDistance;
			GetAverageCameraPosition(out _camX, out _camY, out _cameraDistance);




			Logger.Log("Damp => " + _damp + " Is damping => " + _isDamping); 
			if (_damp)
			{
				_isDamping = true;
				

				float _dampX = Mathf.SmoothDamp(transform.position.x, _camX, ref _dampVelocityX, DampSmoothTimePosition);
				float _dampY = Mathf.SmoothDamp(transform.position.y, _camY, ref _dampVelocityY, DampSmoothTimePosition);
						
				transform.position = new Vector3(_dampX, _dampY, -1);
				
				float _dampOrtho = Mathf.SmoothDamp(_camera.orthographicSize, _cameraDistance + ZoomOffset, ref _dampOrthoSize, DampSmoothTimeZoom);

				//_camera.orthographicSize = _dampOrtho;
			} else {
				transform.position = new Vector3(_camX, _camY, -1);
				//_camera.orthographicSize = _cameraDistance + ZoomOffset;
			}


		}
	}



	float GetDistance(Transform current, Transform next)
	{
		return Vector2.Distance(current.position, next.position);
	}

	void GetAverageCameraPosition(out float _camX, out float _camY, out float _cameraDistance)
	{
		_camX = 0;
		_camY = 0;
		_cameraDistance = 0;
		for (int i = 0; i <  targetTransforms.Count; i++)
		{
			if (targetTransforms[i] != null)
			{

				_camX += targetTransforms[i].position.x;
				_camY += targetTransforms[i].position.y;


				if (i < targetTransforms.Count - 1)
				{
					if (targetTransforms[i + 1] != null)
					{
						_cameraDistance += GetDistance(targetTransforms[i], targetTransforms[i + 1]);
					} else
					{
						if (targetTransforms[i] != targetTransforms[0])
						{
							_cameraDistance += GetDistance(targetTransforms[i], targetTransforms[0]);
						}
					}
				}
			} else
			{
				targetTransforms.Remove(targetTransforms[i]);
				_damp = true;
			}
		}

		_cameraDistance /= targetTransforms.Count;
		_camX /= targetTransforms.Count;
		_camY /= targetTransforms.Count;

	}

}
