using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private List<YarnProject> MainStories;
    [SerializeField] private List<YarnProject> SubStories;
    
    private Dictionary<string, YarnProject> mainStory;
    private Dictionary<int, YarnProject> subStory;
    [SerializeField] List<string> mainStoryNode;
    public string selectedChar;
    public string currNode;
    DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = FindAnyObjectByType<DialogueRunner>();
        foreach (YarnProject proj in MainStories)
        {
            mainStory.Add("Mary", proj);
        }

        foreach (YarnProject proj in SubStories)
        {
            subStory.Add(1, proj);
        }
        
        updateYarnProject();
    }
    

    private YarnProject getSubprojWNPCId(int npcId)
    {
        return subStory[npcId];
    }

    private YarnProject getMainprojWNPCName(string npcName)
    {
        return mainStory[npcName];
    }

    public void updateYarnProject()
    {
        YarnProject mainproj = getMainprojWNPCName(selectedChar);
        foreach (string node in mainproj.NodeNames)
        {
            if (!node.Contains("-") && !mainStoryNode.Contains(node))
            {
                mainStoryNode.Add(node);
            }
        }
    }
    
}
