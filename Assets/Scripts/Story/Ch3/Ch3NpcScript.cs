using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ch3NpcScript : MonoBehaviour
{
    public GameObject dialogueButton; // "대화하기" 버튼
    public Ch3TalkManager talkManager; // Ch3TalkManager 참조
    public Transform player; // 플레이어의 위치

    public float interactionRange = 5f; // NPC와 상호작용 가능한 범위

    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 확인
    public string currentNpc="Null";

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
        
        // 1일차 밤 Npc 대화
        if (isPlayerInRange && talkManager.currentDialogueIndex == 76)
        {
            if (gameObject.name == "Npc_Rayviyak" && !talkManager.HasTalkedToRayviyak)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rayviyak";
            }
            else if (gameObject.name == "Npc_Violet" && !talkManager.HasTalkedToViolet)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Violet";
            }
            else if (talkManager.isCh2HappyEnding && gameObject.name == "Npc_Kuraya" && !talkManager.HasTalkedToKuraya)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Kuraya";
            }
            else if (!talkManager.isCh2HappyEnding && gameObject.name == "Npc_Rusk" && !talkManager.HasTalkedToRusk)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rusk";
            }
            else if (gameObject.name == "Npc_MrHam" && !talkManager.HasTalkedToMrHam)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_MrHam";
            }
            else if (gameObject.name != "Npc_Rayviyak" && gameObject.name != "Npc_Violet" && gameObject.name != "Npc_Kuraya" && gameObject.name != "Npc_Rusk" && gameObject.name != "Npc_MrHam")
            {
                dialogueButton.SetActive(false);
                currentNpc = "Null";
            }
        }
        // 2일차 밤 Npc 대화
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 133)
        {
            if (gameObject.name == "Npc_Violet" && !talkManager.HasTalkedToViolet)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Violet";
            }
            else if (talkManager.isCh2HappyEnding && gameObject.name == "Npc_Kuraya" && !talkManager.HasTalkedToKuraya)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Kuraya";
            }
            else if (!talkManager.isCh2HappyEnding && gameObject.name == "Npc_Rusk" && !talkManager.HasTalkedToRusk)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rusk";
            }
            else if (gameObject.name == "Npc_MrHam" && !talkManager.HasTalkedToMrHam)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_MrHam";
            }
            else if (gameObject.name == "Npc_Coco" || gameObject.name == "Npc_Nicksy")
            {
                dialogueButton.SetActive(false);
            }
            else if (gameObject.name != "Npc_Violet" && gameObject.name != "Npc_Kuraya" && gameObject.name != "Npc_Rusk" && gameObject.name != "Npc_MrHam")
            {
                dialogueButton.SetActive(false);
                currentNpc = "Null";
            }
        }
        // 3일차 밤 Npc 대화
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 226)
        {
            if (gameObject.name == "Npc_Naru" && !talkManager.HasTalkedToNaru)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Naru";
            }
            else if (gameObject.name == "Npc_Violet" && !talkManager.HasTalkedToViolet)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Violet";
            }
            else if (talkManager.isCh2HappyEnding && gameObject.name == "Npc_Nicksy" && !talkManager.HasTalkedToNicksy)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Nicksy";
            }
            else if (!talkManager.isCh2HappyEnding && gameObject.name == "Npc_Coco" && !talkManager.HasTalkedToCoco)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Coco";
            }
            else if (gameObject.name == "Npc_MrHam" && !talkManager.HasTalkedToMrHam)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_MrHam";
            }
            else if (gameObject.name == "Npc_Rayviyak" || gameObject.name == "Npc_Rusk" || gameObject.name == "Npc_Kuraya")
            {
                dialogueButton.SetActive(false);
            }
            else if (gameObject.name != "Npc_Rayviyak" && gameObject.name != "Npc_Naru" && gameObject.name != "Npc_Violet" && gameObject.name != "Npc_Kuraya" && gameObject.name != "Npc_Rusk" && gameObject.name != "Npc_Coco" && gameObject.name != "Npc_Nicksy" && gameObject.name != "Npc_MrHam")
            {
                dialogueButton.SetActive(false);
                currentNpc = "Null";
            }
        }
        // 4일차 밤 Npc 대화
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 356)
        {
            if (gameObject.name == "Npc_Rayviyak" && !talkManager.HasTalkedToRayviyak)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rayviyak";
            }
            else if (gameObject.name == "Npc_Nicksy" && !talkManager.HasTalkedToNicksy)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Nicksy";
            }
            else if (talkManager.isCh2HappyEnding && gameObject.name == "Npc_Kuraya" && !talkManager.HasTalkedToKuraya)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Kuraya";
            }
            else if (!talkManager.isCh2HappyEnding && gameObject.name == "Npc_Rusk" && !talkManager.HasTalkedToRusk)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rusk";
            }
            else if (gameObject.name == "Npc_MrHam" && !talkManager.HasTalkedToMrHam)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_MrHam";
            }
            else if (gameObject.name == "Npc_Ash" && !talkManager.HasTalkedToAsh)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Ash";
            }
            else if (gameObject.name == "Npc_Violet" || gameObject.name == "Npc_Coco")
            {
                dialogueButton.SetActive(false);
            }
            else if (gameObject.name != "Npc_Rayviyak" && gameObject.name != "Npc_Naru" && gameObject.name != "Npc_Violet" && gameObject.name != "Npc_Kuraya" && gameObject.name != "Npc_Rusk" && gameObject.name != "Npc_Coco" && gameObject.name != "Npc_Nicksy" && gameObject.name != "Npc_MrHam" && gameObject.name != "Npc_Ash")
            {
                dialogueButton.SetActive(false);
                currentNpc = "Null";
            }
        }
        // 5일차 밤 Npc 대화
        else if (isPlayerInRange && talkManager.currentDialogueIndex == 418)
        {
            if (gameObject.name == "Npc_Rayviyak" && !talkManager.HasTalkedToRayviyak)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rayviyak";
            }
            else if (gameObject.name == "Npc_Violet" && !talkManager.HasTalkedToViolet)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Violet";
            }
            else if (talkManager.isCh2HappyEnding && gameObject.name == "Npc_Rusk" && !talkManager.HasTalkedToRusk)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rusk";
            }
            else if (!talkManager.isCh2HappyEnding && gameObject.name == "Npc_Rusk" && !talkManager.HasTalkedToRusk)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Rusk";
            }
            else if (gameObject.name == "Npc_MrHam" && !talkManager.HasTalkedToMrHam)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_MrHam";
            }
            else if (gameObject.name == "Npc_Naru" && !talkManager.HasTalkedToNaru)
            {
                dialogueButton.SetActive(true);
                currentNpc = "Npc_Naru";
            }
            else if (gameObject.name == "Npc_Nicksy" || gameObject.name == "Npc_Coco" || gameObject.name == "Npc_Kuraya")
            {
                dialogueButton.SetActive(false);
            }
            else if (gameObject.name != "Npc_Rayviyak" && gameObject.name != "Npc_Naru" && gameObject.name != "Npc_Violet" && gameObject.name != "Npc_Kuraya" && gameObject.name != "Npc_Rusk" && gameObject.name != "Npc_Coco" && gameObject.name != "Npc_Nicksy" && gameObject.name != "Npc_MrHam" && gameObject.name != "Npc_Ash")
            {
                dialogueButton.SetActive(false);
                currentNpc = "Null";
            }
        }
        else
        {
            dialogueButton.SetActive(false);
            currentNpc = "Null";
        }
    }
}
