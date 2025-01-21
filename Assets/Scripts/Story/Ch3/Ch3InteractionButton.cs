using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch3InteractionButton : MonoBehaviour
{
    private Button button; // 버튼 컴포넌트
    public Ch3TalkManager ch3TalkManager; // 토크 매니저 스크립트
    //public Ch3NpcScript ch3NpcScript;
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

            // 상위 계층에서 태그가 "NPC"인 오브젝트 찾기
            Transform npcTransform = transform.parent; // 버튼의 상위 계층부터 탐색 시작
            string npcName = null; // NPC 이름 초기화

            while (npcTransform != null)
            {
                if (npcTransform.CompareTag("NPC")) // 태그가 "NPC"인지 확인
                {
                    npcName = npcTransform.name; // NPC 이름 가져오기
                    break; // 찾으면 루프 종료
                }
                npcTransform = npcTransform.parent; // 상위 계층으로 이동
            }

            if (npcName != null)
            {
                // NPC 이름과 현재 대화 인덱스를 전달하여 대화 진행
                ch3TalkManager.currentNpc = npcName;

                //1일차 밤 Npc 대화
                if (ch3TalkManager.currentDialogueIndex == 76 && npcName == "Npc_Rayviyak")
                {
                    ch3TalkManager.currentDialogueIndex = 77;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 76 && npcName == "Npc_Violet")
                {
                    ch3TalkManager.currentDialogueIndex = 80;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 76 && npcName == "Npc_Kuraya")
                {
                    ch3TalkManager.currentDialogueIndex = 82;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 76 && npcName == "Npc_Rusk")
                {
                    ch3TalkManager.currentDialogueIndex = 83;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 76 && npcName == "Npc_MrHam")
                {
                    ch3TalkManager.currentDialogueIndex = 90;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }

                //2일차 밤 Npc 대화
                if (ch3TalkManager.currentDialogueIndex == 133 && npcName == "Npc_Violet")
                {
                    ch3TalkManager.currentDialogueIndex = 134;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 133 && npcName == "Npc_Kuraya")
                {
                    ch3TalkManager.currentDialogueIndex = 139;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (!ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 133 && npcName == "Npc_Rusk")
                {
                    ch3TalkManager.currentDialogueIndex = 147;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 133 && npcName == "Npc_MrHam")
                {
                    ch3TalkManager.currentDialogueIndex = 153;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }

                //3일차 밤 Npc 대화
                if (ch3TalkManager.currentDialogueIndex == 226 && npcName == "Npc_Naru")
                {
                    ch3TalkManager.currentDialogueIndex = 227;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 226 && npcName == "Npc_Violet")
                {
                    ch3TalkManager.currentDialogueIndex = 232;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 226 && npcName == "Npc_Nicksy")
                {
                    ch3TalkManager.currentDialogueIndex = 233;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (!ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 226 && npcName == "Npc_Coco")
                {
                    ch3TalkManager.currentDialogueIndex = 242;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 226 && npcName == "Npc_MrHam")
                {
                    ch3TalkManager.currentDialogueIndex = 251;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }

                //4일차 밤 Npc 대화
                if (ch3TalkManager.currentDialogueIndex == 356 && npcName == "Npc_Rayviyak")
                {
                    ch3TalkManager.currentDialogueIndex = 357;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 356 && npcName == "Npc_Nicksy")
                {
                    ch3TalkManager.currentDialogueIndex = 363;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 356 && npcName == "Npc_Kuraya")
                {
                    ch3TalkManager.currentDialogueIndex = 371;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (!ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 356 && npcName == "Npc_Rusk")
                {
                    ch3TalkManager.currentDialogueIndex = 384;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 356 && npcName == "Npc_MrHam")
                {
                    ch3TalkManager.currentDialogueIndex = 387;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 356 && npcName == "Npc_Ash")
                {
                    ch3TalkManager.currentDialogueIndex = 389;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }

                //5일차 밤 Npc 대화
                if (ch3TalkManager.currentDialogueIndex == 418 && npcName == "Npc_Rayviyak")
                {
                    ch3TalkManager.currentDialogueIndex = 419;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 418 && npcName == "Npc_Violet")
                {
                    ch3TalkManager.currentDialogueIndex = 430;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 418 && npcName == "Npc_Rusk")
                {
                    ch3TalkManager.currentDialogueIndex = 431;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (!ch3TalkManager.isCh2HappyEnding && ch3TalkManager.currentDialogueIndex == 418 && npcName == "Npc_Rusk")
                {
                    ch3TalkManager.currentDialogueIndex = 439;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 418 && npcName == "Npc_MrHam")
                {
                    ch3TalkManager.currentDialogueIndex = 441;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
                if (ch3TalkManager.currentDialogueIndex == 418 && npcName == "Npc_Naru")
                {
                    ch3TalkManager.currentDialogueIndex = 445;
                    ch3TalkManager.isNpcTalkActivated = true;
                    ch3TalkManager.NpcDialogue(ch3TalkManager.currentDialogueIndex, npcName);
                }
            }
            else
            {
                Debug.LogWarning("NPC 태그를 가진 오브젝트를 찾을 수 없습니다.");
            }
        }
    }
}
