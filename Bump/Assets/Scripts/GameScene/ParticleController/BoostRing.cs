using UnityEngine;
using System.Collections;

public class BoostRing : MonoBehaviour {

    private Transform _transform;
    private SpriteRenderer _sRenderer;
    private Color _color;
    private float _colorAlpha = 1.0f;
    private float _scaleIncrementRate = 1f;
    private float _life;
    void Start()
    {
        _transform = GetComponent<Transform>();
        _sRenderer = GetComponent<SpriteRenderer>();
        _color = new Color();
        _life = Random.Range(3, 5);
    }
    void Update () {
        _transform.localScale += new Vector3(_scaleIncrementRate, _scaleIncrementRate, _scaleIncrementRate) * _life;
        _colorAlpha -= Time.deltaTime * _life;
        _sRenderer.color = new Color(_sRenderer.color.r, _sRenderer.color.g, _sRenderer.color.b, _colorAlpha);
        if (_colorAlpha < 0) { Destroy(gameObject); }
    }


}
