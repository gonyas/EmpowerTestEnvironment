using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayasAnimationsTrigger : MonoBehaviour
{
    public GameObject rRoja, rNegra, rAmarilla, rAzul, goBackLogoTrigger, mapSceneTrigger; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        rRoja.gameObject.GetComponent<RayaRojaAnimations>().PlayAnimation();
        rNegra.gameObject.GetComponent<RayaNegraAnimations>().PlayAnimation();
        rAmarilla.gameObject.GetComponent<RayaAmarillaAnimations>().PlayAnimation();
        rAzul.gameObject.GetComponent<RayaAzulAnimations>().PlayAnimation();

        goBackLogoTrigger.SetActive(true);
        mapSceneTrigger.SetActive(true);
    }
}
