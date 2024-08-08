using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopStart : MonoBehaviour
{
    //public Image selectedImageDisplay;
    public GameObject buyButton;
    public GameObject selectedImageDisplay;
    public string fail;
    public string success;
    public TextMeshProUGUI purchase;

    GameObject player;
    TestPlayer test;

    int selected;
    int[] price = { 3500, 3000 };

    
    void Start()
    {
        player = GameObject.Find("TestPlayer");
        test = player.GetComponent<TestPlayer>();
        if (test != null) 
        { 
            Debug.Log("null"); 
        } else { Debug.Log("not null"); }

        // �θ� ������Ʈ �Ǵ� Ư�� ������Ʈ �Ʒ��� ��� Button ������Ʈ�� ã���ϴ�.
        Button[] buttons = GetComponentsInChildren<Button>();

        // �� ��ư�� �̺�Ʈ �����ʸ� �߰��մϴ�.
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }

        Button buyBtn = buyButton.GetComponent<Button>();
        if (buyBtn != null)
        {
            buyBtn.onClick.AddListener(OnBuyButtonClick);
        }
        else
        {
            Debug.LogError("buyButton does not have a Button component.");
        }
    }

    // ��ư�� ������ �� ȣ��Ǵ� �޼ҵ�
    void OnButtonClick(Button clickedButton)
    {
        
        buyButton.SetActive(true);
        selectedImageDisplay.SetActive(true);
        
        // ��ư�� RectTransform ������Ʈ�� �����ɴϴ�.
        RectTransform rectTransform = clickedButton.GetComponent<RectTransform>();
        if (rectTransform.localPosition.x > -76 && rectTransform.localPosition.x < -74)
        {
            selected = 0;
            Debug.Log(selected);
        }
        else
        {
            selected = 1;
            Debug.Log(selected);
        }
        Debug.Log(rectTransform.localPosition.x);
        // �θ� ������Ʈ�� �������� �� ���� ��ǥ�迡���� ��ư ��ġ
        Vector3 localPosition = rectTransform.localPosition;
        localPosition = new Vector3(
            localPosition.x - 2,
            localPosition.y + 24,
            localPosition.z
        );
        
        selectedImageDisplay.transform.localPosition = localPosition;


        //Debug.Log(rectTransform);
        //Debug.Log(localPosition);
        

    }

    void OnBuyButtonClick()
    {
        Debug.Log("Buy Button Clicked!");
        if (test.bean == 500)
        {
            Debug.Log("500");
        }
        else Debug.Log("not 500");

        if (price[selected] > test.bean)
        {
            purchase.text = fail;
        }
        else
        {
            purchase.text = success;
            test.bean -= price[selected];
            test.list[selected] = true;
        }
        
    }
}
