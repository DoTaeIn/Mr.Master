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
    public bool isMouseOn;
    private bool hasItemInHand;
    BarManager barManager;
    private GameObject dump;
    bool isGrabbing = false;



    [Header("Obj Reference")]
    public string title;
    [TextArea(20, 10)]
    public string description;

    public GameObject obj;
    UIManager uiManager;
    
    private void Awake()
    {
        spriteOutline = GetComponent<SpriteOutline>();
        barManager = FindObjectOfType<BarManager>();
        
    }

    private void Update()
    {
        spriteOutline.UpdateOutline(isMouseOn);
        
    }

    //Click Event Detection
    void OnMouseDrag()
    {
        switch (type)
        {
            case ClickableObjType.Shelf:
                SpawnObj(barManager.cocktail_cup);
                break;
            case ClickableObjType.Dump:
                break;
            case ClickableObjType.Pad:
                break;
            case ClickableObjType.Book:
                break;
            case ClickableObjType.Ice:
                break;
            default:
                break;
        }
    }

    void SpawnObj(GameObject spawnObj)
    {
        Camera mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No main camera found! Ensure your Cinemachine camera is tagged as MainCamera.");
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;
        if(obj == null)
            obj = Instantiate(spawnObj);
        obj.transform.SetParent(barManager.map2.transform);
        obj.transform.position = new Vector3(worldPos.x, worldPos.y, 0f); 
    }


    //Hide Info when mouse gets out
    void OnMouseExit()
    {
        isMouseOn = false;
        if(uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
        
        
        uiManager.startFollow = false;
        
        if(hasItemInHand)
            dump.GetComponent<spawnObj>().isOnDump = false;
    }

    //Show Info when mouse gets In
    void OnMouseEnter()
    {
        isMouseOn = true;
        if(uiManager == null)
            uiManager = FindObjectOfType<UIManager>();


        if (!hasItemInHand)
        {
            uiManager.startFollow = true; 
            uiManager.setTitleDes(title, description);
        }
        else
        {
            dump.GetComponent<spawnObj>().isOnDump = true;
        }
        
        
    }
    
    /**
    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (type == ClickableObjType.Dump)
        {
            if (trigger.GetComponent<spawnObj>() != null)
            {
                if (trigger.GetComponent<spawnObj>().isDumpable)
                {
                    hasItemInHand = true;
                    dump = trigger.gameObject;
                }
            }
        }
    }
    */

    private void OnMouseUp()
    {
        if (obj != null)
        {
            obj = null;
        }

        if (type == ClickableObjType.Drink)
        {
            if (!uiManager.drinksPanel.activeSelf)
            {
                uiManager.showDrinksPanel = true;
            }
        }
    }
    
}
