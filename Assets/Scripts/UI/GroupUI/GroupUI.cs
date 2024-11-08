using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GroupUI : MonoBehaviour
{
    public GameObject categorybtnPrefab;

    private List<BtnDataSet> btnDataList;
    private List<PosData> btnPosList;

    private Vector2 tagBtnPos;

    private int xPos;

    private void Awake()
    {
        btnDataList = new List<BtnDataSet>();
        btnPosList = new List<PosData>();
        xPos = -600;
    }

    void Start()
    {
        SetUIData();
        SetBtnData();

        CreateTagBtnGroup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetUIData()
    {
        tagBtnPos = new Vector2(-600, 300);
    }

    void SetBtnData()
    {
        BtnDataSet inventoryBtn = new BtnDataSet
        {
            btnName = "InventoryBtn",
            btnTxt = "Inventory",
            btnEvent = InventoryFunc
        };

        BtnDataSet diaryBtn = new BtnDataSet
        {
            btnName = "DiaryBtn",
            btnTxt = "Diary",
            btnEvent = DiaryFunc
        };

        BtnDataSet questBtn = new BtnDataSet
        {
            btnName = "QuestBtn",
            btnTxt = "Quest",
            btnEvent = QuestFunc
        };

        BtnDataSet characterBtn = new BtnDataSet
        {
            btnName = "CharacterBtn",
            btnTxt = "Character",
            btnEvent = CharacterFunc
        };

        BtnDataSet settingBtn = new BtnDataSet
        {
            btnName = "SettingBtn",
            btnTxt = "Setting",
            btnEvent = SettingFunc
        };

        btnDataList.Add(inventoryBtn);
        btnDataList.Add(diaryBtn);
        btnDataList.Add(questBtn);
        btnDataList.Add(characterBtn);
        btnDataList.Add(settingBtn);
    }

    void CreateTagBtnGroup()
    {
        int btnDataNum = btnDataList.Count;

        float btnYPos = tagBtnPos.y;

        for(int i=0; i<btnDataNum; i++)
        {
            GameObject btn = Instantiate(categorybtnPrefab);
            btn.transform.SetParent(gameObject.transform, false);

            Vector2 btnPos = new Vector2(xPos, btnYPos - i * 150);

            RectTransform rectTransform = btn.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = btnPos;

            TextMeshProUGUI btnTxt = btn.GetComponentInChildren<TextMeshProUGUI>();

            Button btnComponent = btn.GetComponent<Button>();

            btn.name = btnDataList[i].btnName;
            btnTxt.text = btnDataList[i].btnTxt;
            btnComponent.onClick.AddListener(btnDataList[i].btnEvent);
            btnComponent.onClick.AddListener(() => SoundManager.Instance.PlaySFX("click sound"));
        }
    }

    void InventoryFunc()
    {
        UIManager.Instance.ToggleUI("Inventory");
    }

    void DiaryFunc()
    {
        UIManager.Instance.ToggleUI("Diary");
    }

    void QuestFunc()
    {
        UIManager.Instance.ToggleUI("QuestionGroup");
    }

    void CharacterFunc()
    {
        //UIManager.Instance.ToggleUI("Character");
    }

    void SettingFunc()
    {
        UIManager.Instance.ToggleUI("Setting");
    }
}
