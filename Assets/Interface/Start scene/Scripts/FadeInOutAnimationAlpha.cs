using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeInOutAnimationAlpha : MonoBehaviour
{
    private Image _spriteRenderer;
    private Image[] extrasSprites;
    public float fadeInTime, fadeOutTime, alphaValueIn, alphaValueOut, timeInBetween;
    public GameObject leftArrow, rightArrow, upArrow, downArrow;
    private GameObject currentArrow;
    public GameObject[] extras;
    public string arrowDirection;
    public bool timeOut = false;
    public bool arrowAnimationPlay = true;
    public bool soundEffect = false;
    private AudioSource soundFX;
    
    void OnEnable()
    {  
        soundFX = this.gameObject.GetComponent<AudioSource>();
        _spriteRenderer = gameObject.GetComponent<Image>();
        _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.56f);

        extrasSprites = new Image[4];
        int cont = 0;
        foreach (GameObject extra in extras)
        {
            extrasSprites[cont] = extra.GetComponent<Image>();
            extrasSprites[cont].color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.56f);
            if (cont < 4)
                cont++;
        }
        FadeInAnimation();
    }

    // Update is called once per frame
    void FadeInAnimation()
    {
        var sequence = DOTween.Sequence();
       sequence.Insert(0, t:_spriteRenderer.DOFade(alphaValueIn, fadeInTime));

       for (int i = 0; i < extrasSprites.Length; i++)
            sequence.Insert(0, t:extrasSprites[i].DOFade(alphaValueIn, fadeInTime));
            
        if (timeOut)
            StartCoroutine(BridgeTimeOut());
        else
            StartCoroutine(Bridge());
    }

    void FadeOutAnimation()
    {
       var sequence = DOTween.Sequence();
       sequence.Insert(0, t:_spriteRenderer.DOFade(alphaValueOut, fadeOutTime));

       for (int i = 0; i < extrasSprites.Length; i++)
            sequence.Insert(0, t:extrasSprites[i].DOFade(alphaValueOut, fadeOutTime));
    }

    IEnumerator BridgeTimeOut()
    {
        GameObject arrow = upArrow;

        if (soundEffect)
            soundFX.Play();

        if (arrowDirection == "left")
        {
            leftArrow.SetActive(true);
            arrow = leftArrow;
        } else if (arrowDirection == "right")
        {   
            rightArrow.SetActive(true);
            arrow = rightArrow;
        } else if (arrowDirection == "up")
        {
            upArrow.SetActive(true);
            arrow = upArrow;
        } else if (arrowDirection == "down")
        {
            downArrow.SetActive(true);
            arrow = downArrow;
        }

        yield return new WaitForSeconds(0.5f);
        arrow.SetActive(true);
        yield return new WaitForSeconds(timeInBetween);
        FadeOutAnimation();
        arrow.SetActive(false);
    }

    IEnumerator Bridge()
    {
        GameObject arrow = upArrow;

        if (soundEffect)
            soundFX.Play();

        if (arrowDirection == "left")
        {
            leftArrow.SetActive(true);
            arrow = leftArrow;
        } else if (arrowDirection == "right")
        {   
            rightArrow.SetActive(true);
            arrow = rightArrow;
        } else if (arrowDirection == "up")
        {
            upArrow.SetActive(true);
            arrow = upArrow;
        } else if (arrowDirection == "down")
        {
            downArrow.SetActive(true);
            arrow = downArrow;
        }

        yield return new WaitForSeconds(0.5f);
        arrow.SetActive(true);
        currentArrow = arrow;    
    }

    void Update()
    {
        if (arrowAnimationPlay == false){
            FadeOutAnimation();
            currentArrow.SetActive(false);
            arrowAnimationPlay = true;
        }
    }
}
