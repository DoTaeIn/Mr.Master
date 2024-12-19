using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class StoryManager : MonoBehaviour
{
    public Dictionary<string, YarnProject> mainStory;
    List<string> mainStoryNode;
    public Dictionary<int, YarnProject> subStory;
    public string selectedChar;
    public int currNode;
    DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    private YarnProject getSubprojWNPCId(int npcId)
    {
        return subStory[npcId];
    }

    private YarnProject getMainprojWNPCName(string npcName)
    {
        return mainStory[npcName];
    }

    public void initYarnProject()
    {
        YarnProject mainproj = getMainprojWNPCName(selectedChar);
        
        foreach (string node in mainproj.NodeNames)
        {
            if (!node.Contains("-"))
            {
                mainStoryNode.Add(node);
            }
        }

    }
    

    
    
    
    
    
}
