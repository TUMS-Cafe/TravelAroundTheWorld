using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 

public class ChoiceBtn : MonoBehaviour
{
    public GameObject choiseBtn;
    private Vector2 btnPos;
    private List<BtnDataSet> btnDataList;
    private GameObject player;

    private string yesData;
    private string noData;

    public GameObject bedNarration;

    private void Awake()
    {
        btnDataList = new List<BtnDataSet>();
        player = GameObject.FindWithTag("Player");
        bedNarration = GameObject.Find("BedNarration");
    }
    void Start()
    {
        SetStringData();
        SetBtnData();
        CreateChoiceBtnGroup();
    }
    void SetBtnData()
    {
        btnPos = new Vector2(50, 0);
        BtnDataSet yesBtn = new BtnDataSet
        {
            btnName = "YesBtn",
            btnTxt = yesData,
            btnEvent = GoToNextDay
        };
        BtnDataSet noBtn = new BtnDataSet
        {
            btnName = "NoBtn",
            btnTxt = noData,
            btnEvent = DeactivateUI
        };
        btnDataList.Add(yesBtn);
        btnDataList.Add(noBtn);
    }

    void SetStringData()
    {
        //언어 상태에 따라 다르게 설정하는 기능 추가

        yesData = "예";
        noData = "아니오";
    }

    void CreateChoiceBtnGroup()
    {
        int btnDataNum = btnDataList.Count;
        for (int i = 0; i < btnDataNum; i++)
        {
            GameObject btn = Instantiate(choiseBtn);
            btn.transform.SetParent(gameObject.transform, false);
            RectTransform rectTransform = btn.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = btnPos;
            TextMeshProUGUI btnTxt = btn.GetComponentInChildren<TextMeshProUGUI>();
            Button btnComponent = btn.GetComponent<Button>();
            btn.name = btnDataList[i].btnName;
            btnTxt.text = btnDataList[i].btnTxt;
            btnComponent.onClick.AddListener(btnDataList[i].btnEvent);
            btnPos.x += 670;
        }
    }
    void GoToNextDay()
    {
        SoundManager.Instance.PlaySFX("click sound");

        if (SceneManagerEx.Instance.GetCurrentSceneName() == "Ch0Scene")
        {
            //TalkManager 찾는 코드
            TalkManager talkManager = FindInactiveTalkManager();
            if (talkManager != null)
            {
                Debug.Log("Found TalkManager, even if it was inactive.");
                if (talkManager.isAllNPCActivated)
                {
                    GameObject.Find("MapTutorial").GetComponent<MapTurorial>().isSleeping = true;
                    DayNightCycleManager.Instance.ChangeOnlyDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else
                {
                    GameObject.Find("MapTutorial").GetComponent<MapTurorial>().isSleeping = true;
                }
            }
            else
            {
                Debug.Log("TalkManager not found.");
            }
        }
        else
        {
            Ch1TalkManager talkManager = Ch1TalkManager.Instance;

            if (talkManager != null)
            {
                if (talkManager.currentDialogueIndex == 200)
                {
                    bedNarration.GetComponent<TextMeshProUGUI>().text = "상점을 이용하는 게 좋을 것 같다.";
                    bedNarration.SetActive(true);
                    UIManager.Instance.DeactivatedUI("Bed");
                    return;
                }
                else if (talkManager.currentDialogueIndex == 29) //2->3
                {
                    talkManager.isWaitingForPlayer = false;
                    talkManager.player.SetActive(false);
                    talkManager.map.SetActive(false);
                    talkManager.trainRoom.SetActive(true);
                    talkManager.currentDialogueIndex = 71;
                    talkManager.PrintCh1ProDialogue(talkManager.currentDialogueIndex);
                    DayNightCycleManager.Instance.ChangeDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else if (talkManager.currentDialogueIndex == 111) //3->4
                {
                    talkManager.isWaitingForPlayer = false;
                    talkManager.player.SetActive(false);
                    talkManager.map.SetActive(false);
                    talkManager.trainRoom.SetActive(true);
                    talkManager.currentDialogueIndex = 142;
                    talkManager.PrintCh1ProDialogue(talkManager.currentDialogueIndex);
                    DayNightCycleManager.Instance.ChangeDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else if (talkManager.currentDialogueIndex == 220) //4->5
                {
                    talkManager.isWaitingForPlayer = false;
                    talkManager.player.SetActive(false);
                    talkManager.map.SetActive(false);
                    talkManager.trainRoom.SetActive(true);
                    talkManager.currentDialogueIndex = 271;
                    talkManager.PrintCh1ProDialogue(talkManager.currentDialogueIndex);
                    DayNightCycleManager.Instance.ChangeDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else if (talkManager.currentDialogueIndex == 334) //5->6
                {
                    talkManager.isWaitingForPlayer = false;
                    talkManager.player.SetActive(false);
                    talkManager.map.SetActive(false);
                    talkManager.trainRoom.SetActive(true);
                    talkManager.currentDialogueIndex = 361;
                    talkManager.PrintCh1ProDialogue(talkManager.currentDialogueIndex);
                    DayNightCycleManager.Instance.ChangeDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else if (talkManager.currentDialogueIndex == 404) //6->7
                {
                    talkManager.isWaitingForPlayer = false;
                    talkManager.player.SetActive(false);
                    talkManager.map.SetActive(false);
                    talkManager.trainRoom.SetActive(true);
                    talkManager.currentDialogueIndex = 406;
                    talkManager.PrintCh1ProDialogue(talkManager.currentDialogueIndex);
                    DayNightCycleManager.Instance.ChangeDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else if (talkManager.currentDialogueIndex == 453) // 치타 상점 끝나고 객실로, 7->8
                {
                    talkManager.isWaitingForPlayer = false;
                    talkManager.player.SetActive(false);
                    talkManager.map.SetActive(false);
                    talkManager.trainRoom.SetActive(true);
                    talkManager.currentDialogueIndex++;
                    talkManager.PrintCh1ProDialogue(talkManager.currentDialogueIndex);
                    DayNightCycleManager.Instance.ChangeDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else if (talkManager.currentDialogueIndex == 532) // 마지막 부분
                {
                    talkManager.isWaitingForPlayer = false;
                    talkManager.player.SetActive(false);
                    talkManager.map.SetActive(false);
                    talkManager.trainRoom.SetActive(true);
                    talkManager.currentDialogueIndex++;
                    talkManager.PrintCh1ProDialogue(talkManager.currentDialogueIndex);
                    DayNightCycleManager.Instance.ChangeDay();
                    PlayerManager.Instance.SetCurrentTimeofDay();
                }
                else
                {
                    DayNightCycleManager.Instance.ChangeDay();
                }
            }
        }

        UIManager.Instance.DeactivatedUI("Bed");
        bedNarration.SetActive(false);
        //fadeOut

        //Debug.Log(PlayerManager.Instance.GetDay());
    }

    TalkManager FindInactiveTalkManager()
    {
        // 씬 내의 모든 TalkManager 오브젝트를 포함한 리스트를 찾음
        TalkManager[] allTalkManagers = Resources.FindObjectsOfTypeAll<TalkManager>();

        // TalkManager가 존재하고, 활성화 상태가 아닌 오브젝트 중 하나를 반환
        //1개만 있어서 이렇게 했지만 수정 필요
        return allTalkManagers[0];//.Where(tm => !tm.gameObject.activeInHierarchy).FirstOrDefault();
    }

    void DeactivateUI()
    {
        SoundManager.Instance.PlaySFX("click sound");
        UIManager.Instance.DeactivatedUI("Bed");
        bedNarration.SetActive(false);
        player.GetComponent<PlayerController>().StartMove();
     }
        
}