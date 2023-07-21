using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EmpowerDLLs;

public class LevelCtrl : MonoBehaviour
{
    public Button[] listLevelButtons;
    public Color selectedColour;
    Image[] listLevelImage;
    public GameObject ctrl;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        listLevelButtons = this.gameObject.GetComponentsInChildren<Button>();
        listLevelImage = this.gameObject.GetComponentsInChildren<Image>();
        switch (PersistentData.CurrentGameType)
        {
            case ActivityType.ATTENTION: ClickButton(PersistentData.CurrentLevelAttention);
                break;
            case ActivityType.WORKMEMORY: ClickButton(PersistentData.CurrentLevelWorkingMemory);
                break;
            case ActivityType.INHIBITION: ClickButton(PersistentData.CurrentLevelInhibition);
                break;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickButton(int level)
    {
        int indexColour = level;

        foreach (Image i in listLevelImage)
            i.color = Color.gray;
       
        switch (PersistentData.CurrentGameType)
        {
            case ActivityType.ATTENTION:
                if (level == 4)
                    indexColour = 2;
                else if (level == 7)
                    indexColour = 3;
                PersistentData.CurrentLevelAttention = level;
                ctrl.gameObject.GetComponent<ForestGameCtrlChild>().changeLevel(level);
                break;
            case ActivityType.WORKMEMORY:
                PersistentData.CurrentLevelWorkingMemory = level;
                break;
            case ActivityType.INHIBITION:
                PersistentData.CurrentLevelInhibition = level;
                break;
        }
        listLevelImage[indexColour - 1].color = selectedColour;//Color.yellow;
    }
}