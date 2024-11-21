using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavern_Door : MonoBehaviour
{
    bool isInteractable = false;
    BoxCollider2D boxCollider;


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponent<PlayerCTRL>().isInteracting && other.GetComponent<PlayerCTRL>().isGrabbing)
            {
                isInteractable = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (!other.GetComponent<PlayerCTRL>().isInteracting && other.GetComponent<PlayerCTRL>().isGrabbing)
            {
                isInteractable = false;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isInteractable)
            {
                
            }
        }
    }
}
