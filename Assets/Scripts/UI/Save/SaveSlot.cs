using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    public int slotIndex;
    public TextMeshProUGUI playTimeText;
    public GameObject checkImage;

    private bool isUsed = false;

    private void Start()
    {
        LoadSlot();
    }

    public void OnClickSlot()
    {
        if (!isUsed)
        {
            float playTime = Time.timeSinceLevelLoad;

            SaveManager.Instance.SaveGame(slotIndex, playTime);
            UpdateSlot(playTime, true);
        }
    }

    public void LoadSlot()
    {
        float playTime;
        SaveManager.Instance.LoadGame(slotIndex, out playTime, out isUsed);
        UpdateSlot(playTime, isUsed);
    }

    private void UpdateSlot(float playTime, bool used)
    {
        isUsed = used;
        int minutes = Mathf.FloorToInt(playTime / 60);
        int seconds = Mathf.FloorToInt(playTime % 60);
        playTimeText.text = used ? string.Format("{0:00}:{1:00}", minutes, seconds) : "";
        checkImage.SetActive(used);
    }

    public void OnDeleteSlot()
    {
        SaveManager.Instance.DeleteGame(slotIndex);
        UpdateSlot(0, false);
    }
}