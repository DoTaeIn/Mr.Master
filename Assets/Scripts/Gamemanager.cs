using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Gamemanager : MonoBehaviour
{
    
    private int halfTime = 12;
    private float currentTime = 1.2f;
    private bool isAfternoon;
    [Range(0.001f, 1f)] [SerializeField] private float timesensitivity = 1f;

    [SerializeField] private Light2D sun;

    private void Update()
    {
        #region Time Control
        if (currentTime <= halfTime && !isAfternoon)
        {
            currentTime += Time.deltaTime * timesensitivity;
            sun.intensity = currentTime / halfTime;
            if(currentTime >= halfTime) 
                isAfternoon = true;
        }
        else if(isAfternoon)
        {
            currentTime -= Time.deltaTime * timesensitivity;
            sun.intensity = currentTime / halfTime;
            if(currentTime <= 1.2f)     
                isAfternoon = false;
        }
        
        Math.Clamp(currentTime, 1.2f, halfTime);
        #endregion
    }
    
}
