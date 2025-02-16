using System;
using System.Collections;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    CircleTransition circleTransition;
    private void Awake()
    {
        circleTransition = FindFirstObjectByType<CircleTransition>();
        circleTransition.Init();
    }
    

    public void OnStartBtnClick()
    {
        
        Debug.Log("OnStartBtnClick");
        circleTransition.StartShrink();
        Invoke("waitAndLoadLevel", 1f);
    }

    void waitAndLoadLevel()
    {
        LoadingSceneCTRL.LoadScene("Map");
    }
}
