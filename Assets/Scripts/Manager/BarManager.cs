using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BarManager : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D barbehind;
    [SerializeField] private PolygonCollider2D barfont;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private CinemachineVirtualCamera player;
    [SerializeField] private GameObject player2;
    [SerializeField] private Vector2 mixVec;
    [SerializeField] private Vector2 maxVec;
    
    [Header("Obj requirements")]
    public GameObject map2;
    public GameObject cocktail_cup;
    public GameObject deer_cup;
    
    
    public float scrollSpeed = 5f; // Speed of camera movement
    public float edgeBoundary = 40f; // Width of the edge boundary to trigger movement (in pixels)
    private Vector3 targetPosition;
    



    public void toBehind()
    {
        player.Follow = player2.transform;
        confiner.m_BoundingShape2D = barbehind;
        
    }

    public void toFont()
    {
        player.Follow = FindObjectOfType<PlayerCTRL>().gameObject.transform;
        confiner.m_BoundingShape2D = barfont;
    }

    private void Update()
    {
        if (FindObjectOfType<PlayerCTRL>().isBarBehind)
        {
            if (player2 == null) return;

            // Get the mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            // Check if the mouse is near the left or right edge
            if (mousePosition.x <= edgeBoundary) // Near the left edge
            {
                targetPosition += Vector3.left * scrollSpeed * Time.deltaTime;
            }
            else if (mousePosition.x >= screenWidth - edgeBoundary) // Near the right edge
            {
                targetPosition += Vector3.right * scrollSpeed * Time.deltaTime;
            }

            // Check if the mouse is near the top or bottom edge
            if (mousePosition.y <= edgeBoundary) // Near the bottom edge
            {
                targetPosition += Vector3.down * scrollSpeed * Time.deltaTime;
            }
            else if (mousePosition.y >= screenHeight - edgeBoundary) // Near the top edge
            {
                targetPosition += Vector3.up * scrollSpeed * Time.deltaTime;
            }

            // Clamp the target position to stay within boundaries
            targetPosition.x = Mathf.Clamp(targetPosition.x, mixVec.x, maxVec.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, mixVec.y, maxVec.y);

            // Smoothly update the follow target's position
            player2.transform.position = Vector3.Lerp(player2.transform.position, targetPosition, Time.deltaTime * scrollSpeed);
        }
    }
}

