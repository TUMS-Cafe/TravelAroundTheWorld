using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance;

    public bool isMiniGamePlayed = false; // 미니게임이 진행되었는지 여부
    public bool isAllNpcFound = false; // NPC를 모두 찾았는지 여부

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
}