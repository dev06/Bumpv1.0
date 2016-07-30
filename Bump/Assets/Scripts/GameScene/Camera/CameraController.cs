using UnityEngine;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	public List<Transform> targetTransforms;
	private Camera _camera;
	private float _cameraPositionX;
	private float _cameraPositionY;
	private float _cameraDistance;

	public float Zoom = 5.0f;
	public float ZoomOffset = 2.0f;
	void Start () {
		_camera = GetComponent<Camera>();
	
	}

	void Update ()
	{
		if (targetTransforms.Count > 0)
		{
			for (int i = 0; i < targetTransforms.Count; i++)
			{
				float currentTransformX = targetTransforms[i].position.x / 2.0f;
				float currentTransformY = targetTransforms[i].position.y / 2.0f;
				_cameraPositionX += (currentTransformX);
				_cameraPositionY += (currentTransformY);
				if (i < targetTransforms.Count - 1)
				{
					_cameraDistance += GetDistance(targetTransforms[i], targetTransforms[i + 1]);
				}
			}
			_cameraDistance = _cameraDistance / targetTransforms.Count;
			_cameraPositionX = _cameraPositionX / targetTransforms.Count;
			_cameraPositionY = _cameraPositionY / targetTransforms.Count;
			_camera.orthographicSize = (_cameraDistance / Zoom) + ZoomOffset;
			transform.position = new Vector3(_cameraPositionX, _cameraPositionY, -1);
		}
	}


	float GetDistance(Transform current, Transform next)
	{
		return Vector2.Distance(current.position, next.position);
	}
}
