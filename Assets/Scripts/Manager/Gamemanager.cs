using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Yarn.Unity;

public class Gamemanager : MonoBehaviour
{
    public static string[] dayofWeek = {"MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN"};
    private static int[] dateCounts = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
    public int dayofWeekIndex = 0;
    public int month;
    public int day;
    private int halfTime = 12;
    private float currentTime = 1.2f;
    private bool isAfternoon;
    [Range(0.001f, 1f)] [SerializeField] private float timesensitivity = 1f;
    [SerializeField] private Light2D sun;


    private List<NPC> npcs;
    UIManager uiManager;

    private void Awake()
    {
        uiManager = FindFirstObjectByType<UIManager>();
        npcs = FindObjectsByType<NPC>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList();
    }

    public string getDayOfWeek()
    {
        return dayofWeek[dayofWeekIndex];
    }

    public void nextDay()
    {
        day++;
        dayofWeekIndex = (dayofWeekIndex + 1) % dayofWeek.Length;
        foreach (NPC npc in npcs)
        {
            npc.updateDesireTaste();
        }
    }

    private void Update()
    {
        #region Time Control

        if (sun != null)
        {
            if (currentTime <= halfTime && !isAfternoon)
            {
                currentTime += Time.deltaTime * timesensitivity;
                sun.intensity = currentTime / halfTime;
                if(currentTime >= halfTime) 
                    isAfternoon = true;
            }
            else if(isAfternoon)
            {
                currentTime -= Time.deltaTime * timesensitivity;
                sun.intensity = currentTime / halfTime;
                if(currentTime <= 1.2f)     
                    isAfternoon = false;
            }
            
            Math.Clamp(currentTime, 1.2f, halfTime);
        }
        #endregion

        if (day > dateCounts[month])
        {
            month++;
            day = 1;
        }

        if (month > 12)
            month = 1;
    }

    
    [YarnCommand("setQuest")]
    public void setQuest(string questName, string questDescription)
    {
        if(uiManager == null)
            uiManager = FindFirstObjectByType<UIManager>();
        uiManager.setQuestText(questName, questDescription);
    }
    
    
}
