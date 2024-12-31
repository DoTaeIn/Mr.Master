using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public Cocktail currCocktail;
    private SpriteRenderer spriteRenderer;
    BarManager barManager;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        barManager = FindObjectOfType<BarManager>();
    }

    
    //TODO: Need to fix move to bar front
    private void Update()
    {
        if(currCocktail != null)
            spriteRenderer.color = currCocktail.Color;


        foreach (Collider2D col in hitCollider())
        {
            if (col.gameObject.GetComponent<ClickableObj>() != null)
            {
                if (col.gameObject.GetComponent<ClickableObj>().type == ClickableObjType.returnBlock)
                {
                    Debug.Log("Test");
                    barManager.currentCup = this.gameObject;
                }
            }
        }
    }
    
    List<Collider2D> hitCollider()
    {
        
        //TODO: Does not overlap, and has Nullreference Error on player script 334.
        // 현재 위치와 겹치는 오브젝트를 찾기 위해 OverlapCircle 사용
        List<Collider2D> colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f).ToList(); // 겹치는 오브젝트 탐지
        return colliders;
    }
}
