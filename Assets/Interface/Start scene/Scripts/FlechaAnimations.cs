using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlechaAnimations : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D rigidb;
    private float timeBtwSpawns = 0.05f;
    private float startTimeBtwSpawns;
    public GameObject echo;
    
    void Awake()
    {  
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigidb = gameObject.GetComponent<Rigidbody2D>();
        Animation();

    }

    void Update()
    {
        /*if (!rigidb.IsSleeping()){
            if (timeBtwSpawns <= 0){
                GameObject instance = (GameObject)Instantiate(echo, transform.position, Quaternion.identity);
                Destroy(instance, 8f);
                timeBtwSpawns = startTimeBtwSpawns;
            } else {
                timeBtwSpawns -= Time.deltaTime;
            }
        }*/

    }

    // Update is called once per frame
    void Animation()
    {
       var sequence = DOTween.Sequence();
       sequence.PrependInterval(7.0f);
       //sequence.Insert(0, t:_spriteRenderer.DOColor(Color.red, 0.1f));
       // Add a movement tween at the beginning
       //sequence.Append(transform.DOMoveX(45, 1));
       // Add a rotation tween as soon as the previous one is finished
       //sequence.Append(transform.DORotate(new Vector3(0,180,0), 1));
       // Delay the whole Sequence by 1 second
       //sequence.PrependInterval(1);
       // Insert a scale tween for the whole duration of the Sequence
       //sequence.Insert(2, transform.DOScale(new Vector3(2f,2f,2f), sequence.Duration()));
       sequence.Insert(2, transform.DOLocalMoveX(-951.15f, 4.0f));
       sequence.Insert(2, transform.DOLocalMoveY(-538.11f, 4.0f));
    }

    public void PlayBackToLogoAnimation()
    {
        var sequence = DOTween.Sequence();
        sequence.PrependInterval(9.0f);
        sequence.Append(transform.DOLocalMoveX(-964.2f, 3.0f));
        sequence.Append(transform.DOLocalMoveY(-535.9f, 1.0f));
        sequence.Append(transform.DOScale(new Vector3(1.0f,1.0f,1.0f),1.0f));
    }
}
