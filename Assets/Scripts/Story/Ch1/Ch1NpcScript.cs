using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch1NpcScript : MonoBehaviour
{
    public GameObject dialogueButton; // "대화하기" 버튼
    public Ch1TalkManager talkManager; // Ch1TalkManager 참조
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
        if (!gameObject.activeInHierarchy)
        {
            dialogueButton.SetActive(false);
            return;
        }

        isPlayerInRange = Vector3.Distance(player.position, transform.position) <= interactionRange;

        // 인덱스가 43일 때 레이비야크와 바이올렛에 대한 상호작용 처리
        if (isPlayerInRange && talkManager.currentDialogueIndex == 43)
        {
            if (gameObject.name == "Npc_Rayviyak" && !talkManager.HasTalkedToRayviyak)
            {
                dialogueButton.SetActive(true);
            }
            else if (gameObject.name == "Npc_Violet" && !talkManager.HasTalkedToViolet)
            {
                dialogueButton.SetActive(true);
            }
            else if (gameObject.name != "Npc_Rayviyak" && gameObject.name != "Npc_Violet")
            {
                dialogueButton.SetActive(false);
            }
        }
        else
        {
            dialogueButton.SetActive(false);
        }
    }

    // "대화하기" 버튼을 눌렀을 때 호출되는 함수
    public void OnDialogueButtonClicked()
    {
        if (talkManager.currentDialogueIndex == 43)
        {
            if (gameObject.name == "Npc_Rayviyak" && !talkManager.HasTalkedToRayviyak)
            {
                talkManager.StartDialogueSequence(44, 47);
                talkManager.HasTalkedToRayviyak = true;
                talkManager.currentDialogueIndex = 43;
            }
            else if (gameObject.name == "Npc_Violet" && !talkManager.HasTalkedToViolet)
            {
                talkManager.StartDialogueSequence(49, 75);
                talkManager.HasTalkedToViolet = true;
                talkManager.currentDialogueIndex = 43;
            }
            else
            {
                talkManager.ShowNarration("나레이션", "지금은 바빠 보여.");
                talkManager.currentDialogueIndex = 43;
            }

            if (talkManager.HasTalkedToRayviyak && talkManager.HasTalkedToViolet)
            {
                talkManager.EnableBedInteraction();
            }
        }
    }
}
