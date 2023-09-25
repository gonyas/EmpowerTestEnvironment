using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FranjaAnimations : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    void Awake()
    {  
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Animation();
    }

    // Update is called once per frame
    void Animation()
    {
       var sequence = DOTween.Sequence();
       sequence.Append(transform.DOMoveX(20f, 3.0f));
       sequence.Insert(0, t:_spriteRenderer.DOFade(1.0f, 3.0f));
    }
}
