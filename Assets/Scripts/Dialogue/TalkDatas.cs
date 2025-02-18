using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkData
{
    private int talkId; //TalkID. Must be unique
    private int talkingNPCId; //0 if player, 1- if NPC. if it overs 1-, write with 11--
    private List<string> lines; //Lines that NPCs gonna talk.
    private List<string> hints; //Hints to be happyending.
    private List<string> happyLines; //Lines says when Happy or with agreement.
    private List<string> sadLines; //Lines says when Sad or with disagreement.


    public TalkData(int talkId, int talkingNPCId, List<string> lines, List<string> hints, List<string> happyLines, List<string> sadLines)
    {
        this.talkId = talkId;
        this.talkingNPCId = talkingNPCId;
        this.lines = lines;
        this.hints = hints;
        this.happyLines = happyLines;
        this.sadLines = sadLines;
    }

    public int TalkingNpcId { get => talkingNPCId; set => talkingNPCId = value; }
    public List<string> Lines { get => lines; set => lines = value; }
    public List<string> Hints { get => hints; set => hints = value; }
    public List<string> SadLines { get => sadLines; set => sadLines = value; }
    public List<string> HappyLines { get => happyLines; set => happyLines = value; }
}

public class TalkDatas : MonoBehaviour
{
    //Put Talkid in int, and Talk Data in TalkData
    Dictionary<int, TalkData> talkDatas;

    private void Start()
    {
        talkDatas = new Dictionary<int, TalkData>();
        
        //DEPRICATED
        //This is just a format.
        //Talk Data for specific NPC.
        //Needs to have unique ID of talkid with over 10000.
        /**
        #region [NPC Name | NPCID | First 3 number of Talkid ex) 10020 -> 100]
        
        

        #endregion
        **/

        
        //Test DEPRICATED
        /**
        #region [Marco | 1 | 10000]
        
        talkDatas.Add(10000, new TalkData(10000, 1, new List<string>()
        {
            "안녕?",
            "나는 테스트 NPC 폴로야.",
            "Call me Marco.",
            "Im not sure how long it will get"
        }, new List<string>()
        {
            "그냥 말을 걸어보자."
        }, new List<string>()
        {
            "오 그거 정말 좋은 생각이다!",
            "오늘 정말 행복한 날이야.",
            "하하하하하하하"
        }, new List<string>()
        {
            "오... 아니야...",
            "그건 내가 원하던게 아닌데...",
            "별로야."
        }));


        #endregion
        */
    }
    


    public TalkData getTalkDataById(int talkId)
    {
        return talkDatas[talkId];
    }
}
