using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NavMeshPlus.Components;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;


public class PlayerCTRL : MonoBehaviour
{
    public DrinkManager drinkManager;
    Rigidbody2D rb;
    [SerializeField] Interactables interactables;
    CircleTransition circleTransition;
    TalkDatas talkDatas;
    public DialogueRunner dialogueRunner;
    BarManager barManager;
    NavMeshSurface navMeshSurface;
    
    
    [SerializeField] Animator animator;
    public NPC npc;
    UIManager uiManager;
    
    //Temporary Dictionary for making Cocktail & Drink
    public Dictionary<float, Drink> currentDrink = new Dictionary<float, Drink>();
    //public List<string> testDrinks = new List<string>();
    //Currently holding cocktail. 
    Cocktail currCotail;
    private int shakeAMT;
    
    //Movement
    [Header("Movement")]
    Vector2 movement;
    public float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 3.0f;
    
    [Header("Yarn Spinner")]
    [SerializeField] InMemoryVariableStorage variableStorage;

    [Header("Grab")] 
    public List<GameObject> inArea;
    public bool isGrabbing;
    public bool canGrab;
    public Transform parent;
    public Transform grabPoint;
    public Transform mapParent;
    public BoxCollider2D grabArea;
    public GameObject grabbedObject;
    
    [Header("Bar_Behind")]
    public bool isBarBehind;
    public bool canGoBehind;
    
    //Limiting movement when interacting.
    public bool isInteracting;
    public bool canInteract;
    public int interactObjId = 0;
    
    public GameObject ItemHolder;
    public GameObject interactObj;
    
    [Header("Money")]
    public float money;
    
    SpriteRenderer spriteRenderer;
    SaveSystem saveSystem;
    private void Awake()
    {
        drinkManager = FindFirstObjectByType<DrinkManager>();
        rb = GetComponent<Rigidbody2D>();
        talkDatas = FindFirstObjectByType<TalkDatas>();
        circleTransition = FindFirstObjectByType<CircleTransition>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        saveSystem = FindFirstObjectByType<SaveSystem>();
    }

    
    private void Start()
    {
        //variableStorage.SetValue("$playerName", "Mr.Master");
        //Debug.Log(JsonUtility.ToJson((saveSystem.saveGameObject(this.gameObject), Formatting.Indented)));
        //saveSystem.saveToFileTest(saveSystem.saveGameObject(this.gameObject));

        saveSystem.saveDataToFile(1);


    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && !isGrabbing && other.GetComponent<InteractData>() == null)
        {
            canInteract = true;

            // Add object to the list if it's not already present
            if (!inArea.Contains(other.gameObject))
                inArea.Add(other.gameObject);

            // Update the grabbed object (select the closest or any prioritized object)
            UpdateGrabbedObject();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && !isGrabbing && other.GetComponent<InteractData>() == null)
        {
            canInteract = false;

            // Remove the object from the list
            if (inArea.Contains(other.gameObject))
                inArea.Remove(other.gameObject);

            // Update the grabbed object (in case the removed object was the grabbed one)
            UpdateGrabbedObject();

            // Handle object-specific logic
            if (other.gameObject.GetComponent<TavernChair>() != null)
                other.gameObject.GetComponent<TavernChair>().isInteractable = false;
            else if (other.gameObject.GetComponent<SpriteOutline>() != null)
                other.gameObject.GetComponent<SpriteOutline>().UpdateOutline(false);
        }
    }

    private void UpdateGrabbedObject()
    {
        // Find the prioritized object (e.g., closest object) dynamically
        grabbedObject = inArea
            .OrderBy(obj => Vector2.Distance(this.transform.position, obj.transform.position)) // Closest object
            .FirstOrDefault();

        // Update grab status
        canGrab = grabbedObject != null;

        // Optional: Handle object-specific logic for the new grabbed object
        if (grabbedObject != null)
        {
            if (grabbedObject.GetComponent<TavernChair>() != null)
                grabbedObject.GetComponent<TavernChair>().isInteractable = canGrab;
            else if (grabbedObject.GetComponent<SpriteOutline>() != null)
                grabbedObject.GetComponent<SpriteOutline>().UpdateOutline(canGrab);
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
            {
                parent.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.12f, -0.06f);
                grabPoint.localPosition = new Vector2(0.318f, 0.203f);
                spriteRenderer.flipX = true;
            }
            else if (movement.x < 0)
            {
                parent.gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.12f, -0.06f);
                grabPoint.localPosition = new Vector2(-0.318f, 0.203f);
                spriteRenderer.flipX = false;
            }

            if (movement != Vector2.zero && rb.linearVelocity != Vector2.zero)
                animator.SetBool("isWalk", true);
            else
                animator.SetBool("isWalk", false);

            movement.Normalize();

            // Shift 키를 누르고 있는 동안 runSpeed를 적용
            if (Input.GetKey(KeyCode.LeftShift))
                rb.linearVelocity = movement * runSpeed;
            else
                rb.linearVelocity = movement * walkSpeed;
        }
    }

    void Update()
    {
        // Shift 키를 누르고 있는 동안 Run 애니메이션 활성화
        if (Input.GetKey(KeyCode.LeftShift) && rb.linearVelocity.magnitude > walkSpeed)
            animator.SetBool("isRun", true);
        else
            animator.SetBool("isRun", false);
        
        if(isGrabbing)
            grabbedObject.transform.localPosition = new Vector3(0, 0, 1);
        
        if(grabbedObject != null && grabbedObject.GetComponent<TavernChair>() !=null)
            grabbedObject.GetComponent<TavernChair>().isInteractable = canGrab;
        else if(grabbedObject != null && grabbedObject.GetComponent<InteractData>() ==null)
            grabbedObject.GetComponent<SpriteOutline>().UpdateOutline(canGrab);

        if (isInteracting && !isGrabbing)
        {
            animator.SetBool("isRun", false);
            animator.SetBool("isWalk", false);
            rb.linearVelocity = Vector2.zero;
        }
            
        

        if (Input.GetKeyDown(KeyCode.E))
        {
            //Interacting Algorithm
            //In Bar - In front of the bar
            if (canInteract && !isBarBehind)
            {
                interactables = FindFirstObjectByType<Interactables>();
                Debug.Log("Start interacting");
                if (inArea.Count != 0)
                {
                    for (int i = 0; i < inArea.Count; i++)
                    {
                        Debug.Log(inArea[i].name);
                    }

                    InteractData temp =
                        inArea.OrderBy(obj => Vector2.Distance(transform.position, obj.transform.position)).ToList()[0]
                            .GetComponent<InteractData>();
                    
                    if(temp != null)
                        interactObjId = 
                            temp.interactId;
                }
                
                //Interacting with Interactable Objs
                if (interactables != null && npc == null)
                {
                    Debug.Log("Found Interactables");
                    //Find UI
                    if (uiManager == null)
                        uiManager = FindFirstObjectByType<UIManager>();

                    if (barManager == null)
                        barManager = FindFirstObjectByType<BarManager>();
                    
                    
                    //TODO: interacting door - DONE
                    if (interactables.GetInteractable(interactObjId) != null)
                    {
                        if (interactables.GetInteractableType(interactObjId) == InteractableType.Door)
                        {
                            isInteracting = true;
                            uiManager.setActivePanelWName("circle", true);
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
                    
                }
                
                //If Player can go behind, go for it
                if (canGoBehind)
                {
                    if(barManager == null)
                        barManager = FindFirstObjectByType<BarManager>();
                    barManager.toBehind();
                    isInteracting = true;
                    isBarBehind = true;
                }
                
                //Interacting with NPC
                if (npc != null && !isGrabbing)
                {
                    isInteracting = true;
                    if (uiManager == null)
                        uiManager = FindFirstObjectByType<UIManager>();
                    
                    //Debug.Log("Test NPC");

                    uiManager.setActivePanelWName("choiceui", true);
                    dialogueRunner = uiManager.setDialogueRunnderBName($"{npc.npcName} Dialogue Runner");

                    if (currCotail != null)
                    {
                        if (npc.checkLoveHate(currCotail.Tastes) > 0)
                        {
                            npc.currDialogueIndex += "-0";
                            dialogueRunner.StartDialogue(npc.currDialogueIndex);
                        }
                        else
                        {
                            npc.currDialogueIndex += "-1";
                            dialogueRunner.StartDialogue(npc.currDialogueIndex);
                        }
                    }
                    
                    if (dialogueRunner != null && !npc.hasTalked)
                    {
                        //dialogueRunner.VariableStorage.SetValue("$playerName", "Mr.Master");
                        //dialogueRunner.VariableStorage.SetValue("$npcName", "Mary");


                        dialogueRunner.StartDialogue(npc.currDialogueIndex);
                        npc.hasTalked = true;
                    }
                    else if(npc.hasTalked)
                        isInteracting = false;
                }
                
                //Interacting with Obj that can grab
                else if (canGrab && !isGrabbing)
                {
                    // Grabbing the object
                    Debug.Log("Grabbing object");
                    canGrab = false;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    grabbedObject.transform.SetParent(grabPoint);
                    grabbedObject.transform.localPosition = Vector3.zero; // Position it relative to the player
                    isGrabbing = true;
                }
                // Releasing the object
                else if (isGrabbing)
                {
                    
                    Debug.Log("Releasing object");
                    grabbedObject.transform.SetParent(mapParent);
                    grabbedObject.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
                    grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    canGrab = true;
                    isGrabbing = false;
                    if(navMeshSurface == null)
                        navMeshSurface = FindFirstObjectByType<NavMeshSurface>();
                    
                    navMeshSurface.BuildNavMesh();
                }
                
            }
            
            //In Bar - Behind of the bar
            else if (canInteract && isBarBehind)
            {
                if(barManager == null)
                    barManager = FindFirstObjectByType<BarManager>();
                barManager.toFont();
                isInteracting = false;
                isBarBehind = false;
                if(currCotail != null)
                    barManager.initItemHolderPos();
            }
        }
    }
    
    #region Drink
    public void AddDrinkToCocktail(float drinkAmount, Drink drink)
    {
        currentDrink.Add(drinkAmount, drink);
        Debug.Log(currentDrink.Count);
    }

    public Cocktail CreateCocktailRecipe(string name, float price, int shakeAmt)
    {
        // Calculate properties of the cocktail
        Color color = CalculateColor();
        float proof = CalculateProof();
        float amount = CalculateAmount();
        List<string> tastes = CalculateTastes();
        
        if (drinkManager == null)
            drinkManager = FindFirstObjectByType<DrinkManager>();
        

        if (drinkManager.cocktails == null)
        {
            Debug.LogError("drinkManager.cocktails is null.");
        }
        if (tastes == null || color == null || currentDrink == null)
        {
            Debug.LogError("One or more calculated properties are null.");
        }

        if (currentDrink.Count == 0)
        {
            Debug.LogError("No Drink available.");
        }

        // Create the cocktail and add it to the DrinkManager
        Dictionary<float, Drink> drinks = new Dictionary<float, Drink>(currentDrink);
        
        Cocktail temp = new Cocktail(
            drinkManager.cocktails.Count,
            name ,
            price,
            proof,
            amount,
            tastes,
            color,
            drinks ,
            shakeAmt
        );
        
        currCotail = temp;
        drinkManager.cocktails.Add(temp);
        
        // Clear currentDrink after creating the cocktail
        currentDrink.Clear();
        
        return temp;
    }

    private Color CalculateColor()
    {
        float totalAmount = CalculateAmount();
        float r = 0, g = 0, b = 0;

        foreach (var kvp in currentDrink)
        {
            float amount = kvp.Key;
            Drink drink = kvp.Value;

            r += drink.Color.r * (amount / totalAmount);
            g += drink.Color.g * (amount / totalAmount);
            b += drink.Color.b * (amount / totalAmount);   
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

            weightedProof += drink.Proof * (amount / totalAmount);
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

            foreach (var taste in drink.Tastes)
            {
                if(!uniqueTastes.Contains(taste))
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
        if (collision.gameObject.CompareTag("Interactable") && !isGrabbing)
        {
            if (!inArea.Contains(collision.gameObject))
                inArea.Add(collision.gameObject);
            canInteract = true;
            interactObj = collision.gameObject;
            if(collision.gameObject.GetComponent<InteractData>() != null)
                interactObjId = collision.gameObject.GetComponent<InteractData>().interactId;
        }

        if (collision.gameObject.CompareTag("Bar_Behind"))
        {
            canInteract = true;
            canGoBehind = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Interactable") && !isGrabbing)
        {
            if (inArea.Contains(other.gameObject))
                inArea.Remove(other.gameObject);
            
            canInteract = false;
            interactObj = null;
            if(other.gameObject.GetComponent<InteractData>() != null)
                interactObjId = 0;
            
        }
        
        if (other.gameObject.CompareTag("Bar_Behind"))
        {
            canInteract = false;
            canGoBehind = false;
        }
    }
}
