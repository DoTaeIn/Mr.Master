using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        outline = GetComponent<SpriteOutline>();
        uIManager = FindObjectOfType<UIManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        outline.UpdateOutline(isMouseOn);

        if (isDragging)
            foreach (Collider2D c in hitCollider())
                isOnDump = (c.gameObject.name.ToLower() == "dump");
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
            uIManager.showDrinksList = false;
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
        //Debug.Log("TestTest");
        if (spawnType == SpawnType.Cup)
        {
            if (isOnDump)
            {
                CocktailShaker cd = FindFirstObjectByType<CocktailShaker>();
                cd.isEmpty = true;
                Destroy(gameObject);
                PlayerCTRL playerCtrl = FindFirstObjectByType<PlayerCTRL>();
                playerCtrl.currentDrink.Clear();    
            }
            isOnDump = false;
        }
        
        uIManager.isDraging = false;
        isDragging = false;
    }

    private void OnMouseDown()
    {
        if(spawnType == SpawnType.Shaker)
        {
            uIManager.showListPanel();
            uIManager.showDrinksList = true;
        }
        
        isDragging = true;
    }

    
    List<Collider2D> hitCollider()
    {
        // 현재 오브젝트의 sortingOrder 값
        int currentSortingOrder = spriteRenderer.sortingOrder;

        // 현재 위치와 겹치는 오브젝트를 찾기 위해 OverlapCircle 사용
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, 0.1f).ToList(); // 겹치는 오브젝트 탐지

        
        return colliders;
        
        /**
        foreach (Collider2D col in colliders)
        {
            // 자신은 무시
            if (col.gameObject == this.gameObject) continue;

            SpriteRenderer otherSpriteRenderer = col.GetComponent<SpriteRenderer>();

            if (otherSpriteRenderer != null)
            {
                // 다른 오브젝트의 sortingOrder 확인
                int otherSortingOrder = otherSpriteRenderer.sortingOrder;

                if (otherSortingOrder < currentSortingOrder) // 뒤에 렌더되는 경우
                {
                    Debug.Log("뒤에 있는 오브젝트 감지: " + col.gameObject.name);
                }
            }
            
        }
        */
    }
}
