using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch3BedButton : MonoBehaviour
{
    public GameObject BedUI;
    public Ch3TalkManager talkManager;

    private GameObject player;
    private PlayerAnimationController playerAnimationController;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerAnimationController = player.GetComponent<PlayerAnimationController>();
    }

    public void OnYesButtonClicked()
    {
        BedUI.SetActive(false);
        talkManager.HasTalkedToRayviyak = false;
        talkManager.HasTalkedToViolet = false;
        talkManager.HasTalkedToRusk = false;
        talkManager.HasTalkedToMrHam = false;
        talkManager.HasTalkedToKuraya = false;
        talkManager.Npc_Rayviyak.SetActive(false);
        talkManager.Npc_Violet.SetActive(false);
        talkManager.Npc_Rusk.SetActive(false);
        talkManager.Npc_MrHam.SetActive(false);
        talkManager.Npc_Kuraya.SetActive(false);
        talkManager.isWaitingForPlayer = false;
        talkManager.isNpcTalkActivated = false;
        talkManager.currentNpc = "Null";
        talkManager.map.SetActive(false);
        talkManager.trainRoom.SetActive(true);
        //1일차 밤->2일차 아침
        if (talkManager.currentDialogueIndex == 76)
        {
            talkManager.currentDialogueIndex = 93;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        
        player.GetComponent<PlayerController>().StartMove();
    }

    public void OnNoButtonClicked()
    {
        if (BedUI != null)
        {
            BedUI.SetActive(false);
            //플레이어 이동 활성화 등 UI 비활성화 말고 추가해야 할 조건이 있다면 여기 추가
        }
        else
        {
            Debug.LogError("UI 오류");
        }
    }
}
