using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PlayerCTRL : MonoBehaviour
{
    DrinkManager drinkManager;
    Rigidbody2D rb;
    Interactables interactables;
    CircleTransition circleTransition;
    TalkDatas talkDatas;
    public DialogueRunner dialogueRunner;
    
    
    [SerializeField] Animator animator;
    public NPC npc;
    UIManager uiManager;
    
    //Temporary Dictionary for making Cocktail & Drink
    Dictionary<float, Drink> currentDrink = new Dictionary<float, Drink>();
    
    //Movement
    [Header("Movement")]
    Vector2 movement;
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 3.0f;
    [SerializeField] private Transform playerUnitBody;
    
    [Header("Yarn Spinner")]
    [SerializeField] InMemoryVariableStorage variableStorage;

    [Header("Grab")] 
    public bool isGrabbing;
    public bool canGrab;
    public Transform parent;
    public Transform mapParent;
    public BoxCollider2D grabArea;
    public GameObject grabbedObject;
    
    //Limiting movement when interacting.
    public bool isInteracting;
    public bool canInteract;
    public int interactObjId = 0;
    
    
    public GameObject interactObj;
    
    private void Awake()
    {
        drinkManager = FindObjectOfType<DrinkManager>();
        rb = GetComponent<Rigidbody2D>();
        talkDatas = FindObjectOfType<TalkDatas>();
        circleTransition = FindObjectOfType<CircleTransition>();
    }

    /**
    private void Start()
    {
        variableStorage.SetValue("$playerName", "Mr.Master");
    }
    **/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && !isGrabbing)
        {
            canInteract = true;
            grabbedObject = other.gameObject;
            canGrab = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && !isGrabbing)
        {
            canInteract = false;
            grabbedObject = null;
            canGrab = false;
            if(other.gameObject.GetComponent<TavernChair>() !=null)
                other.gameObject.GetComponent<TavernChair>().isInteractable = canGrab;
            else if(other.gameObject.GetComponent<SpriteOutline>() !=null)
                other.gameObject.GetComponent<SpriteOutline>().UpdateOutline(canGrab);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && !isGrabbing)
        {
            canInteract = true;
        }
    }

    void FixedUpdate()
    {
        if (!isInteracting)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            if (movement.x > 0)
                playerUnitBody.rotation = Quaternion.Euler(0, 180, 0);
            else if (movement.x < 0)
                playerUnitBody.rotation = Quaternion.Euler(0, 0, 0);

            if (movement != Vector2.zero)
                animator.SetBool("isWalk", true);
            else
                animator.SetBool("isWalk", false);

            movement.Normalize();

            // Shift 키를 누르고 있는 동안 runSpeed를 적용
            if (Input.GetKey(KeyCode.LeftShift))
                rb.velocity = movement * runSpeed;
            else
                rb.velocity = movement * walkSpeed;
        }
    }

    void Update()
    {
        // Shift 키를 누르고 있는 동안 Run 애니메이션 활성화
        if (Input.GetKey(KeyCode.LeftShift))
            animator.SetBool("isRun", true);
        else
            animator.SetBool("isRun", false);
        
        if(isGrabbing)
            grabbedObject.transform.localPosition = new Vector3(0, 0, 1);
        
        if(grabbedObject != null && grabbedObject.GetComponent<TavernChair>() !=null)
            grabbedObject.GetComponent<TavernChair>().isInteractable = canGrab;
        else if(grabbedObject != null)
            grabbedObject.GetComponent<SpriteOutline>().UpdateOutline(canGrab);

        if (isInteracting && !isGrabbing)
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isWalk", false);
        }
            
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canInteract)
            {
                interactables = FindObjectOfType<Interactables>();
                //Interacting with Interactable Objs
                if (interactables != null)
                {
                    
                    if (uiManager == null)
                    {
                        uiManager = FindObjectOfType<UIManager>();
                    }
                    //TODO: interacting door.
                    if (interactables.GetInteractableType(interactObjId) == InteractableType.Door)
                    {
                        isInteracting = true;
                        uiManager.setActivePanelWName("circle", true);
                        interactObj.GetComponent<Animator>().SetBool("isOpening", true);
                        circleTransition.StartShrink();
                        interactObj = null;
                        isInteracting = false;
                        LoadSceneWithDelay("Tavern", 1f);
                        
                    }

                    if (interactables.GetInteractableType(interactObjId) == InteractableType.Sign)
                    {
                        //uiManager.setActivePanelWName("talkui", true);
                        Interactable temp = null;
                        foreach (Interactable interobj in interactables.interactables)
                        {
                            if (interobj.Id == interactObjId)
                            {
                                temp = interobj;
                            }
                        }
                        if(temp != null)
                            uiManager.setNameNTalk("나", temp.InteractLines, true);
                    }
                }
                
                //Interacting with NPC
                if (npc != null)
                {
                    isInteracting = true;
                    if (uiManager == null)
                    {
                        uiManager = FindObjectOfType<UIManager>();
                    }
                    Debug.Log("Test NPC");
                    
                    uiManager.setActivePanelWName("choiceui", true);
                    dialogueRunner = FindObjectOfType<DialogueRunner>();
                    if (dialogueRunner != null)
                        dialogueRunner.StartDialogue("100");
                    
                }
                
                //Interacting with OBJ to move
                if (canGrab && !isGrabbing)
                {
                    // Grabbing the object
                    Debug.Log("Grabbing object");
                    canGrab = false;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    grabbedObject.transform.SetParent(parent);
                    grabbedObject.transform.localPosition = Vector3.zero; // Position it relative to the player
                    isGrabbing = true;
                }
                else if (isGrabbing)
                {
                    // Releasing the object
                    Debug.Log("Releasing object");
                    grabbedObject.transform.SetParent(mapParent);
                    grabbedObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    canGrab = true;
                    isGrabbing = false;
                }
            }
        }
    }
    
    #region Drink
    public void AddDrinkToCocktail(float drinkAmount, Drink drink)
    {
        currentDrink.Add(drinkAmount, drink);
    }

    public void CreateCocktailRecipe(string name, float price)
    {
        // Calculate properties of the cocktail
        Color color = CalculateColor();
        float proof = CalculateProof();
        float amount = CalculateAmount();
        List<string> tastes = CalculateTastes();

        // Create the cocktail and add it to the DrinkManager
        drinkManager.cocktails.Add(new Cocktail(
            drinkManager.cocktails.Count,
            name,
            price,
            proof,
            amount,
            tastes,
            color,
            currentDrink
        ));

        // Clear currentDrink after creating the cocktail
        currentDrink.Clear();
    }

    private Color CalculateColor()
    {
        float totalAmount = CalculateAmount();
        float r = 0, g = 0, b = 0;

        foreach (var kvp in currentDrink)
        {
            float amount = kvp.Key;
            Drink drink = kvp.Value;

            r += drink.getColor().r * (amount / totalAmount);
            g += drink.getColor().g * (amount / totalAmount);
            b += drink.getColor().b * (amount / totalAmount);   
        }

        return new Color(r, g, b);
    }

    private float CalculateProof()
    {
        float totalAmount = CalculateAmount();
        float weightedProof = 0;

        foreach (var kvp in currentDrink)
        {
            float amount = kvp.Key;
            Drink drink = kvp.Value;

            weightedProof += drink.getProof() * (amount / totalAmount);
        }

        return weightedProof;
    }

    private float CalculateAmount()
    {
        float totalAmount = 0;

        foreach (var kvp in currentDrink)
        {
            totalAmount += kvp.Key; // Add the amount of each drink
        }

        return totalAmount;
    }

    private List<string> CalculateTastes()
    {
        HashSet<string> uniqueTastes = new HashSet<string>();

        foreach (var kvp in currentDrink)
        {
            Drink drink = kvp.Value;

            foreach (var taste in drink.getTastes())
            {
                uniqueTastes.Add(taste); // Add unique tastes
            }
        }

        return new List<string>(uniqueTastes);
    }
    

    #endregion
    
    
    
    public void LoadSceneWithDelay(string name, float time)
    {
        StartCoroutine(LoadSceneAfterDelay(name, time));
    }

    private IEnumerator LoadSceneAfterDelay(string name, float time)
    {
        yield return new WaitForSeconds(time); // Wait for the specified delay
        LoadingSceneCTRL.LoadScene(name); // Load the scene
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Interactable" && !isGrabbing)
        {
            canInteract = true;
            interactObj = collision.gameObject;
            if(collision.gameObject.GetComponent<InteractData>() != null)
                interactObjId = collision.gameObject.GetComponent<InteractData>().interactId;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Interactable" && !isGrabbing)
        {
            canInteract = false;
            interactObj = null;
            if(other.gameObject.GetComponent<InteractData>() != null)
                interactObjId = 0;
            
        }
    }
}
