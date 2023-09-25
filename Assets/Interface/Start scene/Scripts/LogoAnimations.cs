using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LogoAnimations : MonoBehaviour
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
       sequence.PrependInterval(1);
       sequence.Insert(1, _spriteRenderer.DOFade(1.0f, 1.0f));
       sequence.Insert(1,transform.DORotate(new Vector3(0,0,0), 2));
    }
}
