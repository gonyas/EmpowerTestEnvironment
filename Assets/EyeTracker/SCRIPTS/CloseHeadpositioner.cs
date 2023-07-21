using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseHeadpositioner : MonoBehaviour
{
    public void CloseButton(){
        HeadPositioner.Instance.CloseHeadPosition();
    }
}
