using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : MonoBehaviour
{
    public static SceneManagerEx Instance { get; private set; }


    [Header("Manager Prefabs")]
    public List<GameObject> managersToLoad;
    private Dictionary<string, GameObject> loadedManagers = new Dictionary<string, GameObject>(); 

    public float fadeDuration = 1f;
    public float minLoadingTime = 1f;
    private CanvasGroup fadeCanvasGroup;

    //로딩시간
    float loadingTime;

    // 현재 로딩 중인지를 나타내는 플래그
    private bool isLoading = false;
    private Queue<string> sceneQueue = new Queue<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void InitializeManagers()
    {
        foreach (var managerPrefab in managersToLoad)
        {
            GameObject manager = Instantiate(managerPrefab);
            loadedManagers[managerPrefab.name] = manager;
            DontDestroyOnLoad(manager);
        }
    }

    /*
    private void OnSceneLoaded(string sceneName)
    {
        Debug.Log($"Scene {sceneName} loaded successfully");
    }
    */

    //씬 로드 요청 큐에 추가 
    public void SceanLoadQueue(string sceneName)
    {
        sceneQueue.Enqueue(sceneName);
        if (!isLoading)
        {
            StartCoroutine(ProcessLoadQueue());
        }
    }

    //현재 큐에 들어온 씬 처리
    private IEnumerator ProcessLoadQueue()
    {
        isLoading = true;

        while (sceneQueue.Count > 0)
        {
            string sceneName = sceneQueue.Dequeue();
            yield return StartCoroutine(LoadSceneWithLoadingScene(sceneName));
        }

        isLoading = false;
    }

    private IEnumerator LoadSceneWithLoadingScene(string sceneName)
    {
        SoundManager.Instance.StopAllSounds();

        yield return StartCoroutine(FadeIn());

        //로딩 씬 비동기 로드
        AsyncOperation loadLoadingScene = SceneManager.LoadSceneAsync("LoadingScene");
        yield return new WaitUntil(() => loadLoadingScene.isDone);

        FindFadeCanvasGroup();
        
        float loadingStartTime = Time.time;

        //로딩 씬에서 다음 씬으로 로드
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        asyncOperation.allowSceneActivation = false;
        LoadingSceneController loadingSceneController = FindObjectOfType<LoadingSceneController>();

        if(loadingSceneController == null)
        {
            Debug.LogError("LoadingScreenController not found in LoadingScene.");
            yield break;
        }

        while (!asyncOperation.isDone)
        {
            loadingTime = Time.time - loadingStartTime;

            //float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            float progress = (loadingTime) / 1.5f;
            loadingSceneController.UpdateProgress(progress);


            if (loadingTime > 1.5)//(asyncOperation.progress > 10)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        //yield return new WaitUntil(() => asyncOperation.isDone);


        float loadingElapsedTime = Time.time - loadingStartTime;
        if (loadingElapsedTime < minLoadingTime)
        {
            yield return StartCoroutine(FakeLoadingTask(minLoadingTime - loadingElapsedTime));
        }

        yield return StartCoroutine(FadeOut());

        //OnSceneLoaded(sceneName);
        Clear();
    }

    private void InitializeSceneObjects()
    {
        //필요한 오브젝트 생성
    }

    private IEnumerator FakeLoadingTask(float remainingTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < remainingTime)
        {
            elapsedTime += Time.deltaTime;
            //작업 추가 가능
            yield return null;
        }
    }

    private void FindFadeCanvasGroup()
    {
        fadeCanvasGroup = FindObjectOfType<CanvasGroup>();

        if (fadeCanvasGroup == null)
        {
            Debug.LogError("FadeCanvasGroup not found in the scene.");
        }
    }

    private IEnumerator FadeIn()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            float elapsedTime = 5f;
            while (elapsedTime < fadeDuration)
            {
                fadeCanvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }
    }

    private IEnumerator FadeOut()
    {
        if (fadeCanvasGroup != null)
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(false);
        }
    }

    public bool IsLoading()
    {
        return isLoading;
    }

    public void Clear()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
