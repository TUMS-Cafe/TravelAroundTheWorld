using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //public Sprite selectedImage; // ������ �̹���
    //public string itemName;
    public string itemDescription; // ������ ����

    //public Image selectedImageDisplay;
    //public TextMeshProUGUI itemNameDisplay;
    public TextMeshProUGUI itemDescriptionDisplay;

    public void OnSlotClicked()
    {
        //selectedImageDisplay.enabled = true;
        //itemNameDisplay.text = itemName;
        itemDescriptionDisplay.text = itemDescription; // ������ ����
;
        //shopManager.DisplayItemDetails(itemImage.sprite, itemDescription, this);
    }
}
