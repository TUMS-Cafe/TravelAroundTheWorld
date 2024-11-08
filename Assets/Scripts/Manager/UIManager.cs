using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    //필요한 UI 리스트
    public GameObject inventoryUIPrefab;
    public GameObject settingUIPrefab;
    public GameObject mapUIPrefab;
    public GameObject loadUIPrefab;
    public GameObject saveDataUIPrefab;
    public GameObject saveDataPopupPrefab;
    public GameObject bedInteractionUIPrefab;
    public GameObject diaryUIPrefab;
    public GameObject groupUIPrefab;
    public GameObject questionGroupUIPrefab;
    //public GameObject saveUIPrefab;

    private Dictionary<string, GameObject> uiInstances = new Dictionary<string, GameObject>();
    private Canvas staticUICanvas;
    private Canvas dynamicUICanvas;
    private string currentActiveUI = null;
    private Stack<string> activeUIStack = new Stack<string>();


    // 제외할 씬 리스트
    private readonly List<string> excludedScenes = new List<string>
    {
        "LoadingScene",
        "StartScene",
        "CafeScene",
        "CafeTutorialScene"
    };


    //UIManager 싱글톤 적용
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    //씬 로드될 때 이벤트 추가
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!excludedScenes.Contains(scene.name))
        {
            SetupUIForScene();
        }
        else
        {
            ClearUIInstances();
        }
    }


    //씬 로드될 때 기본 세팅
    private void SetupUIForScene()
    {
        staticUICanvas = GameObject.Find("StaticUICanvas")?.GetComponent<Canvas>();
        dynamicUICanvas = GameObject.Find("DynamicUICanvas")?.GetComponent<Canvas>();

        if (staticUICanvas == null || dynamicUICanvas == null)
        {
            Debug.LogError("UIManager can't find StaticUICanvas or DynamicUICanvas");
            return;
        }

        //Static : 고정된 데이터
        //Dynamic : 플레이어에 연관된 데이터
        string[] staticUINames = {};
        string[] dynamicUINames = {"Group","Inventory", "Map", "Load", "Bed", "Diary", "Setting", "SaveData", "SaveDataPopup", "QuestionGroup"};


        //Static Canvas에 필요한 UI 생성
        foreach (var uiName in staticUINames)
        {
            InstantiateAndDeactivateUI(uiName, staticUICanvas);
        }

        // Dynamic Canvas에 필요한 UI 생성
        foreach (var uiName in dynamicUINames)
        {
            InstantiateAndDeactivateUI(uiName, dynamicUICanvas);
        }
    }


    //UI 생성 및 파괴
    private void InstantiateAndDeactivateUI(string uiName, Canvas canvas)
    {
        if (!uiInstances.ContainsKey(uiName))
        {
            GameObject uiPrefab = GetUIPrefab(uiName);

            if (uiPrefab != null)
            {
                GameObject uiInstance = Instantiate(uiPrefab, canvas.transform, false);
                uiInstances[uiName] = uiInstance;
                uiInstance.SetActive(false);
            }
        }
    }


    //UI 활성화 및 비활성화
    public void ToggleUI(string uiName)
    {
        EnsureUIIsInitialized(uiName);

        if (uiInstances.ContainsKey(uiName) && uiInstances[uiName] != null)
        {
            if (uiInstances[uiName].activeSelf)
            {
                DeactivateCurrentUI();
            }
            else
            {
                // 기존 UI 비활성화
                /*if (activeUIStack.Count > 0)
                {
                    string lastActiveUI = activeUIStack.Pop();
                    if (uiInstances.ContainsKey(lastActiveUI) && uiInstances[lastActiveUI] != null)
                    {
                        uiInstances[lastActiveUI].SetActive(false);
                    }
                }
                */
                // 새 UI 활성화
                uiInstances[uiName].SetActive(true);
                activeUIStack.Push(uiName);
            }
        }
        else
        {
            Debug.LogWarning($"UI instance for {uiName} not found or has been destroyed.");
        }
    }


    //가장 최근에 활성화된 UI 비활성화
    public void DeactivateCurrentUI()
    {
        if (activeUIStack.Count > 0)
        {
            string lastActiveUI = activeUIStack.Pop();
            if (uiInstances.ContainsKey(lastActiveUI) && uiInstances[lastActiveUI] != null)
            {
                uiInstances[lastActiveUI].SetActive(false);
            }

            if (activeUIStack.Count > 0)
            {
                string previousUI = activeUIStack.Peek();
                if (uiInstances.ContainsKey(previousUI) && uiInstances[previousUI] != null)
                {
                    uiInstances[previousUI].SetActive(true);
                }
            }
        }
    }


    //UI 비활성화
    public void DeactivatedUI(string uiName)
    {
        if (uiInstances.ContainsKey(uiName) && uiInstances[uiName] != null)
        {
            uiInstances[uiName].SetActive(false);
        }
    }


    //모든 UI 비활성화
    private void DeactivateAllUI()
    {
        foreach (var key in uiInstances.Keys.ToList())
        {
            if (uiInstances[key] != null)
            {
                uiInstances[key].SetActive(false);
            }
            else
            {
                Debug.LogWarning($"UI instance for {key} has been destroyed and is being removed from the dictionary.");
                uiInstances.Remove(key);
            }
        }

        currentActiveUI = null;
    }


    //필요한 UI프리팹 가져오기
    private GameObject GetUIPrefab(string uiName)
    {
        return uiName switch
        {
            "Inventory" => inventoryUIPrefab,
            "Setting" => settingUIPrefab,
            "Map" => mapUIPrefab,
            "Load" => loadUIPrefab,
            "SaveData" => saveDataUIPrefab,
            "SaveDataPopup" => saveDataPopupPrefab,
            "Bed" => bedInteractionUIPrefab,
            "Diary" => diaryUIPrefab,
            "Group" => groupUIPrefab,
            "QuestionGroup" => questionGroupUIPrefab,
            _ => null,
        };
    }


    //현재 활성화된 UI 존재 여부 판단
    public bool IsUIActive()
    {
        return activeUIStack.Count > 0;
    }


    //원하는 UI 오브젝트 반환하는 함수
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


    //UI 활성화
    public void ActiveUI(string uiName)
    {
        EnsureUIIsInitialized(uiName);

        if (uiInstances.ContainsKey(uiName) && uiInstances[uiName] != null)
        {
            uiInstances[uiName].SetActive(true);
            activeUIStack.Push(uiName);
        }
    }


    // Canvas, UI 초기화 여부 확인 및 초기화
    private void EnsureUIIsInitialized(string uiName)
    {
        if (staticUICanvas == null || dynamicUICanvas == null)
        {
            staticUICanvas = GameObject.Find("StaticUICanvas")?.GetComponent<Canvas>();
            dynamicUICanvas = GameObject.Find("DynamicUICanvas")?.GetComponent<Canvas>();

            if (staticUICanvas == null || dynamicUICanvas == null)
            {
                Debug.LogError("StaticUICanvas 또는 DynamicUICanvas를 찾을 수 없습니다.");
                return;
            }
        }

        if (!uiInstances.ContainsKey(uiName) || uiInstances[uiName] == null)
        {
            //StaticUI에 붙는 UI
            if (uiName == "Inventory" || uiName == "Setting" || uiName == "SaveData")
            {
                InstantiateAndDeactivateUI(uiName, staticUICanvas);
            }
            else
            {
                InstantiateAndDeactivateUI(uiName, dynamicUICanvas);
            }
        }
    }


    //데이터 초기화
    private void ClearUIInstances()
    {
        foreach (var uiInstance in uiInstances.Values)
        {
            if (uiInstance != null)
            {
                Destroy(uiInstance);
            }
        }

        uiInstances.Clear();
        activeUIStack.Clear();
    }


    //씬매니저에서 이벤트 제거
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //UI 생성
    public void CreateUIComponent(GameObject obj, Vector2 pos, GameObject parentObj)
    {
        GameObject placedObj = Instantiate(obj);
        placedObj.transform.SetParent(parentObj.transform, false);

        RectTransform rectTransform = placedObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos;
    }

    public void CreateUIComponentInParent(GameObject obj, Vector2 pos)
    {
        GameObject placedObj = Instantiate(obj);
        GameObject staticUICanvas = GameObject.Find("StaticUICanvas");
        placedObj.transform.SetParent(staticUICanvas.transform, false);

        RectTransform rectTransform = placedObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos;

    }

    public void CreateUIComponent(GameObject obj, Vector2 pos, bool isActivated, GameObject parentObj)
    {
        GameObject placedObj = Instantiate(obj);
        placedObj.transform.SetParent(parentObj.transform, false);

        RectTransform rectTransform = placedObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = pos;
        placedObj.SetActive(isActivated);
    }

    public GameObject CreateUIComponentWithScale(GameObject obj, GameObject parentObj, Vector2 scaleData)
    {
        GameObject placedObj = Instantiate(obj);
        placedObj.transform.SetParent(parentObj.transform, false);

        RectTransform rectTransform = placedObj.GetComponent<RectTransform>();
        rectTransform.localScale = scaleData;
        placedObj.SetActive(false);

        return placedObj;
    }
}
