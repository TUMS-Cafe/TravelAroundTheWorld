using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    private CafeTalkManager cafeTalkManager;
    private RectTransform rectTransform;
    private RectTransform descriptionRectTransform;

    public Vector2 extractPosition = new Vector2(-964.16f, -538.02f);
    public Vector2 cupPosition = new Vector2(-957.68f, -539.35f);
    public Vector2 ingredientPosition = new Vector2(-954.03f, -536.07f);
    public Vector2 makePosition = new Vector2(-953.84f, -540.12f);
    public Vector2 donePosition = new Vector2(-955.17f, -543.63f);


    public Vector2 descExtractPosition = new Vector2(-430, 260);
    public Vector2 descCupPosition = new Vector2(264, 106);
    public Vector2 descIngredientPosition = new Vector2(650, 460);
    public Vector2 descMakePosition = new Vector2(680, 20);
    public Vector2 descDonePosition = new Vector2(550, -350);

    //여기는 대사 idx를 잘 모르겠습니다..
    public Vector2 descIcedAmericanoPosition = new Vector2(675, 31);

    public Vector2 descNewPosition;

    void Start()
    {
        cafeTalkManager = FindObjectOfType<CafeTalkManager>();
        //rectTransform = GetComponent<RectTransform>();
        //descriptionRectTransform = transform.Find("Description").GetComponent<RectTransform>();
        GameObject explainImg = GameObject.Find("ExplainBar");
        rectTransform = explainImg.GetComponent<RectTransform>();

        TextMeshProUGUI descText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        descriptionRectTransform = descText.GetComponent<RectTransform>();

        // Ensure that Description has the same scale as its parent
        descriptionRectTransform.localScale = Vector3.one;

        // Ensure that Description has the correct z-order
        //descriptionRectTransform.SetSiblingIndex(rectTransform.GetSiblingIndex() + 1);
        descNewPosition = Vector2.zero;
    }

    void Update()
    {
        Vector2 newPosition = Vector2.zero;

        switch (cafeTalkManager.currentDialogueIndex)
        {
            case 40:
            case 41:
            case 42:
                newPosition = extractPosition;
                descNewPosition = descExtractPosition;
                break;
            case 43:
            case 44:
            case 45:
                newPosition = cupPosition;
                descNewPosition = descCupPosition;
                break;
            case 46:
                newPosition = ingredientPosition;
                descNewPosition = descIngredientPosition;
                break;
            case 47:
            case 48:
            case 51:
                newPosition = makePosition;
                descNewPosition = descMakePosition;
                break;
            case 49:
                newPosition = donePosition;
                descNewPosition = descDonePosition;
                break;
            default:
                return;
        }

        rectTransform.anchoredPosition = newPosition;
        descriptionRectTransform.anchoredPosition = descNewPosition;
        //descriptionRectTransform.anchoredPosition = Vector2.zero; // 부모 위치 기준으로 Description 위치 조정

        Debug.Log("ExplainBar new position: " + newPosition);
        Debug.Log("Description parent: " + descriptionRectTransform.parent.name);
        Debug.Log("Description current position: " + descriptionRectTransform.anchoredPosition);
    }
}