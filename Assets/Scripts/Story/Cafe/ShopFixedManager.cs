using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopFixedManager : MonoBehaviour
{
    public GameObject selectedImageDisplay;

    public GameObject newSpeech;
    public GameObject buyAnithing;


    private Ch1TalkManager talkManager;

    void Start()
    {
        talkManager = FindObjectOfType<Ch1TalkManager>();
    }


    // 상점 UI를 숨기는 메서드
    public void HideShop()
    {
        SoundManager.Instance.PlaySFX("click sound");
        Debug.Log("우유 구매 여부 : " + PlayerManager.Instance.IsBoughtCafeItem("milk"));
        Debug.Log("티세트 구매 여부 : " + PlayerManager.Instance.IsBoughtCafeItem("teaSet"));
        if (PlayerManager.Instance.IsBoughtCafeItem("milk") || PlayerManager.Instance.IsBoughtCafeItem("teaSet"))
        {
            if (talkManager != null)
            {
                talkManager.OnShopClosed();
            }
            //Transform parentTransform = transform.parent;
            Destroy(transform.parent.gameObject);
        }
        else
        {
            buyAnithing.SetActive(true);
            newSpeech.SetActive(false);
            selectedImageDisplay.SetActive(false);
        }
        

    }

    public void HideShopMilk()
    {
        SoundManager.Instance.PlaySFX("click sound");
        Debug.Log("우유 구매 여부 : " + PlayerManager.Instance.IsBoughtCafeItem("milk"));
        Debug.Log("티세트 구매 여부 : " + PlayerManager.Instance.IsBoughtCafeItem("teaSet"));
        if (PlayerManager.Instance.IsBoughtCafeItem("milk"))
        {
            if (talkManager != null)
            {
                talkManager.OnShopClosed();
            }
            //Transform parentTransform = transform.parent;
            Destroy(transform.parent.gameObject);
        }
        else
        {
            buyAnithing.SetActive(true);
            newSpeech.SetActive(false);
            selectedImageDisplay.SetActive(false);
        }
    }
    public void HideShopTeaSet()
    {
        SoundManager.Instance.PlaySFX("click sound");
        Debug.Log("우유 구매 여부 : " + PlayerManager.Instance.IsBoughtCafeItem("milk"));
        Debug.Log("티세트 구매 여부 : " + PlayerManager.Instance.IsBoughtCafeItem("teaSet"));
        if (PlayerManager.Instance.IsBoughtCafeItem("teaSet"))
        {
            if (talkManager != null)
            {
                talkManager.OnShopClosed();
            }
            //Transform parentTransform = transform.parent;
            Destroy(transform.parent.gameObject);
        }
        else
        {
            buyAnithing.SetActive(true);
            newSpeech.SetActive(false);
            selectedImageDisplay.SetActive(false);
        }
    }

    public void HideSpeech()
    {
        

        transform.parent.gameObject.SetActive(false);
        selectedImageDisplay.SetActive(false);

        
    }

    

}

