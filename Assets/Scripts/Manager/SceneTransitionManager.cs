using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    public ScreenFader screenFader;

    public int toDialogueIdx;

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

        //CheckAndAssignScreenFader();
    }

    // 대화 진행 중 특정 조건에서 호출
    public void HandleDialogueTransition(string fromScene, string toScene, int fromSceneIdx, int toSceneIdx, int returnIdx)
    {
        StartCoroutine(HandleSceneTransition(fromScene, toScene, fromSceneIdx, toSceneIdx, returnIdx));
    }

    IEnumerator HandleSceneTransition(string fromScene, string toScene, int curIdx, int toSceneIdx, int returnIdx)
    {
        // 씬 전환
        yield return TransitionToScene(toScene);

        // 필요한 작업 수행
        PerformPostTransitionTasks();

        yield return WaitForCondition(() => IsSpecificConditionMet(toSceneIdx));

        //TalkManager.Instance.currentDialogueIndex = returnIdx;
        yield return TransitionToScene(fromScene);

        //OnDialogueIndexUpdated(curIdx, returnIdx);
    }

    IEnumerator TransitionToScene(string sceneName)
    {
        // 씬 전환
        SceneManagerEx.Instance.SceanLoadQueue(sceneName);

        yield return new WaitUntil(() => !SceneManagerEx.Instance.IsLoading());

    }

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

    public void UpdateDialogueIndex(int newIndex)
    {
        toDialogueIdx = newIndex;
    }

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