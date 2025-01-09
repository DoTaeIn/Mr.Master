using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;

public class BarManager : MonoBehaviour
{
    [Header("Toggle bar")]
    [SerializeField] private PolygonCollider2D barbehind;
    [SerializeField] private PolygonCollider2D barfont;
    [SerializeField] private CinemachineConfiner2D confiner;
    public CinemachineCamera player;
    [SerializeField] private GameObject player2;
    [SerializeField] private Vector2 mixVec;
    [SerializeField] private Vector2 maxVec;
    public float scrollSpeed = 5f; // Speed of camera movement
    public float edgeBoundary = 40f; // Width of the edge boundary to trigger movement (in pixels)
    private Vector3 targetPosition;
    
    
    [Header("Obj requirements")]
    public GameObject map2;
    public GameObject cocktail_cup;
    public GameObject ice;
    public GameObject beer_cup;
    
    [Header("Current Cup")]
    public GameObject currentCup;
    
    PlayerCTRL playerCTRL;
    UIManager uIManager;

    void Awake()
    {
        playerCTRL = FindFirstObjectByType<PlayerCTRL>();
        uIManager = FindFirstObjectByType<UIManager>();
    }

    private void Start()
    {
        foreach (DialogueRunner dia in uIManager.dialogueRunners)
        {
            dia.onNodeStart.RemoveAllListeners();
            dia.onNodeComplete.RemoveAllListeners();
            dia.onNodeStart.AddListener(FocusOnPlayer);
            dia.onNodeComplete.AddListener(UnFocusOnPlayer);
        }
    }


    public void toBehind()
    {
        player.Follow = player2.transform;
        player.Lens.OrthographicSize = 2.56f;
        confiner.BoundingShape2D = barbehind;
    }

    public void toFont()
    {
        player.Follow = FindFirstObjectByType<PlayerCTRL>().gameObject.transform;
        player.Lens.OrthographicSize = 3.79f;
        confiner.BoundingShape2D = barfont;
    }

    public void FocusOnPlayer(string nodeName)
    {
        player.Lens.OrthographicSize = 2;
        uIManager.isDiaStart = true;
    }

    public void UnFocusOnPlayer(string nodeName)
    {
        player.Lens.OrthographicSize = 3.79f;
        uIManager.isDiaStart = false;
    }

    private void Update()
    {
        if (FindFirstObjectByType<PlayerCTRL>().isBarBehind)
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

        if (currentCup != null)
        {
            if (!FindFirstObjectByType<PlayerCTRL>().isBarBehind)
            {
                
            }
        }
    }

    public void initItemHolderPos()
    {
        Destroy(currentCup.GetComponent<ClickableObj>());
        Destroy(currentCup.GetComponent<SpriteOutline>());
        Destroy(currentCup.GetComponent<spawnObj>());
        currentCup.transform.parent = playerCTRL.ItemHolder.transform;
        Vector3 newPosition = currentCup.transform.position;
        newPosition.x = 0;
        newPosition.y = 0;
// Optionally set newPosition.z if needed
        currentCup.transform.localPosition = newPosition;

        
    }
    
    public void OnDialogueLine(LocalizedLine line)
    {
        // Access the character name
        string characterName = line.CharacterName;

        if (!string.IsNullOrEmpty(characterName))
        {
            Debug.Log($"Character speaking: {characterName}");
        }
        else
        {
            Debug.Log("No character name specified for this line.");
        }

        // Access the dialogue text
        Debug.Log($"Line text: {line.Text}");
    }

}

