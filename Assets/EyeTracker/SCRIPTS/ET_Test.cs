using ChimeraSoftwareSolutions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tobii.Research;
using UnityEngine;

public class ET_Test : MonoBehaviour
{
    public static EyeTracker tracker = new EyeTracker();
    // Start is called before the first frame update
    void Start()
    {
        //Connect to the eyetracker
        tracker.Connect();

        //Create database connection
        tracker.CreateConnection("127.0.0.1","teszt1",3306,"root","root");

        tracker.SetActivity(1,101,1011);
    }
    void Update()
    {
        //if (EyeTracker.IsSaveRunning) 
        //{ 
        //    Debug.Log("Data saving is in progress. Do not close the application!"); 
        //}
        //else
        //{ 
        //    Debug.Log("No saving is currently in progress.");
        //}

        if (Input.GetKeyDown(KeyCode.V))
        {
            tracker.Save(); 
            Debug.Log("Data saving started."); 
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            tracker.StartGazeDataCollection();
            Debug.Log("GazeDataCollection started.");
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            tracker.StopGazeDataCollection();
            Debug.Log("GazeDataCollection stopped.");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            tracker.SetActivity(1, 255,1011);
        }
        if(Input.GetKeyDown(KeyCode.H))
        {
            HeadPositioner.Instance.CalibrateWithHead();
            //CalibrationRunner.Instance.Calibrate();
            _ = Task.Run(() =>
            {
                while (!RecalibrateOrApply.CalibrationAccepted)
                    Task.Delay(100).Wait();     
                Debug.Log("Calibration accepted.");

            });
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            //Check if eyetracker is connected and listening, if not then return.
            if (tracker.State != EyetrackerState.LISTENING) return;


            if (tracker.SightState == SightState.UP)
            {
                Debug.Log("UP");
            }
            else if (tracker.SightState == SightState.DOWN)
            {
                Debug.Log("DOWN!");
            }
            else
            {
                Debug.Log("AWAY");
            }
            var data = tracker.GetLatestGazeData();
            Debug.Log($"Gaze xy: ({data.X},{data.Y}), Pupil xy: ({data.LeftPupilDia},{data.RightPupilDia}) Head xyz: ({data.HeadPosX},{data.HeadPosY},{data.HeadPosZ})");
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            var data = tracker.GetLatestFixationData();
            Debug.Log($"Fixation frequency: {data[0]} | Average Fixation Time: {data[1]} | Total fixation time: {data[2]}");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(GazeTrailFollower.GazeTrailState)
                GazeTrailFollower.Instance.OffToggle();
            else
                GazeTrailFollower.Instance.OnToggle();

            Debug.Log("GazeTrailFollower toggled.");
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            //Log headposition validity
            Debug.Log(tracker.GetHeadPosValidity().ToString());
        }
    }

    private void OnApplicationQuit()
    {
        tracker.StopGazeDataCollection();
        tracker.Disconnect();
    }

}
