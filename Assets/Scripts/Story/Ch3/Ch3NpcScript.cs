using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch3NpcScript : MonoBehaviour
{
    public GameObject dialogueButton; // "대화하기" 버튼
    public Ch3TalkManager talkManager; // Ch1TalkManager 참조
    public Transform player; // 플레이어의 위치

    public float interactionRange = 5f; // NPC와 상호작용 가능한 범위

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인

    void Start()
    {
        // 대화 버튼을 처음에는 비활성화
        dialogueButton.SetActive(false);
    }

    void Update()
    {
        // NPC가 활성화되어 있을 때만 상호작용 가능 여부를 확인
        if (!gameObject.activeInHierarchy)
        {
            dialogueButton.SetActive(false);
            return;
        }

        // 플레이어가 NPC 근처에 있는지 체크
        isPlayerInRange = Vector3.Distance(player.position, transform.position) <= interactionRange;

        // currentDialogueIndex가 33인 경우 정원 NPC와 상호작용
        /*if (isPlayerInRange && talkManager.currentDialogueIndex == 33 && gameObject.name == "Npc_Rayviyak")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 37인 경우 바 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 37 && gameObject.name == "Npc_Violet")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 104인 경우 정원 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 104 && gameObject.name == "Npc_Rayviyak")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 187인 경우 정원 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 187 && gameObject.name == "Npc_Rayviyak")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 191인 경우 바 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 191 && gameObject.name == "Npc_Violet")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 207인 경우 빵집 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 207 && gameObject.name == "Npc_Rusk")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 318 경우 정원 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 318 && gameObject.name == "Npc_Rayviyak")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 322 경우 바 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 322 && gameObject.name == "Npc_Violet")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 326 경우 빵집 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 326 && gameObject.name == "Npc_Rusk")
        {
            dialogueButton.SetActive(true);
        }
        // currentDialogueIndex가 330 경우 의무실 NPC와 상호작용
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 330 && gameObject.name == "Npc_MrHam")
        {
            dialogueButton.SetActive(true);
        }
        else
        {
            dialogueButton.SetActive(false);
        }*/
    }

    // "대화하기" 버튼을 눌렀을 때 호출되는 함수
    public void OnDialogueButtonClicked()
    {
        // currentDialogueIndex가 33인 경우 정원 NPC와 대화 진행
        if (talkManager.currentDialogueIndex == 33 && gameObject.name == "Npc_Rayviyak")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Rayviyak.SetActive(false);
            talkManager.garden.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 37인 경우 바 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 37 && gameObject.name == "Npc_Violet")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Violet.SetActive(false);
            talkManager.cafe.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 104인 경우 정원 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 104 && gameObject.name == "Npc_Rayviyak")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Rayviyak.SetActive(false);
            talkManager.garden.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 187인 경우 정원 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 187 && gameObject.name == "Npc_Rayviyak")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Rayviyak.SetActive(false);
            talkManager.garden.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 191 경우 바 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 191 && gameObject.name == "Npc_Violet")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Violet.SetActive(false);
            talkManager.cafe.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 207 경우 빵집 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 207 && gameObject.name == "Npc_Rusk")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Rusk.SetActive(false);
            talkManager.bakery.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 318 경우 정원 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 318 && gameObject.name == "Npc_Rayviyak")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Rayviyak.SetActive(false);
            talkManager.garden.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 322 경우 바 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 322 && gameObject.name == "Npc_Violet")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Violet.SetActive(false);
            talkManager.cafe.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 326 경우 빵집 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 326 && gameObject.name == "Npc_Rusk")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_Rusk.SetActive(false);
            talkManager.bakery.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
        // currentDialogueIndex가 330 경우 의무실 NPC와 대화 진행
        else if (talkManager.currentDialogueIndex == 330 && gameObject.name == "Npc_MrHam")
        {
            talkManager.map.SetActive(false);
            talkManager.player.SetActive(false);
            talkManager.Npc_MrHam.SetActive(false);
            talkManager.medicalRoom.SetActive(true);
            talkManager.dialogue.SetActive(true);
            talkManager.isWaitingForPlayer = false;
            talkManager.currentDialogueIndex++;
            talkManager.PrintCh3ProDialogue(talkManager.currentDialogueIndex);
        }
    }
}
