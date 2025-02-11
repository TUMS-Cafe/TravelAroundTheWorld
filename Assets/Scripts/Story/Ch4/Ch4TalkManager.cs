using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime;


public class Ch4TalkManager : MonoBehaviour
{
    public static Ch4TalkManager Instance { get; private set; }

    private List<Ch4ProDialogue> ch4ProDialogue;

    public GameObject narration;
    public GameObject dialogue;

    public GameObject imageObj; // 초상화 이미지
    public GameObject nameObj; // 이름
    public GameObject bigImageObj; // 큰 이미지
    public GameObject playerImageObj; // 플레이어 이미지

    //public GameObject player; // 플레이어 캐릭터
    //public GameObject map; // 맵

    public GameObject jazzBar; // 재즈바 화면
    public GameObject trainRoom; // 객실 화면
    public GameObject trainRoomHallway; // 객실 복도 화면
    public GameObject garden; // 정원 화면
    public GameObject bakery; // 빵집 화면
    public GameObject medicalRoom; // 의무실 화면
    public GameObject trainStation; // 기차역 화면
    public GameObject ending; // 엔딩 화면

    public Ch0DialogueBar dialogueBar; // 대화창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar narrationBar; // 나레이션창 스크립트 (타이핑 효과 호출을 위해)

    private const string narrationSpeaker = "나레이션";

    public int currentDialogueIndex = 0; // 현재 대사 인덱스
    private bool isActivated = false; // TalkManager가 활성화되었는지 여부

    private Dictionary<string, Sprite> characterImages; // 캐릭터 이름과 이미지를 매핑하는 사전
    private Dictionary<string, Sprite> characterBigImages; // 캐릭터 이름과 큰 이미지를 매핑하는 사전
    private Sprite characterSprite;

    public bool isTransition = false;

    public string speakerKey;

    public bool isFadingOut = false;
    public ScreenFader screenFader;

    void Awake()
    {
        Instance = this;
        ch4ProDialogue = new List<Ch4ProDialogue>();
        LoadDialogueFromCSV();
        InitializeCharacterImages();
    }
    void LoadDialogueFromCSV() // CSV 읽어오기
    {
        List<Dictionary<string, object>> data_Dialog = Ch0CSVReader.Read("Travel Around The World - CH4(기차역,평원_엔딩) (1)");

        foreach (var row in data_Dialog)
        {
            string time = row["시간"].ToString();
            string location = row["장소"].ToString();
            string speaker = row["인물"].ToString();
            string line = row["대사"].ToString();
            string screenEffect = row["화면 연출"].ToString();
            string backgroundMusic = row["배경음악"].ToString();
            string expression = row["표정"].ToString();
            string note = row["비고"].ToString();

            ch4ProDialogue.Add(new Ch4ProDialogue(time, location, speaker, line, screenEffect, backgroundMusic, expression, note));

            Debug.Log("LoadDialogueCSV List has Data");
        }
        Debug.Log("daat_dialogue is full ");
    }
    
    void InitializeCharacterImages() //이미지 갖고오기
    {
        characterImages = new Dictionary<string, Sprite>
        {
            // 기본 캐릭터 이미지
            ["솔"] = Resources.Load<Sprite>("PlayerImage/Sol"),
            ["나루"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["루나"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["???"] = Resources.Load<Sprite>("NpcImage/Naru"),

            // 솔 표정 이미지
            ["솔_일반"] = Resources.Load<Sprite>("PlayerImage/Sol"),
            ["솔_놀람"] = Resources.Load<Sprite>("PlayerImage/놀람"),
            ["솔_슬픔"] = Resources.Load<Sprite>("PlayerImage/눈물"),
            ["솔_당황"] = Resources.Load<Sprite>("PlayerImage/당황"),
            ["솔_웃음"] = Resources.Load<Sprite>("PlayerImage/웃음"),
            ["솔_화남"] = Resources.Load<Sprite>("PlayerImage/화남"),

            // 나루 표정 이미지
            ["나루_일반"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["나루_웃음"] = Resources.Load<Sprite>("NpcImage/Naru_웃음"),
            ["나루_놀람"] = Resources.Load<Sprite>("NpcImage/Naru_놀람"),
            ["나루_당황"] = Resources.Load<Sprite>("NpcImage/Naru_당황"),
            // 루나 표정 이미지
            // 나루 표정 이미지
            ["루나_일반"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["루나_웃음"] = Resources.Load<Sprite>("NpcImage/Naru_웃음"),
            ["루나_놀람"] = Resources.Load<Sprite>("NpcImage/Naru_놀람"),
            ["루나_당황"] = Resources.Load<Sprite>("NpcImage/Naru_당황"),

            // 기본 NPC 이미지
            ["Default"] = Resources.Load<Sprite>("NpcImage/Default")
        };

        characterBigImages = new Dictionary<string, Sprite>
        {
            ["솔"] = Resources.Load<Sprite>("NpcImage/Sol"),
            ["나루"] = Resources.Load<Sprite>("NpcImage/Naru_full"),
            ["루나"] = Resources.Load<Sprite>("NpcImage/Naru_full"),
            ["???"] = Resources.Load<Sprite>("NpcImage/Naru_full"),
            ["Default"] = Resources.Load<Sprite>("NpcImage/Default")
        };
    }

    void Start()
    {
        if (currentDialogueIndex == 0)
        {
            ActivateTalk("객실", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            bool anyTyping = false;

            // 순서대로 확인
            if (narration != null && narration.GetComponentInChildren<Ch0DialogueBar>().IsTyping())
            {
                narration.GetComponentInChildren<Ch0DialogueBar>().CompleteTypingEffect();
                anyTyping = true;
            }

            if (dialogue != null && dialogue.GetComponentInChildren<Ch0DialogueBar>().IsTyping())
            {
                dialogue.GetComponentInChildren<Ch0DialogueBar>().CompleteTypingEffect();
                anyTyping = true;
            }
            // 타이핑 중이었으면 아래 코드는 실행하지 않음
            if (!anyTyping)
            {
                currentDialogueIndex++;
                if (currentDialogueIndex >= ch4ProDialogue.Count)
                {
                    DeactivateTalk(); // 대사 리스트를 벗어나면 오브젝트 비활성화
                }
                else
                {
                    PrintCh4ProDialogue(currentDialogueIndex);
                }
            }

        }
    }
    public void PrintCh4ProDialogue(int index)
    {

        Debug.Log($"PrintCh4ProDialogue called with index: {index}");
        if (index >= ch4ProDialogue.Count)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            bigImageObj.SetActive(false); // 대화가 끝날 때 bigImageObj를 비활성화
            return;
        }

        Ch4ProDialogue currentDialogue = ch4ProDialogue[index];

        string expressionKey = !string.IsNullOrEmpty(currentDialogue.expression) ? $"_{currentDialogue.expression}" : "";
        speakerKey = currentDialogue.speaker;

        // 인물과 표정을 포함한 최종 키 생성
        string finalKey = speakerKey + expressionKey;

        if (characterImages.ContainsKey(finalKey))
        {
            characterSprite = characterImages[finalKey];
        }
        else
        {
            // 해당사항 없는 경우 기본 이미지 사용
            characterSprite = characterImages.ContainsKey(speakerKey)
                ? characterImages[speakerKey]
                : characterImages["Default"];
        }

        // Set regular image
        if (imageObj.GetComponent<SpriteRenderer>() != null)
        {
            imageObj.GetComponent<SpriteRenderer>().sprite = characterSprite;
        }
        else if (imageObj.GetComponent<Image>() != null)
        {
            imageObj.GetComponent<Image>().sprite = characterSprite;
        }

        // Set big image (화자가 '솔'이 아닐 때만 활성화)
        if (speakerKey != "솔")
        {
            if (characterBigImages.ContainsKey(speakerKey))
            {
                bigImageObj.GetComponent<Image>().sprite = characterBigImages[speakerKey];
            }
            else
            {
                bigImageObj.GetComponent<Image>().sprite = characterBigImages["Default"];
            }
            bigImageObj.SetActive(true); // 화자가 '솔'이 아닐 때 bigImageObj를 활성화
        }
        else
        {
            bigImageObj.SetActive(false); // 화자가 '솔'일 때 bigImageObj를 비활성화
        }

        if (ch4ProDialogue[currentDialogueIndex].line == "(일러스트)" || ch4ProDialogue[currentDialogueIndex].location == "암전")
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        else if (string.IsNullOrEmpty(currentDialogue.speaker) && string.IsNullOrEmpty(currentDialogue.location))
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            bigImageObj.SetActive(false); // 대화가 없을 때 bigImageObj를 비활성화
        }
        else if (currentDialogue.speaker == narrationSpeaker || string.IsNullOrEmpty(currentDialogue.speaker))
        {
            narration.SetActive(true);
            dialogue.SetActive(false);
            bigImageObj.SetActive(false); // 나레이션에서는 bigImageObj를 비활성화
            narrationBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }
        else
        {
            narration.SetActive(false);
            dialogue.SetActive(true);
            dialogueBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }

        CheckTalk(currentDialogue.location);
    }
    public void CheckTalk(string location)
    {
        trainRoom.SetActive(false);
        trainRoomHallway.SetActive(false);
        garden.SetActive(false);
        bakery.SetActive(false);
        medicalRoom.SetActive(false);
        jazzBar.SetActive(false);
        trainStation.SetActive(false);
        ending.SetActive(false);

        switch (location)
        {
            case "객실복도":
                trainRoomHallway.SetActive(true);
                break;
            case "객실":
                trainRoom.SetActive(true);
                break;

            case "의무실":
                medicalRoom.SetActive(true);
                break;

            case "정원":
                garden.SetActive(true);
                break;

            case "빵집":
                bakery.SetActive(true);
                break;

            case "재즈바":
                jazzBar.SetActive(true);
                break;

            case "기차역":
                trainStation.SetActive(true);
                break;

            case "1엔딩":
                trainStation.SetActive(true);
                break;
            case "2엔딩":
                break;
            case "3엔딩":
                ending.SetActive(true);
                break;
            case "암전":
                break;
            case "검은화면":
                break;

        }

        if (currentDialogueIndex > ch4ProDialogue.Count)
        {
            DeactivateTalk();
        }
    }

    public void ActivateTalk(string locationName, int curDialogueIdx)
    {
        this.gameObject.SetActive(true);
        isActivated = true;

        // locationName에 따라 인덱스 조정하여 특정 대화를 시작할 수 있도록 수정
        currentDialogueIndex = ch4ProDialogue.FindIndex(dialogue => dialogue.location == locationName);

        currentDialogueIndex = curDialogueIdx;

        if (currentDialogueIndex >= 0)
        {
            PrintCh4ProDialogue(currentDialogueIndex);
        }
    }
    public void DeactivateTalk()
    {
        this.gameObject.SetActive(false);
        isActivated = false;
    }
    private IEnumerator FadeOutAndLoadScene(GameObject obj, string sceneName)
    {
        isFadingOut = true; // 페이드아웃 시작
        yield return StartCoroutine(screenFader.FadeOut(obj)); // FadeOut이 완료될 때까지 기다립니다.
        narration.SetActive(false);
        dialogue.SetActive(false);
        DeactivateTalk(); // FadeOut이 완료된 후 대화 비활성화
        isFadingOut = false; // 페이드아웃 종료
        SceneManagerEx.Instance.SceanLoadQueue(sceneName); // 씬 로드
    }
}
