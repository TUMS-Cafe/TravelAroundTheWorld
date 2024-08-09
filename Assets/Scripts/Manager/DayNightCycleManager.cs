using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayNightCycleManager : MonoBehaviour
{
    public static DayNightCycleManager Instance { get; private set; }

    private int curDay;
    private bool isNowDayTime;

    public int day;
    public bool isDayTime;

    private string previousSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        string curSceneName = SceneManagerEx.Instance.GetCurrentSceneName();

        switch(curSceneName)
        {
            case "StartScene":
            case "Ch0Scene":
                day = 0;
                curDay = 0;
                break;
            case "Ch1Scene":
                day = 1;
                curDay = 1;
                break;
        }
        isDayTime = true;
        isNowDayTime = true;
        
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentSceneName = scene.name;

        // 씬 전환에 따른 변화 적용
        HandleSceneTransition(previousSceneName, currentSceneName);

        // 현재 씬을 이전 씬으로 업데이트
        previousSceneName = currentSceneName;
    }

    private void HandleSceneTransition(string fromScene, string toScene)
    {
        //챕터 1로 넘어갈 때
        if (fromScene == "Ch0Scene" && toScene == "LoadingScene" && curDay == 0)
        {
            ChangeDay();
        }

        //챕터에서 카페일을 할 때
        else if (fromScene == "LoadingScene" && toScene == "CafeScene")
        {
            // 변화 없음
        }
        //카페일을 하고 카페 밖으로 나왔을 땐
        else if (fromScene == "LoadingScene" && toScene == "Ch1Scene" && isNowDayTime)
        {
            ChangeDayTime();
        }
        else
        {
            Debug.LogWarning($"Unhandled scene transition from {fromScene} to {toScene}");
        }

        // 상태 업데이트 로그 (디버깅용)
        Debug.Log($"CurDay: {curDay}, IsNowDayTime: {isNowDayTime}");
    }

    public void ChangeDay()
    {
        curDay += 1;
        isNowDayTime = true;
    }

    public void ChangeDayTime()
    {
        isNowDayTime = !isNowDayTime;
    }

    public int GetCurrentDay()
    {
        return curDay;
    }

    public bool GetNowDayTime()
    {
        return isNowDayTime;
    }
}