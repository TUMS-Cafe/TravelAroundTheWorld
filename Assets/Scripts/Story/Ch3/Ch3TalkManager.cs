using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ch3TalkManager : MonoBehaviour
{
    public static Ch3TalkManager Instance { get; private set; }
    // 대사들을 저장할 리스트
    private List<Ch3ProDialogue> proDialogue;

    public GameObject narration;
    public GameObject dialogue;

    public GameObject imageObj; // 초상화 이미지 요소
    public GameObject nameObj; // 이름 요소
    public GameObject bigImageObj; // 큰 이미지

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
    
    public bool isNpcTalkActivated=false;
    public string currentNpc = "Null";
    public GameObject Npc_Rayviyak; // 정원 npc
    public GameObject Npc_MrHam; // 병원 npc
    public GameObject Npc_Rusk; // 빵집 npc
    public GameObject Npc_Violet; // 바 npc
    public GameObject Npc_Coco;
    public GameObject Npc_Nicksy;
    public GameObject Npc_Naru;
    public GameObject Npc_Ash;

    public bool isCh2HappyEnding = false;
    public GameObject Npc_Kuraya; // 빵집 npc (ch2 해피엔딩일 때)

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

    private Dictionary<string, Sprite> characterImages; // 캐릭터 이름과 이미지를 매핑하는 사전
    private Sprite characterSprite;
    private Dictionary<string, Sprite> characterBigImages;

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

    public bool HasTalkedToRayviyak = false;
    public bool HasTalkedToViolet = false;
    public bool HasTalkedToRusk = false;
    public bool HasTalkedToMrHam = false;
    public bool HasTalkedToCoco = false;
    public bool HasTalkedToNicksy = false;
    public bool HasTalkedToNaru = false;
    public bool HasTalkedToAsh = false;
    public bool HasTalkedToKuraya = false;

    void Awake()
    {
        Instance = this;
        proDialogue = new List<Ch3ProDialogue>();
        LoadDialogueFromCSV(); // CSV에서 데이터를 로드하는 함수 호출
        InitializeCharacterImages();
        playerController.StopMove();
        trainRoom.SetActive(true);
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
            CheckTalk(locationTrainRoom);
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
            if (mapManager.currentState == MapState.Cafe && mapManager.isInCafeBarZone && (currentDialogueIndex == 10 || currentDialogueIndex == 98 || currentDialogueIndex == 157 || currentDialogueIndex == 255 || currentDialogueIndex == 393 || currentDialogueIndex == 455 || currentDialogueIndex == 688))
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
                Npc_Coco.SetActive(false);
                Npc_Nicksy.SetActive(false);
                Npc_Naru.SetActive(false);
                Npc_Ash.SetActive(false);
                Npc_Kuraya.SetActive(false);
            }

            // 카페에서 일해야 되는데 다른 곳으로 가려고 하면 다시 카페로 플레이어 강제 이동
            if (mapManager.currentState != MapState.Cafe && (currentDialogueIndex == 10 || currentDialogueIndex == 98 || currentDialogueIndex == 157 || currentDialogueIndex == 255 || currentDialogueIndex == 393 || currentDialogueIndex == 455 || currentDialogueIndex == 688))
            {
                player.transform.position = new Vector3(0, 0, 0);
                narration.SetActive(true);
                dialogue.SetActive(false);
                narrationBar.SetDialogue("나레이션", "지금은 일할 시간이야.");
            }
            
            // 객실로 돌아가야 되는데 다른 곳으로 가려고 하면 다시 객실복도로 플레이어 강제 이동
            if (mapManager.currentState != MapState.TrainRoom3 && mapManager.currentState != MapState.Hallway)
            {
                if (isCh2HappyEnding && currentDialogueIndex == 533)
                {
                    player.transform.position = new Vector3(-34, -2, 0);
                    narration.SetActive(true);
                    dialogue.SetActive(false);
                    narrationBar.SetDialogue("나레이션", "돌아다니기엔 너무 늦었다. 오늘은 이만 객실로 돌아가자.");
                }
                else if (!isCh2HappyEnding && currentDialogueIndex == 683)
                {
                    player.transform.position = new Vector3(-34, -2, 0);
                    narration.SetActive(true);
                    dialogue.SetActive(false);
                    narrationBar.SetDialogue("나레이션", "돌아다니기엔 너무 늦었다. 오늘은 이만 객실로 돌아가자.");
                }
            }
        }
        // npc 대화 버튼 눌렸을때 대화 진행
        if (isNpcTalkActivated && !isFadingOut && isWaitingForPlayer && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
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
                    NpcDialogue(currentDialogueIndex, currentNpc);
                }
            }
        }

        // 검은 인영
        if (currentDialogueIndex == 281)
        {
            backGround.SetActive(true);
            trainRoom.SetActive(false);
            cafe.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(true);
        }
        if (currentDialogueIndex == 282)
        {
            backGround.SetActive(false);
            trainRoom.SetActive(false);
            cafe.SetActive(true);
            narration.SetActive(false);
            dialogue.SetActive(true);
        }
        //6일차 엔딩 분기점에서 해피엔딩 여부에 따라 대화 인덱스 변경
        if (isCh2HappyEnding)
        {
            if (currentDialogueIndex == 466)
            {
                currentDialogueIndex = 467;
                PrintProDialogue(467);
            }
            //펭귄여우객실로 자동 이동
            else if (currentDialogueIndex == 487)
            {
                player.transform.position = new Vector3(-34, -2, 0);
                playerController.StopMove();
                mapManager.currentState = MapState.Hallway;
                player.SetActive(true);
                map.SetActive(true);
                trainRoom.SetActive(false);
                cafe.SetActive(false);
                currentDialogueIndex = 488;
                PrintProDialogue(488);
            }
            //미니게임 씬 연결
            else if (currentDialogueIndex == 499)
            {
                SceneManager.LoadScene("HideandSeek");
            }
            // Ch3 미니게임 완료 여부 및 성공 여부 체크
            else if (PlayerManager.Instance.IsCh3MiniGamePlayed())
            {
                if (PlayerManager.Instance.IsCh3MiniGameSuccess())
                {
                    if (currentDialogueIndex == 0)
                    {
                        player.transform.position = new Vector3(-34, -2, 0);
                        playerController.StopMove();
                        mapManager.currentState = MapState.Hallway;
                        player.SetActive(true);
                        map.SetActive(true);
                        trainRoom.SetActive(false);
                        cafe.SetActive(false);
                        currentDialogueIndex = 500;
                        PrintProDialogue(500);
                    }
                    //객실로 돌아가야할 때
                    else if (currentDialogueIndex == 513)
                    {
                        currentDialogueIndex = 533;
                        isWaitingForPlayer = true;
                        player.SetActive(true);
                        map.SetActive(true);
                        playerController.StartMove();
                        trainRoom.SetActive(false);
                        narration.SetActive(false);
                        dialogue.SetActive(false);
                    }
                }
                if (!PlayerManager.Instance.IsCh3MiniGameSuccess())
                {
                    if (currentDialogueIndex == 0)
                    {
                        player.transform.position = new Vector3(-34, -2, 0);
                        playerController.StopMove();
                        mapManager.currentState = MapState.Hallway;
                        player.SetActive(true);
                        map.SetActive(true);
                        trainRoom.SetActive(false);
                        cafe.SetActive(false);
                        currentDialogueIndex = 513;
                        PrintProDialogue(513);
                    }
                    else if (currentDialogueIndex == 533)
                    {
                        isWaitingForPlayer = true;
                        player.SetActive(true);
                        map.SetActive(true);
                        playerController.StartMove();
                        trainRoom.SetActive(false);
                        narration.SetActive(false);
                        dialogue.SetActive(false);
                    }
                }
                //늑대 객실 앞 복도로 자동 이동
                if (currentDialogueIndex == 539)
                {
                    player.transform.position = new Vector3(-34, -2, 0);
                    playerController.StopMove();
                    mapManager.currentState = MapState.Hallway;
                    player.SetActive(true);
                    map.SetActive(true);
                    trainRoom.SetActive(false);
                    cafe.SetActive(false);
                }
                // 7일차 낮 엔딩
                if (currentDialogueIndex == 640)
                {
                    mapManager.currentState = MapState.Null;
                    player.SetActive(false);
                    map.SetActive(false);
                    backGround.SetActive(true);
                    trainRoom.SetActive(false);
                    cafe.SetActive(false);
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    currentDialogueIndex = 743;
                }
                // 다음 챕터 씬 연결
                if (currentDialogueIndex == 745)
                {
                    //StartCoroutine(FadeOutAndLoadScene(cafe, "Ch4Scene"));
                }
            }
        }
        else if (!isCh2HappyEnding)
        {
            if (currentDialogueIndex == 466)
            {
                currentDialogueIndex = 640;
                PrintProDialogue(640);
            }
            //펭귄여우객실로 자동 이동
            else if (currentDialogueIndex == 655)
            {
                player.transform.position = new Vector3(-34, -2, 0);
                player.SetActive(true);
                playerController.StopMove();
                map.SetActive(true);
                mapManager.currentState = MapState.Hallway;
                trainRoom.SetActive(false);
                cafe.SetActive(false);
                currentDialogueIndex = 656;
                PrintProDialogue(656);
            }
            //미니게임 씬 연결
            else if (currentDialogueIndex == 667)
            {
                SceneManager.LoadScene("HideandSeek");
            }
            // Ch3 미니게임 완료 여부 및 성공 여부 체크
            else if (PlayerManager.Instance.IsCh3MiniGamePlayed())
            {
                if (PlayerManager.Instance.IsCh3MiniGameSuccess())
                {
                    if (currentDialogueIndex == 0)
                    {
                        player.transform.position = new Vector3(-34, -2, 0);
                        playerController.StopMove();
                        mapManager.currentState = MapState.Hallway;
                        player.SetActive(true);
                        map.SetActive(true);
                        trainRoom.SetActive(false);
                        cafe.SetActive(false);
                        currentDialogueIndex = 668;
                        PrintProDialogue(668);
                    }
                    //객실로 돌아가야할 때
                    else if (currentDialogueIndex == 673)
                    {
                        currentDialogueIndex = 683;
                        isWaitingForPlayer = true;
                        player.SetActive(true);
                        map.SetActive(true);
                        playerController.StartMove();
                        trainRoom.SetActive(false);
                        narration.SetActive(false);
                        dialogue.SetActive(false);
                    }
                }
                if (!PlayerManager.Instance.IsCh3MiniGameSuccess())
                {
                    if (currentDialogueIndex == 0)
                    {
                        player.transform.position = new Vector3(-34, -2, 0);
                        playerController.StopMove();
                        mapManager.currentState = MapState.Hallway;
                        player.SetActive(true);
                        map.SetActive(true);
                        trainRoom.SetActive(false);
                        cafe.SetActive(false);
                        currentDialogueIndex = 673;
                        PrintProDialogue(673);
                    }
                    else if (currentDialogueIndex == 683)
                    {
                        isWaitingForPlayer = true;
                        player.SetActive(true);
                        map.SetActive(true);
                        playerController.StartMove();
                        trainRoom.SetActive(false);
                        narration.SetActive(false);
                        dialogue.SetActive(false);
                    }
                }
                // 7일차 낮 엔딩
                if (currentDialogueIndex == 743)
                {
                    PlayMusic(locationTrainRoom);
                    mapManager.currentState = MapState.Null;
                    player.SetActive(false);
                    map.SetActive(false);
                    backGround.SetActive(true);
                    trainRoom.SetActive(false);
                    cafe.SetActive(false);
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                }
                // 다음 챕터 씬 연결
                if (currentDialogueIndex == 745)
                {
                    //StartCoroutine(FadeOutAndLoadScene(cafe, "Ch4Scene"));
                }
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
        else if (index == 54) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 55, 1);
        }
        else if (index == 55) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 56, 1);
        }
        // 1일차 밤 주문 리스트
        else if (index == 57) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 58, 1);
        }
        else if (index == 58) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 59, 1);
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
        else if (index == 99) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 100, 1);
        }
        else if (index == 100) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 101, 1);
        }
        else if (index == 101) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 102, 1);
        }
        else if (index == 102) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 103, 1);
        }
        else if (index == 103) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 104, 1);
        }
        //2일차 밤 주문 리스트
        else if (index == 105) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 106, 1);
        }
        else if (index == 116) // 밤 주문 1건 애쉬
        {
            Debug.Log("밤 주문 1건 애쉬");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 117, 1);
        }
        else if (index == 130) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 131, 1);
        }
        else if (index == 131) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 132, 1);
        }

        //3일차 낮 주문 리스트
        else if (index == 164) // 낮 주문 2건 코코,닉시
        {
            Debug.Log("낮 주문 2건 코코,닉시");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 165, 2);
        }
        else if (index == 202) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 203, 1);
        }
        else if (index == 203) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 204, 1);
        }
        else if (index == 204) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 205, 1);
        }
        else if (index == 205) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 206, 1);
        }
        //3일차 밤 주문 리스트
        else if (index == 207) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 208, 1);
        }
        else if (index == 212) // 밤 주문 2건 나루
        {
            Debug.Log("밤 주문 2건 나루");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 213, 2);
        }
        else if (index == 224) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 225, 1);
        }

        //4일차 낮 주문 리스트
        else if (index == 256) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 257, 1);
        }
        else if (index == 257) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 258, 1);
        }
        else if (index == 258) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 259, 1);
        }
        else if (index == 259) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 260, 1);
        }
        else if (index == 260) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 261, 1);
        }
        //4일차 밤 주문 리스트
        else if (index == 262) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 263, 1);
        }
        else if (index == 263) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 264, 1);
        }
        else if (index == 264) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 265, 1);
        }
        else if (index == 266) // 밤 주문 1건 애쉬
        {
            Debug.Log("밤 주문 1건 애쉬");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 266, 1);
        }

        //5일차 낮 주문 리스트
        else if (index == 394) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 395, 1);
        }
        else if (index == 395) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 396, 1);
        }
        else if (index == 396) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 397, 1);
        }
        else if (index == 397) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 398, 1);
        }
        else if (index == 398) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 399, 1);
        }
        //5일차 밤 주문 리스트
        else if (index == 400) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 401, 1);
        }
        else if (index == 401) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 402, 1);
        }
        else if (index == 402) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 403, 1);
        }
        else if (index == 403) // 랜덤 주문 1건
        {
            Debug.Log("밤 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 404, 1);
        }

        //6일차 낮 주문 리스트
        else if (index == 456) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 457, 1);
        }
        else if (index == 457) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 458, 1);
        }
        else if (index == 458) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 459, 1);
        }
        else if (index == 459) // 룸서비스 랜덤 1건
        {
            Debug.Log("배달 랜덤 룸서비스 주문 1건");
            //SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", 460, 1);
        }
        else if (index == 460) // 랜덤 주문 1건
        {
            Debug.Log("낮 랜덤 주문 1건");
            //SceneTransitionManager.Instance.HandleRandomMenuTransition("Ch3Scene", "CafeScene", 461, 1);
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
            ["Mr.Ham"] = Resources.Load<Sprite>("NpcImage/MrHam"),
            ["파이아"] = Resources.Load<Sprite>("NpcImage/Fire"),
            ["코코"] = Resources.Load<Sprite>("NpcImage/Coco"),
            ["닉시"] = Resources.Load<Sprite>("NpcImage/Nicksy"),
            ["나루"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["쿠라야"] = Resources.Load<Sprite>("NpcImage/Kuraya"),
            ["애쉬"] = Resources.Load<Sprite>("NpcImage/Ash"),
            ["검은 인영"] = Resources.Load<Sprite>("NpcImage/sheep"),

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

            // Mr.Ham 표정 이미지
            ["Mr.Ham_일반"] = Resources.Load<Sprite>("NpcImage/MrHam"),
            ["Mr.Ham_웃음"] = Resources.Load<Sprite>("NpcImage/MrHam_웃음"),
            ["Mr.Ham_화남"] = Resources.Load<Sprite>("NpcImage/MrHam_화남"),
            ["Mr.Ham_아쉬움"] = Resources.Load<Sprite>("NpcImage/MrHam_아쉬움"),

            // 파이아 표정 이미지
            ["파이아_일반"] = Resources.Load<Sprite>("NpcImage/Fire"),
            ["파이아_웃음"] = Resources.Load<Sprite>("NpcImage/Fire_웃음"),

            // 코코 표정 이미지
            ["코코_일반"] = Resources.Load<Sprite>("NpcImage/Coco"),
            ["코코_웃음"] = Resources.Load<Sprite>("NpcImage/Coco_웃음"),
            ["코코_놀람"] = Resources.Load<Sprite>("NpcImage/Coco_웃음"),
            ["코코_호기심"] = Resources.Load<Sprite>("NpcImage/Coco_웃음"),
            ["코코_화남"] = Resources.Load<Sprite>("NpcImage/Coco_화남"),
            ["코코_찡그림"] = Resources.Load<Sprite>("NpcImage/Coco_화남"),
            ["코코_지침"] = Resources.Load<Sprite>("NpcImage/Coco_지침"),

            // 닉시 표정 이미지
            ["닉시_일반"] = Resources.Load<Sprite>("NpcImage/Nicksy"),
            ["닉시_웃음"] = Resources.Load<Sprite>("NpcImage/Nicksy_웃음"),
            ["닉시_놀람"] = Resources.Load<Sprite>("NpcImage/Nicksy_웃음"),
            ["닉시_화남"] = Resources.Load<Sprite>("NpcImage/Nicksy_화남"),
            ["닉시_지침"] = Resources.Load<Sprite>("NpcImage/Nicksy_지침"),

            // 나루 표정 이미지
            ["나루_일반"] = Resources.Load<Sprite>("NpcImage/Naru"),
            ["나루_웃음"] = Resources.Load<Sprite>("NpcImage/Naru_웃음"),
            ["나루_화남"] = Resources.Load<Sprite>("NpcImage/Naru_화남"),
            ["나루_당황"] = Resources.Load<Sprite>("NpcImage/Naru_당황"),

            // 쿠라야 표정 이미지
            ["쿠라야_일반"] = Resources.Load<Sprite>("NpcImage/Kuraya"),
            ["쿠라야_놀람"] = Resources.Load<Sprite>("NpcImage/Kuraya"),
            ["쿠라야_웃음"] = Resources.Load<Sprite>("NpcImage/Kuraya_웃음"),
            ["쿠라야_부끄러움"] = Resources.Load<Sprite>("NpcImage/Kuraya_웃음"),
            ["쿠라야_화남"] = Resources.Load<Sprite>("NpcImage/Kuraya_화남"),
            ["쿠라야_찡그림"] = Resources.Load<Sprite>("NpcImage/Kuraya_화남"),

            // 애쉬 표정 이미지
            ["애쉬_일반"] = Resources.Load<Sprite>("NpcImage/Ash"),
            ["애쉬_놀람"] = Resources.Load<Sprite>("NpcImage/Ash"),
            ["애쉬_당황"] = Resources.Load<Sprite>("NpcImage/Ash"),
            ["애쉬_웃음"] = Resources.Load<Sprite>("NpcImage/Ash_웃음"),
            ["애쉬_슬픔"] = Resources.Load<Sprite>("NpcImage/Ash_슬픔"),
            ["애쉬_눈물"] = Resources.Load<Sprite>("NpcImage/Ash_슬픔"),
            ["애쉬_화남"] = Resources.Load<Sprite>("NpcImage/Ash_화남"),
            ["애쉬_찡그림"] = Resources.Load<Sprite>("NpcImage/Ash_화남"),

            // 기본 NPC 이미지
            ["Default"] = Resources.Load<Sprite>("NpcImage/Default")
        };

        characterBigImages = new Dictionary<string, Sprite>
        {
            ["솔"] = Resources.Load<Sprite>("NpcImage/Sol"),
            ["레이비야크"] = Resources.Load<Sprite>("NpcImage/Leviac_full"),
            ["바이올렛"] = Resources.Load<Sprite>("NpcImage/Violet_full"),
            ["러스크"] = Resources.Load<Sprite>("NpcImage/Rusk_full"),
            ["Mr.Ham"] = Resources.Load<Sprite>("NpcImage/MrHam_full"),
            ["파이아"] = Resources.Load<Sprite>("NpcImage/Fire_full"),
            ["코코"] = Resources.Load<Sprite>("NpcImage/Coco_bigfull2"),
            ["닉시"] = Resources.Load<Sprite>("NpcImage/Nicksy_bigfull2"),
            ["나루"] = Resources.Load<Sprite>("NpcImage/Naru_full"),
            ["쿠라야"] = Resources.Load<Sprite>("NpcImage/Kuraya_bigfull"),
            ["애쉬"] = Resources.Load<Sprite>("NpcImage/Ash_bigfull"),
            ["Default"] = Resources.Load<Sprite>("NpcImage/Default")
        };
    }

    public void PrintProDialogue(int index)
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
            speakerKey = currentDialogue.note;
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

        // Set big image
        if (speakerKey == "솔" || speakerKey == "검은 인영")
        {
            bigImageObj.SetActive(false);
        }
        else
        {
            if (characterBigImages.ContainsKey(speakerKey))
            {
                bigImageObj.GetComponent<Image>().sprite = characterBigImages[speakerKey];
            }
            else
            {
                bigImageObj.GetComponent<Image>().sprite = characterBigImages["Default"];
            }
            bigImageObj.SetActive(true);
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

        // 일해야 할 때 카페로 강제 이동 후 이동 가능하게 전환
        if (index == 10 || index == 98 || index == 157 || index == 255 || index == 393 || index == 455 || index == 688)
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
        // 카페 일 끝나고 이동 가능하게 전환 ->아직 아무랑도 대화 안했을때만 실행
        if (!isNpcTalkActivated && !HasTalkedToRayviyak && !HasTalkedToNaru && !HasTalkedToViolet && !HasTalkedToKuraya && !HasTalkedToRusk && !HasTalkedToMrHam && !HasTalkedToCoco && !HasTalkedToNicksy && !HasTalkedToAsh)
        {
            //1일차 밤
            if(currentDialogueIndex == 76)
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
                Npc_Coco.SetActive(false);
                Npc_Nicksy.SetActive(false);
                Npc_Naru.SetActive(false);
                Npc_Ash.SetActive(false);
                if (isCh2HappyEnding)
                {
                    Npc_Kuraya.SetActive(true);
                }
                //대화 다 안해도 침대 상호작용 가능
                EnableBedInteraction();
            }
            //2일차 밤
            if (currentDialogueIndex == 133)
            {
                player.transform.position = new Vector3(2, -3.5f, 0);
                isWaitingForPlayer = true;
                playerController.StartMove();
                map.SetActive(true);
                player.SetActive(true);
                cafe.SetActive(false);
                narration.SetActive(false);
                dialogue.SetActive(false);

                Npc_Rayviyak.SetActive(false);
                Npc_MrHam.SetActive(true);
                Npc_Violet.SetActive(true);
                Npc_Coco.SetActive(true);
                Npc_Nicksy.SetActive(true);
                Npc_Naru.SetActive(false);
                Npc_Ash.SetActive(false);
                if (isCh2HappyEnding)
                {
                    Npc_Kuraya.SetActive(true);
                }
                if (!isCh2HappyEnding)
                {
                    Npc_Rusk.SetActive(true);
                }
                //대화 다 안해도 침대 상호작용 가능
                EnableBedInteraction();
            }
            //3일차 밤
            if (currentDialogueIndex == 226)
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
                Npc_Violet.SetActive(true);
                Npc_Rusk.SetActive(true);
                Npc_Coco.SetActive(true);
                Npc_Nicksy.SetActive(true);
                Npc_Coco.transform.position = new Vector3(16.9f, -0.55f, 0); // Coco 위치 카운터 앞
                Npc_Nicksy.transform.position = new Vector3(16.1f, -0.5f, 0); // Nicksy 위치 카운터 앞
                Npc_Naru.SetActive(true);
                Npc_Ash.SetActive(false);
                if (isCh2HappyEnding)
                {
                    Npc_Kuraya.SetActive(true);
                }
                //대화 다 안해도 침대 상호작용 가능
                EnableBedInteraction();
            }
            //4일차 밤
            if (currentDialogueIndex == 356)
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
                Npc_Violet.SetActive(true);
                Npc_Rusk.SetActive(true);
                Npc_Coco.SetActive(true);
                Npc_Nicksy.SetActive(true);
                Npc_Coco.transform.position = new Vector3(-3, 0.45f, 0); // Coco 위치 무대 앞
                Npc_Nicksy.transform.position = new Vector3(-3.7f, 0.5f, 0); // Nicksy 위치 무대 앞
                Npc_Naru.SetActive(false);
                Npc_Ash.SetActive(true);
                if (isCh2HappyEnding)
                {
                    Npc_Kuraya.SetActive(true);
                }
                if (!isCh2HappyEnding)
                {
                    Npc_Kuraya.SetActive(false);
                }
                //대화 다 안해도 침대 상호작용 가능
                EnableBedInteraction();
            }
            //5일차 밤
            if (currentDialogueIndex == 418)
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
                Npc_Violet.SetActive(true);
                Npc_Rusk.SetActive(true);
                Npc_Coco.SetActive(false);
                Npc_Nicksy.SetActive(false);
                Npc_Naru.SetActive(true);
                Npc_Naru.transform.position = new Vector3(60.09f, -0.06f, 0); // Naru 위치 발코니
                Npc_Ash.SetActive(false);
                if (isCh2HappyEnding)
                {
                    Npc_Kuraya.SetActive(true);
                    Npc_Kuraya.transform.position = new Vector3(11.5f, 1.75f, 0); // Kuraya 위치 화로 앞
                }
                if (!isCh2HappyEnding)
                {
                    Npc_Kuraya.SetActive(false);
                }
                //대화 다 안해도 침대 상호작용 가능
                EnableBedInteraction();
            }
        }
        else
        {
            CheckTalk(currentDialogue.location);
        }
    }

    public void EnableBedInteraction()
    {
        bedUsed = true; // 침대 상호작용 활성화
    }
    public void DisableBedInteraction()
    {
        bedUsed = false; // 침대 상호작용 비활성화
    }

    public void ShowNarration(string speaker, string text)
    {
        narration.SetActive(true);
        dialogue.SetActive(false);
        narrationBar.SetDialogue(speaker, text);
    }
    
    public void NpcDialogue(int index, string npc)
    {
        if (isNpcTalkActivated)
        {
            //1일차 밤 Npc 대화
            if (mapManager.currentState == MapState.Garden && npc == "Npc_Rayviyak" && !HasTalkedToRayviyak && index >= 77 && index <= 80)
            {
                if (index >= 77 && index <= 79)
                {
                    PrintProDialogue(index);
                }
                else if (index == 80)
                {
                    currentDialogueIndex = 76;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRayviyak = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.Cafe && npc == "Npc_Violet" && !HasTalkedToViolet && index >= 80 && index <= 82)
            {
                if (index >= 80 && index <= 81)
                {
                    PrintProDialogue(index);
                }
                else if (index == 82)
                {
                    currentDialogueIndex = 76;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToViolet = true;
                    playerController.StartMove();
                }
            }
            else if (isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Kuraya" && !HasTalkedToKuraya && index >= 82 && index <= 88)
            {
                if (index >= 82 && index <= 87)
                {
                    PrintProDialogue(index);
                }
                else if (index == 88)
                {
                    currentDialogueIndex = 76;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToKuraya = true;
                    playerController.StartMove();
                }
            }
            else if (!isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Rusk" && !HasTalkedToRusk && index >= 88 && index <= 90)
            {
                if (index >= 88 && index <= 89)
                {
                    PrintProDialogue(index);
                }
                else if (index == 90)
                {
                    currentDialogueIndex = 76;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRusk = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.MedicalRoom &&  npc == "Npc_MrHam" && !HasTalkedToMrHam && index >= 90 && index <= 93)
            {
                if (index >= 90 && index <= 92)
                {
                    PrintProDialogue(index);
                }
                else if (index == 93)
                {
                    currentDialogueIndex = 76;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToMrHam = true;
                    playerController.StartMove();
                }
            }

            //2일차 밤 Npc 대화
            if (mapManager.currentState == MapState.Cafe && npc == "Npc_Violet" && !HasTalkedToViolet && index >= 134 && index <= 139)
            {
                if (index >= 134 && index <= 138)
                {
                    PrintProDialogue(index);
                }
                else if (index == 139)
                {
                    currentDialogueIndex = 133;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToViolet = true;
                    playerController.StartMove();
                }
            }
            else if (isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Kuraya" && !HasTalkedToKuraya && index >= 139 && index <= 147)
            {
                if (index >= 139 && index <= 146)
                {
                    PrintProDialogue(index);
                }
                else if (index == 147)
                {
                    currentDialogueIndex = 133;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToKuraya = true;
                    playerController.StartMove();
                }
            }
            else if (!isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Rusk" && !HasTalkedToRusk && index >= 147 && index <= 153)
            {
                if (index >= 147 && index <= 152)
                {
                    playerController.StopMove();
                    PrintProDialogue(index);
                }
                else if (index == 153)
                {
                    currentDialogueIndex = 133;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRusk = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.MedicalRoom && npc == "Npc_MrHam" && !HasTalkedToMrHam && index >= 153 && index <= 155)
            {
                if (index >= 153 && index <= 154)
                {
                    PrintProDialogue(index);
                }
                else if (index == 155)
                {
                    currentDialogueIndex = 133;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToMrHam = true;
                    playerController.StartMove();
                }
            }

            //3일차 밤 Npc 대화
            if (mapManager.currentState == MapState.Garden && npc == "Npc_Naru" && !HasTalkedToNaru && index >= 227 && index <= 232)
            {
                if (index >= 227 && index <= 231)
                {
                    PrintProDialogue(index);
                }
                else if (index == 232)
                {
                    currentDialogueIndex = 226;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToNaru = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.Cafe && npc == "Npc_Violet" && !HasTalkedToViolet && index >= 232 && index <= 233)
            {
                if (index == 232)
                {
                    PrintProDialogue(index);
                }
                else if (index == 233)
                {
                    currentDialogueIndex = 226;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToViolet = true;
                    playerController.StartMove();
                }
            }
            else if (isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Nicksy" && !HasTalkedToNicksy && index >= 233 && index <= 242)
            {
                if (index >= 233 && index <= 241)
                {
                    PrintProDialogue(index);
                }
                else if (index == 242)
                {
                    currentDialogueIndex = 226;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToNicksy = true;
                    playerController.StartMove();
                }
            }
            else if (!isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Coco" && !HasTalkedToCoco && index >= 242 && index <= 251)
            {
                if (index >= 242 && index <= 250)
                {
                    playerController.StopMove();
                    PrintProDialogue(index);
                }
                else if (index == 251)
                {
                    currentDialogueIndex = 226;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToCoco = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.MedicalRoom && npc == "Npc_MrHam" && !HasTalkedToMrHam && index >= 251 && index <= 253)
            {
                if (index >= 251 && index <= 252)
                {
                    PrintProDialogue(index);
                }
                else if (index == 253)
                {
                    currentDialogueIndex = 226;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToMrHam = true;
                    playerController.StartMove();
                }
            }

            //4일차 밤 Npc 대화
            if (mapManager.currentState == MapState.Garden && npc == "Npc_Rayviyak" && !HasTalkedToRayviyak && index >= 357 && index <= 363)
            {
                if (index >= 357 && index <= 362)
                {
                    PrintProDialogue(index);
                }
                else if (index == 363)
                {
                    currentDialogueIndex = 356;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRayviyak = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.Cafe && npc == "Npc_Nicksy" && !HasTalkedToNicksy && index >= 363 && index <= 371)
            {
                if (index >= 363 && index <= 370)
                {
                    PrintProDialogue(index);
                }
                else if (index == 371)
                {
                    currentDialogueIndex = 356;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToNicksy = true;
                    playerController.StartMove();
                }
            }
            else if (isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Kuraya" && !HasTalkedToKuraya && index >= 371 && index <= 384)
            {
                if (index >= 371 && index <= 383)
                {
                    PrintProDialogue(index);
                }
                else if (index == 384)
                {
                    currentDialogueIndex = 356;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToKuraya = true;
                    playerController.StartMove();
                }
            }
            else if (!isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Rusk" && !HasTalkedToRusk && index >= 384 && index <= 386)
            {
                if (index >= 384 && index <= 385)
                {
                    playerController.StopMove();
                    PrintProDialogue(index);
                }
                else if (index == 386)
                {
                    currentDialogueIndex = 356;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRusk = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.MedicalRoom && npc == "Npc_MrHam" && !HasTalkedToMrHam && index >= 387 && index <= 389)
            {
                if (index >= 387 && index <= 388)
                {
                    PrintProDialogue(index);
                }
                else if (index == 389)
                {
                    currentDialogueIndex = 356;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToMrHam = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.Balcony && npc == "Npc_Ash" && !HasTalkedToAsh && index >= 389 && index <= 391)
            {
                if (index >= 389 && index <= 390)
                {
                    PrintProDialogue(index);
                }
                else if (index == 391)
                {
                    currentDialogueIndex = 356;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToAsh = true;
                    playerController.StartMove();
                }
            }

            //5일차 밤 Npc 대화
            if (mapManager.currentState == MapState.Garden && npc == "Npc_Rayviyak" && !HasTalkedToRayviyak && index >= 419 && index <= 430)
            {
                if (index >= 419 && index <= 429)
                {
                    PrintProDialogue(index);
                }
                else if (index == 430)
                {
                    currentDialogueIndex = 418;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRayviyak = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.Cafe && npc == "Npc_Violet" && !HasTalkedToViolet && index >= 430 && index <= 431)
            {
                if (index == 430)
                {
                    PrintProDialogue(index);
                }
                else if (index == 431)
                {
                    currentDialogueIndex = 418;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToViolet = true;
                    playerController.StartMove();
                }
            }
            else if (isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Rusk" && !HasTalkedToRusk && index >= 431 && index <= 439)
            {
                if (index >= 431 && index <= 438)
                {
                    PrintProDialogue(index);
                }
                else if (index == 439)
                {
                    currentDialogueIndex = 418;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRusk = true;
                    playerController.StartMove();
                }
            }
            else if (!isCh2HappyEnding && mapManager.currentState == MapState.Bakery && npc == "Npc_Rusk" && !HasTalkedToRusk && index >= 439 && index <= 441)
            {
                if (index >= 439 && index <= 440)
                {
                    playerController.StopMove();
                    PrintProDialogue(index);
                }
                else if (index == 441)
                {
                    currentDialogueIndex = 418;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToRusk = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.MedicalRoom && npc == "Npc_MrHam" && !HasTalkedToMrHam && index >= 441 && index <= 445)
            {
                if (index >= 441 && index <= 444)
                {
                    PrintProDialogue(index);
                }
                else if (index == 445)
                {
                    currentDialogueIndex = 418;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToMrHam = true;
                    playerController.StartMove();
                }
            }
            else if (mapManager.currentState == MapState.Balcony && npc == "Npc_Naru" && !HasTalkedToNaru && index >= 445 && index <= 453)
            {
                if (index >= 445 && index <= 452)
                {
                    PrintProDialogue(index);
                }
                else if (index == 453)
                {
                    currentDialogueIndex = 418;
                    narration.SetActive(false);
                    dialogue.SetActive(false);
                    isNpcTalkActivated = false;
                    HasTalkedToNaru = true;
                    playerController.StartMove();
                }
            }
        }
        else
        {
            return;
        }
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
        switch (location)
        {
            case locationCafe:
                PlayMusic(locationCafe);
                //엔딩 때 카페 화면 끄기
                if (currentDialogueIndex==757)
                {
                    cafe.SetActive(false);
                }
                else
                {
                    cafe.SetActive(true);
                }
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
                //일해야 할 때 객실 화면 끄기
                if (currentDialogueIndex == 10 || currentDialogueIndex == 98 || currentDialogueIndex == 157 || currentDialogueIndex == 255 || currentDialogueIndex == 393 || currentDialogueIndex == 455 || currentDialogueIndex == 688)
                {
                    trainRoom.SetActive(false);
                }
                else
                {
                    trainRoom.SetActive(true);
                }
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
