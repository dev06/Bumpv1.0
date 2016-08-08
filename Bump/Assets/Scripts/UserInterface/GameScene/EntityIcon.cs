using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EntityIcon : MonoBehaviour {

	public static int SCALE = 100;

	public EntityMovementHandler Target;
	public float value;

	private RectTransform _rTransform;
	private Image _innerImage;
	private Image _fill;
	private Text _text;
	private float fillVel;
	private float _destroyVel;
	private Color _color;
	private bool _expand;

	void Start ()
	{
		Init();
	}

	void Init()
	{
		_innerImage = transform.Find("InnerIcon").GetComponent<Image>();
		_fill = transform.Find("Fill").GetComponent<Image>();
		_text = transform.Find("Text").GetComponent<Text>();
		_fill.color = Target.Color;
		_innerImage.color =  new Color(Target.Color.r, Target.Color.g, Target.Color.b, .5f);
		_rTransform = GetComponent<RectTransform>();
		_expand = true;
	}

	// Update is called once per frame
	void Update ()
	{
		_fill.fillAmount = Mathf.SmoothDamp(_fill.fillAmount, Target.Health / 100.0f, ref fillVel, .5f);
		_text.text = "" + ((int)(_fill.fillAmount * SCALE));
		
		AnimatePopUp(_expand);
		DestroyThis(); 
	}

	void AnimatePopUp(bool _expand)
	{
		if (_expand)
		{
			_rTransform.localScale = Vector3.Lerp(_rTransform.localScale, new Vector3(.547f, .547f, .547f), Time.deltaTime * 5.0f);
		}
		else
		{
			_rTransform.localScale = Vector3.Lerp(_rTransform.localScale, Vector3.zero, Time.deltaTime * 5.0f);
		}
	}



	void DestroyThis()
	{
		if (Target == null)
		{
			_expand = false;
		}
	}
}
