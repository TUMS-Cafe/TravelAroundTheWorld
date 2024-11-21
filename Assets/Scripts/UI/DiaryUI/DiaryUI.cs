using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class DiaryUI : MonoBehaviour
{
    public GameObject saveBtn;
    public GameObject exitBtn;

    private Vector2 saveBtnPos;
    private Vector2 exitBtnPos;

    private UnityEngine.Events.UnityAction btnEvent;

    private void Start()
    {
        SetInitData();
        CreateBtn();
    }

    void SetInitData()
    {
        saveBtnPos = new Vector2(200, 200);
        exitBtnPos = new Vector2(100, -100);
    }

    void CreateBtn()
    {
        UIManager.Instance.CreateUIComponent(saveBtn, saveBtnPos, gameObject);
        UIManager.Instance.CreateUIComponent(exitBtn, exitBtnPos, gameObject);
    }
}
