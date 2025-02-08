using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HideandSeek : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float gameDuration = 120f;
    public GameObject[] npcObjects;
    public string previousSceneName;

    private float remainingTime;
    private int npcsFound;
    private bool gameActive = false;

    // 플레이어 데이터 전달용 변수
    public bool miniGameCompleted = false; // 미니게임 진행 여부
    public bool allNpcsFound = false; // NPC 모두 찾았는지 여부

    private void Start()
    {
        remainingTime = gameDuration;
        npcsFound = 0;
        gameActive = true;
        UpdateTimerText();
        StartCoroutine(GameTimer());
    }

    public void FindNpc()
    {
        if (!gameActive) return;

        npcsFound++;
        if (npcsFound >= npcObjects.Length)
        {
            EndGame(true);
        }
    }

    private void EndGame(bool success)
    {
        gameActive = false;
        miniGameCompleted = true;
        allNpcsFound = success;

        if (StateManager.Instance != null)
        {
            StateManager.Instance.isMiniGamePlayed = true;
            StateManager.Instance.isAllNpcFound = success;
        }

        if (success)
        {
            Debug.Log("모든 NPC를 찾았습니다!");
        }
        else
        {
            Debug.Log("시간 초과! 게임 종료.");
        }

        StartCoroutine(LoadPreviousSceneWithDelay());
    }

    private IEnumerator GameTimer()
    {
        while (remainingTime > 0 && gameActive)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerText();
            yield return null;
        }

        if (remainingTime <= 0 && gameActive)
        {
            EndGame(false);
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private IEnumerator LoadPreviousSceneWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(previousSceneName);
    }
}