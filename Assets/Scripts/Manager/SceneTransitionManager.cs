using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    public ScreenFader screenFader;
    public bool isTransitioning { get; private set; } = false;

    public int toDialogueIdx;

    private string destScene;

    private int returnDialogueIndex;
    private string targetScene;

    private int cafeDeliveryNum;

    private int cafeDeliveryTransitionNum;

    private List<CafeOrder> cafeOrders = new List<CafeOrder>();

    private List<CafeOrder> toCafeOrders = new List<CafeOrder>();

    private int randomMenuNum;

    private int randomMenuTransitionNum;

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
            return;
        }

    }

    //현재 도착하는 씬 위치 반환
    public string GetDescScene()
    {
        return destScene;
    }

    public string GetCafeOrders()
    {
        return toCafeOrders[0].MenuItem;
    }
 
    //직접 와서 주문 받는 형식(주문자가 정해져 있음)
    public void HandleDialogueTransition(string fromScene, string toScene, int returnIdx, List<CafeOrder> orders)
    {
        returnDialogueIndex = returnIdx;
        targetScene = fromScene;
        destScene = toScene;
        //cafeOrders = new List<CafeOrder>(orders);
        toCafeOrders = new List<CafeOrder>(orders);
        cafeOrders = new List<CafeOrder>();

        StartCoroutine(HandleSceneTransition(fromScene, toScene, returnIdx, orders));
    }

    //랜덤 없이, 한 음료만 주문하고 카페 씬으로 이동 addzzz
    public void HandleDialogueTransition2(int returnIdx, CafeOrder order)
    {
        returnDialogueIndex = returnIdx;
        toCafeOrders = new List<CafeOrder> { order };
        cafeOrders = new List<CafeOrder>();

        StartCoroutine(HandleSceneTransition("Ch1Scene", "CafeScene", returnIdx, toCafeOrders));
    }

    //직접 와서 주문받는 형식 
    IEnumerator HandleSceneTransition(string fromScene, string toScene, int returnIdx, List<CafeOrder> orders)
    {
        // 씬 전환
        yield return TransitionToScene(toScene);

        // 필요한 작업 수행
        PerformPostTransitionTasks();

        //해당 조건 수행될때까지 대기
        yield return WaitForCondition(() => AreSpecificOrderConditionsMet(orders));

        SceneManager.sceneLoaded += OnSceneLoaded;

        cafeOrders = new List<CafeOrder>();

        yield return TransitionToScene(fromScene);
    }

    //모든 직접 주문 메뉴가 처리되었는지 확인(리스트 기준)
    private bool AreSpecificOrderConditionsMet(List<CafeOrder> orders)
    {
        foreach (var order in orders)
        {
            if (!IsSpecificOrderConditionMet(order))
            {
                return false;
            }
        }
        return true;
    }

    //직접 주문 메뉴별로 처리되었는지 확인(value 기준)
    private bool IsSpecificOrderConditionMet(CafeOrder order)
    {
        return cafeOrders.Exists(o =>
            //o.CustomerName == order.CustomerName &&
            o.MenuItem == order.MenuItem); //&&
            //o.MenuQuantity == order.MenuQuantity);
    }

    //랜덤메뉴 설정
    public void HandleRandomMenuTransition(string fromScene, string toScene, int returnIdx, int randomNum)
    {
        Debug.Log(randomMenuTransitionNum);
        returnDialogueIndex = returnIdx;
        targetScene = fromScene;
        destScene = toScene;
        randomMenuNum = 0;
        randomMenuTransitionNum = randomNum;
        StartCoroutine(HandleRandomMenuSceneTransition(fromScene, toScene, returnIdx, randomNum));
        
    }

    //랜덤메뉴 설정
    IEnumerator HandleRandomMenuSceneTransition(string fromScene, string toScene, int returnIdx, int randomNum)
    {
        // 씬 전환
        yield return TransitionToScene(toScene);

        // 필요한 작업 수행
        PerformPostTransitionTasks();

        //해당 조건 수행될때까지 대기
        yield return WaitForCondition(() => IsSpecificRandomMenuConditionMet(randomNum));

        SceneManager.sceneLoaded += OnSceneLoaded;

        randomMenuTransitionNum = 0;

        yield return TransitionToScene(fromScene);

    }

    //랜덤메뉴 개수 확인
    private bool IsSpecificRandomMenuConditionMet(int randomNum)
    {
        return randomMenuNum == randomNum;
    }




    //배달 랜덤 메뉴 설정
    public void HandleDialogueTransition(string fromScene, string toScene, int returnIdx, int deliveryNum)
    {
        returnDialogueIndex = returnIdx;
        targetScene = fromScene;
        destScene = toScene;
        cafeDeliveryTransitionNum = deliveryNum;
        cafeDeliveryNum = 0;
        StartCoroutine(HandleSceneTransition(fromScene, toScene, returnIdx, deliveryNum));
    }

    //배달 랜덤 메뉴 설정
    IEnumerator HandleSceneTransition(string fromScene, string toScene, int returnIdx, int deliveryNum)
    {
        // 씬 전환
        yield return TransitionToScene(toScene);

        // 필요한 작업 수행
        PerformPostTransitionTasks();

        //해당 조건 수행될때까지 대기
        yield return WaitForCondition(() => IsSpecificDeliveryConditionMet(deliveryNum));

        SceneManager.sceneLoaded += OnSceneLoaded;

        cafeDeliveryTransitionNum = 0;

        yield return TransitionToScene(fromScene);

    }

    //배달 랜덤 메뉴 개수 확인 
    private bool IsSpecificDeliveryConditionMet(int deliveryNum)
    {
        return cafeDeliveryNum == deliveryNum;
    }

    //배달 주문수 업데이트
    public void UpdateCafeDelivery(int newNum)
    {
        cafeDeliveryNum = newNum;
    }

    //스토리 진행 중 다른 씬으로 이동
    public void HandleDialogueTransition(string fromScene, string toScene, int fromSceneIdx, int toSceneIdx, int returnIdx)
    {
        returnDialogueIndex = returnIdx; 
        targetScene = fromScene;
        StartCoroutine(HandleSceneTransition(fromScene, toScene, fromSceneIdx, toSceneIdx, returnIdx));
    }

    //스토리 진행 중 다른 씬으로 이동
    IEnumerator HandleSceneTransition(string fromScene, string toScene, int curIdx, int toSceneIdx, int returnIdx)
    {
        // 씬 전환
        yield return TransitionToScene(toScene);

        // 필요한 작업 수행
        PerformPostTransitionTasks();

        yield return WaitForCondition(() => IsSpecificConditionMet(toSceneIdx));

        SceneManager.sceneLoaded += OnSceneLoaded;

        yield return TransitionToScene(fromScene);

    }

    //스토리 진행 중 다른 씬으로 이동
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //tutorial의 경우
        if (targetScene == "Ch0Scene")
        {
            if (scene.name == targetScene)
            {
                // 씬 로드 후 TalkManager 인스턴스를 찾고 인덱스 설정
                StartCoroutine(WaitAndSetDialogueIndex());
                SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 등록 해제
            }
        }

        else
        {
            if (scene.name == targetScene)
            {
                // 씬 로드 후 ChNTalkManager 인스턴스를 찾고 인덱스 설정
                StartCoroutine(WaitAndSetStoryDialogueIndex());
                SceneManager.sceneLoaded -= OnSceneLoaded; // 이벤트 등록 해제
            }
        }
    }

    //TutorialScene에서 CafeTutorial 상호작용시 
    private IEnumerator WaitAndSetDialogueIndex()
    {
        // TalkManager가 초기화될 때까지 대기
        TalkManager talkManager = null;
        while (talkManager == null)
        {
            talkManager = FindObjectOfType<TalkManager>();
            if (talkManager != null)
            {
                break;
            }
            yield return null;
        }

        // TalkManager의 currentDialogueIndex 설정
        Debug.Log($"Found TalkManager in {targetScene}, setting dialogue index to {returnDialogueIndex}.");
        TalkManager.Instance.SetDialogueIndex(returnDialogueIndex, true);
        Debug.Log($"Dialogue index set to {returnDialogueIndex} in {targetScene}.");
    }

    private IEnumerator WaitAndSetStoryDialogueIndex()
    {
        // TalkManager가 초기화될 때까지 대기
        Ch1TalkManager ch1TalkManager = null;
        while (ch1TalkManager == null)
        {
            ch1TalkManager = FindObjectOfType<Ch1TalkManager>();
            if (ch1TalkManager != null)
            {
                Debug.Log($"Found TalkManager in {targetScene}, setting dialogue index to {returnDialogueIndex}.");
                break;
            }
            yield return null;
        }

        Debug.Log($"Setting dialogue index to {returnDialogueIndex}");
        Ch1TalkManager.Instance.SetDialogueIndex(returnDialogueIndex, true);
        Debug.Log($"Dialogue index set to {returnDialogueIndex} in {targetScene}.");
    }


    //씬 전환
    IEnumerator TransitionToScene(string sceneName)
    {
        isTransitioning = true; // 씬 전환 시작 시 isTransitioning을 true로 설정
        SceneManagerEx.Instance.SceanLoadQueue(sceneName);

        yield return new WaitUntil(() => !SceneManagerEx.Instance.IsLoading());

        while (SceneManager.GetActiveScene().name != sceneName)
        {
            yield return null;
        }

        isTransitioning = false; // 씬 전환이 완료되면 isTransitioning을 false로 설정
    }

    //스토리 확인시 스토리 대화 index 확인
    private bool IsSpecificConditionMet(int returnIdx)
    {
        return toDialogueIdx == returnIdx;
    }

    private IEnumerator WaitForCondition(System.Func<bool> condition)
    {
        while (!condition())
        {
            yield return null;
        }
    }

    //대화 idx 업데이트
    public void UpdateDialogueIndex(int newIndex)
    {
        toDialogueIdx = newIndex;
    }

    //직접 주문수 업데이트
    public void UpdateCafeOrders(List<CafeOrder> newOrders)
    {
        cafeOrders = new List<CafeOrder>(newOrders);
    }

    //랜덤주문수 업데이트
    public void UpdateRandomMenuDelivery(int newNum)
    {
        randomMenuNum = newNum;
    }

    public int GetDeliveryNum()
    {
        return cafeDeliveryTransitionNum;
    }

    public int GetRandomMenuNum()
    {
        return randomMenuTransitionNum;
    }

    //
    private void OnDialogueIndexUpdated(int fromSceneIdx, int returnIdx)
    {
        fromSceneIdx = returnIdx;
    }

    private void PerformPostTransitionTasks()
    {
        // 로깅 작업 수행
        Debug.Log("Performing post-transition tasks...");
    }
    
}


/*
 * TalkManager에서 FadeIn과 같이 사용할 경우
 * 
 * private IEnumerator PerformFadeInAndHandleDialogue(int fromDialogueIdx, int returnDialogueIdx)
    {
        yield return StartCoroutine(screenFader.FadeIn(cafe));

        // 페이드 인이 완료된 후 씬 전환 작업 수행
        SceneTransitionManager.Instance.HandleDialogueTransition("Ch0Scene", "CafeTutorialScene", fromDialogueIdx, 3, returnDialogueIdx);
    }
 * 
 * 
 * 이동한 후 작업해야 할 경우
 * -CafeScene, CafeTutorialScene-
 * currentDialogueIdx 뒤 변화하는 부분 추가
 * SceneTransitionManager.Instance.UpdateDialogueIndex(currentDialogueIndex);
 */