using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnType
{
    Cup,
    Shaker
}

public class spawnObj : MonoBehaviour
{
    public bool isOnDump;
    public bool isDumpable;
    public bool isMouseOn;
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
        Camera mainCamera = Camera.main;
        uIManager.isDraging = true;
        uIManager.showDrinksPanel = true;
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found! Ensure your Cinemachine camera is tagged as MainCamera.");
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f); 
        
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        if (hit != null && hit.GetComponent<ClickableObj>() != null) // Adjust tag as needed
        {
            if (hit.GetComponent<ClickableObj>().type == ClickableObjType.Dump)
            {
                Debug.Log($"Overlapping with: {hit.name}");
                dumpObj = hit.GetComponent<ClickableObj>();
                isOnDump = true;
            }
            else
                isOnDump = false;
        }
    }

    private void OnMouseUp()
    {
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
            uIManager.showDrinksPanel = true;
        }
    }
}
