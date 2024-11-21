using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitBtn : MonoBehaviour
{
    private UnityEngine.Events.UnityAction exitBtnEvent;

    void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        exitBtnEvent = CancleUIFunc;
        Button btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(exitBtnEvent);
    }

    public void CancleUIFunc()
    {
        if (SceneManagerEx.Instance.GetCurrentSceneName() != "StartScene")
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerController>().StartMove();
        }

        SoundManager.Instance.PlaySFX("click sound");
        UIManager.Instance.DeactivateCurrentUI();
    }
}
