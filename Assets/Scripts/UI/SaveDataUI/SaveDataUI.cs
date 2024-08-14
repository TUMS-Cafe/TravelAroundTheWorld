using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataUI : MonoBehaviour
{
    public GameObject saveDataBackground;
    public GameObject titleTxt;
    public GameObject scrollView;

    private Vector2 titlePos;
    private Vector2 defaultPos;
    private Vector2 scrollPos;

    private UIManager uIManager;

    private void Awake()
    {
        uIManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {
        SetInitData();
        CreateUIComponent(saveDataBackground, defaultPos);
        CreateUIComponent(titleTxt, titlePos);
        CreateUIComponent(scrollView, scrollPos);
    }


    void SetInitData()
    {
        defaultPos = Vector2.zero;
        titlePos = new Vector2(-420, 395);
        scrollPos = new Vector2(0, -25);
    }

    public void CreateUIComponent(GameObject obj, Vector2 pos)
    {
        GameObject placedObj = Instantiate(obj);
        placedObj.transform.SetParent(gameObject.transform, false);

        RectTransform rectTransform = placedObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos;

    }

    public void CreateUIComponent(GameObject obj, Vector2 pos, bool isActivated)
    {
        GameObject placedObj = Instantiate(obj);
        placedObj.transform.SetParent(gameObject.transform, false);

        RectTransform rectTransform = placedObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos;
        placedObj.SetActive(isActivated);
    }

}
