using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public enum SpawnType
{
    Cup,
    Shaker,
    Ice
}

public class spawnObj : MonoBehaviour
{
    public bool isOnDump;
    public bool isDumpable;
    public bool isMouseOn;
    public bool isDragging;
    public SpawnType spawnType;
    UIManager uIManager;
    SpriteOutline outline;
    ClickableObj dumpObj;

    private void Awake()
    {
        outline = GetComponent<SpriteOutline>();
        uIManager = FindObjectOfType<UIManager>();
    }

    private void Update()
    {
        outline.UpdateOutline(isMouseOn);

        if (isDragging)
        {
            Debug.Log(hitCollider().gameObject.name);
        }
    }

    private void OnMouseEnter()
    {
        isMouseOn = true;
        
    }

    private void OnMouseExit()
    {
        isMouseOn = false;
    }

    private void OnMouseDrag()
    {
        isDragging = true;
        Camera mainCamera = Camera.main;
        if (spawnType == SpawnType.Shaker)
        {
            uIManager.isDraging = true;
            uIManager.showDrinksList = true;
        }
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found! Ensure your Cinemachine camera is tagged as MainCamera.");
            return;
        }
        
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f); 
        
        /**
        Collider2D hitObj = hitCollider();
        Debug.Log(hitObj);
        
        if (hitObj != null && hitObj.GetComponent<ClickableObj>() != null) // Adjust tag as needed
        {
            if (hitObj.GetComponent<ClickableObj>().type == ClickableObjType.Dump)
            {
                Debug.Log($"Overlapping with: {hitObj.name}");
                dumpObj = hitObj.GetComponent<ClickableObj>();
                isOnDump = true;
            }
            else
            {
                isOnDump = false;
                Debug.Log($"Overlapping with: {hitObj.name}");
            }
            
                
        }
        */
    }

    private void OnMouseUp()
    {
        Debug.Log("TestTest");
        if (spawnType == SpawnType.Cup)
        {
            if (isOnDump)
                Destroy(gameObject);
            isOnDump = false;
        }
        
        uIManager.isDraging = false;
    }

    private void OnMouseDown()
    {
        if(spawnType == SpawnType.Shaker)
        {
            uIManager.showListPanel();
            uIManager.showDrinksList = true;
        }
        
        isDragging = false;
    }

    Collider2D hitCollider()
    {
        Camera mainCamera = Camera.main;
        
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        
        return hit;
    }
}
