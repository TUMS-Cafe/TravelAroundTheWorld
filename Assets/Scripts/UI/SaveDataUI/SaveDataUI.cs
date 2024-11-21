using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveDataUI : MonoBehaviour
{
    public GameObject saveDataBackground;
    public GameObject titleTxt;
    public GameObject scrollView;
    public GameObject exitBtn;

    private Vector2 titlePos;
    private Vector2 defaultPos;
    private Vector2 scrollPos;
    private Vector2 exitPos;

    void Start()
    {
        SetInitData();
        UIManager.Instance.CreateUIComponent(saveDataBackground, defaultPos, gameObject);
        UIManager.Instance.CreateUIComponent(titleTxt, titlePos, gameObject);
        UIManager.Instance.CreateUIComponent(scrollView, scrollPos, gameObject); 
        SetScrollViewData();
        UIManager.Instance.CreateUIComponent(exitBtn, exitPos, gameObject);
    }


    void SetInitData()
    {
        defaultPos = Vector2.zero;
        titlePos = new Vector2(-315, 395);
        scrollPos = new Vector2(0, -25);
        exitPos = new Vector2(494, 315);
    }
    

    public void SetScrollViewData()
    {
        if (SaveDataManager.Instance.HasSaveData())
            return;

        else
        {
            List<int> saveDataList = SaveDataManager.Instance.GetAvailableSaveSlots();
            int saveDataListCount = saveDataList.Count;

            
                ScrollRect scrollRect = scrollView.GetComponent<ScrollRect>();

                for (int i = 0; i < saveDataListCount; i++)
                {
                    TextMeshProUGUI[] textArr = scrollRect.content.GetChild(i).GetComponents<TextMeshProUGUI>();
                    textArr[0].text = PlayerManager.Instance.GetIsAutoSaveToString();
                    textArr[1].text = PlayerManager.Instance.GetSceneName();
                    textArr[2].text = PlayerManager.Instance.GetMapLocation().ToString();
                    textArr[3].text = PlayerManager.Instance.GetSaveTime().ToString();
                }
            
        }
        
    }
}
