using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[SerializeField]
public class PlayerData
{
    //저장된 시간
    public DateTime saveTime;
    //자동저장 여부 
    public bool isAutoSave;
    //현재 위치한 장소명
    public MapState mapLocation;
    //플레이어 위치 좌표
    public Vector3 playerPosition;
    //현재 씬 이름
    public string currentSceneName;
    //현재 날짜
    public int currentDay;
    //현재 시간(낮/밤)
    public bool currentTimeofDay;
    //모은 돈
    public int money;
    //현재까지 진행한 스토리 대사 번호
    public int dialogueIdx;
    //해금한 카페 아이템 종류
    public List<string> unlockedCafeItems;
    //인벤토리 아이템
    public List<GameObject> inventoryItem;
    //해결한 메인 퀘스트 
    public List<string> completedMainQuestIds;
    //해결한 서브 퀘스트 
    public List<string> completedSubQuestIds;
    //해금한 일러스트 번호
    public List<int> unlockedIllustrationIds;
    //해금한 엔딩 번호
    public List<int> unlockedEndingIds;
    //챕터 별 엔딩 여부
    public List<bool> chapterEndings;
}
