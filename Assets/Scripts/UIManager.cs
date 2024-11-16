using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UIs")]
    [SerializeField] private GameObject talkUI;
    private Dictionary<string, GameObject> talkUIDictionary = new Dictionary<string, GameObject>();
    private string currentPanelName;
    
    [Header("Talk UI")]
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] private TMP_Text talkTxt;
    [SerializeField] private Image person1_ImgL;
    [SerializeField] private Image person2_ImgR;
    //Text Output delay
    public float delay = 0.1f;
    
    //Current name of the talk
    private string name;
    
    //Text line of talk
    private int maxTalk = 0;
    private int currentTalk = 0;
    private int maxPage = 0;
    private int currentPage = 0;
    private List<string> talks = new List<string>();
    private bool isDone;

    
    //Get&Set
    public string Name { get => name; set => name = value; }
    public int MaxTalk { get => maxTalk; set => maxTalk = value; }
    private List<string> Talks { get => talks; set => talks = value; }

    private void Start()
    {
        GameObject[] UIPanels = GameObject.FindGameObjectsWithTag("UIPanel");
        for(int i = 0; i < UIPanels.Length; i++)
        {
            talkUIDictionary.Add(UIPanels[i].name.ToLower(), UIPanels[i]);
            Debug.Log(UIPanels[i].name);
        }
            
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


    public void setNameNTalk(string name, List<string> talks)
    {
        nameTxt.text = name;

        for (int i = 0; i < talks.Count; i++)
        {
            StartCoroutine(PrintTextOneByOne(talks[i]));
        }
    }

    public void nextLine()
    {
        if (!isDone)
        {
            if (talkTxt.isTextOverflowing && talkTxt.textInfo.pageCount > talkTxt.pageToDisplay )
            {
                talkTxt.pageToDisplay++;
            }
            else if (talkTxt.textInfo.pageCount == talkTxt.pageToDisplay)
            {
                talkTxt.pageToDisplay = 1;
            }
            else if (!talkTxt.isTextOverflowing)
            {
                if (currentTalk > maxTalk)
                {
                    currentTalk = 0;
                    if (currentPage > maxPage)
                    {
                        currentPage = 0;
                        isDone = true;
                    }
                    else
                        currentPage++;
                }
                else
                    currentTalk++;
            }
        }
        else
        {
            isDone = false;
            setActivePanelWName(currentPanelName, false);
        }
    }

    IEnumerator PrintTextOneByOne(string line)
    {
        talkTxt.text = ""; // Clear the current text
        for (int i = 0; i < line.Length; i++)
        {
            talkTxt.text += line[i]; // Add one character at a time
            yield return new WaitForSeconds(delay); // Wait before adding the next character
        }
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
    
}
