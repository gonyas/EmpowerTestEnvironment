using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInAnimation : MonoBehaviour
{
    private Image _spriteRenderer;
    public float fadeInTime;
    
    void Awake()
    {  
        _spriteRenderer = gameObject.GetComponent<Image>();
        Animation();
    }

    // Update is called once per frame
    void Animation()
    {
       var sequence = DOTween.Sequence();
       sequence.Insert(0, t:_spriteRenderer.DOFade(1.0f, fadeInTime));
    }
}
