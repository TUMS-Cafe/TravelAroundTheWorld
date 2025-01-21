using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch3TriggerArea : MonoBehaviour
{
    public GameObject interactionButton; // 상호작용 버튼
    public string locationName; // npc가 있는 장소
    public Transform playerTransform; // 플레이어의 Transform
    public float interactionDistance = 5f; // 상호작용 거리
    public bool talkActived = false; // 대화 활성화 상태
    public Ch3TalkManager ch3TalkManager; // TalkManager 인스턴스

    private void Start()
    {
        interactionButton.SetActive(false); // 처음엔 버튼 비활성화
    }

    private void Update()
    {
        if (talkActived) // 대화 활성화 상태일 경우
        {
            return; // 버튼 업데이트 하지 않음
        }

        // 플레이어와 NPC 사이 거리를 계산
        float distance = Vector3.Distance(playerTransform.position, transform.position);

        // 거리가 설정된 거리 이내인지 확인
        if (distance <= interactionDistance)
        {
            interactionButton.SetActive(true); // 버튼 활성화
            Ch3InteractionButton interactionButtonScript = interactionButton.GetComponent<Ch3InteractionButton>();
            if (interactionButtonScript != null)
            {
                interactionButtonScript.SetLocation(locationName); // 버튼에 위치 정보 설정
            }
            Managers.Instance.CurrentLocation = locationName; // 현재 위치 설정
        }
        else
        {
            interactionButton.SetActive(false);
            Managers.Instance.CurrentLocation = null; // 버튼 비활성화
        }
    }

    // 버튼을 비활성화하는 메서드
    public void DisableButton()
    {
        talkActived = true; // 대화 활성화 상태 설정
        interactionButton.SetActive(false); // 버튼 비활성화
    }
}
