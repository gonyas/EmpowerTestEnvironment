using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FocusEffectCtrl : MonoBehaviour
{
    public GameObject upArrow, downArrow, leftArrow, rightArrow;
    
    public void playArrowAnimation(string arrow)
    {
        if  (arrow == "up")
        {
            upArrow.SetActive(true);
        } else if (arrow == "down")
        {
            downArrow.SetActive(true);
        } else if (arrow == "right")
        {
            rightArrow.SetActive(true);
        } else if (arrow == "left")
        {
            leftArrow.SetActive(true);
        }

        return;
    }
}
