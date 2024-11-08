using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    //Refactor : 해당 부분 가져오도록 설정 
    
    
    public GameObject inventoryUI;

    private Button itemSlot;
    private Vector2 selectedItemImgPos;

    private void Awake()
    {
        SetInit();
    }

    private void Start()
    {
        itemSlot.onClick.AddListener(ShowItemData);
    }

    public void ShowItemData()
    {
        /*
         * Audiosouce 추가 예정
         */

        /*
         * 현재 클릭한 itemSlot 게임오브젝트 정보 가져오기
         * 
         * 해당 아이템 이미지 정보 클릭한 itemSlot에서 가져오기
         * 가져온 이미지 정보 itemImg, itemName, itemDesc에 작성하
         * 
         */

        inventoryUI = transform.parent.gameObject;


        GameObject clickSlot = EventSystem.current.currentSelectedGameObject;
        Vector2 selectedSlotPos = clickSlot.GetComponent<RectTransform>().anchoredPosition;

        inventoryUI.GetComponent<InventoryUI>().selectedSlotImg.SetActive(true);
        inventoryUI.GetComponent<InventoryUI>().selectedSlotImg.GetComponent<RectTransform>().anchoredPosition = selectedSlotPos;

        inventoryUI.GetComponent<InventoryUI>().selectedItemImg.SetActive(true);
        inventoryUI.GetComponent<InventoryUI>().selectedItemImg.GetComponent<RectTransform>().anchoredPosition = selectedItemImgPos;

        inventoryUI.GetComponent<InventoryUI>().itemNamePrefab.SetActive(true);
        inventoryUI.GetComponent<InventoryUI>().itemDescPrefab.SetActive(true);
        inventoryUI.GetComponent<InventoryUI>().itemImagePrefab.SetActive(true);


    }

    void SetInit()
    {
        itemSlot = gameObject.GetComponent<Button>();
        selectedItemImgPos = new Vector2(407, 137);
        
    }


}
