using UnityEngine;
using UnityEngine.UI;

public class CalibrationPoint : MonoBehaviour
{
    private float _startTime;
    private float _length;
    private float _speed;
    private Vector3 _zoomIn;
    private Vector3 _zoomOut;
    private Image _image;
    private bool _animation;
    // Start is called before the first frame update
    void Start()
    {
        _image = GetComponent<Image>();
        _animation = false;
        _length = 1.5f;
        _speed = 0.5f;

        _zoomOut = new Vector3(1f, 1f, 1f);
        _zoomIn = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_animation)
        {
            var covered = (Time.time - _startTime) * _speed;
            var unitCovered = covered / _length;
            _image.rectTransform.localScale = Vector3.Lerp(_zoomOut, _zoomIn, unitCovered);
        }
    }

    public void StartAnim()
    {
        transform.localScale = _zoomOut;
        _startTime = Time.time;
        _animation = true;
    }
}
