using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class CustomLineView : LineView
{
    public List<Image> playerImages;
    
    UIManager uiManager;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
    }

    private void Update()
    {
        foreach (Image img in playerImages)
        {
            if(img.gameObject.name == uiManager.currSpeaker)
                img.gameObject.SetActive(true);
            else
                img.gameObject.SetActive(false);
        }
            
    }

    /** DEPRICATAED
    public Image isExsist(string name)
    {
        foreach (Image image in playerImages)
        {
            if(image.name == name)
                return image;
        }
        
        return null;
    }*/
}
