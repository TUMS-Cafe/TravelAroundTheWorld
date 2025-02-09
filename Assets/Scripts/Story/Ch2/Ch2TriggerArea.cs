using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch2TriggerArea : MonoBehaviour
{
    public GameObject interactionButton; // 상호작용 버튼
    public string locationName; // NPC가 있는 장소
    public Transform playerTransform; // 플레이어의 Transform
    public float interactionDistance = 1.0f; // 거리 넉넉하게 조정
    public bool talkActived = false; // 대화 활성화 상태 추가 (중복 실행 방지)
    public Ch2TalkManager ch2TalkManager; // Ch2TalkManager 참조 추가

    private void Start()
    {
        interactionButton.SetActive(false); // 처음엔 버튼 비활성화
    }

    private void Update()
    {
    }
}
