using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAPUIController : MonoBehaviour
{
    [Header("Pages")]
    public GameObject Inventory;
    public GameObject Save;
    public GameObject Setting;

    public GameObject uiPanel;
    private bool isUIPanelOpen = false;

    private void Start()
    {
        ShowPage(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUIPanel();
        }
    }

    void ToggleUIPanel()
    {
        isUIPanelOpen = !isUIPanelOpen;
        uiPanel.SetActive(isUIPanelOpen);

        if (isUIPanelOpen)
        {
            ShowPage(1);
        }
        else
        {
            Save.SetActive(false);
            Setting.SetActive(false);
        }
    }

    public void ShowPage(int pageNumber)
    {
        Inventory.SetActive(pageNumber == 1);
        Save.SetActive(pageNumber == 2);
        Setting.SetActive(pageNumber == 3);
    }
}