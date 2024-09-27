using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryTalkManager : MonoBehaviour
{
    private Dictionary<int, StoryDialogueNode> dialogueNodes = new Dictionary<int, StoryDialogueNode>();

    public GameObject narration;
    public GameObject dialogue;

    public GameObject imageObj; // 초상화 이미지
    public GameObject nameObj; // 이름
    public GameObject bigImageObj; // 큰 이미지
    public GameObject playerImageObj; // 플레이어 이미지

    public GameObject letter; // 편지지 화면
    public TextMeshProUGUI letterText;

    public GameObject player; // 플레이어 캐릭터
    public GameObject map; // 맵

    public GameObject questObject; // 퀘스트 오브젝트
    public TextMeshProUGUI questText; // 퀘스트 내용 텍스트
    private bool isQuestActive = false; // 퀘스트 오브젝트가 활성화되었는지 여부

    public GameObject cafe; // 카페 화면
    public GameObject trainRoom; // 객실 화면
    public GameObject trainRoomHallway; // 객실 복도 화면
    public GameObject garden; // 정원 화면
    public GameObject bakery; // 빵집 화면
    public GameObject medicalRoom; // 의무실 화면
    public GameObject jazzBar; // 재즈바 화면
    public GameObject balcony; // 발코니 화면

    public GameObject Npc_Rayviyak; // 정원 npc
    public GameObject Npc_MrHam; // 병원 npc
    public GameObject Npc_Rusk; // 빵집 npc
    public GameObject Npc_Violet; // 바 npc

    public GameObject cheetahShopCh0; // 치타샵 UI
    public bool isShopActive = false;

    public ScreenFader screenFader; // 페이드인/아웃 효과 스크립트
    private bool isFadingOut = false; // 페이드 아웃 중인지 여부 (페이드 아웃 중에는 입력 무시하기 위해)

    public Ch0DialogueBar dialogueBar; // 대화창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar narrationBar; // 나레이션창 스크립트 (타이핑 효과 호출을 위해)

    public bool bedUsed = false; // 침대를 사용했는지 여부

    public GameObject destPointObject;

    // 문자열 상수 선언
    private const string narrationSpeaker = "나레이션";
    private const string letterSpeaker = "편지지";
    private const string locationCafe = "카페";
    private const string locationEngineRoom = "엔진룸";
    private const string locationOtherRoom1 = "다른 방 1";
    private const string locationOtherRoom2 = "다른 방 2";
    private const string locationGarden = "정원";
    private const string locationBakery = "빵집";
    private const string locationMedicalRoom = "의무실";
    private const string locationTrainRoom = "객실";
    private const string locationJazzBar = "재즈바";

    public PlayerController playerController; // PlayerController 참조
    public Transform playerTransform; // 플레이어의 Transform 참조
    public Ch1MapManager mapManager; // Ch1MapManager 참조

    public string currentMusic = ""; // 현재 재생 중인 음악의 이름을 저장

    private Dictionary<string, Sprite> characterImages; // 캐릭터 이름과 이미지를 매핑하는 사전
    private Dictionary<string, Sprite> characterBigImages; // 캐릭터 이름과 큰 이미지를 매핑하는 사전
    private Sprite characterSprite;

    public bool isWaitingForPlayer = false; // 플레이어가 특정 위치에 도달할 때까지 기다리는 상태인지 여부

    public bool isTransition = false;

    public string speakerKey;

    // DialogueTree 객체 추가
    private DialogueTree dialogueTree;

    void Awake()
    {
        InitializeCharacterImages();
        mapManager = FindObjectOfType<Ch1MapManager>(); // MapManager 가져오기
        playerTransform = playerController.transform; // 플레이어 Transform 가져오기
        player.SetActive(false);
    }

    void Start()
    {
        LoadDialogueFromCSV();  // CSV 데이터를 읽고 노드 생성
        LinkNodesFromCSV();     // 노드를 연결

        // 루트 노드가 있는 경우 DialogueTree 초기화
        if (dialogueNodes.ContainsKey(1))
        {
            dialogueTree = new DialogueTree(dialogueNodes[1]);
        }
    }

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 시
        if (Input.GetMouseButtonDown(0))
        {
            // 현재 노드의 ID를 가져와서 전달
            int currentNodeId = dialogueTree.currentNode.nodeId; // 예시: 현재 노드의 ID를 가져옴
            HandleDialogueProgression(currentNodeId); // 매개변수로 currentNodeId를 전달
        }
    }

    void LoadDialogueFromCSV()
    {
        // CSV 파일을 읽어옴
        List<Dictionary<string, object>> data_Dialog = Ch0CSVReader.Read("Travel Around The World - CH1 (1)");

        bool isFirstRow = true;

        foreach (var row in data_Dialog)
        {
            if (isFirstRow)
            {
                isFirstRow = false;
                continue;
            }

            string dayString = row["일자"].ToString();
            int day = int.Parse(System.Text.RegularExpressions.Regex.Match(dayString, @"\d+").Value);

            int nodeId=0;
            int nextNodeId=0;

            string placeTag = row["placeTag"].ToString();
            string dayTag = row["DayTag"].ToString(); 

            StoryDialogue dialogue = new StoryDialogue(
                day,
                row["장소"].ToString(),              // location
                row["인물"].ToString(),              // speaker
                row["대사"].ToString(),              // line
                row["화면"].ToString(),              // screenEffect
                row["배경음악"].ToString(),          // backgroundMusic
                row["표정"].ToString(),              // expression
                row["비고"].ToString(),              // note
                row["퀘스트"].ToString(),            // quest
                row["퀘스트 내용"].ToString(),       // questContent
                nodeId,                              // NodeID
                nextNodeId,                          // NextNodeID
                placeTag,                            // PlaceTag
                dayTag                               // DayTag
            );

            StoryDialogueNode node = new StoryDialogueNode(dialogue, nodeId, row["장소"].ToString());
            dialogueNodes[nodeId] = node;
        }
    }

    // 노드 간 연결 작업
    void LinkNodesFromCSV()
    {
        List<Dictionary<string, object>> data_Dialog = Ch0CSVReader.Read("Travel Around The World - CH1");

        foreach (var row in data_Dialog)
        {
            if (int.TryParse(row["NodeID"].ToString(), out int nodeId) && int.TryParse(row["NextNodeID"].ToString(), out int nextNodeId))
            {
                // 현재 노드가 딕셔너리에 존재하는지 확인
                if (dialogueNodes.ContainsKey(nodeId))
                {
                    StoryDialogueNode currentNode = dialogueNodes[nodeId];

                    // 다음 노드가 딕셔너리에 존재하는지 확인
                    if (dialogueNodes.ContainsKey(nextNodeId))
                    {
                        StoryDialogueNode nextNode = dialogueNodes[nextNodeId];
                        currentNode.AddChild(nextNode); // 자식 노드로 추가
                    }
                    else
                    {
                        Debug.LogWarning($"Next node with ID {nextNodeId} not found in the dialogueNodes dictionary.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Current node with ID {nodeId} not found in the dialogueNodes dictionary.");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid NodeID or NextNodeID in the row: {row["NodeID"]}, {row["NextNodeID"]}");
            }
        }
    }

    // 현재 대화에서 다음 장소로 이동하는 로직
    public void MovePlayerToNextLocation(StoryDialogueNode currentNode)
    {
        Vector3 targetPosition = Vector3.zero;

        // 노드가 가리키는 위치로 플레이어 이동
        switch (currentNode.dialogue.location)
        {
            case "카페":
                targetPosition = new Vector3(0, 0, 0); // 카페 좌표 설정
                break;
            case "객실":
                targetPosition = new Vector3(-48, 3, 0); // 객실 좌표 설정
                break;
            case "정원":
                targetPosition = new Vector3(-20, 0, 0); // 정원 좌표 설정
                break;
            case "빵집":
                targetPosition = new Vector3(15, 0, 0); // 빵집 좌표 설정
                break;
            case "의무실":
                targetPosition = new Vector3(30, 0, 0); // 의무실 좌표 설정
                break;
            default:
                Debug.LogWarning("Unknown location: " + currentNode.dialogue.location);
                return;
        }

        // 플레이어를 해당 좌표로 이동
        playerTransform.position = targetPosition;

        // 맵 상태 업데이트
        mapManager.UpdateMapState();
    }

    // 대화를 진행하는 로직
    public void HandleDialogueProgression(int currentNodeId)
    {
        if (dialogueTree != null && dialogueTree.currentNode != null)
        {
            StoryDialogueNode currentNode = dialogueTree.currentNode;
            StoryDialogue currentDialogue = currentNode.dialogue;

            // 나레이션인지, 일반 대사인지 확인하여 처리
            if (currentDialogue.speaker == "나레이션")
            {
                // 나레이션일 경우
                narration.SetActive(true);       // 나레이션 바 활성화
                dialogue.SetActive(false);       // 대화 바 비활성화
                narrationBar.SetDialogue("나레이션", currentDialogue.line); // 나레이션 출력
            }
            else
            {
                // 일반 대사일 경우
                narration.SetActive(false);      // 나레이션 바 비활성화
                dialogue.SetActive(true);        // 대화 바 활성화
                dialogueBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 대사 출력
            }

            // 대화 후 플레이어 이동 처리
            MovePlayerToNextLocation(currentNode);

            // 다음 대화로 넘어가는 로직 추가
            if (currentNode.children.Count > 0)
            {
                // 다음 노드로 진행
                dialogueTree.MoveToNextNode(); // 첫 번째 자식 노드로 이동
            }
            else
            {
                Debug.LogWarning("No more dialogues to progress.");
            }
        }
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
}
