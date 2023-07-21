using ChimeraSoftwareSolutions;
using UnityEngine;

public class StartCalibrationButton : MonoBehaviour
{
    public static GameObject StartButtonInstance { get; private set; }
    public static UnityEngine.UI.Button StartButton { get; private set; }
    private void Start()
    {
        StartButtonInstance = this.gameObject;
        StartButton = StartButtonInstance.GetComponent<UnityEngine.UI.Button>();
    }
    public void StartCalibration()
    {
        UIRender.isResultShowing = false;
        CalibrationRunner.Instance.Calibrate();
        HeadPositioner.Instance.PositionAccepted = true;
        HeadPositioner.Instance.TrackBoxGuideActive = false;
    }
    public static bool CalibrationButton = false;
    public void ToggleCalibrationButton()
    {
        UIRender.isResultShowing = false;
        CalibrationButton = !CalibrationButton;
    }
}
