using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextAnimation : MonoBehaviour
{
    public TextMeshProUGUI text;

    // Update is called once per frame
    public void Animation()
    {
        var sequence = DOTween.Sequence();
        sequence.PrependInterval(6.0f);
        sequence.Append(text.DOFade(1.0f, 4.0f));
    }
}
