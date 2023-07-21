using ChimeraSoftwareSolutions;
using System.Linq;
using UnityEngine;

public class GazeTrailFollower : GazeTrailBase
{
    public static GazeTrailFollower Instance { get; private set; }
    public static bool GazeTrailState => Instance.On;

    public void OnToggle()
    {
        On = true;
    }
    public void OffToggle()
    {
        On = false;
    }
    private void Awake()
    {
        Instance = this;
        base.OnAwake();
        
    }
    // Start is called before the first frame update
    void Start()
    {
        base.OnStart();
    }

    // Update is called once per frame

    protected override bool GetRay(out Ray ray)
    {
        if(EyeTracker._tracker == null || GazeDataProcessor.GazeData.Count == 0)
            return base.GetRay(out ray);

        var data = GazeDataProcessor.GazeData.Last();

        if (data.Validity == Tobii.Research.Validity.Invalid) return base.GetRay(out ray);
        if (double.IsNaN(data.X) || double.IsNaN(data.Y)) return base.GetRay(out ray);

        ray = Camera.main.ScreenPointToRay(new Vector3((float)data.X == float.NaN ? 100 : (float)data.X, (float)data.Y == float.NaN ? 100 : (float)data.Y));
        
        //ray = GazeDataProcessor.GazeData.Last().CombinedGazeRayScreen;
        return data.CombinedGazeRayScreenValid;
    
    }

    protected override bool HasEyeTracker { get => EyeTracker._tracker != null; }

    protected override bool CalibrationInProgress { get => CalibrationRunner.CalibrationInProgress; }
}
