using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        StartCoroutine(Loadmapscene());
    }

    IEnumerator Loadmapscene()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Map");
    }

    public void SkipIntro()
    {
        SceneManager.LoadScene("Map");
    }
}