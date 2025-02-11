using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventorySlot> slots = new List<InventorySlot>();
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public Image itemImage;

    private InventorySlot selectedSlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void AddItem(Item newItem)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == newItem)
            {
                slot.IncreaseItemCount();
                return;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.item == null)
            {
                slot.SetItem(newItem);
                return;
            }
        }
    }

    public void SelectSlot(InventorySlot slot)
    {
        if (selectedSlot != null)
            selectedSlot.Deselect();

        selectedSlot = slot;
        selectedSlot.Select();

        if (slot.item != null)
        {
            itemNameText.text = slot.item.itemName;
            itemImage.sprite = slot.item.itemImage;
            itemDescriptionText.text = slot.item.description;
        }
    }
}