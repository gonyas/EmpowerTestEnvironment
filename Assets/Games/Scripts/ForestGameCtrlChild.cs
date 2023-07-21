using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmpowerDLLs;
using TMPro;

public class ForestGameCtrlChild : ForestGameCtrl
{
    // Eyetracker control
    public EyeTrackerCtrl eyeTrackerCtrl;

    [Header("RESULT PANEL")]
    public TextMeshProUGUI valueTotalCorrectAnswers;
    public TextMeshProUGUI valueCorrectPresent;
    public TextMeshProUGUI valueCorrectAbsent;
    public TextMeshProUGUI valueTotalIncorrectAnswers;
    public TextMeshProUGUI valueIncorrectPresent;
    public TextMeshProUGUI valueIncorrectAbsent;

    public TextMeshProUGUI valueCorrectPepperClassify;
    public TextMeshProUGUI valueIncorrectPepperClassify;
    public TextMeshProUGUI valueCorrectSelSort;
    public TextMeshProUGUI valueCorrectSelNotSort;
    public TextMeshProUGUI valueLongSequence;
    public TextMeshProUGUI valueMeanRT;
    public TextMeshProUGUI valueMeanRTPresent;
    public TextMeshProUGUI valueMeanRTAbsent;
    public TextMeshProUGUI valuePercentTrialsNoAnswer;

    void Update()
    {
        Controls();
    }

     public override IEnumerator StartGame()
        {
            // Before the game starts we do the calibration of the eyetracker
            if (eyeTrackerCtrl.tracker != null)
            {
                eyeTrackerCtrl.gameObject.SetActive(true);
                StartCoroutine(eyeTrackerCtrl.CalibrateEyeTracker());
                yield return new WaitUntil(() => eyeTrackerCtrl.endCalibration == true || eyeTrackerCtrl.skipCalibration == true);

                if (eyeTrackerCtrl.skipCalibration)
                    eyeTrackerCtrl.usingCalibration = false;
            }
            else
                eyeTrackerCtrl.usingCalibration = false;

            coroutineDone = false;  
            StartCoroutine(PrepareGame());
            yield return new WaitUntil(() => coroutineDone == true);

            if (eyeTrackerCtrl.usingCalibration)
                eyeTrackerCtrl.tracker.StartGazeDataCollection();

            coroutineDone = false;
            StartCoroutine(PlayGame());
            yield return new WaitUntil (()=> coroutineDone == true);

            if (eyeTrackerCtrl.usingCalibration)
            {
                eyeTrackerCtrl.tracker.StopGazeDataCollection();
                eyeTrackerCtrl.tracker.Save();
            }

            EndGameGraphics();
            yield return base.StartGame();
        }

        void EndGameGraphics()
        {
        pauseButton.SetActive(false);

        // Set values in result panel
        if (type == ActivityType.ATTENTION)
        {
            if (correctPresent == 0)
                meanRT = 0.0f;
            else
                meanRT = meanRT / correctPresent;
            valueTotalCorrectAnswers.text = (correctPresent + correctAbsent).ToString();
            valueCorrectPresent.text = correctPresent.ToString();
            valueCorrectAbsent.text = correctAbsent.ToString();
            valueTotalIncorrectAnswers.text = (errorPresent + errorAbsent).ToString();
            valueIncorrectPresent.text = errorPresent.ToString();
            valueIncorrectAbsent.text = errorAbsent.ToString();
            valueMeanRT.text = meanRT.ToString("0.00");
        }
        else if (type == ActivityType.WORKMEMORY)
        {
            meanRT = meanRT / numTrials;
            valueCorrectPepperClassify.text = correctPepperClassify.ToString();
            valueIncorrectPepperClassify.text = incorrectPepperClassify.ToString();
            valueCorrectSelSort.text = correctSelSorting.ToString();
            valueCorrectSelNotSort.text = correctSelNotSorting.ToString();
            valueLongSequence.text = longestSequence.ToString();
            valueMeanRT.text = meanRT.ToString("0.00");
        }
        else if (type == ActivityType.INHIBITION)
        {
            meanRT = meanRT / numTrials;
            meanRTPresent = meanRTPresent / (numTrials / 2);
            meanRTAbsent = meanRTAbsent / (numTrials / 2);
            valueTotalCorrectAnswers.text = hits.ToString();
            valueTotalIncorrectAnswers.text = errors.ToString();
            valueMeanRT.text = meanRT.ToString("0.00");
            valueCorrectPresent.text = correctPresent.ToString();
            valueCorrectAbsent.text = correctAbsent.ToString();
            valueIncorrectPresent.text = errorPresent.ToString();
            valueIncorrectAbsent.text = errorAbsent.ToString();
            valueMeanRTPresent.text = meanRTPresent.ToString("0.00");
            valueMeanRTAbsent.text = meanRTAbsent.ToString("0.00");
            valuePercentTrialsNoAnswer.text = percentTrialsNoAnswer.ToString() + "%";
        }
        instructionsPanel.SetActive(false);
        resultsPanel.SetActive(true);
        gameSceneGroup.SetActive(false);
        guiCamera.SetActive(true);
        
       /* PersistentData.CurrentActivity.open = false;
        PersistentData.CurrentActivity.done = true;
        PersistentData.CurrentActivity.correctAbsent = correctAbsent;
        PersistentData.CurrentActivity.correctPepperClassify= correctPepperClassify;
        PersistentData.CurrentActivity.correctPresent = correctPresent;
        PersistentData.CurrentActivity.correctSelNotSorting = correctSelNotSorting;
        PersistentData.CurrentActivity.correctSelSorting = correctSelSorting;
        PersistentData.CurrentActivity.errorAbsent = errorAbsent;
        PersistentData.CurrentActivity.errorPresent = errorPresent;
        PersistentData.CurrentActivity.errors = errors;
        PersistentData.CurrentActivity.hits = hits;
        PersistentData.CurrentActivity.incorrectPepperClassify = incorrectPepperClassify;
        PersistentData.CurrentActivity.longestSequence = longestSequence;
        PersistentData.CurrentActivity.meanRT = meanRT;
        PersistentData.CurrentActivity.meanRTAbsent = meanRTAbsent;
        PersistentData.CurrentActivity.meanRTPresent = meanRTPresent;
        PersistentData.CurrentActivity.percentTrialsNoAnswer = percentTrialsNoAnswer;
        PersistentData.CurrentActivity.rtPerTrial = rtPerTrial;*/
    }
}
