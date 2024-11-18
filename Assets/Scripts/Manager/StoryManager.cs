using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class StoryManager : MonoBehaviour
{
    public Dictionary<int, YarnProject> subStory;

    public YarnProject getProjWNPCId(int npcId)
    {
        return subStory[npcId];
    }
    
    
}
