using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartAnimation : MonoBehaviour
{
    private Image logoSprite, effectSprite, arrowSprite;
    public GameObject logo, effect, wood, arrow;
    public GameObject[] credits;
    public AudioSource arrowSFX, fenceSFX, BGMSound;
    private Image[] creditSprites;
    private float logoFadeInTime, effectFadeInTime, creditsFadeInTime;
    
    void Awake()
    {  
        logoFadeInTime = 0.5f;
        effectFadeInTime = 1.5f;
        creditsFadeInTime = 0.5f;

        logoSprite = logo.gameObject.GetComponent<Image>();
        effectSprite = effect.gameObject.GetComponent<Image>();
        arrowSprite = arrow.gameObject.GetComponent<Image>();

        creditSprites = new Image[1];
        for (int i=0; i < credits.Length; i++){
            creditSprites[i] = credits[i].gameObject.GetComponent<Image>();
        }

        StartCoroutine(AwakeAnimation());
    }

    IEnumerator AwakeAnimation()
    {
        yield return new WaitForSeconds(0.8f);
        BGMSound.Play();
        yield return new WaitForSeconds(0.7f);
        EffectAnimation();
        wood.gameObject.GetComponent<Animator>().Play("Wood");
        LogoAnimation();
        fenceSFX.Play();
        yield return new WaitForSeconds(2.0f);
        arrowSFX.Play();
        ArrowAnimation();
        CreditsAnimation();
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene("Interface");
    }

    void EffectAnimation()
    {
       var sequence = DOTween.Sequence();
       sequence.Insert(0, t:effectSprite.DOFade(1.0f, effectFadeInTime));
    }

    void LogoAnimation()
    {
       var sequence = DOTween.Sequence();
       sequence.Insert(0, t:logoSprite.DOFade(1.0f, effectFadeInTime));
    }

    void ArrowAnimation()
    {
       var sequence = DOTween.Sequence();
       sequence.Insert(0, t:arrowSprite.transform.DOLocalMoveX(332.7f, 2.0f));
       sequence.Insert(0, t:arrowSprite.transform.DOLocalMoveY(228.3f, 2.0f));
    }

    void CreditsAnimation()
    {
       var sequence = DOTween.Sequence();
       for (int i = 0; i < creditSprites.Length; i++){
            sequence.Insert(0, t:creditSprites[i].DOFade(1.0f, creditsFadeInTime));
       }
    }
}
