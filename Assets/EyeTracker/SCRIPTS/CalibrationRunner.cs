using ChimeraSoftwareSolutions.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.Research;
using UnityEngine;
using UnityEngine.UI;


namespace ChimeraSoftwareSolutions
{
    public class CalibrationRunner : MonoBehaviour
    {
        /// <summary>
        /// Instance of <see cref="Calibration"/> for easy access.
        /// Assigned in Awake() so use earliest in Start().
        /// </summary>
        public static CalibrationRunner Instance { get; private set; }
        /// <summary>
        /// Flag indicating if the latest calibration was successful
        /// or not, true/false.
        /// </summary>
        public bool LatestCalibrationSuccessful { get; private set; }

        /// <summary>
        /// Is calibration in progress?
        /// </summary>
        /// 
        public static bool CalibrationInProgress { get { return _calibrationInProgress; } set { _calibrationInProgress = value; } }

        public static CalibrationResult calibrationResult;

        public static Dictionary<Vector2, CalibrationSampleCollection> ResultPoints = new Dictionary<Vector2, CalibrationSampleCollection>();
        [SerializeField]
        [Tooltip("This key will start a calibration.")]
        private KeyCode _startKey = KeyCode.C;

        private bool _showCalibrationPanel;
        private static bool _calibrationInProgress;
        /// <summary>
        /// Calibration points.
        /// Example:
        /// (0.2f, 0.2f)
        /// (0.8f, 0.2f)
        /// (0.2f, 0.8f)
        /// (0.8f, 0.8f)
        /// (0.5f, 0.5f)
        /// </summary>
        [SerializeField]
        [Tooltip("Calibration points.")]
        public Vector2[] _points;

        [SerializeField]
        private Image _calibrationPoint;

        [SerializeField]
        private Canvas _canvas;

        [SerializeField]
        private Image _panel;

        private CalibrationPoint _pointScript;

        private bool ShowCalibrationPanel
        {
            get { return _showCalibrationPanel; }
            set
            {
                _showCalibrationPanel = value;
                _pointScript.gameObject.SetActive(_showCalibrationPanel);
                _canvas.gameObject.SetActive(_showCalibrationPanel);
                _panel.color = _showCalibrationPanel ? Color.black : new Color(0, 0, 0, 0);
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _pointScript = _calibrationPoint.GetComponent<CalibrationPoint>();
            ShowCalibrationPanel = false;
        }

        public bool StartCalibration(Vector2[] points = null, System.Action<bool> resultCallback = null)
        {
            if (_calibrationInProgress)
                Debug.Log("Already performing a calibration");

            UIRender.isResultShowing = false;

            StartCoroutine(PerformCalibration(points, resultCallback));
            return true;
        }

        private IEnumerator PerformCalibration(Vector2[] points, System.Action<bool> resultCallback)
        {
            if (points != null)
                _points = points;

            _showCalibrationPanel = true;
            ShowCalibrationPanel = true;
            var calibration = new ScreenBasedCalibration(EyeTracker._tracker);
            calibration.EnterCalibrationMode();
            yield return new WaitForSeconds(1f);

            foreach (var point in _points)
            {
                CalibrationStatus status = CalibrationStatus.Failure;
                while (status != CalibrationStatus.Success)
                {
                    Debug.Log(string.Format("Show point on screen at ({0}, {1})", point.x, point.y));
                    _calibrationPoint.rectTransform.anchoredPosition = new Vector2(Screen.width * point.x, Screen.height * (1 - point.y));
                    yield return new WaitForSeconds(1f);
                    _pointScript.StartAnim();

                    yield return new WaitForSeconds(3f);

                    // Collect data
                    NormalizedPoint2D normalizedPoint = new NormalizedPoint2D(point.x, point.y);
                    status = calibration.CollectData(normalizedPoint);

                    if (status != CalibrationStatus.Success)
                        LogHelper.Info($"The point X: {normalizedPoint.X} Y: {normalizedPoint.Y} calibration failed. Retrying.");
                }
            } // End of point foreach

            calibrationResult = calibration.ComputeAndApply();
            Debug.Log(string.Format("Compute and apply returned {0} and collected at {1} points.",
    calibrationResult.Status, calibrationResult.CalibrationPoints.Count));

            foreach (var testPoints in calibrationResult.CalibrationPoints)
            {
                Vector2 point = new Vector2() { x = (float)Math.Round(testPoints.PositionOnDisplayArea.X * 10) / 10, y = (float)Math.Round(testPoints.PositionOnDisplayArea.Y * 10) / 10 };
                ResultPoints.Add(point, testPoints.CalibrationSamples);
            }

            //CalibrationService.DeleteCalibrationData(EyeTracker.GetCurrentActivityID(), DATABASE.EMPOWER_DB);
            //CalibrationService.SaveCalibrationResult(calibrationResult, EyeTracker.GetCurrentActivityID(), DataSaveMethod.DB, DATABASE.EMPOWER_DB);
            calibration.LeaveCalibrationMode();


            if (resultCallback != null)
                resultCallback(calibrationResult.Status == CalibrationStatus.Success);
            else
                resultCallback(calibrationResult.Status == CalibrationStatus.Failure);

            yield return calibrationResult;



            ShowCalibrationPanel = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(_startKey))
            {
                if (HeadPositioner.Instance.TrackBoxGuideActive == true) return;
                Calibrate();
            }
        }

        public void Calibrate()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            CalibrationInProgress = true;
            RecalibrateOrApply.CalibrationAccepted = false;
            var calibrationStartResult = Instance.StartCalibration(resultCallback: (calibrationResult) =>
            {
                UIRender.self.ShowCalibrationPanel(calibrationResult ? "Success" : "Failure", calibrationResult);
                _calibrationInProgress = false;
            });
        }
    }
}

