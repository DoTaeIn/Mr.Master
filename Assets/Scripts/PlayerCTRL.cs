using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCTRL : MonoBehaviour
{
    DrinkManager drinkManager;
    Rigidbody2D rb;
    Interactables interactables;
    CircleTransition circleTransition;
    TalkDatas talkDatas;
    
    
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


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canInteract)
            {
                interactables = FindObjectOfType<Interactables>();
                //Interacting with Interactable Objs
                isInteracting = true;
                if (interactables != null)
                {
                    if (uiManager == null)
                    {
                        uiManager = FindObjectOfType<UIManager>();
                    }
                    //TODO: interacting door.
                    if (interactables.GetInteractableType(interactObjId) == InteractableType.Door)
                    {
                        uiManager.setActivePanelWName("circle", true);
                        interactObj.GetComponent<Animator>().SetBool("isOpening", true);
                        circleTransition.StartShrink();
                        interactObj = null;
                        isInteracting = false;
                        LoadSceneWithDelay("Tavern", 1f);
                        
                    }

                    if (interactables.GetInteractableType(interactObjId) == InteractableType.Sign)
                    {
                        uiManager.setActivePanelWName("talkui", true);
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
                    if (uiManager == null)
                    {
                        uiManager = FindObjectOfType<UIManager>();
                    }
                    Debug.Log("Test NPC");
                    uiManager.setActivePanelWName("talkui", true);
                    TalkData temp = talkDatas.getTalkDataById(npc.talkDataIds[0]);
                    uiManager.setNameNTalk(npc.npcName, temp.Lines, false);
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
        SceneManager.LoadScene(name); // Load the scene
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Interactable")
        {
            canInteract = true;
            interactObj = collision.gameObject;
            if(collision.gameObject.GetComponent<InteractData>() != null)
                interactObjId = collision.gameObject.GetComponent<InteractData>().interactId;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Interactable")
        {
            canInteract = false;
            interactObj = null;
            if(other.gameObject.GetComponent<InteractData>() != null)
                interactObjId = 0;
        }
    }
}
