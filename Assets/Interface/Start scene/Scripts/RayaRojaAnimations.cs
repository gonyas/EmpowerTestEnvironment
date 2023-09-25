using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RayaRojaAnimations : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    void Awake()
    {  
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public void PlayAnimation()
    {
       var sequence = DOTween.Sequence();
       sequence.PrependInterval(1.5f);
       sequence.Insert(2, t:_spriteRenderer.DOFade(1.0f, 0.1f));
       sequence.Insert(2, transform.DOScale(new Vector3(1f,1f,1f), sequence.Duration()));
       sequence.Insert(2, transform.DOLocalMoveX(-954.9f, 4.0f));
       sequence.Insert(2, transform.DOLocalMoveY(-536.4f, 4.0f));
       sequence.Insert(2, transform.DORotate(new Vector3(0,0,0), 3));
    }
}
