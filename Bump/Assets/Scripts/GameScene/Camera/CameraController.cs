using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class CameraController : MonoBehaviour {



	public List<Transform> targetTransforms;

	public float ZoomOffset                                                                        = 1.0f;
<<<<<<< HEAD
	public float MAXZoom                                                                           = 225.0f;
=======

>>>>>>> 7a9338b50ce1ea3bf7d19cc7f36ced345cfd89a1
	public float SmoothPosition                                                                    = 1.0f; // TIME IT TAKES TO DAMP TO NEXT POSITION

	public float SmoothZoom                                                                        = .25f;     // TIME IT TAKES TO DAMP TO NEXT ZOOM

	public float PositionResetAmount;

	public float ZoomResetAmount;

	public float ZoomDampTo;

	public float PositionDampTo;




	private GameSceneManager _gsm;

	private Camera _camera;

	private float _cameraPositionX;

	private float _cameraPositionY;

	private float _cameraDistance;

	private float DampSmoothTimePosition;

	private float DampSmoothTimeZoom;

	private float targetDamp;

	private float _dampVelocityX;

	private float _dampVelocityY;

	private float _dampOrthoSize;

	private float _cameraWidth;

	private float _cameraHeight;

	private float MAXZoom;

	private bool damp;



	private GameObject _gameCanvas;
	private RectTransform _gameCanvasTransfrom;

	//USE TO FOR DAMPING CAMERA POSITIONS AND ZOOMS
	private float vel;

	private float count;

	private float prevCount;

	void Start () {
		_camera = GetComponent<Camera>();
		_gsm = GameObject.Find("GameSceneManager").GetComponent<GameSceneManager>();
		_gameCanvas = _gsm.GetGameCanvas;
		_gameCanvasTransfrom = _gameCanvas.GetComponent<RectTransform>();
		MAXZoom = _gameCanvasTransfrom.sizeDelta.y / 2.0f;
	}

	void Update ()
	{
<<<<<<< HEAD
		
=======
>>>>>>> 7a9338b50ce1ea3bf7d19cc7f36ced345cfd89a1
		if (targetTransforms.Count > 0)
		{
			float _camX;

			float _camY;

			float _cameraDistance;

			float _avgVelocity;

			GetAverageCameraPositionAndCameraDistance(out _camX, out _camY, out _cameraDistance, out _avgVelocity);

			DampSmoothTimePosition = (DampSmoothTimePosition > PositionDampTo) ? DampSmoothTimePosition - (Time.deltaTime / SmoothPosition) : PositionDampTo;

			DampSmoothTimeZoom = Mathf.SmoothDamp(DampSmoothTimeZoom, ZoomDampTo, ref targetDamp, 1.0f / SmoothZoom);

			float _dampY = Mathf.SmoothDamp(transform.position.y, _camY, ref _dampVelocityY, DampSmoothTimePosition);

			float _dampX = Mathf.SmoothDamp(transform.position.x, _camX, ref _dampVelocityX, DampSmoothTimePosition);

			float _dampOrtho = Mathf.SmoothDamp(_camera.orthographicSize, (_cameraDistance) + ZoomOffset, ref _dampOrthoSize, DampSmoothTimeZoom);

			transform.position = new Vector3(_dampX, _dampY, -1);

			_camera.orthographicSize = (_dampOrtho > MAXZoom) ? MAXZoom : _dampOrtho;

			float _negVerticalBound;

			float _posVerticalBound;

			float _negHorizontalBound;

			float _posHorizontalBound;

			GetVerticalCameraBounds(out _negVerticalBound, out  _posVerticalBound, _camera.orthographicSize, _gameCanvasTransfrom.sizeDelta.y);

			GetHorizontalCameraBounds(out _negHorizontalBound, out _posHorizontalBound, _cameraHeight, _gameCanvasTransfrom.sizeDelta.x);

			SetCameraBounds(_negVerticalBound, _posVerticalBound, _negHorizontalBound, _posHorizontalBound);
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
				if (i < targetTransforms.Count - 1)
				{
					Transform current = targetTransforms[i];
					if (targetTransforms[i + 1] != null)
					{
						Transform next = targetTransforms[i + 1];
						_cameraDistance += GetDistance(current, next);
					} else
					{
						for (int ii = 0; ii < targetTransforms.Count; ii++)
						{
							if (targetTransforms[ii] != null)
							{
								_cameraDistance += GetDistance(current, targetTransforms[ii]);
							}
						}
					}
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


	void GetVerticalCameraBounds(out float negVerticalBound, out float posVerticalBound, float _camOrthoSize, float _canvasHeight)
	{
		negVerticalBound = 0;
		posVerticalBound = 0;
		float height = _camOrthoSize * 2.0f;
		_cameraHeight = height;
		float difference = _canvasHeight - height;
		negVerticalBound = -(difference / 2.0f);
		posVerticalBound = difference / 2.0f;
	}


	void GetHorizontalCameraBounds(out float negHorizontalBound, out float posHorizontalBound, float _cameraHeight, float _canvasWidth)
	{
		negHorizontalBound = 0;
		posHorizontalBound = 0;
		float width = _cameraHeight * _camera.aspect;
		_cameraWidth = width;
		float difference = _canvasWidth - width;
		negHorizontalBound = -(difference / 2.0f);
		posHorizontalBound = difference / 2.0f;
	}


	void SetCameraBounds(float negVertical, float posVertical, float negHorizontal, float posHorizontal)
	{
		Vector3 boundedPosition = transform.position;
		if (boundedPosition.y < negVertical)
		{
			boundedPosition.y = negVertical;
		}
		else if (boundedPosition.y > posVertical)
		{
			boundedPosition.y = posVertical;
		}
		if (boundedPosition.x < negHorizontal)
		{
			boundedPosition.x = negHorizontal;
		}
		else if (boundedPosition.x > posHorizontal)
		{
			boundedPosition.x = posHorizontal;
		}
		transform.position = boundedPosition;
	}
}
