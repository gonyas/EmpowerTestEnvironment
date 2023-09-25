using ChimeraSoftwareSolutions;
using UnityEngine;

public class RecalibrateOrApply : MonoBehaviour
{
    public static RecalibrateOrApply Instance { get; set; }
    public static bool AcceptButton = false;
    public static bool CalibrationAccepted = false;
    // Needed for Unity games
    public static bool ContinueWithoutTracker = false;

    public void Recalibrate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CalibrationRunner.Instance.Calibrate();
        CalibrationAccepted = false; 
        // Needed for Unity games
        ContinueWithoutTracker = false;
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
        //Needed for Unity games
        // UnityEngine.Cursor.visible = false;
        // UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        CalibrationAccepted = true;
        // Needed for Unity games
        ContinueWithoutTracker= false;
    }

    public void ToggleAcceptButton()
    {
        UIRender.isResultShowing = false;
        AcceptButton = !AcceptButton;
    }

    // Needed for Unity games
    public void WithoutEyeTracker()
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
        CalibrationAccepted = true;
        ContinueWithoutTracker = true;
    }
}
