using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClickableObjType
{
    None,
    Drink,
    Shelf,
    Dump,
    Pad,
    Book,
    Ice
}

public class ClickableObj : MonoBehaviour
{
    public ClickableObjType type;
    SpriteOutline spriteOutline;
    private bool isMouseOn;

    public string title;
    [TextArea(20, 10)]
    public string description;
    
    UIManager uiManager;
    
    private void Awake()
    {
        spriteOutline = GetComponent<SpriteOutline>();
    }

    private void Update()
    {
        spriteOutline.UpdateOutline(isMouseOn);
    }

    //Click Event Detection
    void OnMouseDown()
    {
        Debug.Log($"Click! {gameObject.name}");
    }
    
    

    //Hide Info when mouse gets out
    void OnMouseExit()
    {
        isMouseOn = false;
        if(uiManager == null)
            uiManager = FindObjectOfType<UIManager>();

        uiManager.startFollow = false;
    }

    //Show Info when mouse gets In
    void OnMouseEnter()
    {
        isMouseOn = true;
        if(uiManager == null)
            uiManager = FindObjectOfType<UIManager>();

        uiManager.startFollow = true; 
        uiManager.setTitleDes(title, description);
    }
}
