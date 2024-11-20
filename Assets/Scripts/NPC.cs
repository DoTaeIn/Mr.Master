using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;
using Random = UnityEngine.Random;

public enum NPCStatusType
{
    Idle,
    Walking,
    Thinking,
    Happy,
    Sad,
    Complex,
    HaveSmth
}


[RequireComponent(typeof(NavMeshAgent))]
public class NPC : MonoBehaviour
{
    [Header("NPC Info")]
    public int npcId;
    public string npcName;
    public NPCStatusType npcStatus;
    public YarnProject yarnLine;
    public GameObject[] emotions;
    
    [Header("NPC Motion")]
    public List<TavernChair> chairs;
    [SerializeField] bool isPlanning2Sit = false;
    [SerializeField] private GameObject leg_R;
    [SerializeField] private GameObject leg_L;
    private Vector2 currPos;
    private Transform sttingPos;
    
    NavMeshAgent navMeshAgent;
    Animator animator;
    Rigidbody2D rb;
    private TavernChair randomPos;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        //Navmesh default setting
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
    }

    private void Start()
    {
        moveToRandomChair();
    }

    private void Update()
    {
        
        //Animation
        if(!navMeshAgent.isStopped)
        {
            Debug.Log("Walking");
            animator.SetBool("isWalk", true);
        }
        else
        {
            Debug.Log("Stoped");
            animator.SetBool("isWalk", false);
        }
        
        
        //Show emotion
        switch (npcStatus)
        {
            case NPCStatusType.Idle:
                activeEmotion(0);
                break;
            case NPCStatusType.Walking:
                activeEmotion(1);
                break;
            case NPCStatusType.Thinking:
                activeEmotion(2);
                break;
            case NPCStatusType.Happy:
                activeEmotion(3);
                break;
            case NPCStatusType.Sad:
                activeEmotion(4);
                break;
            case NPCStatusType.Complex:
                activeEmotion(5);
                break;
            case NPCStatusType.HaveSmth:
                activeEmotion(6);
                break;
            default:
                break;
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

    
    //Repersent emotion since it only need to show one at time, shows only one.
    public void activeEmotion(int i)
    {
        for (int j = 0; j < emotions.Length; j++)
        {
            if (i != j)
            {
                emotions[j].SetActive(false);
            }
            else
            {
                emotions[j].SetActive(true);
            }
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

    public void moveToPos()
    {
        isPlanning2Sit = true;
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
}
