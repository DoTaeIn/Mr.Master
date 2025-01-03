using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairRadius : MonoBehaviour
{
    BoxCollider2D boxCollider;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            if (other.gameObject.GetComponent<InteractData>() != null)
            {
                
            }
            else if (other.gameObject.GetComponent<TavernChair>() != null)
            {
                other.GetComponent<TavernChair>().isInRadius = true;
            }
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            Debug.Log("Exit Chair");
            if (other.gameObject.GetComponent<InteractData>() != null)
            {
                
            }
            else if (other.GetComponent<TavernChair>() != null && !other.gameObject.GetComponent<TavernChair>().isSelected)
            {
                other.gameObject.GetComponent<TavernChair>().isInRadius = false;
            }
            else
            {
                
            }
        }
    }
}
