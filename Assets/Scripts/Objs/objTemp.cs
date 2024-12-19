using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum tempCTRLType
{
    HOT,
    COLD
}

public class objTemp : MonoBehaviour
{
    public float temp;
    public float ratio;
    public tempCTRLType tempType;
    public bool isStart;

    public float minTemp = 0;
    public float maxTemp = 78;


    private void Update()
    {
        if (isStart)
        {
            if (tempType == tempCTRLType.HOT)
                temp += Time.deltaTime * ratio;
            else
                temp -= Time.deltaTime * ratio;
        }
        
        
        Math.Clamp(temp, minTemp, maxTemp);
            
    }
}
