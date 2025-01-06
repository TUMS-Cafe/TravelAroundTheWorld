using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Ch2TalkManager : MonoBehaviour
{
    public TextAsset jsonFile; // JSON 파일을 직접 연결
    public TMP_Text narrationText; // 나레이션 대사를 표시할 UI Text 컴포넌트
    public TMP_Text dialogueText; // 다이얼로그 대사를 표시할 UI Text 컴포넌트
    private List<Dialogue> dialogues; // 대사 리스트
    private int currentDialogueIndex = 0; // 현재 대사 인덱스
    public GameObject narration;
    public GameObject dialogue;

    public GameObject imageObj; // 초상화 이미지
    public GameObject nameObj; // 이름
    public GameObject bigImageObj; // 큰 이미지
    public GameObject playerImageObj; // 플레이어 이미지
    private Dictionary<string, Sprite> characterImages; // 캐릭터 이미지 딕셔너리
    private Dictionary<string, Sprite> characterBigImages; // 캐릭터 큰 이미지 딕셔너리

    public GameObject Npc_Rusk; // 빵집 npc

    public GameObject backGround; //검은 배경
    public GameObject cafe; // 카페 화면
    public GameObject cafe2; //카페 3인칭 화면
    public GameObject trainRoom; // 객실 화면
    public GameObject trainRoomHallway; // 객실 복도 화면
    public GameObject bakery; //빵집 화면 

    public Ch0DialogueBar dialogueBar; // 대화창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar narrationBar; // 나레이션창 스크립트 (타이핑 효과 호출을 위해)

    void Start()
    {
        //currentDialogueIndex = 192; //text
        InitializeCharacterImages(); // 캐릭터 이미지 초기화
        LoadDialogueData(); // 대사 데이터 로드
        DisplayCurrentDialogue(); // 현재 대사 표시
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            NextDialogue();
        }
    }

    void LoadDialogueData()
    {
        if (jsonFile != null)
        {
            DialogueList dialogueList = JsonUtility.FromJson<DialogueList>(jsonFile.text);
            dialogues = dialogueList.dialogues;
            Debug.Log($"대사 데이터 로드 완료. 총 {dialogues.Count}개의 대사가 로드되었습니다.");
        }
        else
        {
            Debug.LogError("JSON 파일이 연결되지 않았습니다.");
        }
    }

    void DisplayCurrentDialogue()
    {
        if (dialogues != null && currentDialogueIndex < dialogues.Count)
        {
            Debug.Log($"현재 대사 인덱스: {currentDialogueIndex}, 대사: {dialogues[currentDialogueIndex].대사}");

            var currentDialogue = dialogues[currentDialogueIndex];

            // 대사 표시
            narrationText.text = currentDialogue.대사;
            dialogueText.text = currentDialogue.대사;

            // 이름 설정
            nameObj.GetComponent<TMP_Text>().text = currentDialogue.인물;

            // ???를 쿠라야로 처리
            string characterKey = currentDialogue.인물 == "???" ? "쿠라야" : currentDialogue.인물;

            // 다이얼로그가 활성화될 조건
            if (characterKey == "솔" || characterKey == "솔 " || characterKey == "쿠라야" || characterKey == "러스크")
            {
                // 표정 키 생성
                string expressionKey = !string.IsNullOrEmpty(currentDialogue.표정)
                    ? characterKey + "_" + currentDialogue.표정
                    : characterKey + "_일반";

                // 표정 이미지 설정
                if (characterImages.ContainsKey(expressionKey))
                {
                    imageObj.GetComponent<Image>().sprite = characterImages[expressionKey];
                    Debug.Log($"{characterKey} 표정 변경: {expressionKey}");
                }
                else
                {
                    Debug.LogWarning($"표정 이미지를 찾을 수 없습니다: {expressionKey}");
                }

                // 다이얼로그 활성화
                SetScene(dialogue, true);
                SetScene(narration, false); // 나레이션 비활성화
            }
            else
            {
                // 나레이션 활성화
                SetScene(narration, true);
                SetScene(dialogue, false); // 다이얼로그 비활성화
            }

            // 화면 변경
            UpdateSceneBasedOnDialogueIndex(currentDialogueIndex);
        }
        else
        {
            narrationText.text = "대사가 끝났습니다.";
        }
    }

    void HandleNarrationVisibility(string character)
    {
        if (character == "나레이션" || character == "열차 안내 음성")
        {
            // 나레이션 활성화
            SetScene(narration, true);
            SetScene(dialogue, false); // 대화창 비활성화
        }
        else if (character == "솔" || character =="???" || character == "러스크" || character == "쿠라야")
        {
            // '솔', '???', '러스크', '쿠라야'일 경우 대화창 활성화
            SetScene(dialogue, true);
            SetScene(narration, false); // 나레이션 비활성화
        }
        else
        {
            // 다른 경우 대화창 비활성화
            SetScene(dialogue, false);
            SetScene(narration, false); // 나레이션 비활성화
        }
    }

    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;
            Debug.Log($"다음 대사: {dialogues[currentDialogueIndex].대사}");
            DisplayCurrentDialogue();
        }
    }

    // 화면 변경 함수
    void UpdateSceneBasedOnDialogueIndex(int index)
    {
        // 인덱스에 따라 화면을 설정
        switch (index)
        {
            case 8: 
                SetScene(backGround, true); // 검은 화면을 활성화
                break;
            case 10:
                DeactivateAllScenes();
                SetScene(trainRoom, true); //객실 화면 활성화
                break;
            case 14:
                DeactivateAllScenes();
                SetScene(cafe, true); //카페 화면 활성화
                break;
            case 23:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 45:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 46:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 49:
                DeactivateAllScenes();
                
                SetScene(cafe, true);
                break;
            case 79:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 80:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 85:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 98:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 99:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 102:
                DeactivateAllScenes();
                SetScene(bakery, true);
                break;
            case 157:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 161:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 172:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 194: //5일차 낮
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 196:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 215:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 217:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 234:
                DeactivateAllScenes();
                SetScene(bakery, true);
                break;
            case 269:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 271:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 293:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 295:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 304:
                DeactivateAllScenes();
                SetScene(bakery, true);
                break;
            case 382: //8일차 낮
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 384: 
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            default:
                break; //아무 것도 활성화하지 않음
        }
    }
    void HandleCharacterImages(string character)
    {
        if (characterImages.ContainsKey(character))
        {
            // 이름 설정
            nameObj.GetComponent<TMP_Text>().text = character;

            // 캐릭터 이미지 설정 (초상화 이미지)
            SetCharacterImage(imageObj, character);

            // 캐릭터 큰 이미지 설정
            if (characterBigImages.ContainsKey(character))
            {
                SetCharacterImage(bigImageObj, characterBigImages[character]);
            }
        }
        else if (character == "???") {
            nameObj.GetComponent<TMP_Text>().text = "???";
        }
        else
        {/*
            // 기본 이미지 설정
            nameObj.GetComponent<TMP_Text>().text = "기본 캐릭터";

            // 기본 이미지로 설정
            SetCharacterImage(imageObj, "Default");
            SetCharacterImage(bigImageObj, "Default"); */
        }
    }

    void SetCharacterImage(GameObject imageObj, string character)
    {
        // 해당 캐릭터의 이미지를 딕셔너리에서 찾아서 적용
        if (characterImages.ContainsKey(character))
        {
            imageObj.GetComponent<Image>().sprite = characterImages[character];
        }
        else
        {
            Debug.LogWarning("이미지를 찾을 수 없습니다: " + character);
        }
    }

    // 큰 이미지 설정 함수 (대화창 밖에서 큰 이미지를 업데이트 할 때)
    void SetCharacterImage(GameObject imageObj, Sprite sprite)
    {
        if (sprite != null)
        {
            imageObj.GetComponent<Image>().sprite = sprite;
        }
        else
        {
            Debug.LogWarning("이미지가 NULL입니다.");
        }
    }

    // 화면을 활성화하거나 비활성화
    void SetScene(GameObject scene, bool isActive)
    {
        if (scene != null)
        {
            scene.SetActive(isActive);
        }
    }
    // 모든 화면 비활성화
    void DeactivateAllScenes()
    {
        SetScene(backGround, false);
        SetScene(cafe, false);
        SetScene(trainRoom, false);
        SetScene(trainRoomHallway, false);
        SetScene(cafe2, false);
        SetScene(bakery, false);
    }

    // 이미지 가져오는 코드
    void InitializeCharacterImages()
    {
        characterImages = new Dictionary<string, Sprite>
        {
            // 기본 캐릭터 이미지
            ["솔"] = Resources.Load<Sprite>("PlayerImage/Sol"),
            ["레이비야크"] = Resources.Load<Sprite>("NpcImage/Leviac"),
            ["바이올렛"] = Resources.Load<Sprite>("NpcImage/Violet"),
            ["러스크"] = Resources.Load<Sprite>("NpcImage/Rusk"),
            ["Mr. Ham"] = Resources.Load<Sprite>("NpcImage/MrHam"),

            // 솔 표정 이미지
            ["솔_일반"] = Resources.Load<Sprite>("PlayerImage/Sol"),
            ["솔_놀람"] = Resources.Load<Sprite>("PlayerImage/놀람"),
            ["솔_슬픔"] = Resources.Load<Sprite>("PlayerImage/눈물"),
            ["솔_당황"] = Resources.Load<Sprite>("PlayerImage/당황"),
            ["솔_웃음"] = Resources.Load<Sprite>("PlayerImage/웃음"),
            ["솔_화남"] = Resources.Load<Sprite>("PlayerImage/화남"),
            ["솔_찡그림"] = Resources.Load<Sprite>("PlayerImage/찡그림"),

            // 레이비야크 표정 이미지
            ["레이비야크_일반"] = Resources.Load<Sprite>("NpcImage/Leviac"),
            ["레이비야크_웃음"] = Resources.Load<Sprite>("NpcImage/Leviac_웃음"),

            // 쿠라야 표정 이미지
            ["쿠라야_일반"] = Resources.Load<Sprite>("NpcImage/Kuraya"),
            ["쿠라야_웃음"] = Resources.Load<Sprite>("NpcImage/Kuraya_웃음"),
            ["쿠라야_화남"] = Resources.Load<Sprite>("NpcImage/Kuraya_화남"),

            // 바이올렛 표정 이미지
            ["바이올렛_일반"] = Resources.Load<Sprite>("NpcImage/Violet"),
            ["바이올렛_웃음"] = Resources.Load<Sprite>("NpcImage/Violet_웃음"),
            ["바이올렛_윙크"] = Resources.Load<Sprite>("NpcImage/Violet_윙크"),

            // 러스크 표정 이미지
            ["러스크_일반"] = Resources.Load<Sprite>("NpcImage/Rusk"),
            ["러스크_웃음"] = Resources.Load<Sprite>("NpcImage/Rusk_웃음"),
            ["러스크_화남"] = Resources.Load<Sprite>("NpcImage/Rusk_화남"),
            ["러스크_당황"] = Resources.Load<Sprite>("NpcImage/Rusk_당황"),

            // Mr. Ham 표정 이미지
            ["Mr. Ham_일반"] = Resources.Load<Sprite>("NpcImage/MrHam"),
            ["Mr. Ham_웃음"] = Resources.Load<Sprite>("NpcImage/MrHam_웃음"),
            ["Mr. Ham_화남"] = Resources.Load<Sprite>("NpcImage/MrHam_화남"),
            ["Mr. Ham_아쉬움"] = Resources.Load<Sprite>("NpcImage/MrHam_아쉬움"),

            // 루카스 표정 이미지
            ["루카스_일반"] = Resources.Load<Sprite>("NpcImage/Lucas"),
            ["루카스_곤란"] = Resources.Load<Sprite>("NpcImage/Lucas_곤란"),
            ["루카스_찡그림"] = Resources.Load<Sprite>("NpcImage/Lucas_찡그림"),

            // 슬로우 표정 이미지
            ["슬로우_일반"] = Resources.Load<Sprite>("NpcImage/Slow"),
            ["슬로우_당황"] = Resources.Load<Sprite>("NpcImage/Slow_당황"),
            ["슬로우_화남"] = Resources.Load<Sprite>("NpcImage/Slow_화남"),

            // 가이 표정 이미지
            ["가이_일반"] = Resources.Load<Sprite>("NpcImage/Gai"),
            ["가이_당황"] = Resources.Load<Sprite>("NpcImage/Gai_당황"),

            // 파이아 표정 이미지
            ["파이아_일반"] = Resources.Load<Sprite>("NpcImage/Fire"),
            ["파이아_웃음"] = Resources.Load<Sprite>("NpcImage/Fire_웃음"),

            // 기본 NPC 이미지
            ["Default"] = Resources.Load<Sprite>("NpcImage/Default")
        };

        characterBigImages = new Dictionary<string, Sprite>
        {
            ["솔"] = Resources.Load<Sprite>("NpcImage/Sol"),
            ["레이비야크"] = Resources.Load<Sprite>("NpcImage/Leviac_full"),
            ["바이올렛"] = Resources.Load<Sprite>("NpcImage/Violet_full"),
            ["러스크"] = Resources.Load<Sprite>("NpcImage/Rusk_full"),
            ["Mr. Ham"] = Resources.Load<Sprite>("NpcImage/MrHam_full"),
            ["루카스"] = Resources.Load<Sprite>("NpcImage/Lucas_big"),
            ["슬로우"] = Resources.Load<Sprite>("NpcImage/Slow_big"),
            ["가이"] = Resources.Load<Sprite>("NpcImage/Gai_big"),
            ["파이아"] = Resources.Load<Sprite>("NpcImage/Fire_full"),
            ["Default"] = Resources.Load<Sprite>("NpcImage/Default")
        };
    }
}