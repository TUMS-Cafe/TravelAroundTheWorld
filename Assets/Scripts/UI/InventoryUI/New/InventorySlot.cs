using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Item item;
    public int itemCount;

    public Image itemImage;
    public TextMeshProUGUI itemCountText;
    public GameObject selectionIndicator;

    private void Start()
    {
        selectionIndicator.SetActive(false);
        UpdateUI();
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
        itemCount = 1;
        UpdateUI();
    }

    public void IncreaseItemCount()
    {
        itemCount++;
        UpdateUI();
    }

    public void Select()
    {
        selectionIndicator.SetActive(true);
    }

    public void Deselect()
    {
        selectionIndicator.SetActive(false);
    }

    public void OnClickSlot()
    {
        InventoryManager.Instance.SelectSlot(this);
    }

    private void UpdateUI()
    {
        if (item != null)
        {
            itemImage.sprite = item.itemImage;
            itemImage.gameObject.SetActive(true);
            itemCountText.text = itemCount.ToString();
            itemCountText.gameObject.SetActive(true);
        }
        else
        {
            itemImage.gameObject.SetActive(false);
            itemCountText.gameObject.SetActive(false);
        }
    }
}