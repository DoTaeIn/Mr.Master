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
        Debug.Log(gameObject.name);
    }

    //Show Info when mouse gets In
    void OnMouseEnter()
    {
        isMouseOn = true;
        Debug.Log(gameObject.name);
    }
}
