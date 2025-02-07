using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Ch2TalkManager : MonoBehaviour
{
    public TextAsset jsonFile; // JSON 파일을 직접 연결
    public TMP_Text narrationText; // 나레이션 대사를 표시할 UI Text 컴포넌트
    public TMP_Text dialogueText; // 다이얼로그 대사를 표시할 UI Text 컴포넌트
    private List<Dialogue> dialogues; // 대사 리스트
    public int currentDialogueIndex = 0; // 현재 대사 인덱스
    public GameObject narration;
    public GameObject dialogue;
    public bool playerMoved = false;
    public bool isInputDisabled = false; // 입력 차단 여부
    public bool DoNotDisplayDialogue = false; //다이얼로그 출력 여부
    private int overrideDialogueIndex = -1; // 특정 대사 인덱스를 강제로 출력

    public GameObject choiceUI; // 선택지 UI 패널
    public Button choiceButton1; // 첫 번째 선택 버튼 
    public Button choiceButton2; // 두 번째 선택 버튼 
    public TMP_Text choiceText1; // 첫 번째 선택지 텍스트
    public TMP_Text choiceText2; // 두 번째 선택지 텍스트

    public string currentMusic = ""; // 현재 재생 중인 음악의 이름을 저장

    public GameObject player; // 플레이어 캐릭터
    public GameObject map; // 맵

    public GameObject currentNPC; // 현재 대화할 NPC
    public GameObject Npc_Rayviyak; //NPC 레이비야크
    public GameObject Npc_Violet; //NPC 바이올렛
    public GameObject Npc_Rusk; //NPC 러스크
    public GameObject Npc_MrHam; //NPC 러스크

    public GameObject imageObj; // 초상화 이미지
    public GameObject nameObj; // 이름
    public GameObject bigImageObj; // 큰 이미지
    public GameObject playerImageObj; // 플레이어 이미지
    private Dictionary<string, Sprite> characterImages; // 캐릭터 이미지 딕셔너리
    private Dictionary<string, Sprite> characterBigImages; // 캐릭터 큰 이미지 딕셔너리


    public GameObject backGround; //검은 배경
    public GameObject cafe; // 카페 화면
    public GameObject cafe2; //카페 3인칭 화면(낮)
    public GameObject cafe3; //카페 3인칭 화면(밤)
    public GameObject trainRoom; // 객실 화면
    public GameObject trainRoomHallway; // 객실 복도 화면
    public GameObject bakery; //빵집 화면 
    public GameObject medicalRoom; // 의무실 화면
    public GameObject garden; //정원 화면

    public bool isWaitingForPlayer = false; // 플레이어가 특정 위치에 도달할 때까지 기다리는 상태인지 여부
    public bool isWaitingForNPC = false; // NPC 기다리고 있는지 여부

    public Ch2MapManager mapManager; // 맵 매니저 참조

    public Ch0DialogueBar dialogueBar; // 대화창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar narrationBar; // 나레이션창 스크립트 (타이핑 효과 호출을 위해)

    void Awake()
    {
        //Instance = this;
        //ch1ProDialogue = new List<Ch1ProDialogue>();
        //LoadDialogueFromCSV();
        //InitializeCharacterImages();
        //mapManager = map.GetComponent<Ch1MapManager>();
        //playerController = player.GetComponent<PlayerController>(); // 플레이어 컨트롤러 참조 설정
        //player.SetActive(false);
    }

    void Start()
    {
        //player.SetActive(false);

        dialogueBar = dialogue.GetComponentInChildren<Ch0DialogueBar>();
        narrationBar = narration.GetComponentInChildren<Ch0DialogueBar>();

        InitializeCharacterImages(); // 캐릭터 이미지 초기화
        LoadDialogueData(); // 대사 데이터 로드

        if (!isWaitingForNPC )
        {
            DisplayCurrentDialogue();
        }

        // 선택 버튼을 누르면 해당 대사로 이동
        //choiceButton1.onClick.AddListener(() => SelectChoice(342)); // case 340 선택 → case 342
        //choiceButton2.onClick.AddListener(() => SelectChoice(359)); // case 341 선택 → case 359

        // 기본적으로 선택지 UI 비활성화
        choiceUI.SetActive(false);
    }

    void Update()
    {

        // 입력이 비활성화된 경우 스페이스바와 클릭을 무시
        if (isInputDisabled) return;



        // 특정 대화에서 플레이어 이동 대기를 활성화
        if (isWaitingForPlayer && mapManager != null)
        {
            // 플레이어가 특정 지점에 도달했는지 확인
            if (mapManager.currentState == MapState.Cafe && mapManager.isInCafeBarZone)
            {
                Debug.Log("플레이어가 카페에 도착했습니다.");
                isWaitingForPlayer = false;
                DisableMap(); // 맵 비활성화
                player.SetActive(false);
                SetScene(cafe, true); // 카페 화면 활성화

                currentDialogueIndex++;
                DisplayCurrentDialogue(); // 대사 진행

                // 카페 도달 후 대기 상태 해제
                isWaitingForPlayer = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) //대화 진행(sapce 혹은 마우스 클릭)
        {
            NextDialogue();
        }
    }

    public void StartDialogueWithNPC()
    {
        if (isWaitingForNPC && currentNPC != null)
        {
            Debug.Log($"[대화 시작] {currentNPC.name}와 대화 진행");

            isWaitingForNPC = false; // 대화 상태 해제
            DisplayCurrentDialogue(); // 대사 출력
        }
        isInputDisabled = false;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) //대화 진행(sapce 혹은 마우스 클릭)
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
    //현재 대사를 UI에 표시
    public void DisplayCurrentDialogue() 
    {
        if (dialogues != null && currentDialogueIndex < dialogues.Count)
        {
            var currentDialogue = dialogues[currentDialogueIndex];

            if (string.IsNullOrEmpty(currentDialogue.대사))
            {
                Debug.LogWarning($"❌ 대사가 null이거나 비어 있음. (index: {currentDialogueIndex})");
                return; // 바로 함수 종료
            }
            // 대사 표시
            narrationText.text = currentDialogue.대사;
            dialogueText.text = currentDialogue.대사;

            if (currentDialogueIndex == 14 || currentDialogueIndex == 74 || currentDialogueIndex == 123 || currentDialogueIndex == 206 || currentDialogueIndex == 245) // 특정 대화 인덱스
            {
                isWaitingForPlayer = true; // 플레이어 이동 대기 상태 활성화
                EnableMap(); // 맵 활성화
                player.SetActive(true); // 플레이어 활성화
                Debug.Log("플레이어가 카페로 이동할 수 있습니다.");
            }

            // 이름 설정
            nameObj.GetComponent<TMP_Text>().text = currentDialogue.인물;

            // ???를 쿠라야로 처리
            string characterKey = currentDialogue.인물 == "???" ? "쿠라야" : currentDialogue.인물;



            // 다이얼로그가 활성화될 조건
            if (characterKey == "솔" || characterKey == "솔 " || characterKey == "쿠라야" || characterKey == "러스크" || characterKey == "파이아" ||characterKey == "바이올렛" || characterKey == "레이비야크" || characterKey == "Mr.Ham" || characterKey == "나루")
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
                dialogueBar.SetDialogue(currentDialogue.인물, currentDialogue.대사); // 타이핑 효과 적용

            }
            else
            {
                // 나레이션 활성화
                SetScene(narration, true);
                SetScene(dialogue, false); // 다이얼로그 비활성화
                narrationBar.SetDialogue("나레이션", currentDialogue.대사); // 타이핑 효과 적용

            }

            // 화면 변경
            UpdateSceneBasedOnDialogueIndex(currentDialogueIndex);

            // 
            HandleDialogueProgression(currentDialogueIndex);
        }
       

    }
    public void EnableMap()
    {
        if (map != null)
        {
            map.SetActive(true);
            player.SetActive(true); // 플레이어 활성화
            Debug.Log("맵이 활성화되었습니다.");
        }
    }
    public void DisableMap()
    {
        if (map != null)
        {
            map.SetActive(false);
            player.SetActive(false); // 플레이어 비활성화
            Debug.Log("맵이 비활성화되었습니다.");
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
        else if (character == "솔" || character =="???" || character == "러스크" || character == "쿠라야" || character == "파이아" || character == "바이올렛" || character == "레이비야크" || character == "Mr.Ham" || character == "나루")
        {
            // '솔', '???', '러스크', '쿠라야', '파이아'일 경우 대화창 활성화
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

    //다음 대사로 이동
    public void NextDialogue()
    {

        if (dialogueBar.IsTyping()) // 만약 타이핑 중이면
        {
            dialogueBar.CompleteTypingEffect(); // 타이핑 즉시 완료
            return;
        }

        if (narrationBar.IsTyping()) // 나레이션 타이핑 중이면
        {
            narrationBar.CompleteTypingEffect();
            return;
        }
        
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;
            if (!DoNotDisplayDialogue) DisplayCurrentDialogue();
        }
        else
        {
        }
    }
    void ShowChoice(string option1, string option2, int nextIndex1, int nextIndex2)
    {
        isInputDisabled = true; // 선택할 때까지 입력 차단
        choiceUI.SetActive(true); // 선택지 UI 활성화

        // 선택지 텍스트 설정
        choiceText1.text = option1;
        choiceText2.text = option2;

        //기존 텍스트 비활성화
        dialogueText.gameObject.SetActive(false);

        // 버튼 이벤트 설정
        choiceButton1.onClick.RemoveAllListeners();
        choiceButton2.onClick.RemoveAllListeners();

        choiceButton1.onClick.AddListener(() => SelectChoice(nextIndex1));
        choiceButton2.onClick.AddListener(() => SelectChoice(nextIndex2));
    }
    void SelectChoice(int nextDialogueIndex)
    {
        choiceUI.SetActive(false); // 선택지 UI 숨기기
        isInputDisabled = false; // 입력 다시 활성화
        currentDialogueIndex = nextDialogueIndex; // 선택한 대사로 이동

        //  나레이션 텍스트 다시 활성화
        dialogueText.gameObject.SetActive(true);

        DisplayCurrentDialogue(); // 다음 대사 출력
    }

    // 화면 변경 함수
    void UpdateSceneBasedOnDialogueIndex(int index)
    {
        // 인덱스에 따라 화면을 설정
        switch (index)
        {
            case 0: 
                SetPlayerActive(false); // 플레이어 비활성화
                SetScene(backGround, true); // 검은 화면을 활성화
                break;
            case 9: 
                SetScene(backGround, true); // 검은 화면을 활성화
                break;
            case 11:
                DeactivateAllScenes();
                //SetPlayerActive(true); // 플레이어 활성화
                SetScene(trainRoom, true); //객실 화면 활성화
                break;
            case 14:
                DeactivateAllScenes();
                SetScene(trainRoom, false); //객실 화면 비활성화
                player.transform.position = new Vector2(0, 0); // 플레이어 위치 이동
                break;
            case 15:
                DeactivateAllScenes();
                SetScene(cafe, true); //카페 화면 활성화
                break;
            case 24:
                DeactivateAllScenes();
                SetScene(cafe2, true);
                break;
            case 26:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 27:
                DeactivateAllScenes();
                SetScene(dialogue, false); // 대화창 비활성화
                SetScene(narration, false); // 나레이션 비활성화
                SetScene(backGround, true); // 검은 화면을 활성화
                break;
            case 46:
                SetScene(dialogue, false); // 대화창 비활성화
                SetScene(narration, false); // 나레이션 비활성화
                break;
            case 56:
                DeactivateAllScenes();
                SetScene(cafe3, true);
                player.GetComponent<PlayerController>().enabled = false; // 플레이어 이동 비활성화
                //player.GetComponent<PlayerController>().enabled = true; // 다시 이동 가능하게 설정
                break;
            case 59:
                //레이비야크와 상호작용
                InteractWithNPC(Npc_Rayviyak, new Vector2(-15, 0), map);
                break;
            case 62:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                break;
            case 63:
                //바이올렛과 상호작용
                InteractWithNPC(Npc_Violet, new Vector2(0, 0), cafe3, map);
                // 레이비야크 비활성화
                Npc_Rayviyak.SetActive(false);
                break;
            case 65:
                ResetNpcInteraction();
                break;
            case 66:
                //러스크와 상호작용
                InteractWithNPC(Npc_Rusk, new Vector2(-1, 0), bakery, map);
                // 바이올렛 비활성화
                Npc_Violet.SetActive(false);
                break;
            case 69:
                ResetNpcInteraction();
                break;
            case 70:
                DeactivateAllScenes();
                SetScene(medicalRoom, true);
                Npc_Rusk.SetActive(false); // 러스크 비활성화
                SetScene(dialogue, false); // 대화창 비활성화
                SetScene(narration, false); // 나레이션 비활성화

                break;
            case 71:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 74:
                SetScene(trainRoom, false); //객실 화면 비활성화
                break;
            case 75:
                DeactivateAllScenes();
                SetScene(cafe, true);
                player.transform.position = new Vector2(0, 0); // 플레이어 위치 이동
                break;
            case 102:
                //레이비야크와 상호작용
                InteractWithNPC(Npc_Rayviyak, new Vector2(-15, 0), map);
                break;
            case 103:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                break;
            case 104:
                //바이올렛과 상호작용
                InteractWithNPC(Npc_Violet, new Vector2(0, 0), cafe3, map);
                // 레이비야크 비활성화
                Npc_Rayviyak.SetActive(false);
                break;
            case 106:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                break;
            case 107:
                //러스크와 상호작용
                InteractWithNPC(Npc_Rusk, new Vector2(-1, 0), bakery, map);
                // 바이올렛 비활성화
                Npc_Violet.SetActive(false);
                break;
            case 110:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                break;
            case 111:
                //mrHam과 상호작용
                InteractWithNPC(Npc_MrHam, new Vector2(0, 0), medicalRoom, map);
                // 러스크 비활성화
                Npc_Rusk.SetActive(false);
                break;
            case 117:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                // MrHam 비활성화
                Npc_MrHam.SetActive(false);
                break;
            case 118:
                DeactivateAllScenes();
                SetScene(backGround, true);
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                break;
            case 119:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 124:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 133:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 134:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 136:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 140:
                DeactivateAllScenes();
                SetScene(bakery, true);
                player.transform.position = new Vector2(0, 0); // 플레이어 위치 이동
                break;
            case 189:
                //레이비야크와 상호작용
                InteractWithNPC(Npc_Rayviyak, new Vector2(-15, 0), map);
                break;
            case 190:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                break;
            case 191:
                //바이올렛과 상호작용
                InteractWithNPC(Npc_Violet, new Vector2(0, 0), cafe3, map);
                // 레이비야크 비활성화
                Npc_Rayviyak.SetActive(false);
                break;
            case 192:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                break;
            case 195:
                //러스크와 상호작용
                InteractWithNPC(Npc_Rusk, new Vector2(-1, 0), bakery, map);
                // 바이올렛 비활성화
                Npc_Violet.SetActive(false);
                break;
            case 196:
                //mrHam과 상호작용
                InteractWithNPC(Npc_MrHam, new Vector2(0, 0), medicalRoom, map);
                // 러스크 비활성화
                Npc_Rusk.SetActive(false);
                break;
            case 202:
                ResetNpcInteraction(); //NPC 상호작용 끝내기
                // MrHam 비활성화
                Npc_MrHam.SetActive(false);
                break;
            case 203:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 204:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 207:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 223: //연료실
                //DeactivateAllScenes();
                //SetScene(medicalRoom, true);
                break;
            case 241:
                DeactivateAllScenes();
                SetScene(trainRoomHallway, true);
                break;
            case 243:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 245:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 246:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 253:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 292:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 297:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 340:
                // 340과 341의 대사 함께 출력
                //narrationText.gameObject.SetActive(false);
                ShowChoice("네, 도와주세요.", "...아뇨, 제 힘으로 해결해 볼게요.",342,359); // 선택지 UI 표시
                break;
            case 367:
                DeactivateAllScenes();
                SetScene(garden, true);
                break;
            case 370:
                DeactivateAllScenes();
                SetScene(cafe3, true);
                break;
            case 374:
                DeactivateAllScenes();
                SetScene(bakery, true);
                break;
            case 377:
                DeactivateAllScenes();
                SetScene(medicalRoom, true);
                break;
            case 380:
                DeactivateAllScenes();
                SetScene(backGround, true);
                break;
            case 381:
                DeactivateAllScenes();
                SetScene(trainRoom, true);
                break;
            case 382:
                DeactivateAllScenes();
                SetScene(cafe, true);
                break;
            case 408:
                ShowChoice("기차에 어떻게 타게 된 건지 자세히 묻는다. ", "부모님에 대해서 자세히 묻는다. ", 410, 605);
                break ;
            case 713:
                ShowChoice("네, 알려드릴게요.", "...그럴 순 없어요.", 715, 717);
                break;
            case 716:
                break;
            default:
                break; //아무 것도 활성화하지 않음
        }
    
    }
    //NPC 상호작용
    void InteractWithNPC(GameObject npc, Vector2 playerPosition, GameObject sceneToActivate, GameObject sceneToDeactivate = null)
    {
        isInputDisabled = true; // 스페이스바, 클릭 입력 차단
        isWaitingForNPC = true; // 플레이어가 직접 다가가야 대화 가능
        currentNPC = npc; // 현재 대화할 NPC 설정
        npc.SetActive(true); // NPC 활성화

        if (!playerMoved)
        {
            player.transform.position = playerPosition;
            playerMoved = true; // 위치를 한 번만 초기화하도록 설정
        }

        DeactivateAllScenes(); // 다른 씬 비활성화
        SetScene(sceneToActivate, true); // 활성화할 씬 설정

        if (sceneToDeactivate != null) // 비활성화할 씬이 지정되었을 경우
        {
            SetScene(sceneToDeactivate, false);
        }
    }
    //NPC 상호작용 끝내기
    void ResetNpcInteraction() {
        if (player != null && player.GetComponent<PlayerController>() != null)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
        if (player != null && player.GetComponent<Rigidbody2D>() != null)
        {
            player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        currentNPC = null; // 대화 종료 후 NPC 초기화
        isWaitingForNPC = false; // 다시 상호작용 가능
        playerMoved = false;
    }

    private void HandleDialogueProgression(int index)
    {
        if (index == 46) // 예: 특정 인덱스에서 카페 씬으로 전환
        {
            Debug.Log("카페 씬으로 전환");
            SceneTransitionManager.Instance.HandleDialogueTransition("Ch2Scene", "CafeScene", 15, 1);
        }
    }

    // 플레이어 활성화/비활성화 함수
    void SetPlayerActive(bool isActive)
    {
        if (player != null)
        {
            player.SetActive(isActive);
            if (isActive)
            {
                // 플레이어가 활성화되면, 일정 시간 후 PlayerController가 초기화될 수 있도록 기다리기
                StartCoroutine(WaitForPlayerInitialization());
            }
        }
        else
        {
            Debug.LogError("플레이어 객체가 null입니다.");
        }
    }
    IEnumerator WaitForPlayerInitialization()
    {
        // 플레이어가 활성화된 후 잠시 대기
        yield return new WaitForSeconds(0.5f); // 잠시 대기 시간 설정

        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            // PlayerController가 초기화된 후에 처리할 작업
            Debug.Log("PlayerController 초기화 완료.");
        }
        else
        {
            Debug.LogError("PlayerController 초기화 실패.");
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

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"[디버그] {other.gameObject.name}과 충돌 감지됨! 현재 NPC: {currentNPC?.name}");

        if (isWaitingForNPC && other.gameObject == currentNPC)
        {
            Debug.Log($"[디버그] 플레이어가 {currentNPC.name}과 접촉 → 대화 시작");
        }
    }

    // 화면을 활성화하거나 비활성화
    public void SetScene(GameObject scene, bool isActive)
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
        SetScene(cafe3, false);
        SetScene(bakery, false);
        SetScene(garden, false);
        SetScene(medicalRoom, false);
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
            ["Naru"] = Resources.Load<Sprite>("NpcImage/Naru"),

            // 솔 표정 이미지
            ["솔_일반"] = Resources.Load<Sprite>("PlayerImage/Sol"),
            ["솔_nan"] = Resources.Load<Sprite>("PlayerImage/Sol"),
            ["솔 _nan"] = Resources.Load<Sprite>("PlayerImage/Sol"),
            ["솔_놀람"] = Resources.Load<Sprite>("PlayerImage/놀람"),
            ["솔 _놀람"] = Resources.Load<Sprite>("PlayerImage/놀람"),
            ["솔_슬픔"] = Resources.Load<Sprite>("PlayerImage/눈물"),
            ["솔 _슬픔"] = Resources.Load<Sprite>("PlayerImage/눈물"),
            ["솔_당황"] = Resources.Load<Sprite>("PlayerImage/당황"),
            ["솔 _당황"] = Resources.Load<Sprite>("PlayerImage/당황"),
            ["솔_웃음"] = Resources.Load<Sprite>("PlayerImage/웃음"),
            ["솔 _웃음"] = Resources.Load<Sprite>("PlayerImage/웃음"),
            ["솔_화남"] = Resources.Load<Sprite>("PlayerImage/화남"),
            ["솔_찡그림"] = Resources.Load<Sprite>("PlayerImage/찡그림"),

            // 레이비야크 표정 이미지
            ["레이비야크_일반"] = Resources.Load<Sprite>("NpcImage/Leviac"),
            ["레이비야크_nan"] = Resources.Load<Sprite>("NpcImage/Leviac"),
            ["레이비야크_웃음"] = Resources.Load<Sprite>("NpcImage/Leviac_웃음"),
            ["레이비야크_당황"] = Resources.Load<Sprite>("NpcImage/Leviac_당황"),
            ["레이비야크_화남"] = Resources.Load<Sprite>("NpcImage/Leviac_화남"),

            // 쿠라야 표정 이미지
            ["쿠라야_일반"] = Resources.Load<Sprite>("NpcImage/Kuraya"),
            ["쿠라야_nan"] = Resources.Load<Sprite>("NpcImage/Kuraya"),
            ["쿠라야_웃음"] = Resources.Load<Sprite>("NpcImage/Kuraya_웃음"),
            ["쿠라야_화남"] = Resources.Load<Sprite>("NpcImage/Kuraya_화남"),
            ["쿠라야_슬픔"] = Resources.Load<Sprite>("NpcImage/Kuraya_슬픔"),

            // 바이올렛 표정 이미지
            ["바이올렛_일반"] = Resources.Load<Sprite>("NpcImage/Violet"),
            ["바이올렛_웃음"] = Resources.Load<Sprite>("NpcImage/Violet_웃음"),
            ["바이올렛_윙크"] = Resources.Load<Sprite>("NpcImage/Violet_윙크"),
            ["바이올렛_당황"] = Resources.Load<Sprite>("NpcImage/Violet_당황"),
            ["바이올렛_슬픔"] = Resources.Load<Sprite>("NpcImage/Violet_슬픔"),
            ["바이올렛_놀람"] = Resources.Load<Sprite>("NpcImage/Violet_놀람"),

            // 러스크 표정 이미지
            ["러스크_일반"] = Resources.Load<Sprite>("NpcImage/Rusk"),
            ["러스크_nan"] = Resources.Load<Sprite>("NpcImage/Rusk"),
            ["러스크_웃음"] = Resources.Load<Sprite>("NpcImage/Rusk_웃음"),
            ["러스크_화남"] = Resources.Load<Sprite>("NpcImage/Rusk_화남"),
            ["러스크_당황"] = Resources.Load<Sprite>("NpcImage/Rusk_당황"),

            // Mr. Ham 표정 이미지
            ["Mr.Ham_일반"] = Resources.Load<Sprite>("NpcImage/MrHam"),
            ["Mr.Ham_nan"] = Resources.Load<Sprite>("NpcImage/MrHam"),
            ["Mr.Ham_웃음"] = Resources.Load<Sprite>("NpcImage/MrHam_웃음"),
            ["Mr.Ham_화남"] = Resources.Load<Sprite>("NpcImage/MrHam_화남"),
            ["Mr.Ham_아쉬움"] = Resources.Load<Sprite>("NpcImage/MrHam_아쉬움"),
            ["Mr.Ham_놀람"] = Resources.Load<Sprite>("NpcImage/MrHam_놀람"),
            ["Mr.Ham_당황"] = Resources.Load<Sprite>("NpcImage/MrHam_당황"),

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
            ["파이아_nan"] = Resources.Load<Sprite>("NpcImage/Fire"),
            ["파이아_웃음"] = Resources.Load<Sprite>("NpcImage/Fire_웃음"),

            //나루 표정 이미지
            ["나루_일반"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["나루_nan"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["나루_웃음"] = Resources.Load<Sprite>("NpcImage/Naru_웃음"),
            ["나루_놀람"] = Resources.Load<Sprite>("NpcImage/Naru_놀람"),

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