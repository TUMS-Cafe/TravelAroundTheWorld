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
        talkManager.DisableBedInteraction();

        talkManager.HasTalkedToRayviyak = false;
        talkManager.HasTalkedToViolet = false;
        talkManager.HasTalkedToRusk = false;
        talkManager.HasTalkedToMrHam = false;
        talkManager.HasTalkedToCoco = false;
        talkManager.HasTalkedToNicksy = false;
        talkManager.HasTalkedToNaru = false;
        talkManager.HasTalkedToAsh = false;
        talkManager.HasTalkedToKuraya = false;

        talkManager.Npc_Rayviyak.SetActive(false);
        talkManager.Npc_Violet.SetActive(false);
        talkManager.Npc_Rusk.SetActive(false);
        talkManager.Npc_MrHam.SetActive(false);
        talkManager.Npc_Kuraya.SetActive(false);
        talkManager.Npc_Coco.SetActive(false);
        talkManager.Npc_Nicksy.SetActive(false);
        talkManager.Npc_Naru.SetActive(false);
        talkManager.Npc_Ash.SetActive(false);

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
        //2일차 밤->3일차 아침
        if (talkManager.currentDialogueIndex == 133)
        {
            talkManager.currentDialogueIndex = 155;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        //3일차 밤->4일차 아침
        if (talkManager.currentDialogueIndex == 226)
        {
            talkManager.currentDialogueIndex = 253;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        //4일차 밤->5일차 아침
        if (talkManager.currentDialogueIndex == 356)
        {
            talkManager.currentDialogueIndex = 391;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        //5일차 밤->6일차 아침
        if (talkManager.currentDialogueIndex == 418)
        {
            talkManager.currentDialogueIndex = 453;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        //6일차 밤->7일차 아침 (해피엔딩, 미니게임 성공)
        if (talkManager.isCh2HappyEnding && PlayerManager.Instance.IsCh3MiniGameSuccess() && talkManager.currentDialogueIndex == 533)
        {
            talkManager.currentDialogueIndex = 534;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        //6일차 밤->7일차 아침 (해피엔딩, 미니게임 실패)
        if (talkManager.isCh2HappyEnding && !PlayerManager.Instance.IsCh3MiniGameSuccess() && talkManager.currentDialogueIndex == 533)
        {
            talkManager.currentDialogueIndex = 534;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        //6일차 밤->7일차 아침 (배드엔딩, 미니게임 성공)
        if (!talkManager.isCh2HappyEnding && PlayerManager.Instance.IsCh3MiniGameSuccess() && talkManager.currentDialogueIndex == 683)
        {
            talkManager.currentDialogueIndex = 684;
            talkManager.PrintProDialogue(talkManager.currentDialogueIndex);
        }
        //6일차 밤->7일차 아침 (해피엔딩, 미니게임 실패)
        if (!talkManager.isCh2HappyEnding && !PlayerManager.Instance.IsCh3MiniGameSuccess() && talkManager.currentDialogueIndex == 683)
        {
            talkManager.currentDialogueIndex = 684;
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
