using UnityEngine;
using System.Collections;

public class BoostRing : MonoBehaviour {

    private Transform _transform;
    private SpriteRenderer _sRenderer;
    private Color _color;
    private float _colorAlpha = 1.0f;
    private float _scaleIncrementRate = 1f;
    void Start()
    {
        _transform = GetComponent<Transform>();
        _sRenderer = GetComponent<SpriteRenderer>();
        _color = new Color();
    }
    void Update () {
        _transform.localScale += new Vector3(_scaleIncrementRate, _scaleIncrementRate, _scaleIncrementRate);
        _colorAlpha -= Time.deltaTime * 2.0f;
        _sRenderer.color = new Color(1, 1, 1, _colorAlpha);
        if (_colorAlpha < 0) Destroy(gameObject);
    }


}
