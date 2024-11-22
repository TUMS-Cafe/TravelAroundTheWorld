using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BedButton : MonoBehaviour
{
    public GameObject BedUI;

    public void OnYesButtonClicked(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("씬 존재하지 않음");
        }
    }

    public void OnNoButtonClicked()
    {
        if (BedUI != null)
        {
            BedUI.SetActive(false);
            //플레이어 이동 활성화 등 UI 비활성화 말고 추가해야 할 조건이 있다면 여기 추가
        }
        else
        {
            Debug.LogError("UI 오류");
        }
    }
}