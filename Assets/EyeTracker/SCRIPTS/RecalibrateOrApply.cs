using ChimeraSoftwareSolutions;
using UnityEngine;

public class RecalibrateOrApply : MonoBehaviour
{
    public static RecalibrateOrApply Instance { get; set; }
    public static bool AcceptButton = false;
    public static bool CalibrationAccepted = false;
    public static bool ContinueWithoutTracker = false;
    public void Recalibrate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CalibrationRunner.Instance.Calibrate();
        CalibrationAccepted = false;
    }
    void Awake()
    {
        Instance = this;
    }
    public void ApplyCalibration()
    {
        var images = UIRender.self.transform.root.GetChild(0).GetComponentsInChildren<UnityEngine.UI.Image>();
        foreach (var item in images)
        {
            if (item.CompareTag("Diff_back"))
            {
                item.gameObject.SetActive(false);
            }
        }
        UIRender.self.gameObject.SetActive(false);
        UIRender.isResultShowing = false;
        // UnityEngine.Cursor.visible = true;
        // UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        CalibrationAccepted = true;
        ContinueWithoutTracker= false;
    }

    public void WithoutEyeTracker()
    {
        UIRender.self.gameObject.SetActive(false);
        UIRender.isResultShowing = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        CalibrationAccepted = true;
        ContinueWithoutTracker = true;
    }

    public void ToggleAcceptButton()
    {
        UIRender.isResultShowing = false;
        AcceptButton = !AcceptButton;
    }
}
