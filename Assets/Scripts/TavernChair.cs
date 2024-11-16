using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernChair : MonoBehaviour
{
    public int id;
    bool isOccupied = false;
    public bool isInteractable = false;
    public NPC currentNPC = null;
    
    BoxCollider2D boxCollider;
    PlayerCTRL player;

    private void Awake()
    {
        boxCollider = GetComponentInChildren<BoxCollider2D>();
        player = FindObjectOfType<PlayerCTRL>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player entered the trigger zone");
            isInteractable = true;
            other.GetComponent<PlayerCTRL>().npc = currentNPC;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Debug.Log("Player exited the trigger zone");
            isInteractable = false;
            other.GetComponent<PlayerCTRL>().npc = null;
        }
    }

}
