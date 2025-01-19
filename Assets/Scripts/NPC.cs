using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Random = UnityEngine.Random;

public enum NPCEmotion
{
    Satisfied,
    Unsatisfied,
    HaveSmth,
    LightlyDrunk,
    HeavilyDrunk,
    Happy,
    Sad
}


[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    [Header("NPC Info")]
    public int npcId;
    public string npcName;
    public YarnProject yarnLine;
    public List<string> like_favor;
    public List<string> desire_favor;
    public List<string> hate_favor;
    
    [Header("NPC Motion")]
    public List<TavernChair> chairs;
    [SerializeField] bool isPlanning2Sit = false;
    [SerializeField] private GameObject leg_R;
    [SerializeField] private GameObject leg_L;
    private Vector2 currPos;
    private Transform sttingPos;
    
    [Header("NPC Emotion")]
    [SerializeField] NPCEmotion emotion;
    [SerializeField] List<GameObject> emotionGM;
    Dictionary<NPCEmotion, GameObject> emotions = new Dictionary<NPCEmotion, GameObject>();
    [SerializeField] GameObject[] bubbles;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] TMP_Text npc_short_Line;
    [SerializeField] List<Sprite> emotionSprites; //0: Thinking, 1: Happy, 2: Sad, 3: Question, 4: Surprise

    [Header("Love Point")] 
    public Dictionary<int, int> lovePoints = new Dictionary<int, int>(); //NPCID, How much they like them 0: player

    [Header("NPC Dialogue")] 
    public bool hasTalked;
    public string currDialogueIndex;
    public int index = 11;
    
    NavMeshAgent navMeshAgent;
    Animator animator;
    Rigidbody2D rb;
    private TavernChair randomPos;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        //Navmesh default setting
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }
    

    private void Start()
    {
        moveToRandomChair();

        foreach (string name in yarnLine.NodeNames)
        {
            Debug.Log(name);
        }
        lovePoints.Add(0, 0);
        
        NPCEmotion[] enumValues = (NPCEmotion[])System.Enum.GetValues(typeof(NPCEmotion));

        // Populate the dictionary dynamically
        for (int i = 0; i < enumValues.Length; i++)
        {
            emotions.Add(enumValues[i], emotionGM[i]);
        }
    }


    private void Update()
    {
        //Debug.Log(navMeshAgent.velocity.magnitude);
        //Animation
        if(navMeshAgent.velocity.magnitude > 0.1f)
        {
            //Debug.Log("Walking");
            animator.SetBool("isWalk", true);
            if (navMeshAgent.velocity.x < 0)
            {
                // Moving left: flip horizontally
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (navMeshAgent.velocity.x > 0)
            {
                // Moving right: reset to normal scale
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            //Debug.Log("Stoped");
            animator.SetBool("isWalk", false);
        }
        
        
        
     
        
    }
    
    /**
     * <summary>
     * Returns Yarn Project Nodes in form of List<string>
     * <returns>List<string></returns>
     * </summary>
     */
    public List<string> yarnNodes()
    {
        return yarnLine.NodeNames.ToList();
    }

    
    /**
    * <summary>
    * Returns randome chair posigion in form of TavernChair
    * <returns>TavernChair</returns>
    * </summary>
    */
    public TavernChair getRandomChairPos()
    {
        List<TavernChair> temp = new List<TavernChair>();

        foreach (TavernChair chair in chairs)
        {
            if(!chair.isOccupied && chair.isInRadius)
                temp.Add(chair);
        }
        
        int tempint = Random.Range(0, temp.Count);
        temp[tempint].isSelected = true;
        return temp[tempint];
    }
    
    
    /**
     * <summary>
     * i is for what sprite to active. (Emotion Sprites)
     * j is for what panel to active. (Speach or Think)
     * </summary>
     */
    public void activeSmallInteraction(int i, int j, string line)
    {
        for (int k = 0; k < bubbles.Length; k++)
        {
            if (k != j)
            {
                bubbles[k].SetActive(false);
            }
            else
            {
                bubbles[k].SetActive(true);
            }
        }

        if (j == 0)
        {
            npc_short_Line.text = line;
        }
        else
        {
            sr.sprite = emotionSprites[j];
        }
    }

    
    //Make it look like its sitting.
    public void sittingMotion(bool isSitting)
    {
        if (isSitting)
        {
            currPos = transform.position;
            navMeshAgent.enabled = false;
            randomPos.GetComponent<CircleCollider2D>().enabled = false;
            leg_R.transform.localPosition = new Vector3(-2.980232e-08f, -1.490116e-08f, 0f);
            leg_R.transform.localRotation = Quaternion.Euler(0f, 0f, -63.732f);
            
            leg_L.transform.localPosition = new Vector2(-0.078f, -0.005f);
            leg_L.transform.localRotation = Quaternion.Euler(0f, 0f, -65.997f);
            
           gameObject.transform.SetParent(randomPos.transform);
           gameObject.transform.localPosition = Vector3.zero;
           rb.constraints = RigidbodyConstraints2D.FreezeAll;
           
            isPlanning2Sit = false;
        }
        else
        {
            gameObject.transform.SetParent(null);
            gameObject.transform.position = currPos;
            leg_R.transform.localPosition = new Vector2(0, 0);
            leg_R.transform.localRotation = Quaternion.Euler(0f, 0f, 0);
            
            leg_L.transform.localPosition = new Vector2(0, 0);
            leg_L.transform.localRotation = Quaternion.Euler(0f, 0f, 0);
            navMeshAgent.enabled = true;
            randomPos.GetComponent<CircleCollider2D>().enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            randomPos.isSelected = false;
        }
    }
    
    
    //Uses navmesh to move to random pos of chair
    [YarnCommand("MoveToChair")]
    public void moveToRandomChair()
    {
        isPlanning2Sit = true;
        chairs = FindObjectsByType<TavernChair>(FindObjectsSortMode.None).ToList();
        randomPos = getRandomChairPos();
        navMeshAgent.SetDestination(randomPos.transform.position);
    }

    public void sit(Vector3 pos)
    {
        gameObject.transform.position = pos;
        sittingMotion(isSitting: true);
        randomPos.currentNPC = this;
        isPlanning2Sit = false;
    }

    [YarnCommand("MoveToPos")]
    public void moveToPos(int x, int y)
    {
        isPlanning2Sit = true;
        navMeshAgent.SetDestination(new Vector3(x, y, 0));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable") && isPlanning2Sit && other.GetComponent<TavernChair>() != null && other.GetComponent<TavernChair>() == randomPos)
        {
            //TODO: Sit
            if (isPlanning2Sit)
            {
                navMeshAgent.isStopped = true;
                sit(randomPos.transform.position);
            }
        }    
    }


    [YarnCommand("addLovePoint")]
    public void addLovePoint(int x, int y)
    {
        lovePoints[x] += y;
    }

    [YarnCommand("updateDialogueIndex")]
    public void updateDialogueIndex(int x)
    {
        
    }


    public int checkLoveHate(List<string> list)
    {
        int temp = 0;
        foreach (string favor in list)
        {
            if (desire_favor.Contains(favor))
                temp++;
            else if (hate_favor.Contains(favor))
                temp--;
            else if(like_favor.Contains(favor))
            {
                temp += 2;
            }

        }
        
        return temp;
    }

    public void updateDesireTaste()
    {
        desire_favor.Clear();

        for (int i = 0; i < 2; i++)
        {
            int temp = Random.Range(0, like_favor.Count + 1);
            desire_favor.Add(like_favor[temp]);
        }
    }


    [YarnCommand("updateYarnOrder")]
    public void updateYarnOrder()
    {
        index++;
        currDialogueIndex = "Ma" + index ;
    }
    
}
