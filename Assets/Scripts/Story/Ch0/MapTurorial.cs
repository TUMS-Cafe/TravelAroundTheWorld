using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTurorial : MonoBehaviour
{
    public ScreenFader screenFader; // 페이드인/아웃 효과 스크립트
    public TalkManager talkManager; // 토크 매니저 스크립트

    public GameObject mapTutorial_1;
    public GameObject mapTutorial_2;
    public GameObject mapTutorial_3;

    public GameObject player;
    public PlayerController playerController;
    public GameObject map;
    public GameObject mapManager;
    public Ch0MapManager ch0MapManager;

    public List<GameObject> npcs; // NPC 오브젝트를 참조하기 위한 리스트
    public int activeNpcCount = 0; // 활성화된 NPC의 수

    private int spaceBarIndex = 0;

    private bool engineRoomAccessed = false; // 엔진룸 접근 여부
    private bool trainRoom1Accessed = false; // 다른방1 접근 여부
    private bool trainRoom2Accessed = false; // 다른방2 접근 여부
    public bool isEngineRoom = false;
    public bool isOtherRoom1 = false;
    public bool isOtherRoom2 = false;
    public bool bedUsed = false; // 침대를 사용했는지 여부
    public bool isSleeping = false; // 잠에 드는지 여부

    //문자열 상수 선언
    private const string EngineRoom = "엔진룸";
    private const string OtherRoom1 = "다른 방 1";
    private const string OtherRoom2 = "다른 방 2";
    private const string TrainRoom3 = "객실";

    // Start is called before the first frame update
    void Start()
    {
        mapTutorial_1.SetActive(false);
        mapTutorial_2.SetActive(false);
        mapTutorial_3.SetActive(false);
        mapManager.SetActive(true);
        StartCoroutine(screenFader.FadeIn(map));  // 맵 활성화 (맵 완료되면 코드 수정 필요)
        StartCoroutine(screenFader.FadeIn(mapTutorial_1)); // 맵 튜토리얼1 창 활성화
        //player.SetActive(true); //플레이어 오브젝트 활성화
        // player의 SpriteRenderer의 Order in Layer를 7로 설정
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.sortingOrder = 7;
        }
        //playerController.StopMove(); //튜토리얼 창 뜰때 플레이어 움직임 멈춤
        ActivateNPC(true);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            spaceBarIndex++;
            Tutorial();
        }

        int talkActivedNPC = CheckAllNpcTalkStatus();

        if (activeNpcCount == talkActivedNPC)
        {
            talkManager.isAllNPCActivated = true;
        }

        CheckMapState();
    }

    void Tutorial()
    {
        if (spaceBarIndex == 1)
        {
            mapTutorial_1.SetActive(false);
            StartCoroutine(screenFader.FadeIn(mapTutorial_2)); // 처음 스페이스바를 누르면 맵 튜토리얼2 창 활성화
        }
        else if (spaceBarIndex == 2)
        {
            mapTutorial_2.SetActive(false);
            StartCoroutine(screenFader.FadeIn(mapTutorial_3)); //세번째 스페이스바를 누르면 맵 튜토리얼2 창 페이드아웃
        }
        else if (spaceBarIndex == 3)
        {
            StartCoroutine(screenFader.FadeOut(mapTutorial_3)); //세번째 스페이스바를 누르면 맵 튜토리얼2 창 페이드아웃
            playerController.StartMove(); //튜토리얼 창 꺼질때 플레이어 움직임 재개
        }
    }

    // 모든 NPC를 활성화 또는 비활성화
    private void ActivateNPC(bool isActive)
    {
        foreach (GameObject npc in npcs)
        {
            npc.SetActive(isActive); // NPC 오브젝트 활성화 또는 비활성화
            activeNpcCount++;
        }
    }

    // 모든 NPC의 talkActivated 상태를 확인
    private int CheckAllNpcTalkStatus()
    {
        int talkActivedCount = 0; // talkActived가 true인 NPC의 수

        foreach (GameObject npc in npcs)
        {
            if (npc.activeInHierarchy) // NPC가 활성화되어 있는 경우
            {
                var triggerArea = npc.GetComponent<Ch0TriggerArea>();
                if (triggerArea != null)
                {
                    if (triggerArea.talkActived) // talkActived가 true인 경우
                    {
                        talkActivedCount++; // talkActived가 true인 NPC의 수 증가
                    }
                }
            }
        }

        return talkActivedCount;
    }

    void CheckMapState()
    {
        if (isEngineRoom)
        {
            if (!engineRoomAccessed)
            {
                playerController.StopMove(); //대사 나올때 플레이어 움직임 멈춤
                talkManager.ActivateTalk(EngineRoom); //엔진룸 처음 접근 시 대사 출력 
                engineRoomAccessed = true;
            }
        }
        if (isOtherRoom1)
        {
            if (!trainRoom1Accessed)
            {
                playerController.StopMove(); //대사 나올때 플레이어 움직임 멈춤
                talkManager.ActivateTalk(OtherRoom1); //다른방1 처음 접근 시 대사 출력 
                trainRoom1Accessed = true;
            }
        }
        if (isOtherRoom2)
        {
            if (!trainRoom2Accessed)
            {
                playerController.StopMove(); //대사 나올때 플레이어 움직임 멈춤
                talkManager.ActivateTalk(OtherRoom2); //다른방2 처음 접근 시 대사 출력 
                trainRoom2Accessed = true;
            }
        }
        //현재 위치가 객실이고 침대를 아직 사용하지 않은 상태에서 잠에 들려고 할떄
        if (ch0MapManager.currentState == MapState.TrainRoom302 && !bedUsed && isSleeping)
        {
            playerController.StopMove(); //대사 나올때 플레이어 움직임 멈춤
            talkManager.ActivateTalk(TrainRoom3); // 잠에 들려고 할 시 대사 출력
            bedUsed = true; //침대 사용
        }
    }
}
