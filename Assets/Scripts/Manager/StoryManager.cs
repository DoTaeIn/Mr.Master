using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class StoryManager : MonoBehaviour
{
    public Dictionary<int, YarnProject> subStory;
    DialogueRunner dialogueRunner;

    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    public YarnProject getProjWNPCId(int npcId)
    {
        return subStory[npcId];
    }

    public void Test()
    {
        YarnProject projWNPCId = getProjWNPCId(0);
        if(dialogueRunner.IsDialogueRunning)
            dialogueRunner.Stop();
        
        dialogueRunner.StartDialogue(projWNPCId.NodeNames[0]);
        
            
    }
    
    
}
