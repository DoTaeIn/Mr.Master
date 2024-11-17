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
        //TODO : Rewrite the code for NPC talking and Boolean Interactable objs like lights.
        //TODO: Make NPCs to move to bar. Limit movement from getting out from bar when working.
        //TODO: Create cocktail, give drink, create 주문서, 
        //TODO: flip tha bar
        
        interactables.Add(tavern_door);
        
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
