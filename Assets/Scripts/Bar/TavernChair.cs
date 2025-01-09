using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernChair : MonoBehaviour
{
    public int id;
    public bool isOccupied = false;
    public bool isInteractable = false;
    public NPC currentNPC = null;
    public bool isInRadius = true;
    public Transform sittingPos;
    public bool isSelected;
    
    BoxCollider2D boxCollider;
    SpriteOutline spriteOutline;
    PlayerCTRL player;
    

    private void Awake()
    {
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        player = FindObjectOfType<PlayerCTRL>();
        spriteOutline = GetComponent<SpriteOutline>();
    }

    private void Update()
    {
        isOccupied = (currentNPC != null);
        spriteOutline.UpdateOutline(isInteractable);
        boxCollider.gameObject.SetActive(isInRadius);
        boxCollider.gameObject.SetActive(isOccupied);
        
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        if (other.tag == "Player")
        {
            if (isOccupied)
            {
                //Debug.Log("Player entered the trigger zone");
                isInteractable = true;
                other.GetComponent<PlayerCTRL>().npc = currentNPC;
                other.GetComponent<PlayerCTRL>().canInteract = true;
            }
            else
            {
                isInteractable = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (isOccupied)
            {
                //Debug.Log("Player exited the trigger zone");
                isInteractable = false;
                other.GetComponent<PlayerCTRL>().npc = null;
                other.GetComponent<PlayerCTRL>().canInteract = false;
            }
            else
            {
                isInteractable = false;
            }
        }
    }

}
