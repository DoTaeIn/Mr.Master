using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public List<Interactable> interactables;
    
    private void Start()
    {
        InitializeInteractables();
    }

    private void InitializeInteractables()
    {
        interactables = new List<Interactable>();
        
        
        //Exterior of Tavern. Initial code is 2--, if the number overs 2--, make code with 12--.
        Interactable tavern_door = new Interactable(200, "Tavern Door", "바 입구", true, InteractableType.Door);
        Interactable tavern_Tall_Sign = new Interactable(201, "Tavern Tall Sign", "영업 표지판", true, InteractableType.Sign, new List<string>() {
            "영업중 이라고 표시되어 있다. 영업 종료로 바꿀까?",
            "영업 종료로 바꾸었다.",
            "영업 종료라고 적혀있다. 영업중으로 바꿀까?",
            "높은 영업표지판에 겨우겨우 손을 뻗어  영업중으로 바꾸었다."
        });
        Interactable tavern_Small_Light = new Interactable(202, "Tavern Small Light", "작은 조명", true,
            InteractableType.Light, new List<string>()
            {
                "작은 조명이다. 불을 붙일까?",
                "조그만한 불이 들어왔다.",
                "작은 조명이다. 불을 끌까?",
                "조그만한 불을 불어 껏다."
            });
        
        
        
        interactables.Add(tavern_door);
        interactables.Add(tavern_Tall_Sign);
        interactables.Add(tavern_Small_Light);
    }

    
    
    //Returns Interactable with ID. Returns Null if there is non.
    public Interactable GetInteractable(int id)
    {
        for (int i = 0; i < interactables.Count; i++)
        {
            if (interactables[i].Id == id)
            {
                return interactables[i];
            }
        }
        return null;
    }

    public InteractableType GetInteractableType(int id)
    {
        return GetInteractable(id).Type;
    }
}
