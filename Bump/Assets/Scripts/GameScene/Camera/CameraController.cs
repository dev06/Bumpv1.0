using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class CameraController : MonoBehaviour {

	public List<Transform> targetTransforms;


	private Camera _camera;
	private float _cameraPositionX;
	private float _cameraPositionY;
	private float _cameraDistance;
	private float DampSmoothTimePosition;
	private float DampSmoothTimeZoom                                                               = 0.1F;
	private float targetDamp;
	private float _dampVelocityX                                                                   = 0.0f;
	private float _dampVelocityY                                                                   = 0.0f;
	private float _dampOrthoSize                                                                   = 0.0f;
	private bool damp;

	//USE TO FOR DAMPING CAMERA POSITIONS AND ZOOMS
	private float vel;
	private float count;
	private float prevCount;
	public float ZoomOffset                                                                        = 1.0f;
	public float MAXZoom                                                                           = 250.0f;
	public float SmoothPosition                                                                    = 1.0f; // TIME IT TAKES TO DAMP TO NEXT POSITION
	public float SmoothZoom                                                                        = .25f;     // TIME IT TAKES TO DAMP TO NEXT ZOOM
	public float PositionResetAmount;
	public float ZoomResetAmount;

	void Start () {
		_camera = GetComponent<Camera>();
	}

	void Update ()
	{
		if (targetTransforms.Count > 0)
		{
			float _camX;
			float _camY;
			float _cameraDistance;
			float _avgVelocity;

			GetAverageCameraPositionAndCameraDistance(out _camX, out _camY, out _cameraDistance, out _avgVelocity);
			
			DampSmoothTimePosition = (DampSmoothTimePosition > 0) ? DampSmoothTimePosition - (Time.deltaTime / SmoothPosition) : 0;
			float _dampY = Mathf.SmoothDamp(transform.position.y, _camY, ref _dampVelocityY, DampSmoothTimePosition);
			float _dampX = Mathf.SmoothDamp(transform.position.x, _camX, ref _dampVelocityX, DampSmoothTimePosition);
			float _dampOrtho = Mathf.SmoothDamp(_camera.orthographicSize, (_cameraDistance) + ZoomOffset, ref _dampOrthoSize, DampSmoothTimeZoom);
			DampSmoothTimeZoom = Mathf.SmoothDamp(DampSmoothTimeZoom, 0, ref targetDamp, 1.0f / SmoothZoom);
			

			transform.position = new Vector3(_dampX, _dampY, -1);
			_camera.orthographicSize = (_dampOrtho > MAXZoom) ? MAXZoom : _dampOrtho;
		}
	}

	float GetDistance(Transform current, Transform next)
	{
		return Vector2.Distance(current.position, next.position);
	}


	void GetAverageCameraPositionAndCameraDistance(out float _camX, out float _camY, out float _cameraDistance, out float _avgVelocity)
	{
		_camX = 0;
		_camY = 0;
		_cameraDistance = 0;
		_avgVelocity = 0;
		prevCount = (float)targetTransforms.Count;
		for (int i = 0; i <  targetTransforms.Count; i++)
		{
			if (targetTransforms[i] != null)
			{

				_camX += targetTransforms[i].position.x;
				_camY += targetTransforms[i].position.y;

				// CALCULATE THE DISTANCE BETWEEN CAMERA AND TRANSFROM

				if (targetTransforms.Count > 1)
				{
					_cameraDistance += GetDistance(transform, targetTransforms[i]);
				}

			} else
			{
				targetTransforms.Remove(targetTransforms[i]);
				DampSmoothTimePosition = PositionResetAmount;
				DampSmoothTimeZoom = ZoomResetAmount;
				damp = true;
			}
		}
		count = Mathf.SmoothDamp(prevCount, count, ref vel, .01f);
		prevCount = count;
		_cameraDistance /= count;
		_avgVelocity /= count;
		_camX /= count;
		_camY /= count;
	}
}
