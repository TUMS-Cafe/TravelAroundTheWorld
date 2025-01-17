using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;

public class Ch3TalkManager : MonoBehaviour
{
    public static Ch3TalkManager Instance { get; private set; }
    // 대사들을 저장할 리스트
    private List<Ch3ProDialogue> proDialogue;

    public GameObject narration;
    public GameObject dialogue;

    public GameObject imageObj; // 초상화 이미지 요소
    public GameObject nameObj; // 이름 요소

    public GameObject backGround; //검은 배경

    public GameObject cafe; // 카페 화면
    public GameObject cafe2; // 카페2 화면
    public GameObject garden; // 정원 화면
    public GameObject gardenCafe; // 정원+카페 화면
    public GameObject bakery; // 빵집 화면
    public GameObject medicalRoom; // 의무실 화면
    public GameObject trainRoom; // 객실 화면
    public GameObject trainRoomPenguinFox; // 펭귄 여우 객실 화면
    public GameObject trainRoomHallway; // 객실복도 화면
    public GameObject trainRoomHallwayWolf; // 객실복도(늑대 객실 앞) 화면
    public GameObject balcony; //발코니 화면

    public GameObject Npc_Rayviyak; // 정원 npc
    public GameObject Npc_MrHam; // 병원 npc
    public GameObject Npc_Rusk; // 빵집 npc
    public GameObject Npc_Violet; // 바 npc

    public ScreenFader screenFader; // 페이드인/아웃 효과 스크립트
    private bool isFadingOut = false; // 페이드 아웃 중인지 여부 (페이드 아웃 중에는 입력 무시하기 위해)

    public Ch0DialogueBar dialogueBar; // 대화창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar narrationBar; // 나레이션창 스크립트 (타이핑 효과 호출을 위해)

    // 문자열 상수 선언
    private const string narrationSpeaker = "나레이션";
    private const string locationCafe = "카페";
    private const string locationCafe2 = "카페2";
    private const string locationGarden = "정원";
    private const string locationGardenCafe = "정원+카페";
    private const string locationBakery = "빵집";
    private const string locationMedicalRoom = "의무실";
    private const string locationTrainRoom = "객실";
    private const string locationTrainRoomPenguinFox = "펭귄 여우 객실";
    private const string locationTrainRoomHallway = "객실복도";
    private const string locationTrainRoomHallwayWolf = "객실복도(늑대 객실 앞)";
    private const string locationBalcony = "발코니";
    private const string unknownSpeaker = "??";

    public int currentDialogueIndex = 0; // 현재 대사 인덱스
    private bool isActivated = false; // TalkManager가 활성화되었는지 여부

    public bool isAllNPCActivated = false; //모든 npc와 대화 완료되었는지 여부

    private Dictionary<string, Sprite> characterImages; // 캐릭터 이름과 이미지를 매핑하는 사전
    private Sprite characterSprite;

    public PlayerController playerController;
    public GameObject player; // 플레이어 캐릭터
    public GameObject map; // 맵
    public Ch3MapManager mapManager;

    public string currentMusic = ""; // 현재 재생 중인 음악의 이름을 저장

    public bool isAnimationPlaying = false;
    public bool isWaitingForPlayer = false;
    public bool isTransition = false;

    public string speakerKey;
    public bool bedUsed = false; // 침대를 사용했는지 여부

    void Awake()
    {
        Instance = this;
        proDialogue = new List<Ch3ProDialogue>();
        LoadDialogueFromCSV(); // CSV에서 데이터를 로드하는 함수 호출
        InitializeCharacterImages();
        playerController.StopMove();
        //trainRoom.SetActive(true);
    }

    void Start()
    {
        if (isTransition)
        {
            ActivateTalk(locationCafe, currentDialogueIndex);
        }

        // 플레이어가 로드된 경우
        if (PlayerManager.Instance.GetIsLoaded())
        {
            currentDialogueIndex = PlayerManager.Instance.GetDialogueIdx();
        }

        // currentDialogueIndex가 0일 경우에만 초기화
        if (currentDialogueIndex == 0)
        {
            ActivateTalk(locationTrainRoom, 0);
            player.SetActive(false);
        }
        else
        {
            // 이미 설정된 인덱스가 있는 경우 그 인덱스부터 대화 시작
            ActivateTalk(locationCafe, currentDialogueIndex);
            player.SetActive(false);
        }
    }

    void Update()
    {
        if (isActivated && !isFadingOut && !isWaitingForPlayer && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (isAnimationPlaying)
            {
                return;
            }

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
                if (currentDialogueIndex >= proDialogue.Count)
                {
                    DeactivateTalk(); // 대사 리스트를 벗어나면 오브젝트 비활성화
                }
                else
                {
                    HandleDialogueProgression(currentDialogueIndex);
                }
            }
        }
        // 플레이어가 특정 위치에 도달했는지 확인하는 부분
        if (isWaitingForPlayer && mapManager != null)
        {
            // 카페바에 도착하면 스토리 다시 진행
            if (mapManager.currentState == MapState.Cafe && mapManager.isInCafeBarZone && (currentDialogueIndex == 10))
            {
                isWaitingForPlayer = false;
                player.SetActive(false);
                map.SetActive(false);
                cafe.SetActive(true);
                currentDialogueIndex++;
                PrintProDialogue(currentDialogueIndex);
                Npc_Rayviyak.SetActive(false);
                Npc_MrHam.SetActive(false);
                Npc_Rusk.SetActive(false);
                Npc_Violet.SetActive(false);
            }
            /*
            // 객실에 도착하면 스토리 다시 진행
            else if (mapManager.currentState == MapState.TrainRoom3 && (currentDialogueIndex == 32))
            {
                isWaitingForPlayer = false;
                player.SetActive(false);
                map.SetActive(false);
                trainRoom.SetActive(true);
                currentDialogueIndex++;
                PrintProDialogue(currentDialogueIndex);
            }
            */

            // 카페에서 일해야 되는데 다른 곳으로 가려고 하면 다시 카페로 플레이어 강제 이동
            if (mapManager.currentState != MapState.Cafe && (currentDialogueIndex == 10))
            {
                player.transform.position = new Vector3(0, 0, 0);
                narration.SetActive(true);
                dialogue.SetActive(false);
                narrationBar.SetDialogue("나레이션", "지금은 일할 시간이야.");
            }
        }
    }

    private void HandleDialogueProgression(int index)
    {
        // 1일차 낮 주문 리스트
        if (index == 27) // 낮 주문 1건 파이아
        {
            Debug.Log("낮 주문 1건 파이아");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 28, 1);
        }
        else if (index == 32) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 33, 1);
        }
        else if (index == 48) // 낮 주문 2건 코코,닉시
        {
            Debug.Log("낮 주문 2건 코코,닉시");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 49, 2);
        }
        else if (index == 54) // 룸서비스 랜덤 2건 각각
        {
            Debug.Log("배달 랜덤 룸서비스 주문 2건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 56, 2);
        }
        // 1일차 밤 주문 리스트
        else if (index == 57) // 랜덤 주문 2건 각각
        {
            Debug.Log("밤 랜덤 주문 2건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 59, 2);
        }
        else if (index == 67) // 밤 주문 1건 애쉬
        {
            Debug.Log("밤 주문 1건 애쉬");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 68, 1);
        }
        else if (index == 74) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 75, 1);
        }
        //2일차 낮 주문 리스트
        else if (index == 98) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 99, 1);
        }
        else if (index == 99) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 100, 1);
        }
        else if (index == 100) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 101, 1);
        }
        else if (index == 101) // 룸서비스 랜덤 2건 각각
        {
            Debug.Log("배달 랜덤 룸서비스 주문 2건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 103, 2);
        }
        //2일차 밤 주문 리스트
        else if (index == 104) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 105, 1);
        }
        else if (index == 115) // 밤 주문 1건 애쉬
        {
            Debug.Log("밤 주문 1건 애쉬");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 116, 1);
        }
        else if (index == 129) // 랜덤 주문 2건 각각
        {
            Debug.Log("밤 랜덤 주문 2건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 131, 2);
        }
        else
        {
            // 기본 대화 진행
            PrintProDialogue(index);
        }
    }

    //애니메이션 시작될 때 호출
    public void OnAnimationStart()
    {
        isAnimationPlaying = true;
    }

    // 애니메이션이 끝날 때 호출
    public void OnAnimationEnd()
    {
        isAnimationPlaying = false;
    }

    void LoadDialogueFromCSV()
    {
        List<Dictionary<string, object>> data_Dialog = Ch0CSVReader.Read("Travel Around The World - CH3(설원)");

        foreach (var row in data_Dialog)
        {
            string dayString = row["일자"].ToString();
            int day = int.Parse(System.Text.RegularExpressions.Regex.Match(dayString, @"\d+").Value);
            string location = row["장소"].ToString();
            string speaker = row["인물"].ToString();
            string line = row["대사"].ToString();
            string screenEffect = row["화면 연출"].ToString();
            string backgroundMusic = row["배경음악"].ToString();
            string expression = row["표정"].ToString();
            string note = row["비고"].ToString();
            string note2 = row["비고 2"].ToString();
            string questContent = row["퀘스트 내용"].ToString();

            proDialogue.Add(new Ch3ProDialogue(day, location, speaker, line, screenEffect, backgroundMusic, expression, note, note2, questContent));
        }
    }

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

            // 레이비야크 표정 이미지
            ["레이비야크_일반"] = Resources.Load<Sprite>("NpcImage/Leviac"),
            ["레이비야크_웃음"] = Resources.Load<Sprite>("NpcImage/Leviac_웃음"),

            // 바이올렛 표정 이미지
            ["바이올렛_일반"] = Resources.Load<Sprite>("NpcImage/Violet"),
            ["바이올렛_웃음"] = Resources.Load<Sprite>("NpcImage/Violet_웃음"),
            ["바이올렛_윙크"] = Resources.Load<Sprite>("NpcImage/Violet_윙크"),

            // 러스크 표정 이미지
            ["러스크_일반"] = Resources.Load<Sprite>("NpcImage/Rusk"),
            ["러스크_웃음"] = Resources.Load<Sprite>("NpcImage/Rusk_웃음"),

            // Mr. Ham 표정 이미지
            ["Mr. Ham_일반"] = Resources.Load<Sprite>("NpcImage/MrHam"),
            ["Mr. Ham_웃음"] = Resources.Load<Sprite>("NpcImage/MrHam_웃음"),
            ["Mr. Ham_화남"] = Resources.Load<Sprite>("NpcImage/MrHam_화남"),
            ["Mr. Ham_아쉬움"] = Resources.Load<Sprite>("NpcImage/MrHam_아쉬움"),

            // 기본 NPC 이미지
            ["Default"] = Resources.Load<Sprite>("NpcImage/Default")
        };
    }

    void PrintProDialogue(int index)
    {
        if (index >= proDialogue.Count)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            return; // 대사 리스트를 벗어나면 오브젝트 비활성화 후 리턴
        }

        Ch3ProDialogue currentDialogue = proDialogue[index];

        string expressionKey = !string.IsNullOrEmpty(currentDialogue.expression) ? $"_{currentDialogue.expression}" : "";
        speakerKey = "";

        //인물이 ??인 경우 이미지 처리
        if (currentDialogue.speaker == unknownSpeaker)
        {
            Debug.Log(currentDialogue.day);
            speakerKey = "코코";
        }
        else
        {
            speakerKey = currentDialogue.speaker;
        }

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

        //이미지 적용
        if (imageObj.GetComponent<SpriteRenderer>() != null)
        {
            imageObj.GetComponent<SpriteRenderer>().sprite = characterSprite;
        }
        else if (imageObj.GetComponent<Image>() != null)
        {
            imageObj.GetComponent<Image>().sprite = characterSprite;
        }

        //인물에 따라 대사/나레이션/텍스트 창 활성화
        //인물 혹은 장소가 없는 경우 - 대사 모두 비활성화
        if (string.IsNullOrEmpty(currentDialogue.speaker) || string.IsNullOrEmpty(currentDialogue.location))
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        //인물이 나레이션일 경우 - 나레이션창 활성화
        else if (currentDialogue.speaker == narrationSpeaker)
        {
            narration.SetActive(true);
            dialogue.SetActive(false);
            narrationBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }
        //인물이 있을 경우 - 대사창 활성화
        else
        {
            narration.SetActive(false);
            dialogue.SetActive(true);
            dialogueBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }

        // 카페로 강제 이동 후 이동 가능하게 전환
        if (index == 10)
        {
            player.transform.position = new Vector3(0, 0, 0);
            mapManager.currentState = MapState.Cafe;
            isWaitingForPlayer = true;
            player.SetActive(true);
            map.SetActive(true);
            playerController.StartMove();
            trainRoom.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        // 카페 일 끝나고 이동 가능하게 전환
        else if (index == 75)
        {
            player.transform.position = new Vector3(2, -3.5f, 0);
            isWaitingForPlayer = true;
            playerController.StartMove();
            map.SetActive(true);
            player.SetActive(true);
            cafe.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(false);
            Npc_Rayviyak.SetActive(true);
            Npc_MrHam.SetActive(true);
            Npc_Rusk.SetActive(true);
            Npc_Violet.SetActive(true);
        }
        else
        {
            CheckTalk(currentDialogue.location);
        }
    }

    public void StartDialogueSequence(int startIndex, int endIndex)
    {
        for (int i = startIndex; i <= endIndex; i++)
        {
            PrintProDialogue(i);
        }
    }

    public void EnableBedInteraction()
    {
        bedUsed = true; // 침대 상호작용 활성화
    }

    public void ShowNarration(string speaker, string text)
    {
        narration.SetActive(true);
        dialogue.SetActive(false);
        narrationBar.SetDialogue(speaker, text);
    }

    public void OnDialogueButtonClicked(int index)
    {
        /*
        if (currentDialogueIndex == 43)
        {
            // 현재 오브젝트의 이름을 확인하기 위해 호출한 객체에서 정보를 받아야 함
            GameObject currentNpc = GameObject.FindGameObjectWithTag("CurrentNpc"); // 'CurrentNpc'는 현재 상호작용하는 NPC에 태그 지정

            if (currentNpc != null && currentNpc.name == "Npc_Rayviyak" && !HasTalkedToRayviyak)
            {
                StartDialogueSequence(44, 47);
                HasTalkedToRayviyak = true;
                currentDialogueIndex = 43; // 대화 후 인덱스 유지
            }
            else if (currentNpc != null && currentNpc.name == "Npc_Violet" && !HasTalkedToViolet)
            {
                StartDialogueSequence(49, 75);
                HasTalkedToViolet = true;
                currentDialogueIndex = 43; // 대화 후 인덱스 유지
            }
            else if (currentNpc != null && currentNpc.name != "Npc_Rayviyak" && currentNpc.name != "Npc_Violet")
            {
                ShowNarration("나레이션", "지금은 바빠 보여.");
                currentDialogueIndex = 43; // 인덱스 유지
            }

            // 레이비야크와 바이올렛 둘 다 대화가 끝났다면, 침대와 상호작용할 수 있게 설정
            if (HasTalkedToRayviyak && HasTalkedToViolet)
            {
                EnableBedInteraction();
            }
        }
        else
        {
            // 기존의 다른 인덱스에 대한 대화 로직
            PrintCh1ProDialogue(index);
        }
        */
    }

    public void ActivateTalk(string locationName, int curDialogueIdx)
    {
        this.gameObject.SetActive(true);
        isActivated = true;

        // locationName에 따라 인덱스 조정하여 특정 대화를 시작할 수 있도록 수정
        currentDialogueIndex = proDialogue.FindIndex(dialogue => dialogue.location == locationName);

        currentDialogueIndex = curDialogueIdx;

        if (currentDialogueIndex >= 0)
        {
            PrintProDialogue(currentDialogueIndex);
        }
    }

    void DeactivateTalk()
    {
        this.gameObject.SetActive(false);
        isActivated = false;
    }

    public void SetDialogueIndex(int index, bool isTransitionValue)
    {
        isTransition = true;
        currentDialogueIndex = index;
    }

    //대사 위치에 따라 특정 화면을 활성화 시키거나 음악을 재생하는 함수
    void CheckTalk(string location)
    {
        cafe.SetActive(false);
        cafe2.SetActive(false);
        garden.SetActive(false);
        gardenCafe.SetActive(false);
        bakery.SetActive(false);
        medicalRoom.SetActive(false);
        trainRoom.SetActive(false);
        trainRoomHallway.SetActive(false);
        trainRoomPenguinFox.SetActive(false);
        trainRoomHallwayWolf.SetActive(false);
        balcony.SetActive(false);

        switch (location)
        {
            case locationCafe:
                PlayMusic(locationCafe);
                cafe.SetActive(true);
                break;
            case locationCafe2:
                PlayMusic(locationCafe);
                cafe2.SetActive(true);
                break;
            case locationGarden:
                PlayMusic(locationGarden);
                garden.SetActive(true);
                break;
            case locationBakery:
                PlayMusic(locationBakery);
                bakery.SetActive(true);
                break;
            case locationMedicalRoom:
                PlayMusic(locationMedicalRoom);
                medicalRoom.SetActive(true);
                break;
            case locationTrainRoom:
                PlayMusic(locationTrainRoom);
                trainRoom.SetActive(true);
                break;
            case locationTrainRoomHallway:
                PlayMusic(locationTrainRoom);
                trainRoomHallway.SetActive(true);
                break;
            case locationTrainRoomPenguinFox:
                PlayMusic(locationTrainRoom);
                trainRoomPenguinFox.SetActive(true);
                break;
            case locationTrainRoomHallwayWolf:
                PlayMusic(locationTrainRoom);
                trainRoomHallwayWolf.SetActive(true);
                break;
            case locationBalcony:
                balcony.SetActive(true);
                break;
        }
    }

    public void PlayMusic(string location = null)
    {
        string newMusic = ""; // 재생할 음악 이름

        // 대사 상의 location에 따른 음악 설정
        switch (location)
        {
            case locationCafe:
                newMusic = "CAFE";
                break;
            case locationGarden:
                newMusic = "GARDEN";
                break;
            case locationBakery:
                newMusic = "BAKERY";
                break;
            case locationMedicalRoom:
                newMusic = "amedicaloffice_001";
                break;
            case locationTrainRoom:
                newMusic = "a room";
                break;
            default:
                newMusic = "CAFE";
                break;
        }

        // 새로운 음악이 현재 음악과 다를 경우에만 음악 재생
        if (currentMusic != newMusic)
        {
            SoundManager.Instance.PlayMusic(newMusic, loop: true);
            currentMusic = newMusic;
        }
    }

    private IEnumerator FadeOutAndDeactivateTalk(GameObject obj)
    {
        isFadingOut = true; // 페이드아웃 시작
        yield return StartCoroutine(screenFader.FadeOut(obj)); // FadeOut이 완료될 때까지 기다립니다.
        narration.SetActive(false);
        dialogue.SetActive(false);
        DeactivateTalk(); // FadeOut이 완료된 후 대화 비활성화
        isFadingOut = false; // 페이드아웃 종료
        playerController.StartMove(); //대사 끝나고 플레이어 움직임 재개
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

    private IEnumerator PerformFadeInAndHandleDialogue(int fromDialogueIdx, int returnDialogueIdx)
    {
        yield return StartCoroutine(screenFader.FadeOut(cafe));

        // 페이드 인이 완료된 후 씬 전환 작업 수행
        //SceneTransitionManager.Instance.HandleDialogueTransition("Ch0Scene", "CafeTutorialScene", fromDialogueIdx, 72, returnDialogueIdx);
        currentDialogueIndex = returnDialogueIdx;
    }
}
