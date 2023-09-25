using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInAnimationAlpha : MonoBehaviour
{
    private Image _spriteRenderer;
    public float fadeInTime, alphaValue;
    
    void Awake()
    {  
        _spriteRenderer = gameObject.GetComponent<Image>();
        Animation();
    }

    // Update is called once per frame
    void Animation()
    {
       var sequence = DOTween.Sequence();
       sequence.Insert(0, t:_spriteRenderer.DOFade(alphaValue, fadeInTime));
    }
}
