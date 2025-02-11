using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void SaveGame(int slotIndex, float playTime)
    {
        PlayerPrefs.SetFloat("Slot_" + slotIndex + "_Time", playTime);
        PlayerPrefs.SetInt("Slot_" + slotIndex + "_Used", 1);
        PlayerPrefs.Save();
    }

    public void LoadGame(int slotIndex, out float playTime, out bool isUsed)
    {
        isUsed = PlayerPrefs.GetInt("Slot_" + slotIndex + "_Used", 0) == 1;
        playTime = PlayerPrefs.GetFloat("Slot_" + slotIndex + "_Time", 0);
    }

    public void DeleteGame(int slotIndex)
    {
        PlayerPrefs.DeleteKey("Slot_" + slotIndex + "_Time");
        PlayerPrefs.DeleteKey("Slot_" + slotIndex + "_Used");
        PlayerPrefs.Save();
    }
}