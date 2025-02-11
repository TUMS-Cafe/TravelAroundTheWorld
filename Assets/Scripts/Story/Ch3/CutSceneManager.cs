using System.Collections;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameObject[] cutSceneObjects;  // 컷씬 화면 오브젝트 3개
    //public AudioClip[] cutSceneSounds;  // 각 컷씬마다 나올 효과음
    //public AudioSource audioSource;  // 효과음을 재생할 AudioSource
    public Ch3TalkManager talkManager;

    private void Start()
    {
        // 시작할 때 모든 컷씬 오브젝트 비활성화
        foreach (GameObject obj in cutSceneObjects)
        {
            obj.SetActive(false);
        }
    }

    public void ShowCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        talkManager.isAnimationPlaying = true;

        for (int i = 0; i < cutSceneObjects.Length; i++)
        {
            cutSceneObjects[i].SetActive(true);  // 현재 컷씬 활성화
            /*
            if (cutSceneSounds.Length > i)
            {
                audioSource.PlayOneShot(cutSceneSounds[i]);  // 효과음 재생
            }
            */

            yield return new WaitForSeconds(2f);  // 2초 대기

            cutSceneObjects[i].SetActive(false);  // 현재 컷씬 비활성화
        }

        talkManager.currentDialogueIndex++;  // 다음 대화로 진행
        talkManager.isAnimationPlaying = false;
    }
}