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

    private void Awake()
    {
        btnDataList = new List<BtnDataSet>();
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        SetBtnData();

        CreateChoiceBtnGroup();
    }

    void SetBtnData()
    {
        btnPos = new Vector2(50, 0);

        BtnDataSet yesBtn = new BtnDataSet
        {
            btnName = "YesBtn",
            btnTxt = "예",
            btnEvent = GoToNextDay
        };

        BtnDataSet noBtn = new BtnDataSet
        {
            btnName = "NoBtn",
            btnTxt = "아니오",
            btnEvent = DeactivateUI
        };

        btnDataList.Add(yesBtn);
        btnDataList.Add(noBtn);
    }

    void CreateChoiceBtnGroup()
    {
        int btnDataNum = btnDataList.Count;

        for(int i=0; i<btnDataNum; i++)
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
    }

    void DeactivateUI()
    {
        SoundManager.Instance.PlaySFX("click sound");
        UIManager.Instance.DeactivatedUI("Bed");
        player.GetComponent<PlayerController>().StartMove();
       }
}
