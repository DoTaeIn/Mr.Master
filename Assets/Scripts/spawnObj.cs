using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObj : MonoBehaviour
{
    public bool isOnDump;
    public bool isDumpable;
    public bool isMouseOn;
    SpriteOutline outline;
    private int originalLayer;

    private void Awake()
    {
        outline = GetComponent<SpriteOutline>();
        originalLayer = gameObject.layer;
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
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        Camera mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("No main camera found! Ensure your Cinemachine camera is tagged as MainCamera.");
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
        Debug.Log(worldPos);
        worldPos.z = 0f;
        transform.position = new Vector3(worldPos.x, worldPos.y, 0f); 
    }

    private void OnMouseUp()
    {
        gameObject.layer = originalLayer;
        if (isOnDump)
            Destroy(gameObject);
        
    }
}
