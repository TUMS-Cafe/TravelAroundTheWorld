using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ch0DialogueBar : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameTxt;
    [SerializeField] Ch0TypeEffect talk;

    // 문자열 상수 선언
    private const string narrationSpeaker = "나레이션";

    private string speakerName;

    public void SetDialogue(string nameData, string dialogue)
    {
        if (nameTxt != null)
        {
            if (string.IsNullOrEmpty(nameData) || nameData == narrationSpeaker)
            {
                nameTxt.gameObject.SetActive(false);
            }
            else
            {
                nameTxt.gameObject.SetActive(true);
                nameTxt.text = nameData;
            }
        }
        talk.SetMsg(dialogue);
        if (talk.IsTyping())
        {
            GetAndPlayDialogueSound();
        }
    }

    private void Update()
    {
        //
        
    }

    public void GetAndPlayDialogueSound()
    {
        speakerName = SetSpeakerKey();

        string dialogueSoundName = "";

        switch(speakerName)
        {
            case "솔":
                dialogueSoundName = "sol_text";
                break;
            case "바이올렛":
                dialogueSoundName = "violet_text";
                break;
            case "Mr.Ham":
                dialogueSoundName = "Ham_text";
                break;
            case "러스크":
                dialogueSoundName = "rusk_text";
                break;
            case "레이비야크":
                dialogueSoundName = "ray_text";
                break;
            case "파이아":
                dialogueSoundName = "Fia_text";
                break;
            case "가이":
                dialogueSoundName = "Guy_text";
                break;
            case "슬로우":
                dialogueSoundName = "Slow_text";
                break;
            case "루카스":
                dialogueSoundName = "Lucas_text";
                break;
            default:
                Debug.Log("speakerName is null or Narration. speakerName is Sol");
                dialogueSoundName = "sol_text";
                break;
        }
        Debug.Log($"SoundFile Name is {dialogueSoundName}");
        SoundManager.Instance.PlayDialogueSound(dialogueSoundName);
    }

    //코드 정리 후 수정 예정
    private string SetSpeakerKey()
    {
        string speakerKey = "";

        string sceneName = SceneManagerEx.Instance.GetCurrentSceneName();

        if(sceneName == "Ch0Scene")
        {
            TalkManager talkManager = GameObject.FindObjectOfType<TalkManager>();
            speakerKey = talkManager.speakerKey;
        }

        else  if(sceneName == "ch1Scene")
        {
            Ch1TalkManager talkManager = GameObject.FindObjectOfType<Ch1TalkManager>();
            speakerKey = talkManager.speakerKey;
        }

        else if(sceneName == "CafeTutorialScene")
        {
            CafeTalkManager talkManager = GameObject.FindAnyObjectByType<CafeTalkManager>();
            speakerKey = talkManager.currentDialogue.speaker;
        }

        else if (sceneName == "Ch4Scene")
        {
            Ch4TalkManager talkManager = GameObject.FindObjectOfType<Ch4TalkManager>();
            speakerKey = talkManager.speakerKey;
        }

        Debug.Log($"SpeakerKey : {speakerKey}");

        return speakerKey;
        
    }
    

    public bool IsTyping()
    {
        // 현재 타이핑 중인지 확인
        return talk.IsTyping(); 
    }

    public void CompleteTypingEffect()
    {
        // 타이핑 이펙트를 즉시 완료
        talk.CompleteEffect();

    }
}
