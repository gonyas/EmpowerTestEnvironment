using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FadeInOutAnimationText : MonoBehaviour
{
    TextMeshPro tmp;
    public float fadeInTime, fadeOutTime, prependInterval;
    
    public void OnEnable()
    {  
        tmp = gameObject.GetComponent<TextMeshPro>();
        FadeInAnimation();
    }

    // Update is called once per frame
    void FadeInAnimation()
    {
       var sequence = DOTween.Sequence();
       sequence.Append(tmp.DOFade(1.0f, fadeInTime));
       StartCoroutine(intervalAnim());
    }

    IEnumerator intervalAnim(){
        yield return new WaitForSeconds(prependInterval);
        fadeOutAnimation();
    }

    void fadeOutAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Append(tmp.DOFade(0.0f, fadeOutTime));
    }
}
