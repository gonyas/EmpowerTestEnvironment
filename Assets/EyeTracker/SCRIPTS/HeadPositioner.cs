using System.Linq;
using Tobii.Research;
using UnityEngine;
using UnityEngine.UI;

public class HeadPositioner : MonoBehaviour
{

    /// <summary>
    /// Get <see cref="HeadPositioner"/> instance. This is assigned
    /// in Awake(), so call earliest in Start().
    /// </summary>
    public static HeadPositioner Instance { get; private set; }

    [SerializeField]
    [Tooltip("Activate or deactivate the track box guide.")]
    private bool _trackBoxGuideActive;

    [SerializeField]
    [Tooltip("This key will show or hide the Head Positioner module.")]
    private KeyCode _toggleKey = KeyCode.H;

    [SerializeField]
    private GameObject _CanvasTrackBox;

    private Color _colorMoverGood;
    private Color _colorMoverBad;
    private Color _colorEyeGood;
    private Color _colorEyeBad;

    private Rect _box;
    private Image _mover;
    private Image _eyeLeft;
    private Image _eyeRight;
    private Slider _sliderLeft;
    private Slider _sliderRight;
    private Vector3 _eyeScaleLeft;
    private Vector3 _eyeScaleRight;
    private IEyeTracker _eyeTracker;
    private float _trackabilityLeft;
    private float _trackabilityRight;
    private UserPositionGuideEventArgs latestTrackBoxData;
    public bool PositionAccepted = false;
    public UnityEngine.UI.Button _startButton;
    public StartCalibrationButton StartCalibrationButton;

    public void ToggleButton(bool StartButtonState)
    {
        StartCalibrationButton.StartButton.interactable = StartButtonState;
    }
   
    /// <summary>
    /// Activate or deactivate the track box guide.
    /// </summary>
    public bool TrackBoxGuideActive
    {
        get
        {
            return _trackBoxGuideActive;
        }

        set
        {
            _trackBoxGuideActive = value;
            _CanvasTrackBox.SetActive(_trackBoxGuideActive);
        }
    }
   

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        TrackBoxGuideActive = false; 
        _eyeTracker = EyeTrackingOperations.FindAllEyeTrackers().First();
        _eyeTracker.UserPositionGuideReceived += _eyeTracker_UserPositionGuideReceived;
        var box = _CanvasTrackBox.transform.Find("TrackboxArea");
        _box = box.GetComponent<Image>().rectTransform.rect;
        _mover = box.Find("PanelMover").GetComponent<Image>();
        _eyeLeft = _CanvasTrackBox.transform.Find("LeftEye").GetComponent<Image>();
        _eyeRight = _CanvasTrackBox.transform.Find("RightEye").GetComponent<Image>();
        _sliderLeft = _CanvasTrackBox.transform.Find("SliderLeft").GetComponent<Slider>();
        _sliderRight = _CanvasTrackBox.transform.Find("SliderRight").GetComponent<Slider>();

        _eyeScaleLeft = _eyeLeft.rectTransform.localScale;
        _eyeScaleRight = _eyeRight.rectTransform.localScale;

        _colorMoverGood = new Color32(0, 100, 0, 220);
        _colorMoverBad = new Color32(100, 0, 0, 220);
        _colorEyeGood = new Color32(200, 255, 200, 255);
        _colorEyeBad = new Color32(255, 100, 100, 255);

        TrackBoxGuideActive = _trackBoxGuideActive;
  
    }

    private void _eyeTracker_UserPositionGuideReceived(object sender, UserPositionGuideEventArgs e) => latestTrackBoxData = e;
    // Update is called once per frame
    void Update()
    {
    //    if (!StartCalibrationButton.CalibrationButton)
    //    {
    //        Cursor.visible = true;
    //        Cursor.lockState = CursorLockMode.None;
    //    }
    //    else
    //    {
    //        //if(!UIRender.isResultShowing)
    //        //{
    //        //    Cursor.visible = false;
    //        //    Cursor.lockState = CursorLockMode.Locked;
    //        //} else
    //        //{
    //        //    Cursor.visible = true;
    //        //    Cursor.lockState = CursorLockMode.None;
    //        //}
    //    }
        if (_CanvasTrackBox.activeSelf != TrackBoxGuideActive)
        {
            TrackBoxGuideActive = _trackBoxGuideActive;
        }
        if (RecalibrateOrApply.AcceptButton)
        {
            if (Input.GetKeyDown(_toggleKey))
            {
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                TrackBoxGuideActive = !TrackBoxGuideActive;
            }
        }
        if (_eyeTracker == null || !_trackBoxGuideActive)
        {
            return;
        }

        if(latestTrackBoxData != null)
        lock (latestTrackBoxData)
        {
            var data = latestTrackBoxData;
            var goLeft = ToVector3(data.LeftEye.UserPosition);
            var goRight = ToVector3(data.RightEye.UserPosition);
            var goLeftValid = data.LeftEye.Validity == Validity.Valid;
            var goRightValid = data.RightEye.Validity == Validity.Valid;
            PositionMover(goLeft.z, goRight.z, goLeftValid, goRightValid);
            PositionEye(goLeft, goLeftValid, _eyeLeft, _eyeScaleLeft);
            PositionEye(goRight, goRightValid, _eyeRight, _eyeScaleRight);

            var delta = Time.deltaTime / 2f;
            SetTrackabilitySlider(ref _trackabilityLeft, goLeftValid, _sliderLeft, delta);
            SetTrackabilitySlider(ref _trackabilityRight, goRightValid, _sliderRight, delta);
            var AvgZ = (goLeft.z + goRight.z) / 2f;

            if(goLeftValid && goRightValid && AvgZ > 0.34 && AvgZ < 0.66)
                ToggleButton(true);
            else
                ToggleButton(false);
        }
        
    }

    /// <summary>
    /// Set trackability slider.
    /// </summary>
    /// <param name="trackability">Reference to the trackability history value.</param>
    /// <param name="valid">The eye validity.</param>
    /// <param name="slider">The slider to manimpulate.</param>
    /// <param name="delta">The delta change.</param>
    private static void SetTrackabilitySlider(ref float trackability, bool valid, Slider slider, float delta)
    {
        trackability = Mathf.Clamp01(trackability + (valid ? delta : -delta));
        slider.value = trackability;
    }

    /// <summary>
    /// Move the depth indicator panel (the "mover").
    /// </summary>
    /// <param name="zLeft">The normalized track box z value of the left eye.</param>
    /// <param name="zRight">The normalized track box z value of the right eye.</param>
    /// <param name="validLeft">The validity of the left eye origin.</param>
    /// <param name="validRight">The validity of the right eye origin.</param>
    private void PositionMover(float zLeft, float zRight, bool validLeft, bool validRight)
    {
        if (!(validLeft || validRight))
        {
            // If nothing is valid, don't do anything.
            return;
        }
        
        // Select the scale.
        var scale = 1 - (validLeft && validRight ? (zLeft + zRight) / 2f : validLeft ? zLeft : zRight);

        // Set the scale.;
        _mover.rectTransform.localScale = Vector3.one * Mathf.Lerp(0.5f, 1f, scale);

        // Set the color.
        _mover.color = Color.Lerp(_colorMoverGood, _colorMoverBad, Mathf.Abs(0.5f - scale) * 2.5f); //STOCK
    }

    /// <summary>
    /// Position eye in box.
    /// For a rect that is 512 wide and 256 high with pivot in center:
    /// Furthest away scale 0 - 128 in x and 0 - 64 in y
    /// Nearest             0 - 256    x     0 - 128   y
    /// (0, 0, 0) - the upper, right corner closest to the eye tracker,
    /// and (1, 1, 1) - the lower left, furthest away, corner.
    /// </summary>
    /// <param name="eyePos">The eye position normalized vector.</param>
    /// <param name="valid">The validity flag.</param>
    /// <param name="eye">The eye UI image component.</param>
    /// <param name="eyeScale">The UI scale of the eye.</param>
    private void PositionEye(Vector3 eyePos, bool valid, Image eye, Vector3 eyeScale)
    {
        if (!valid)
        {
            // "Close" the eye if signal is invalid.
            eye.rectTransform.localScale = Vector3.zero;
            return;
        }

        // Change the range of the z scale from 0 - 1 to 0.5 - 1.
        var scale = Mathf.Lerp(0.5f, 1f, 1 - eyePos.z);

        // Get an indicator on how near the center we are placed in the box.
        var distanceToCenter = Vector3.Distance(Vector3.one / 2f, eyePos) * 2f;

        // Scale x between 0 and 512 as if it was nearest, subtract half, then scale in z.
        eyePos.x = (Mathf.Lerp(0, _box.width, 1 - eyePos.x) - (_box.width / 2f)) * scale;

        // Scale x between 0 and 256 as if it was nearest, then subtract half, then scale in z.
        eyePos.y = (Mathf.Lerp(0, _box.height, 1 - eyePos.y) - (_box.height / 2f)) * scale;

        // Set the eye position.
        eye.rectTransform.anchoredPosition = eyePos;

        // Scale the eye.
        eye.rectTransform.localScale = eyeScale * scale;

        // Set the color.
        eye.color = Color.Lerp(_colorEyeGood, _colorEyeBad, distanceToCenter);
    }

    public static Vector3 ToVector3(NormalizedPoint3D value)
    {
        return new Vector3(value.X, value.Y, value.Z);
    }

    private void OnApplicationQuit()
    {
        _eyeTracker.UserPositionGuideReceived -= _eyeTracker_UserPositionGuideReceived;
    }

    public void CalibrateWithHead()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        TrackBoxGuideActive = !TrackBoxGuideActive;
    }
    public void CloseHeadPosition()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        TrackBoxGuideActive = false;
    }
}
