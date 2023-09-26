using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EmpowerDLLs;

public class DemoSequenceForestCtrl : MonoBehaviour
{
    public GameCtrl gameCtrl;
    public GameObject target;
    public GameObject[] distractors;
    public Transform positionTarget;
    public Transform positionDistractor;
    public GameObject textBubble;
    public TextMeshProUGUI bubbleText;
    public GameObject bubbleImage;
    public AudioSource correctSound;
    public GameObject skipButton;
    public GameObject selectTarget;
    public GameObject selectDistractor;
    public GameObject nextButton;
    public GameObject repeatButton;
    public GameObject welldonePanel;

    bool waitForKey = false;
    int level = 0;
    Coroutine demoSeq = null;
    bool nextStep = false;
    bool repeatTrial = false;
    private string wordType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        skipButton.SetActive(false);
        repeatButton.SetActive(false);
        welldonePanel.SetActive(false);
        target.SetActive(false);
        selectTarget.SetActive(false);
        selectDistractor.SetActive(false);
        nextButton.SetActive(false);
        foreach (GameObject g in distractors)
            g.SetActive(false);

        if (PersistentData.CurrentLevelAttention > 0 && PersistentData.CurrentLevelAttention <= 4)
            level = 1;
        else if (PersistentData.CurrentLevelAttention > 4 && PersistentData.CurrentLevelAttention < 11)
            level = 2;
        else if (PersistentData.CurrentLevelAttention >= 11)
            level = 3;
        demoSeq = StartCoroutine(PlayDemoSequence());
    }

    private void OnDisable()
    {
        if (demoSeq != null)
            StopCoroutine(demoSeq);
        if (textBubble)
            textBubble.SetActive(false);
        if (skipButton)
            skipButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && waitForKey == true)
        {
            waitForKey = false;
        }

        if (waitForKey == true && Input.GetMouseButtonDown(0))
        {
            waitForKey = false;
        }
    }

    IEnumerator PlayDemoSequence()
    {
        gameCtrl.endDemo = false; gameCtrl.skipDemo = false;
        waitForKey = false;
        nextButton.SetActive(false);

        //DEMO_FOREST_1
        //string description = "Let's review how to play with this game. If you know the instructions, you can skip this demostration clicking the SKIP BUTTON on the top right corner.";
        wordType = "DEMO_FOREST_1";
        string description = Dictionary.getWord(wordType, PersistentData.LanguageSelected);
        bubbleText.text = "";
        skipButton.SetActive(true);

        textBubble.SetActive(true);
        // foreach (char c in description)
        // {
        //     bubbleText.text += c;
        //     yield return new WaitForSeconds(0.1f);
        // }
        bubbleText.text = description;

        nextStep = false;
        yield return new WaitForSeconds (2.0f);
        nextButton.SetActive(true);
        yield return new WaitUntil(() => nextStep == true);

        //DEMO_FOREST_2
        //description = "First you have to review the forest and search for the target, a mushroom like this";
        wordType = "DEMO_FOREST_2";
        description = Dictionary.getWord(wordType, PersistentData.LanguageSelected);
        bubbleText.text = description;
        //bubbleText.text = "";
        // foreach (char c in description)
        // {
        //     bubbleText.text += c;
        //     yield return new WaitForSeconds(0.1f);
        // }
        bubbleImage.SetActive(true);

        nextStep = false;
        yield return new WaitForSeconds (2.0f);
        nextButton.SetActive(true);
        yield return new WaitUntil(() => nextStep == true);

        textBubble.SetActive(false);
        bubbleImage.SetActive(false);

        //distractors[0].transform.position = positionDistractor.position;
        distractors[level - 1].SetActive(true);

        yield return new WaitForSeconds(1.5f);
        selectDistractor.SetActive(true);
        
        //DEMO_FOREST_3
        textBubble.SetActive(true);
        //description = "If you see an object different from the target, you have to keep looking and wait for the next object. Look, this is not the target, keep looking and wait for the next item.";
        wordType = "DEMO_FOREST_3";
        description = Dictionary.getWord(wordType, PersistentData.LanguageSelected);
        bubbleText.text = description;
        // bubbleText.text = "";
        // foreach (char c in description)
        // {
        //     bubbleText.text += c;
        //     yield return new WaitForSeconds(0.1f);
        // }

        nextStep = false;
        yield return new WaitForSeconds (2.0f);
        nextButton.SetActive(true);
        yield return new WaitUntil(() => nextStep == true);

        distractors[level - 1].SetActive(false);
        selectDistractor.SetActive(false);
        correctSound.Play();
        textBubble.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        target.SetActive(true);
        //target.transform.position = positionTarget.position;
        yield return new WaitForSeconds(1.5f);

        //DEMO_FOREST_4
        textBubble.SetActive(true);
        //description = "If you see the target, you have to press the space bar in the keyboard. Look, now there is a correct mushroom, please press the space bar...";
        wordType = "DEMO_FOREST_4";
        selectTarget.SetActive(true);

        description = Dictionary.getWord(wordType, PersistentData.LanguageSelected);
        bubbleText.text = description;
        // bubbleText.text = "";
        // foreach (char c in description)
        // {
        //     bubbleText.text += c;
        //     yield return new WaitForSeconds(0.1f);
        // }

        waitForKey = true;
        yield return new WaitUntil(() => waitForKey == false);

        textBubble.SetActive(false);
        correctSound.Play();
        yield return new WaitForSeconds(1.5f);
        target.SetActive(false);
        selectTarget.SetActive(false);

        //DEMO_FOREST_5
        textBubble.SetActive(true);
        //description = "If you press the space bar when you see the correct mushroom, you will hear a correct sound.";
        wordType = "DEMO_FOREST_5";
        description = Dictionary.getWord(wordType, PersistentData.LanguageSelected);
        bubbleText.text = description;
        // bubbleText.text = "";
        // foreach (char c in description)
        // {
        //     bubbleText.text += c;
        //     yield return new WaitForSeconds(0.1f);
        // }

        nextStep = false;
        yield return new WaitForSeconds (2.0f);
        nextButton.SetActive(true);
        yield return new WaitUntil(() => nextStep == true);

        //DEMO_FOREST_6
        //description = "Now it is time to practice the game before you start playing. Let's practice!";
        wordType = "DEMO_FOREST_6";
        description = Dictionary.getWord(wordType, PersistentData.LanguageSelected);
        bubbleText.text = description;
        // bubbleText.text = "";
        // foreach (char c in description)
        // {
        //     bubbleText.text += c;
        //     yield return new WaitForSeconds(0.1f);
        // }

        nextStep = false;
        yield return new WaitForSeconds (2.0f);
        nextButton.SetActive(true);
        yield return new WaitUntil(() => nextStep == true);

        do
        {
            nextButton.SetActive(false);
            repeatButton.SetActive(false);
            textBubble.SetActive(false);
            skipButton.SetActive(false);
            selectTarget.SetActive(false);
            selectDistractor.SetActive(false);

            gameCtrl.endTrial = false;

            gameCtrl.PlayTest();
            yield return new WaitUntil(() => gameCtrl.endTrial == true);

            welldonePanel.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            welldonePanel.SetActive(false);

            //DEMO_FOREST_7_1 & 7_2 & 7_3
            //description = "You have " + gameCtrl.GetHits() + " correct answers and " + gameCtrl.GetErrors() + " incorrect answers.\nDo you want to start playing or repeat the trial?";
            description = Dictionary.getWord("DEMO_FOREST_7_1", PersistentData.LanguageSelected) + gameCtrl.GetHits() + Dictionary.getWord("DEMO_FOREST_7_2", PersistentData.LanguageSelected) + gameCtrl.GetErrors() + Dictionary.getWord("DEMO_FOREST_7_3", PersistentData.LanguageSelected);
            textBubble.SetActive(true);
            bubbleText.text = description;
            // bubbleText.text = "";
            // foreach (char c in description)
            // {
            //     bubbleText.text += c;
            //     yield return new WaitForSeconds(0.05f);
            // }

            nextStep = repeatTrial = false;
            yield return new WaitForSeconds (2.0f);
            nextButton.SetActive(true);
            repeatButton.SetActive(true);
            yield return new WaitUntil(() => nextStep == true || repeatTrial == true);

        } while (nextStep == false);

        repeatButton.SetActive(false);
        //DEMO_FOREST_8
        //description = "LET's PLAY!!!!";
        wordType = "DEMO_FOREST_8";
        description = Dictionary.getWord(wordType, PersistentData.LanguageSelected);
        textBubble.SetActive(true);
        bubbleText.text = description;
        // bubbleText.text = "";
        
        // foreach (char c in description)
        // {
        //     bubbleText.text += c;
        //     yield return new WaitForSeconds(0.05f);
        // }

        textBubble.SetActive(false);
        skipButton.SetActive(false);
        selectTarget.SetActive(false);
        selectDistractor.SetActive(false);

        gameCtrl.endDemo = true;
    }

    public void ButtonSkipClick ()
    {
        if (demoSeq != null)
            StopCoroutine(demoSeq);
        textBubble.SetActive(false);
        skipButton.SetActive(false);
        target.SetActive(false);
        selectTarget.SetActive(false);
        foreach (GameObject g in distractors)
            g.SetActive(false);
        selectDistractor.SetActive(false);
        gameCtrl.endDemo = true;
        gameCtrl.skipDemo = true;
    }

    public void NextStepButtonClick()
    {
        nextStep = true;
        nextButton.SetActive(false);
    }

    public void RepeatButtonClick()
    {
        repeatTrial = true;
        repeatButton.SetActive(false);
    }
}
