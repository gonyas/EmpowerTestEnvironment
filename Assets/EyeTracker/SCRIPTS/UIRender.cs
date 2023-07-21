using ChimeraSoftwareSolutions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRender : MonoBehaviour
{
    public static UIRender self;
    //Needed for the Unity games
    public GameObject ResultText;
    //Needed for the Unity games
    public GameObject ResultTextFix;
    //Needed for the Unity games
    public GameObject AcceptButton;
    //Needed for the Unity games
    public GameObject StartCalibration;
    //Needed for the Unity games
    public GameObject RecalibrateButton;
    //Needed for the Unity games
    public GameObject WithoutEyeTracker;
    public List<GameObject> DifferenceLines = new List<GameObject>();
    public static bool isResultShowing = false;

    public void Awake()
    {
        self = this;
        //Needed for the Unity games
        RecalibrateButton.SetActive(false);
        AcceptButton.SetActive(false);
        WithoutEyeTracker.SetActive(false);
        ResultTextFix.SetActive(false);
        ResultText.SetActive(false);
    }

    //Needed for the Unity games
    public void OnDisable()
    {
        RecalibrateButton.SetActive(false);
        AcceptButton.SetActive(false);
        WithoutEyeTracker.SetActive(false);
        ResultTextFix.SetActive(false);
        ResultText.SetActive(false);
    }

    public void ShowCalibrationPanel(string CalibrationResult, bool AcceptButtonState)
    {
        UIRender.isResultShowing = false;
        isResultShowing = true;
        DrawLines();
        self.gameObject.SetActive(true);
        // ResultText = GameObject.FindGameObjectWithTag("CalibrationResult_Text");
        // AcceptButton = GameObject.FindGameObjectWithTag("Accept_Calibration");

        //Needed for the Unity games
        AcceptButton.SetActive(true);
        RecalibrateButton.SetActive(true);
        WithoutEyeTracker.SetActive(true);
        ResultTextFix.SetActive(true);
        ResultText.SetActive(true);

        var txt = ResultText.GetComponent<TextMeshProUGUI>();
        txt.text = CalibrationResult;
        var button = AcceptButton.GetComponent<UnityEngine.UI.Button>();
        button.interactable = AcceptButtonState;
        // Kurzor megjelen�t�se
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;


    }
    private void DrawLines()
    {
        if (DifferenceLines.Count > 0)
        {
            foreach (var item in DifferenceLines)
            {
                Destroy(item);
            }
            DifferenceLines.Clear();
        }

        var screen_width = Screen.width;
        var screen_height = Screen.height;
        foreach (var point in CalibrationRunner.Instance._points)
        {
            if (!CalibrationRunner.ResultPoints.ContainsKey(point))
                continue;

            var pointAPosition = new Vector2(screen_width * point.x, screen_height * (1 - point.y));

            var pointBLeftPosition = new Vector2(screen_width * CalibrationRunner.ResultPoints[point][0].LeftEye.PositionOnDisplayArea.X, screen_height * (1 - CalibrationRunner.ResultPoints[point][0].LeftEye.PositionOnDisplayArea.Y));
            var pointBRightPosition = new Vector2(screen_width * CalibrationRunner.ResultPoints[point][0].RightEye.PositionOnDisplayArea.X, screen_height * (1 - CalibrationRunner.ResultPoints[point][0].RightEye.PositionOnDisplayArea.Y));

            MakeLine(pointAPosition, pointBLeftPosition, Color.red);
            MakeLine(pointAPosition, pointBRightPosition, Color.green);
        }
        CalibrationRunner.ResultPoints.Clear();
        var images = self.transform.root.GetChild(0).GetComponentsInChildren<UnityEngine.UI.Image>();
        foreach (var item in images)
        {
            if (item.CompareTag("Diff_back"))
            {
                item.gameObject.SetActive(true);
            }
        }


    }
    void PositionCircles()
    {

        for (int i = 0; i < CalibrationRunner.calibrationResult.CalibrationPoints.Count; i++)
        {
            var screen_width = Screen.width;
            var screen_height = Screen.height;
            var point = CalibrationRunner.calibrationResult.CalibrationPoints;

            var children = self.transform.root.GetChild(0).GetComponentsInChildren<UnityEngine.UI.Image>();
            foreach (var child in children)
            {
                if (child.CompareTag("Circle_LeftUp"))
                {
                    child.rectTransform.position = new Vector2(screen_width * point[4].PositionOnDisplayArea.X, screen_height * point[4].PositionOnDisplayArea.Y);
                    child.color = Color.white;
                }
                else if (child.CompareTag("Circle_RightUp"))
                {
                    child.rectTransform.position = new Vector2(screen_width * point[1].PositionOnDisplayArea.X, screen_height * point[1].PositionOnDisplayArea.Y);
                    child.color = Color.white;
                }
                else if (child.CompareTag("Circle_LeftDown"))
                {
                    child.rectTransform.position = new Vector2(screen_width * point[2].PositionOnDisplayArea.X, screen_height * point[2].PositionOnDisplayArea.Y);
                    child.color = Color.white;
                }
                else if (child.CompareTag("Circle_RightDown"))
                {
                    child.rectTransform.position = new Vector2(screen_width * point[3].PositionOnDisplayArea.X, screen_height * point[3].PositionOnDisplayArea.Y);
                    child.color = Color.white;
                }
            }
        }
    }

    void MakeLine(Vector2 pointA, Vector2 pointB, Color col)
    {
        GameObject newObject = new GameObject();
        Vector2 new_PointB = new Vector2(Mathf.RoundToInt(pointB.x), Mathf.RoundToInt(pointB.y));
        pointB = new_PointB;
        newObject.name = "line from " + pointA.ToString() + " to " + pointB.ToString();

        UnityEngine.UI.Image newImage = newObject.AddComponent<UnityEngine.UI.Image>();
        newImage.color = col;

        RectTransform rect = newObject.GetComponent<RectTransform>();
        rect.SetParent(transform);
        rect.localScale = Vector3.one;

        rect.anchorMin = new Vector2(pointA.x / Screen.width, pointA.y / Screen.height);
        rect.anchorMax = rect.anchorMin;
        rect.sizeDelta = new Vector2(0f, 1f);
        rect.pivot = new Vector2(0f, 0.5f);

        Vector3 pointAToVector3 = new Vector3(pointA.x, pointA.y, 0);
        Vector3 pointBToVector3 = new Vector3(pointB.x, pointB.y, 0);

        rect.localPosition = pointAToVector3;

        // Get the vector from point A to point B
        Vector3 diff = pointAToVector3 - pointBToVector3;

        // Set the width of the UI element to the distance between the two points
        rect.sizeDelta = new Vector2(diff.magnitude, 1f);

        // Calculate the rotation needed to face the direction from A to B
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg - 180f;

        // Apply the rotation to the UI element
        rect.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        //Vonal hozz�ad�sa a list�hoz
        DifferenceLines.Add(newObject);

    }
}
