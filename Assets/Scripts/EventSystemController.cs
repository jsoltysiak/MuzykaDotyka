using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystemController : MonoBehaviour
{
    public void ButtonPlayEvent()
    {
        GameManager.Instance.StartButton();
    }
    
    public void ButtonNextEvent()
    {
        GameManager.Instance.NextButton();
    }
}
