using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch3InteractionButton : MonoBehaviour
{
    private Button button; // 버튼 컴포넌트
    public Ch3TalkManager ch3TalkManager; // 토크 매니저 스크립트
    private string location; // 상호작용할 위치
    private Ch3TriggerArea ch3TriggerArea; // Ch3TriggerArea 컴포넌트
    public PlayerController playerController;

    private void Start()
    {
        button = GetComponent<Button>(); // 버튼 컴포넌트를 가져옴
        button.onClick.AddListener(OnInteract); // 버튼 클릭 시 OnInteract 메서드 호출
        gameObject.SetActive(false); // 시작할 때 버튼 비활성화

        // 현재 버튼 오브젝트의 상위 오브젝트들 중에서 Ch3TriggerArea를 찾음
        ch3TriggerArea = GetComponentInParent<Ch3TriggerArea>(); // 클래스 이름 대문자 C로 수정
    }

    // 위치 설정 메서드
    public void SetLocation(string locationName)
    {
        location = locationName;
    }

    public void OnInteract()
    {
        if (ch3TalkManager != null)
        {
            playerController.StopMove(); // 대화 버튼 클릭할 때 플레이어 움직임 멈춤

            // 대화 활성화 후 버튼 비활성화
            gameObject.SetActive(false); // 버튼 비활성화
                                         // 버튼 비활성화 상태를 TriggerArea에 전달
            if (ch3TriggerArea != null)
            {
                ch3TriggerArea.DisableButton(); // 버튼 비활성화 호출
            }

            // Ch3TalkManager의 메서드를 호출하여 대화 진행
            ch3TalkManager.OnDialogueButtonClicked(ch3TalkManager.currentDialogueIndex);
        }
    }
}
