using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Research;
using ChimeraSoftwareSolutions;
using System.Threading.Tasks;

public class EyeTrackerCtrl : MonoBehaviour
{
    public GameObject calibration;
    public GameObject calibrationResult;
    public GameObject gazeTrail;
    public GameObject headPosition;
    [HideInInspector]
    public bool usingCalibration = true;

    public EyeTracker tracker;
    public bool endCalibration = false;
    public bool skipCalibration = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        calibration.SetActive(false);
        calibrationResult.SetActive(false);
        gazeTrail.SetActive(false);
        headPosition.SetActive(false);

        // Create an eyetracker control
        tracker = new EyeTracker();
        if (tracker != null)
        {
            ChimeraSoftwareSolutions.Returns result = tracker.Connect();
            if (result == Returns.FAILED)
            {
                tracker = null;
                usingCalibration = false;
            }
            else
            {
                tracker.SetActivity(1,1);//PersistentData.CurrentStudent.studentID, PersistentData.CurrentActivity.activityId);
                //tracker.CreateConnection("127.0.0.1", "empowerdb", 3306, "root", "EMPOWERpwd");
                tracker.CreateConnection("127.0.0.1", "empower", 3306, "root", "empower1");
            }
        }
        endCalibration = skipCalibration = false;
    }

    public IEnumerator CalibrateEyeTracker ()
    {
        if (tracker == null)
            yield break;

        headPosition.SetActive(true);
        calibration.SetActive(true);
        calibrationResult.SetActive(true);
        endCalibration = skipCalibration = false;
        //gazeTrail.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        RecalibrateOrApply.CalibrationAccepted = RecalibrateOrApply.ContinueWithoutTracker =false;
        HeadPositioner.Instance.CalibrateWithHead();
            //CalibrationRunner.Instance.Calibrate();
            // _ = Task.Run(() =>
            // {
            //     while (!RecalibrateOrApply.CalibrationAccepted)
            //         Task.Delay(100).Wait();     
            //     Debug.Log("Calibration accepted.");

            // });

        // CalibrationRunner.Instance.Calibrate();

        yield return new WaitUntil (() => RecalibrateOrApply.CalibrationAccepted || RecalibrateOrApply.ContinueWithoutTracker);
        headPosition.SetActive(false);
        calibration.SetActive(false);
        calibrationResult.SetActive(false);
        if (RecalibrateOrApply.CalibrationAccepted && !RecalibrateOrApply.ContinueWithoutTracker)
            endCalibration = true;
        else if (RecalibrateOrApply.CalibrationAccepted && RecalibrateOrApply.ContinueWithoutTracker)
        {
            skipCalibration = true;
            //tracker.Disconnect();
        }

        // HeadPositioner.Instance.CalibrateWithHead();
        //     //CalibrationRunner.Instance.Calibrate();
        //     _ = Task.Run(() =>
        //     {
        //         while (!RecalibrateOrApply.CalibrationAccepted)
        //             Task.Delay(100).Wait();     
        //         Debug.Log("Calibration accepted.");
        //         endCalibration = true;
        //     });
        
        //yield return 0;
    }

    public void Disconnect()
    {
        if (tracker != null)
        {
            tracker.StopGazeDataCollection();
            //tracker.Disconnect();
        }
    }

    private void OnApplicationQuit()
    {
        if (tracker != null)
        {
            tracker.StopGazeDataCollection();
            tracker.Disconnect();
        }
    }
}
