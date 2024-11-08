using UnityEngine;
using UnityEngine.UI;

public class SaveBtn : MonoBehaviour
{
    private UnityEngine.Events.UnityAction saveBtnEvent;

    void Start()
    {
        SetBtnEvent();
        //TestGetSaveData();
    }

    void SetBtnEvent()
    {
        saveBtnEvent = SavePlayerDataFunc;
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(saveBtnEvent);
    }

    public void SavePlayerDataFunc()
    {
        Debug.Log("Save PlayerData");

        SaveDataManager.Instance.SaveGame(PlayerManager.Instance.currentData);
    }

    private void TestGetSaveData()
    {
        PlayerManager.Instance.SetPlayerData(0);
    }
}
