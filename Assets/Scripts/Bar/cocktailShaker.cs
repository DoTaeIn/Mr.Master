using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CocktailShaker : MonoBehaviour
{
    [SerializeField] private bool isFirst;
    [SerializeField] private Vector2 pos;
    [SerializeField] private Vector2 lastVel;
    [SerializeField] private int count;
    [SerializeField] private float time;
    [SerializeField] private float intensity = 1;
    [SerializeField] private int distance = 5;
    public bool isEmpty;
    

    private Rigidbody2D rb;
    private UIManager _uiManager;
    public PlayerCTRL ctrl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _uiManager = FindFirstObjectByType<UIManager>();
        ctrl = FindFirstObjectByType<PlayerCTRL>();
    }

    void OnMouseDown()
    {
        if (isFirst)
        {
            pos = gameObject.transform.position;
            lastVel = rb.linearVelocity;
            isFirst = false;
        }
    }

    void OnMouseUp()
    {
        isFirst = true;
        time = 0;
    }

    private void OnMouseDrag()
    {
        Vector2 temp = new Vector2(lastVel.x * rb.linearVelocity.x, lastVel.y * rb.linearVelocity.y);
        time += Time.deltaTime;
        if(temp.x <= 0 || temp.y <= 0)
            if (Vector2.Distance(transform.position, pos) >= distance && Vector2.Distance(transform.position, pos)/time >= intensity)
            {
                count++;
                pos = gameObject.transform.position;
                time = 0;
            }
            
    }

    private void Update()
    {
        _uiManager.showIntensity(count);

        foreach (Collider2D c in hitCollider())
        {
            if (c.gameObject.CompareTag("Cup"))
            {
                if (!isEmpty)
                {
                    Cup cup = c.GetComponent<Cup>();
                    if(ctrl == null)
                        ctrl = FindFirstObjectByType<PlayerCTRL>();
                    cup.currCocktail = ctrl.CreateCocktailRecipe("test", 10f, count / 10);
                    Debug.Log(cup.currCocktail.Drinks.Count);
                    isEmpty = true;
                    for (int i = _uiManager.drinkList.Count - 1; i >= 0; i--)
                    {
                        Destroy(_uiManager.drinkList[i]);
                        _uiManager.drinkList.RemoveAt(i);
                    }
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
