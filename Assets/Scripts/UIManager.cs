using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    
    [Header("Dialogue")]
    DialogueRunner dialogueRunner;
    
    //Current name of the talk
    private string name;
    
    //Text line of talk
    private int maxTalk = 0;
    private int currentTalk = 0;
    private int maxPage = 0;
    private int currentPage = 0;
    private List<string> talks = new List<string>();
    private bool isDone;

    private PlayerCTRL player;

    
    //Get&Set
    public string Name { get => name; set => name = value; }
    public int MaxTalk { get => maxTalk; set => maxTalk = value; }
    private List<string> Talks { get => talks; set => talks = value; }

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
        
        dialogueRunner.onDialogueComplete.AddListener(playerFinishInteract);
            
    }

    private void playerFinishInteract()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCTRL>();
        player.isInteracting = false;
    }

    private void Awake()
    {
        transition = FindObjectOfType<CircleTransition>();
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private void Update()
    {
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
        else
        {
            
        }
    }


    
    //Void for button on talk panel. It will show next line if available, 
    //Will show next page if overflows,
    //and next Will end Talk when end of conversation.
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
    
    private void EndTalk()
    {
        currentTalk = 0; // Reset current talk
        talkTxt.pageToDisplay = 1; // Reset page display
        setActivePanelWName(currentPanelName, false); // Deactivate the current panel
        Debug.Log("Talk ended."); // Optional debug log
        player = FindObjectOfType<PlayerCTRL>();
        player.isInteracting = false;
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
    }
    
    void OnDisable()
    {
        // Unsubscribe to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Callback method triggered when a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log($"Scene changed to: {scene.name}");
        transition.StartExpand();
        dialogueRunner.onDialogueComplete.AddListener(playerFinishInteract);
    }
    
}
