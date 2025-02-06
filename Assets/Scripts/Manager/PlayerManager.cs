using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    public PlayerData currentData;

    public string isAutoSaveTrue = "자동";
    public string isAutoSaveFalse = "수동";

    //저장된 데이터가 로드되었는지 설정
    private bool isLoaded = false;

    public int playingSaveDataIdx;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPlayerData(int idx)
    {
        if (SaveDataManager.Instance.HasSaveData())
        {
            playingSaveDataIdx = idx;
            isLoaded = true;
        }
        else
        {
            currentData = new PlayerData();
            InitializePlayerData();
            //Debug.LogError("Failed to load save data.");
        }
        SaveDataManager.Instance.SetActiveSlot(idx);
    }

    public bool GetIsLoaded()
    {
        return isLoaded;
    }

    public void SetIsLoaded()
    {
        isLoaded = !isLoaded;
    }

    public DateTime GetSaveTime()
    {
        return currentData.saveTime;
    }

    public void SetSaveTime()
    {
        currentData.saveTime = DateTime.Now;
    }

    public bool GetIsAutoSave()
    {
        return currentData.isAutoSave;
    }

    public string GetIsAutoSaveToString()
    {
        if (currentData.isAutoSave)
            return isAutoSaveTrue;
        else
            return isAutoSaveFalse;
    }

    public void SetIsAutoSave()
    {
        //카페에 다녀왔을 경우/침대와 상호작용했을 경우 ++
    }

    public MapState GetMapLocation()
    {
        return currentData.mapLocation;
    }

    public void SetMapLocation(MapState map)
    {
        currentData.mapLocation = map;
    }

    public Vector3 GetPlayerPosition()
    {
        return currentData.playerPosition;
    }

    public void SetPlayerPosition()
    {
        currentData.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    public string GetSceneName()
    {
        return currentData.currentSceneName;
    }

    public void SetSceneName(string curScene)
    {
        currentData.currentSceneName = curScene;
    }

    //날짜
    public int GetDay()
    {
        return currentData.currentDay;
    }

    public void SetDay()
    {
        currentData.currentDay = DayNightCycleManager.Instance.GetCurrentDay();
    }

    //시간
    public bool GetCurrentTimeofDay()
    {
        return currentData.currentTimeofDay;
    }

    public int GetMoney()
    {
        return currentData.money;
    }

    public int GetDialogueIdx()
    {
        return currentData.dialogueIdx;
    }

    public void SetDialogueIdx(int idx)
    {
        currentData.dialogueIdx = idx;
    }

    public void SetCurrentTimeofDay()
    {
        currentData.currentTimeofDay = DayNightCycleManager.Instance.GetNowDayTime();
    }

    public void AddCafeItem(string name)
    {
        currentData.unlockedCafeItems.Add(name);
    }

    public bool IsBoughtCafeItem(string name)
    {
        return currentData.unlockedCafeItems.Contains(name);
    }

    //inventory형식 수정 예정-아이템 형식 미정
    public void AddInventoryItem(GameObject inventoryItem)
    {
        currentData.inventoryItem.Add(inventoryItem);
    }

    public void DeleteInventoryItem(GameObject inventoryItem)
    {
        currentData.inventoryItem.Remove(inventoryItem);
    }

    public void AddCompleteMainQuestIds(string mainQuestionName)
    {
        currentData.completedMainQuestIds.Add(mainQuestionName);
    }

    public void AddCompletedSubQuestIds(string subQuestionName)
    {
        currentData.completedSubQuestIds.Add(subQuestionName);
    }

    public void AddIllustrationIds(int id)
    {
        currentData.unlockedIllustrationIds.Add(id);
    }

    public void AddEndingIds(int id)
    {
        currentData.unlockedEndingIds.Add(id);
    }

    public void EarnMoney(int money)
    {
        currentData.money += money;
    }

    public void PayMoney(int money)
    {
        //수정 필요
        if (money > currentData.money)
            return;
        currentData.money -= money;
    }

    public void SetEndings(int chapter, bool result)
    {
        currentData.chapterEndings[chapter] = result;
    }

    public int CountEndings()
    {
        return currentData.chapterEndings.Count(e => e);
    }
    
    private void InitializePlayerData()
    {
        currentData.saveTime = DateTime.Now;
        currentData.isAutoSave = false;
        currentData.mapLocation = MapState.Null;
        currentData.playerPosition = new Vector3(0, 0, 0);
        currentData.currentSceneName = "Ch0Scene";
        currentData.currentDay = DayNightCycleManager.Instance.day;
        currentData.currentTimeofDay = DayNightCycleManager.Instance.isDayTime;
        currentData.money = 0;
        currentData.dialogueIdx = 0;
        currentData.unlockedCafeItems = new List<string>();
        currentData.inventoryItem = new List<GameObject>();
        currentData.completedMainQuestIds = new List<string>();
        currentData.completedSubQuestIds = new List<string>();
        currentData.unlockedIllustrationIds = new List<int>();
        currentData.unlockedEndingIds = new List<int>();
        currentData.chapterEndings = new List<bool> { false, false, false, false };
    }

    //씬 전환 후 자동저장(카페 작업 후 자동저장)
    //ChN에서도 되도록 수정 필요
    public void SaveAutoDataAfterCafeScene()
    {
        currentData.saveTime = DateTime.Now;
        currentData.isAutoSave = true;

        //씬 종류에 따라 다른 매니저 가져오도록 설정
        currentData.mapLocation = GameObject.FindObjectOfType<Ch1MapManager>().currentState;
        currentData.playerPosition = GameObject.FindObjectOfType<Ch1MapManager>().playerPosition;

        currentData.currentSceneName = SceneManagerEx.Instance.GetCurrentSceneName();

        currentData.currentDay = DayNightCycleManager.Instance.GetCurrentDay();
        currentData.currentTimeofDay = DayNightCycleManager.Instance.GetNowDayTime();

        //다른 데이터는 작업 과정 중 할당되므로 작업할 필요 없음.

    }


    //다이어리 사용해서 저장
    public void SavePlayerDataWithDiary()
    {
        currentData.saveTime = DateTime.Now;
        currentData.isAutoSave = false;

        //씬 종류에 따라 다른 매니저 가져오도록 설정
        currentData.mapLocation = GameObject.FindObjectOfType<Ch0MapManager>().currentState;
        currentData.playerPosition = GameObject.FindObjectOfType<Ch0MapManager>().playerPosition;

        currentData.currentSceneName = SceneManagerEx.Instance.GetCurrentSceneName();

        currentData.currentDay = DayNightCycleManager.Instance.GetCurrentDay();
        currentData.currentTimeofDay = DayNightCycleManager.Instance.GetNowDayTime();
    }
    


}
