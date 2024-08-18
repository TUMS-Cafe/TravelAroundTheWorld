using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject inventoryUIPrefab;
    public GameObject settingUIPrefab;
    public GameObject mapUIPrefab;
    public GameObject loadUIPrefab;
    public GameObject saveDataUIPrefab;
    public GameObject saveDataPopupPrefab;
    public GameObject bedInteractionUIPrefab;
    public GameObject diaryInteractionUIPrefab;
    public GameObject staticUICanvasPrefab;
    public GameObject dynamicUICanvasPrefab;


    private Dictionary<string, GameObject> uiInstances = new Dictionary<string, GameObject>();
    private Canvas canvas;
    private string currentActiveUI = null;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        
         canvas = FindObjectOfType<Canvas>();
         
    }


    public void ToggleUI(string uiName)
    {
        //??? ?? ?? ?? ??? ?? ??
        canvas = GameObject.Find("StaticUICanvas").gameObject.GetComponent<Canvas>();

        if (currentActiveUI == uiName)
        {
            DeactivateAllUI();
            currentActiveUI = null;
            return;
        }

        DeactivateAllUI();

        if (!uiInstances.ContainsKey(uiName))
        {
            GameObject uiPrefab = GetUIPrefab(uiName);

            if(uiPrefab != null)
            {
                GameObject uiInstance = Instantiate(uiPrefab, canvas.transform, false);
                uiInstances[uiName] = uiInstance;
            }
        }

        if(uiInstances.ContainsKey(uiName))
        {
            uiInstances[uiName].SetActive(true);
            currentActiveUI = uiName;
        }
    }

    public void DeactivatedUI(string uiName)
    {
        uiInstances[uiName].SetActive(false);
        currentActiveUI = null;
    }


    private void DeactivateAllUI()
    {
        foreach (var uiInstance in uiInstances.Values)
        {
            uiInstance.SetActive(false);
            currentActiveUI = null;
        }
    }

    private GameObject GetUIPrefab(string uiName)
    {
        switch(uiName)
        {
            case "Inventory":
                return inventoryUIPrefab;
            case "Setting":
                return settingUIPrefab;
            case "Map":
                return mapUIPrefab;
            case "Load":
                return loadUIPrefab;
            case "SaveData":
                return saveDataUIPrefab;
            case "SaveDataPopup":
                return saveDataPopupPrefab;
            case "Bed":
                return bedInteractionUIPrefab;
            case "Diary":
                return diaryInteractionUIPrefab;
            default:
                return null;
        }
    }


    public bool IsUIActive()
    {
        return currentActiveUI != null;
    }

    public GameObject FindChildByName(GameObject parent, string childName)
    {
        if (parent == null)
        {
            Debug.LogWarning("Parent object is null.");
            return null;
        }

        Transform[] children = parent.GetComponentsInChildren<Transform>(true);
        Transform childTransform = children.FirstOrDefault(t => t.name == childName);
        return childTransform != null ? childTransform.gameObject : null;
    }

    public void ActiveUI(string uiName)
    {
        if (!uiInstances.ContainsKey(uiName))
        {
            GameObject uiPrefab = GetUIPrefab(uiName);

            if (uiPrefab != null)
            {
                GameObject uiInstance = Instantiate(uiPrefab, canvas.transform, false);
                uiInstances[uiName] = uiInstance;
            }
        }

        if (uiInstances.ContainsKey(uiName))
        {
            uiInstances[uiName].SetActive(true);
            currentActiveUI = uiName;
        }
    }
}
