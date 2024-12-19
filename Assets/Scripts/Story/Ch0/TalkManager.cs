using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;


public class TalkManager : MonoBehaviour
{
    public static TalkManager Instance { get; private set; }
    // 대사들을 저장할 리스트
    private List<ProDialogue> proDialogue;

    public GameObject opening;
    public GameObject narration;
    public GameObject dialogue;

    public GameObject imageObj; // 초상화 이미지 요소
    public GameObject nameObj; // 이름 요소

    public GameObject invitation; // 초대장 화면
    public TextMeshProUGUI invitationText;
    public GameObject backGround; //검은 배경

    public GameObject forest; // 숲 화면

    public GameObject trainStation; // 기차역 화면
    public GameObject train; // 기차 화면
    public GameObject trainBackground; // 기차 배경 화면

    public GameObject cafe; // 카페 화면
    public GameObject trainRoom; // 객실 화면
    public GameObject trainRoomHallway; // 객실 복도 화면
    public GameObject garden; // 정원 화면
    public GameObject bakery; // 빵집 화면
    public GameObject medicalRoom; // 의무실 화면

    public ScreenFader screenFader; // 페이드인/아웃 효과 스크립트
    private bool isFadingOut = false; // 페이드 아웃 중인지 여부 (페이드 아웃 중에는 입력 무시하기 위해)

    public Ch0DialogueBar dialogueBar; // 대화창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar narrationBar; // 나레이션창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar openingBar; // 오프닝 대사창 스크립트 (타이핑 효과 호출을 위해)

    public GameObject mapTutorial; //맵 튜토리얼
    public MapTurorial MapTutorial;
    public Ch0MapManager mapManager;

    // 문자열 상수 선언
    private const string narrationSpeaker = "나레이션";
    private const string invitationSpeaker = "초대장";
    private const string locationHome = "집";
    private const string locationForest = "숲";
    private const string locationTrainStation = "기차역";
    private const string locationCafe = "카페";
    private const string locationEngineRoom = "엔진룸";
    private const string locationOtherRoom1 = "다른 방 1";
    private const string locationOtherRoom2 = "다른 방 2";
    private const string locationGarden = "정원";
    private const string locationBakery = "빵집";
    private const string locationMedicalRoom = "의무실";
    private const string locationTrainRoom = "객실";
    private const string unknownSpeaker = "???";

    public int currentDialogueIndex = 0; // 현재 대사 인덱스
    private bool isActivated = false; // TalkManager가 활성화되었는지 여부

    public bool isAllNPCActivated = false; //모든 npc와 대화 완료되었는지 여부

    private Dictionary<string, Sprite> characterImages; // 캐릭터 이름과 이미지를 매핑하는 사전
    private Sprite characterSprite;

    public PlayerController playerController;

    public string currentMusic = ""; // 현재 재생 중인 음악의 이름을 저장
    public Sprite openPaperImg;

    public bool isAnimationPlaying = false;

    private Animator trainAnimator;
    private Animator letterAnimator;

    public bool isTransition = false;

    //스페이스바 키 보여주는 오브젝트
    public GameObject pressKeyObject;

    public string speakerKey;

    void Awake()
    {
        Instance = this;
        letterAnimator = invitation.GetComponent<Animator>();
        trainAnimator = train.GetComponent<Animator>();
        proDialogue = new List<ProDialogue>();
        LoadDialogueFromCSV(); // CSV에서 데이터를 로드하는 함수 호출
        InitializeCharacterImages();
        playerController.StopMove();
        backGround.SetActive(true);
    }

    void Start()
    {
        if (!isTransition)
            ActivateTalk(locationHome); // 오브젝트 활성화
        else
        {
            OnAnimationEnd();
            ActivateTalk(locationCafe);
        }
            
    }

    void Update()
    {
        if (isActivated && !isFadingOut && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if (isAnimationPlaying)
            {
                return;
            }

            bool anyTyping = false;

            // 순서대로 확인
            if (opening != null && opening.GetComponentInChildren<Ch0TypeEffect>().IsTyping())
            {
                opening.GetComponentInChildren<Ch0TypeEffect>().CompleteEffect();
                anyTyping = true;
            }

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
                    PrintProDialogue(currentDialogueIndex);
                }
            }
        }

        // 맵 이동 조작 튜토리얼
        if (currentDialogueIndex == 67)
        {
            mapTutorial.SetActive(true);
            DeactivateTalk(); // 대화 잠시 종료
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
        List<Dictionary<string, object>> data_Dialog = Ch0CSVReader.Read("Travel Around The World - CH0");

        foreach(var row in data_Dialog)
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
            string quest = row["퀘스트"].ToString();
            string questContent = row["퀘스트 내용"].ToString();

            proDialogue.Add(new ProDialogue(day, location, speaker, line, screenEffect, backgroundMusic, expression, note, quest, questContent));
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

        ProDialogue currentDialogue = proDialogue[index];

        string expressionKey = !string.IsNullOrEmpty(currentDialogue.expression) ? $"_{currentDialogue.expression}" : "";
        speakerKey = "";

        //인물이 ???인 경우 장소에 따라 이미지 처리
        if (currentDialogue.speaker == unknownSpeaker)
        {
            if (currentDialogue.location == locationTrainStation)
            {
                speakerKey = "바이올렛";
            }
            else if (currentDialogue.location == locationGarden)
            {
                speakerKey = "레이비야크";
            }
            else if (currentDialogue.location == locationBakery)
            {
                speakerKey = "러스크";
            }
            else if (currentDialogue.location == locationMedicalRoom)
            {
                speakerKey = "Mr. Ham";
            }
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

        if (index < 2)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            opening.SetActive(true);
            openingBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }
        //오프닝 대사 이후부터 인물에 따라 대사/나레이션/텍스트 창 활성화
        //인물이 초대장일 경우 - 초대장 활성화
        else if (currentDialogue.speaker == invitationSpeaker)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            opening.SetActive(false);
            if (!string.IsNullOrEmpty(invitationText.text))
            {
                invitationText.text += "\n";
            }
            invitationText.text += currentDialogue.line;

        }
        //인물 혹은 장소가 없는 경우 - 대사 모두 비활성화
        else if (string.IsNullOrEmpty(currentDialogue.speaker) || string.IsNullOrEmpty(currentDialogue.location))
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            opening.SetActive(false);
        }
        //인물이 나레이션일 경우 - 나레이션창 활성화
        else if ((currentDialogue.speaker == narrationSpeaker))
        {
            narration.SetActive(true);
            dialogue.SetActive(false);
            opening.SetActive(false);
            narrationBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }
        //인물이 있을 경우 - 대사창 활성화
        else
        {
            narration.SetActive(false);
            dialogue.SetActive(true);
            opening.SetActive(false);
            dialogueBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }

        CheckTalk(currentDialogue.location);
    }

    public void ActivateTalk(string locationName)
    {
        this.gameObject.SetActive(true);
        isActivated = true;

        //위치가 객실이고 npc와 다 대화하지 않은 경우 예외처리 
        if (locationName == locationTrainRoom && !isAllNPCActivated)
        {
            currentDialogueIndex = 133;
            PrintProDialogue(currentDialogueIndex);
        }
        else
        {
            // locationName에 따라 인덱스 조정하여 특정 대화를 시작할 수 있도록 수정
            currentDialogueIndex = proDialogue.FindIndex(dialogue => dialogue.location == locationName);

            if (currentDialogueIndex >= 0)
            {
                PrintProDialogue(currentDialogueIndex);
            }
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
        //PrintProDialogue(currentDialogueIndex); // 대사 출력
    }

    void CheckTalk(string location)
    {
        invitation.SetActive(false);
        forest.SetActive(false);
        trainStation.SetActive(false);
        train.SetActive(false);
        trainBackground.SetActive(false);
        cafe.SetActive(false);
        trainRoom.SetActive(false);
        trainRoomHallway.SetActive(false);
        garden.SetActive(false);
        bakery.SetActive(false);
        medicalRoom.SetActive(false);

        switch (location)
        {
            case locationHome:
                PlayMusic(locationHome);
                if(currentDialogueIndex == 0)
                {
                    pressKeyObject.SetActive(true);
                }

                if(currentDialogueIndex == 1)
                {
                    pressKeyObject.SetActive(false);
                }

                if (currentDialogueIndex == 2)
                {
                    StartCoroutine(screenFader.FadeIn(invitation));
                }

                else if (currentDialogueIndex >= 3 && currentDialogueIndex <= 23)
                {
                    invitation.SetActive(true);
                    if (currentDialogueIndex == 3)
                        letterAnimator.SetTrigger("isTwinkled");

                    if (currentDialogueIndex >= 3 && currentDialogueIndex <= 5)
                    {
                        invitationText.gameObject.SetActive(false);
                    }

                    else if (currentDialogueIndex >= 6)
                    {
                        invitation.GetComponent<SpriteRenderer>().sprite = openPaperImg;
                        if (currentDialogueIndex == 6)
                        {
                            OnAnimationStart();
                            letterAnimator.SetBool("isOpened", true);

                            //currentDialogueIndex += 1;
                            //SoundManager.Instance.PlaySFX("twinkle");
                            //invitationText.gameObject.SetActive(true);

                        }
                        else
                        {
                            invitationText.gameObject.SetActive(true);
                        }
                    }
                    if (currentDialogueIndex == 23)
                    {
                        StartCoroutine(screenFader.FadeOut(invitation));
                        SoundManager.Instance.PlaySFX("packing");
                    }
                }
                break;
            case locationForest:
                if (currentDialogueIndex == 24)
                {
                    SoundManager.Instance.PlaySFX("soil_walk");
                    StartCoroutine(screenFader.FadeIn(forest));
                }
                else
                {
                    forest.SetActive(true);
                }
                break;
            case locationTrainStation:
                if (currentDialogueIndex == 28)
                {
                    SoundManager.Instance.StopSFX();
                    StartCoroutine(screenFader.FadeIn(trainStation));
                }
                else
                {
                    trainStation.SetActive(true);
                    if (currentDialogueIndex >= 32)
                    {
                        trainStation.SetActive(false);
                        train.SetActive(true);
                        trainBackground.SetActive(true);
                        if (currentDialogueIndex == 32)
                        {
                            OnAnimationStart();
                            trainAnimator.SetTrigger("PlayTrainAnimation");
                            SoundManager.Instance.PlaySFX("horn");
                            SoundManager.Instance.PlaySFX("a train operation");
                        }
                        if (currentDialogueIndex == 37)
                        {
                            SoundManager.Instance.StopSFX();
                        }
                        if (currentDialogueIndex == 49)
                        {
                            OnAnimationStart();
                            StartCoroutine(PerformFadeInAndHandleDialogue(49, 50));
                        }
                    }
                }
                break;
            // 카페 튜토리얼 이후 ~ 맵 튜토리얼 이전
            case locationCafe:
                PlayMusic(locationCafe);
                if (currentDialogueIndex == 50)
                {
                    StartCoroutine(screenFader.FadeIn(cafe));
                }
                else if (currentDialogueIndex <= 66)
                {
                    cafe.SetActive(true);
                    if (currentDialogueIndex == 66)
                    {
                        StartCoroutine(screenFader.FadeOut(cafe));
                    }
                }
                break;
            // 맵 튜토리얼
            case locationEngineRoom:
                if (currentDialogueIndex == 68)
                {
                    StartCoroutine(screenFader.FadeIn(trainRoomHallway));
                }
                else if (currentDialogueIndex == 69)
                {
                    DeactivateTalk();
                    playerController.StartMove(); //대사 끝나고 플레이어 움직임 재개
                }
                break;
            case locationOtherRoom1:
                if (currentDialogueIndex == 70)
                {
                    StartCoroutine(screenFader.FadeIn(trainRoomHallway));
                }
                else if (currentDialogueIndex == 71)
                {
                    DeactivateTalk();
                    playerController.StartMove(); //대사 끝나고 플레이어 움직임 재개
                }
                break;
            case locationOtherRoom2:
                if (currentDialogueIndex == 72)
                {
                    StartCoroutine(screenFader.FadeIn(trainRoomHallway));
                }
                else if (currentDialogueIndex == 73)
                {
                    DeactivateTalk();
                    playerController.StartMove(); //대사 끝나고 플레이어 움직임 재개
                }
                break;
            case locationGarden:
                PlayMusic(locationGarden);
                if (currentDialogueIndex == 74)
                {
                    StartCoroutine(screenFader.FadeIn(garden));
                }
                else
                {
                    garden.SetActive(true);
                    if (currentDialogueIndex == 82)
                    {
                        StartCoroutine(FadeOutAndDeactivateTalk(garden)); //npc 대화 끝나고 대화 종료
                    }
                }
                break;
            case locationBakery:
                PlayMusic(locationBakery);
                if (currentDialogueIndex == 83)
                {
                    StartCoroutine(screenFader.FadeIn(bakery));
                }
                else
                {
                    bakery.SetActive(true);
                    if (currentDialogueIndex == 108)
                    {
                        StartCoroutine(FadeOutAndDeactivateTalk(bakery)); //npc 대화 끝나고 대화 종료
                    }
                }
                break;
            case locationMedicalRoom:
                PlayMusic(locationMedicalRoom);
                if (currentDialogueIndex == 109)
                {
                    StartCoroutine(screenFader.FadeIn(medicalRoom));
                }
                else
                {
                    medicalRoom.SetActive(true);
                    if (currentDialogueIndex == 132)
                    {
                        StartCoroutine(FadeOutAndDeactivateTalk(medicalRoom)); //npc 대화 끝나고 대화 종료
                    }
                }
                break;
            case locationTrainRoom:
                //침대를 사용했고 잠에 들려고 하는 경우만 객실 활성화
                if (MapTutorial.bedUsed && MapTutorial.isSleeping)
                {
                    //모든 npc와 대화한 경우
                    if (isAllNPCActivated)
                    {
                        
                        if (currentDialogueIndex == 133)
                        {
                            StartCoroutine(screenFader.FadeIn(trainRoom));
                        }
                        else if (currentDialogueIndex >= 134 && currentDialogueIndex <= 136)
                        {
                            trainRoom.SetActive(true);
                            if(currentDialogueIndex == 134)
                            {
                                DayNightCycleManager.Instance.ChangeDayTime();
                                Debug.Log(DayNightCycleManager.Instance.GetNowDayTime());
                            }
                            if (currentDialogueIndex == 136)
                            {
                                StartCoroutine(FadeOutAndLoadScene(trainRoom, "ch1Scene 1"));
                            }
                        }
                    }
                    //모든 npc와 대화하지 않은 경우
                    else
                    {
                        if (currentDialogueIndex == 133)
                        {
                            StartCoroutine(screenFader.FadeIn(trainRoom));
                        }
                        else if (currentDialogueIndex == 134)
                        {
                            MapTutorial.bedUsed = false; // 침대 사용과 잠에 드는지 여부 둘다 false로 초기화
                            MapTutorial.isSleeping = false;
                            DeactivateTalk();
                            playerController.StartMove(); //대사 끝나고 플레이어 움직임 재개
                        }
                    }
                }
                break;
        }
    }

    public void PlayMusic(string location = null)
    {
        string newMusic = ""; // 재생할 음악 이름

        // 대사 상의 location에 따른 음악 설정
        switch (location)
         {
            case locationHome:
                newMusic = "TRAIN STATION 1.3";
                break;
            case locationForest:
                newMusic = "TRAIN STATION 1.3";
                break;
            case locationTrainStation:
                newMusic = "TRAIN STATION 1.3";
                break;
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
        yield return StartCoroutine(screenFader.FadeOut(trainStation));
        yield return StartCoroutine(screenFader.FadeOut(train));
        
        // 페이드 인이 완료된 후 씬 전환 작업 수행
        SceneTransitionManager.Instance.HandleDialogueTransition("Ch0Scene", "CafeTutorialScene", fromDialogueIdx, 56, returnDialogueIdx);
        currentDialogueIndex = returnDialogueIdx;
    }

}