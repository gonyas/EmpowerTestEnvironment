using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInOutAnimation : MonoBehaviour
{
    private Image _spriteRenderer;
    public float fadeInTime, fadeOutTime, prependInterval;
    public GameObject text, effect, starEffect;
    public AudioSource sound;
    
    public void OnEnable()
    {  
        _spriteRenderer = gameObject.GetComponent<Image>();
        Animator anim = text.gameObject.GetComponent<Animator>();
        anim.Play("NextRoundAnim");
        FadeInAnimation();

    }

    // Update is called once per frame
    void FadeInAnimation()
    {
       sound.Play();
       var sequence = DOTween.Sequence();
       sequence.Insert(0,_spriteRenderer.DOFade(1.0f, fadeInTime));
       StartCoroutine(intervalAnim());
    }

    IEnumerator intervalAnim(){
        yield return new WaitForSeconds(0.9f);
        starEffect.SetActive(true);
        yield return new WaitForSeconds(prependInterval);
        fadeOutAnimation();
    }

    void fadeOutAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.Insert(0,_spriteRenderer.DOFade(0.0f, fadeOutTime));
        starEffect.SetActive(false);
    }
}
