using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class UIManager : MonoBehaviour
{
    [Header("UIs")]
    [SerializeField] private GameObject talkUI;
    private Dictionary<string, GameObject> talkUIDictionary = new Dictionary<string, GameObject>();
    public string currentPanelName;
    CircleTransition transition;
    
    [Header("Talk UI")]
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private TMP_Text talkTxt;
    [SerializeField] private Image person1_ImgL;
    [SerializeField] private Image person2_ImgR;
    //Text Output delay
    public float delay = 0.1f;
    
    [Header("Choice UI")]
    public GameObject[] dontDestory;
    public CinemachineVirtualCamera virtualCamera;
    private Camera mainCamera;
    
    [Header("Dialogue")]
    public List<DialogueRunner> dialogueRunners;
    public DialogueRunner currDialogueRunner;
    
    [Header("Bar UI")]
    DrinkManager drinkManager;
    [Space]
    //Info Panel
    public GameObject infoPanel;
    public TMP_Text title_Txt;
    public TMP_Text description_Txt;
    public bool startFollow;
    [Space]
    //Drink Panel
    public GameObject drinksPanel;
    public bool showDrinksPanel;
    public List<DrinkSO> Alcohols;
    public List<DrinkSO> NonAlcohols;
    public List<GarnishSO> Garnishs;
    public GameObject panelPrefab;
    public GameObject AlcoholsParent;
    public GameObject NonAlcoholsParent;
    [Space]
    //Child Dirnk Panel
    public bool isMeasureOpen;
    public GameObject measurePanel;
    public TMP_Text drinkName;
    public TMP_Text drinkAmount;
    public Slider drinkAmountSlider;
    private Cocktail newCocktail;
    [HideInInspector] public DrinkSO selectedDrink;

    [Space]
    //Shaker Panel
    public GameObject drinkListPanel;
    public GameObject drinksListPrefab;
    public Transform drinksListParent;
    public TMP_InputField drinkNameInput;
    public bool showDrinksList;
    public List<GameObject> drinkList;
    [SerializeField] private List<Image> amtImg;
    
    [Header("Date UI")]
    public TMP_Text dateText;
    private Gamemanager gm;
    
    //Current name of the talk
    private string name;
    
    //Text line of talk
    [Header("Talk")]
    private int maxTalk = 0;
    private int currentTalk = 0;
    private int maxPage = 0;
    private int currentPage = 0;
    private List<string> talks = new List<string>();
    private bool isDone;
    public bool isDraging = false;
    
    [Header("Quest")]
    [SerializeField] private TMP_Text questText;
    [SerializeField] private TMP_Text descriptionText;

    public PlayerCTRL player;
    
    
    //Get&Set
    public string Name { get => name; set => name = value; }
    public int MaxTalk { get => maxTalk; set => maxTalk = value; }
    private List<string> Talks { get => talks; set => talks = value; }
    

    public void setIsMeasureOpen(bool value)
    {
        isMeasureOpen = value;
        drinkAmountSlider.value = 0;
    }
    
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
        GameObject[] UIPanels = GameObject.FindGameObjectsWithTag("UIPanel");
        for(int i = 0; i < UIPanels.Length; i++)
        {
            talkUIDictionary.Add(UIPanels[i].name.ToLower(), UIPanels[i]);
            Debug.Log(UIPanels[i].name);
        }

        for (int i = 0; i < dontDestory.Length; i++)
        {
            DontDestroyOnLoad(dontDestory[i]);
        }
        
        currDialogueRunner.onDialogueComplete.AddListener(playerFinishInteract);
            
    }

    private void playerFinishInteract()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCTRL>();
        player.isInteracting = false;
    }

    private void Awake()
    {
        transition = FindObjectOfType<CircleTransition>();
        gm = FindObjectOfType<Gamemanager>();
        foreach (DrinkSO drink in Alcohols)
        {
            GameObject gm = Instantiate(panelPrefab, AlcoholsParent.transform);
            gm.GetComponent<drinkSelect>().currDrink = drink;
            gm.GetComponent<drinkSelect>().setTxt(drink.name, drink.proof, drink.tastes, drink.price, drink.amount, drink.MAXamount);
        }

        foreach (DrinkSO syrup in NonAlcohols)
        {
            GameObject gm = Instantiate(panelPrefab, NonAlcoholsParent.transform);
            gm.GetComponent<drinkSelect>().currDrink = syrup;
            gm.GetComponent<drinkSelect>().setTxt(syrup.name, syrup.proof, syrup.tastes, syrup.price, syrup.amount, syrup.MAXamount);
        }
    }
    
    

    private void Update()
    {
        //DATE CONTROL
        string dateFormat = gm.month.ToString("00") + " / " + gm.day.ToString("00") + "\n" + gm.getDayOfWeek();
        dateText.text = dateFormat;
        
        
        //Deactivate All Panels except last one called.
        int temp = 0;
        foreach (GameObject obj in talkUIDictionary.Values)
        {
            if(obj.activeSelf)
                temp++;
        }
            
        if (temp > 1)
        {
            foreach (var dict in talkUIDictionary)
            {
                if (dict.Key != currentPanelName)
                {
                    dict.Value.SetActive(false);
                }
            }
        }

        
        //Next Dialouge
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currDialogueRunner.IsDialogueRunning)
            {
                LineView lineView = FindFirstObjectByType<LineView>();
                lineView.OnContinueClicked();
            }
        }
        
        
        //InfoPanel follow
        if (startFollow)
        {
            infoPanel.SetActive(true);
            Vector2 mousePos = Input.mousePosition;

            // Adjust position based on whether it's out of view
            Vector2 newPos = IsPartiallyVisible(infoPanel.GetComponent<RectTransform>())
                ? new Vector2(mousePos.x - 200, mousePos.y - 130)
                : new Vector2(mousePos.x + 300, mousePos.y - 130);

            // Clamp position within screen bounds
            newPos.x = Mathf.Clamp(newPos.x, 0, Screen.width - infoPanel.GetComponent<RectTransform>().sizeDelta.x);
            newPos.y = Mathf.Clamp(newPos.y, 0, Screen.height - infoPanel.GetComponent<RectTransform>().sizeDelta.y);

            infoPanel.GetComponent<RectTransform>().position = newPos;
        }
        else
            infoPanel.SetActive(false);
        

        if (showDrinksPanel)
        {
            drinksPanel.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                showDrinksPanel = false;
            }
        }
        else
        {
            drinksPanel.SetActive(false);
        }
        
        measurePanel.SetActive(isMeasureOpen);
        //debugMouse();
        
        if(showDrinksPanel)
            drinkAmount.text = drinkAmountSlider.value.ToString() + "ml / " ;
        

        if (isDraging)
        {
            drinkListPanel.SetActive(true);
            Vector2 mousePos = Input.mousePosition;
            drinkListPanel.transform.position = new Vector2(mousePos.x + 250, mousePos.y);
        }

        if (!drinkAmountSlider.IsActive())
        {
            drinkAmountSlider.value = drinkAmountSlider.minValue;
        }

        foreach (DialogueRunner dia in dialogueRunners)
        {
            if(dia != currDialogueRunner)
                dia.gameObject.SetActive(false);
            else
                dia.gameObject.SetActive(true);
            
        }
        
    }
    
    public void HideDrinkList()
    {
        Debug.Log("Test");
        showDrinksList = false;
        drinkListPanel.SetActive(false);
    }

    public void showListPanel()
    {
        drinkListPanel.SetActive(true);
        Vector2 mousePos = Input.mousePosition;
        drinkListPanel.transform.position = new Vector2(mousePos.x + 250, mousePos.y);
    }

    public void setMeausrePanel(string title, string amount)
    {
        drinkName.text = title;
        drinkAmountSlider.maxValue = float.Parse(amount);
    }

    public void putDrink()
    {
        Drink temp = new Drink(selectedDrink.id, selectedDrink.name, selectedDrink.price, selectedDrink.proof, selectedDrink.amount, selectedDrink.color, selectedDrink.tastes);
        if(player == null)
            player = FindFirstObjectByType<PlayerCTRL>();
        
        player.AddDrinkToCocktail(drinkAmountSlider.value, temp);

        CocktailShaker cs = FindFirstObjectByType<CocktailShaker>();
        cs.isEmpty = false;
        
        GameObject gm = Instantiate(drinksListPrefab);
        drinkList.Add(gm);
        gm.transform.SetParent(drinksListParent);
        gm.GetComponent<drinkList>().drink = selectedDrink;
        gm.GetComponent<drinkList>().amount_fl = int.Parse(drinkAmountSlider.value.ToString());
        gm.GetComponent<drinkList>().Initiate();
        //Debug.Log(drinkAmountSlider.value + "ml");
    }
    
    

    public void SetShowDrinkPanel(bool show)
    {
        showDrinksPanel = show;
    }

    
    //Print name and talk lines in UI. 
    public void setNameNTalk(string name, List<string> talks, bool isChoice)
    {
        this.Name = name; // Store the name
        this.Talks = talks; // Store the talk lines
        this.MaxTalk = talks.Count - 1; // Set the maximum talk index

        // Set the initial line of the conversation
        currentTalk = 0;
        nameTxt.text = name; // Display the name
        if(!isChoice)
            StartCoroutine(PrintTextOneByOne(talks[currentTalk]));
    }
    
    public string mouseHitString()
    {
        Camera cam = Camera.main;
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        // Cast a ray at the mouse position
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

        if (hit.collider != null)
        {
            return hit.collider.gameObject.name;
        }
        else
        {
            return "";
        }
    }


    //[OUTDATED]
    //Void for button on talk panel. It will show next line if available, 
    //Will show next page if overflows,
    //and next Will end Talk when end of conversation.
    /**
    public void nextLine()
    {
        // If text is still printing, skip to the full text immediately
        if (!isDone && talkTxt.text != Talks[currentTalk])
        {
            StopAllCoroutines(); // Stop the ongoing printing coroutine
            talkTxt.text = Talks[currentTalk]; // Show the full text
            isDone = true; // Mark as done
            return;
        }

        // Handle text overflow: Show the next page if available
        if (talkTxt.isTextOverflowing && talkTxt.pageToDisplay < talkTxt.textInfo.pageCount)
        {
            talkTxt.pageToDisplay++; // Move to the next page
            return;
        }

        // If no more pages, move to the next line
        if (talkTxt.pageToDisplay == talkTxt.textInfo.pageCount)
        {
            talkTxt.pageToDisplay = 1; // Reset page display

            if (currentTalk < MaxTalk)
            {
                currentTalk++; // Go to the next talk line
                StartCoroutine(PrintTextOneByOne(Talks[currentTalk]));
            }
            else
            {
                // If no more lines, end the talk
                EndTalk();
            }
        }
    }
    **/
    private void EndTalk()
    {
        currentTalk = 0; // Reset current talk
        talkTxt.pageToDisplay = 1; // Reset page display
        setActivePanelWName(currentPanelName, false); // Deactivate the current panel
        Debug.Log("Talk ended."); // Optional debug log
        player = FindObjectOfType<PlayerCTRL>();
        player.isInteracting = false;
    }

    public void setTitleDes(string title, string description)
    {
        title_Txt.text = title;
        description_Txt.text = description;
    }
    
    //Void that prints word one by one.
    IEnumerator PrintTextOneByOne(string line)
    {
        talkTxt.text = ""; // Clear the current text
        isDone = false; // Mark as not done

        foreach (char c in line)
        {
            talkTxt.text += c; // Add one character at a time
            yield return new WaitForSeconds(delay); // Wait before adding the next character
        }

        isDone = true; // Mark as done
    }


    
    //Get UI GameObj that has same name
    GameObject GetKeyByValue(string name)
    {
        foreach (var pair in talkUIDictionary)
        {
            if (pair.Key == name) // Compare value
                return pair.Value;     // Return the key
        }

        // If the value is not found
        throw new KeyNotFoundException("The value was not found in the dictionary.");
    }

    
    //Set active with name.
    public void setActivePanelWName(string name, bool active)
    {
        currentPanelName = name;
        GameObject panel = GetKeyByValue(name);
        panel.SetActive(active);
    }
    
    
    //Detects SceneLoading
    void OnEnable()
    {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
    
    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    // Callback method triggered when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
    }
    
    private void OnActiveSceneChanged(Scene previousScene, Scene currentScene)
    {
        // Check if this is the scene you want to handle
        if (currentScene.name == "Tavern")
        {
            player = FindObjectOfType<PlayerCTRL>();
            player.isInteracting = true;
            Debug.Log("Test");
            setActivePanelWName("circle", true);
            transition.StartExpand();

            StartCoroutine(ActivateBarUIWithDelay());
        }
    }
    
    private IEnumerator ActivateBarUIWithDelay()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Activate the second panel and assign cameras
        setActivePanelWName("barui", true);
        mainCamera = Camera.main;
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        player = FindObjectOfType<PlayerCTRL>();
        player.isInteracting = false;
    }
    
    private bool IsPartiallyVisible(RectTransform rectTransform)
    {
        if (mainCamera == null) return false;

        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        foreach (var corner in worldCorners)
        {
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(corner);

            // If any corner is inside the viewport, the object is partially visible
            if (viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
                viewportPoint.y >= 0 && viewportPoint.y <= 1)
            {
                return true;
            }
        }

        // None of the corners are in the viewport
        return false;
    }


    public void showIntensity(int intensity)
    {
        for (int i = 0; i < intensity / 10; i++)
        {
            amtImg[i].color = Color.blue;
        }

        for (int i = intensity / 10; i < 5; i++)
        {
            amtImg[i].color = Color.white;
        }
    }

    public DialogueRunner setDialogueRunnderBName(string name)
    {
        foreach (DialogueRunner obj in dialogueRunners)
        {
            if (obj.name == name)
            {
                currDialogueRunner = obj;
                //Debug.Log("Dialogue runner: " + obj.name);
                return obj;
            }
            
        }
        return null;
    }

    public void setQuestText(string title, string description)
    {
        questText.text = title;
        descriptionText.text = description;
    }
    
}
