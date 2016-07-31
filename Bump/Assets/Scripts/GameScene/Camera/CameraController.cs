using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class CameraController : MonoBehaviour {

	public List<Transform> targetTransforms;


	private Camera _camera;
	private float _cameraPositionX;
	private float _cameraPositionY;
	private float _cameraDistance;

	//USE TO FOR DAMPING CAMERA POSITIONS AND ZOOMS
	private float vel;
	private float count;
	private float prevCount;
	public float _dampVelocityX = 0.0f;
	public float _dampVelocityY = 0.0f;
	public float _dampOrthoSize = 0.0f;
	public float DampSmoothTimePosition;
	public float DampSmoothTimeZoom = 0.1F;
	public float ZoomOffset = 2.0f;
	private float targetDamp;

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

			GetAverageCameraPositionAndCameraDistance(out _camX, out _camY, out _cameraDistance);
			float _dampY = Mathf.SmoothDamp(transform.position.y, _camY, ref _dampVelocityY, DampSmoothTimePosition);
			float _dampX = Mathf.SmoothDamp(transform.position.x, _camX, ref _dampVelocityX, DampSmoothTimePosition);
			transform.position = new Vector3(_dampX, _dampY, -1);
			DampSmoothTimePosition = (targetTransforms.Count == 1) ? Mathf.SmoothDamp(DampSmoothTimePosition, 0, ref targetDamp, .25f) : DampSmoothTimePosition;
			float _dampOrtho = Mathf.SmoothDamp(_camera.orthographicSize, (_cameraDistance) + ZoomOffset, ref _dampOrthoSize, DampSmoothTimeZoom);
			_camera.orthographicSize = _dampOrtho;
		}
	}



	float GetDistance(Transform current, Transform next)
	{
		return Vector2.Distance(current.position, next.position);
	}


	void GetAverageCameraPositionAndCameraDistance(out float _camX, out float _camY, out float _cameraDistance)
	{
		_camX = 0;
		_camY = 0;
		_cameraDistance = 0;
		prevCount = (float)targetTransforms.Count;
		for (int i = 0; i <  targetTransforms.Count; i++)
		{
			if (targetTransforms[i] != null)
			{

				_camX += targetTransforms[i].position.x;
				_camY += targetTransforms[i].position.y;


				if (i < targetTransforms.Count - 1)
				{
					float _cameraDistaceIntensity = 1;
					if (targetTransforms[i + 1] != null)
					{
						_cameraDistance += Mathf.Pow(GetDistance(targetTransforms[i], targetTransforms[i + 1]), _cameraDistaceIntensity);
					} else
					{
						for (int ii = 0; ii < targetTransforms.Count; ii++)
						{
							if (targetTransforms[ii] != null)
							{
								if (targetTransforms[i] != targetTransforms[ii])
								{
									_cameraDistance += Mathf.Pow(GetDistance(targetTransforms[i], targetTransforms[ii]), _cameraDistaceIntensity);
								}
							}
						}
					}
				}
			} else
			{
				targetTransforms.Remove(targetTransforms[i]);
			}
		}


		count = Mathf.SmoothDamp(prevCount, count, ref vel, .01f);
		prevCount = count;
		_cameraDistance /= count;
	
		_camX /= count;
		_camY /= count;

	}

}
