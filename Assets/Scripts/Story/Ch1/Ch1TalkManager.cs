using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Ch1TalkManager : MonoBehaviour
{
    private List<Ch1ProDialogue> ch1ProDialogue;

    public GameObject narration;
    public GameObject dialogue;

    public GameObject imageObj; // 초상화 이미지
    public GameObject nameObj; // 이름
    public GameObject bigImageObj; // 큰 이미지

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

    public ScreenFader screenFader; // 페이드인/아웃 효과 스크립트
    private bool isFadingOut = false; // 페이드 아웃 중인지 여부 (페이드 아웃 중에는 입력 무시하기 위해)

    public Ch0DialogueBar dialogueBar; // 대화창 스크립트 (타이핑 효과 호출을 위해)
    public Ch0DialogueBar narrationBar; // 나레이션창 스크립트 (타이핑 효과 호출을 위해)

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

    public int currentDialogueIndex = 0; // 현재 대사 인덱스
    private bool isActivated = false; // TalkManager가 활성화되었는지 여부

    public QuestManager questManager; // 퀘스트 매니저 참조
    public PlayerController playerController; // 플레이어 컨트롤러 참조
    public Ch0MapManager mapManager; // 맵 매니저 참조
    public Ch1TriggerArea ch1TriggerArea; // Ch1TriggerArea 참조

    private Dictionary<string, Sprite> characterImages; // 캐릭터 이름과 이미지를 매핑하는 사전
    private Dictionary<string, Sprite> characterBigImages; // 캐릭터 이름과 큰 이미지를 매핑하는 사전

    public bool isWaitingForPlayer = false; // 플레이어가 특정 위치에 도달할 때까지 기다리는 상태인지 여부

    void Awake()
    {
        ch1ProDialogue = new List<Ch1ProDialogue>();
        LoadDialogueFromCSV();
        InitializeCharacterImages(); 
        mapManager = map.GetComponent<Ch0MapManager>();
        playerController = player.GetComponent<PlayerController>(); // 플레이어 컨트롤러 참조 설정
    }

    void Start()
    {
        playerController = player.GetComponent<PlayerController>(); // 플레이어 컨트롤러 참조 설정
        ActivateTalk("객실");
    }

    void Update()
    {
        if (isActivated && Input.GetKeyDown(KeyCode.Space) && !isWaitingForPlayer)
        {
            if (isQuestActive)
            {
                // 퀘스트 오브젝트, 나레이션, 다이얼로그 모두 비활성화
                questObject.SetActive(false);
                narration.SetActive(false);
                dialogue.SetActive(false);
                isQuestActive = false; // 퀘스트 비활성화 상태로 설정
            }
            else
            {
                currentDialogueIndex++;
                PrintCh1ProDialogue(currentDialogueIndex);
            }
        }

        // 플레이어가 특정 위치에 도달했는지 확인하는 부분
        if (isWaitingForPlayer && mapManager != null)
        {
            if (mapManager.currentState == MapState.Cafe && mapManager.isInCafeBarZone && currentDialogueIndex == 5)
            {
                isWaitingForPlayer = false; // 대기 상태 해제
                player.SetActive(false);
                map.SetActive(false);
                cafe.SetActive(true);
                currentDialogueIndex++; // 다음 대사로 넘어가기
                PrintCh1ProDialogue(currentDialogueIndex); // 다음 대사 출력
            }
            else if (mapManager.currentState == MapState.TrainRoom3 && currentDialogueIndex == 23) // 인덱스 23에만 실행
            {
                isWaitingForPlayer = false; // 대기 상태 해제
                player.SetActive(false);
                map.SetActive(false);
                trainRoom.SetActive(true);
                currentDialogueIndex++;
                PrintCh1ProDialogue(currentDialogueIndex); // 대사 출력
            }
            else if (mapManager.currentState == MapState.TrainRoom3 && currentDialogueIndex == 99) // 인덱스 99에만 실행
            {
                isWaitingForPlayer = false; // 대기 상태 해제
                player.SetActive(false);
                map.SetActive(false);
                trainRoom.SetActive(true);
                currentDialogueIndex++;
                PrintCh1ProDialogue(currentDialogueIndex); // 대사 출력
            }
            else if (mapManager.currentState == MapState.Cafe && mapManager.isInCafeBarZone && currentDialogueIndex == 65)
            {
                isWaitingForPlayer = false; // 대기 상태 해제
                player.SetActive(false); // 플레이어 비활성화
                map.SetActive(false); // 맵 비활성화
                cafe.SetActive(true); // 카페 활성화
                currentDialogueIndex++;
                PrintCh1ProDialogue(currentDialogueIndex); // 대사 출력
            }
            else if (mapManager.currentState == MapState.Balcony && currentDialogueIndex == 188)
            {
                isWaitingForPlayer = false; // 대기 상태 해제
                player.SetActive(false); // 플레이어 비활성화
                map.SetActive(false); // 맵 비활성화
                balcony.SetActive(true); // 발코니 활성화
                currentDialogueIndex++;
                PrintCh1ProDialogue(currentDialogueIndex); // 대사 출력
            }

            // 카페 영역을 벗어나려 할 때 경고 메시지를 표시하는 로직
            if (mapManager.currentState != MapState.Cafe && (currentDialogueIndex == 5 || currentDialogueIndex == 65))
            {
                // 플레이어를 다시 카페 영역으로 이동
                player.transform.position = new Vector3(0, 0, 0); // 카페 중앙으로 위치 재설정

                // 경고 메시지 출력
                narration.SetActive(true);
                dialogue.SetActive(false);
                narrationBar.SetDialogue("나레이션", "지금은 일할 시간이야.");
            }
        }
    }

    void LoadDialogueFromCSV()
    {
        List<Dictionary<string, object>> data_Dialog = Ch0CSVReader.Read("Travel Around The World - CH1");

        foreach (var row in data_Dialog)
        {
            string dayString = row["일자"].ToString();
            int day = int.Parse(System.Text.RegularExpressions.Regex.Match(dayString, @"\d+").Value);
            string location = row["장소"].ToString();
            string speaker = row["인물"].ToString();
            string line = row["대사"].ToString();
            string screenEffect = row["화면"].ToString();
            string backgroundMusic = row["배경음악"].ToString();
            string expression = row["표정"].ToString();
            string note = row["비고"].ToString();
            string quest = row["퀘스트"].ToString();
            string questContent = row["퀘스트 내용"].ToString();

            ch1ProDialogue.Add(new Ch1ProDialogue(day, location, speaker, line, screenEffect, backgroundMusic, expression, note, quest, questContent));

            // 퀘스트가 존재하면 QuestManager를 통해 퀘스트 저장
            if (!string.IsNullOrEmpty(quest))
            {
                QuestManager.Instance.AddQuest(quest, questContent);
            }
        }
    }

    void InitializeCharacterImages()
    {
        characterImages = new Dictionary<string, Sprite>();
        characterBigImages = new Dictionary<string, Sprite>(); // 큰 이미지 사전 초기화

        // 작은 이미지 로드
        characterImages["솔"] = Resources.Load<Sprite>("PlayerImage/Sol");
        characterImages["루카스"] = Resources.Load<Sprite>("NpcImage/Lucas");
        characterImages["슬로우"] = Resources.Load<Sprite>("NpcImage/Slow");
        characterImages["가이"] = Resources.Load<Sprite>("NpcImage/Gai");
        characterImages["레이비야크"] = Resources.Load<Sprite>("NpcImage/Leviac");
        characterImages["바이올렛"] = Resources.Load<Sprite>("NpcImage/Violet");
        characterImages["파이아"] = Resources.Load<Sprite>("NpcImage/Fire");

        // 큰 이미지 로드
        characterBigImages["루카스"] = Resources.Load<Sprite>("NpcImage/Lucas_big");
        characterBigImages["슬로우"] = Resources.Load<Sprite>("NpcImage/Slow_big");
        characterBigImages["가이"] = Resources.Load<Sprite>("NpcImage/Gai_big");
    }

    public void PrintCh1ProDialogue(int index)
    {
        if (index >= ch1ProDialogue.Count)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            return;
        }

        Ch1ProDialogue currentDialogue = ch1ProDialogue[index];

        Sprite characterSprite = characterImages.ContainsKey(currentDialogue.speaker) ? characterImages[currentDialogue.speaker] : Resources.Load<Sprite>("NpcImage/Default");

        // 작은 이미지 설정
        if (imageObj.GetComponent<SpriteRenderer>() != null)
        {
            imageObj.GetComponent<SpriteRenderer>().sprite = characterSprite;
        }
        else if (imageObj.GetComponent<Image>() != null)
        {
            imageObj.GetComponent<Image>().sprite = characterSprite;
        }

        // 큰 이미지 설정
        Sprite bigCharacterSprite = characterBigImages.ContainsKey(currentDialogue.speaker) ? characterBigImages[currentDialogue.speaker] : null;
        if (bigImageObj != null && bigCharacterSprite != null)
        {
            bigImageObj.GetComponent<Image>().sprite = bigCharacterSprite;
            bigImageObj.SetActive(true); // 큰 이미지 활성화
        }
        else
        {
            bigImageObj.SetActive(false); // 큰 이미지 비활성화
        }

        if (currentDialogue.speaker == letterSpeaker)
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
            if (!string.IsNullOrEmpty(letterText.text))
            {
                letterText.text += "\n";
            }
            letterText.text += currentDialogue.line;
        }
        else if (string.IsNullOrEmpty(currentDialogue.speaker) && string.IsNullOrEmpty(currentDialogue.location))
        {
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        else if ((currentDialogue.speaker == narrationSpeaker) || string.IsNullOrEmpty(currentDialogue.speaker))
        {
            narration.SetActive(true);
            dialogue.SetActive(false);
            narrationBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }
        else
        {
            narration.SetActive(false);
            dialogue.SetActive(true);
            dialogueBar.SetDialogue(currentDialogue.speaker, currentDialogue.line); // 타이핑 효과 적용
        }

        // 인덱스 33에서 퀘스트 오브젝트 활성화 및 퀘스트 내용 출력
        if (index == 32)
        {
            string quest = currentDialogue.quest; // CSV에서 읽어온 퀘스트 이름
            string questContent = currentDialogue.questContent; // CSV에서 읽어온 퀘스트 내용

            questText.text = $"{quest}\n\n{questContent}"; // 퀘스트 이름과 내용을 퀘스트 텍스트에 출력
            questObject.SetActive(true); // 퀘스트 오브젝트 활성화

            // Map, Player, NPC 비활성화
            map.SetActive(false);
            player.SetActive(false);
            Npc_Rayviyak.SetActive(false);

            isQuestActive = true; // 퀘스트 활성화 상태로 설정
        }
        else if (index == 197)
        {
            balcony.SetActive(false);
            cheetahShopCh0.SetActive(true); // CheetahShop Ch0 UI 활성화
        }
        else if (index == 198)
        {
            balcony.SetActive(true);
            cheetahShopCh0.SetActive(false);
        }
        // 인덱스 62에서 player의 위치를 TrainRoom3로 이동
        else if (index == 62)
        {
            player.transform.position = new Vector3(-44.5f, 9f, 0f);
            mapManager.currentState = MapState.TrainRoom3; // 맵 상태를 TrainRoom3로 변경
        }
        else if (index == 65)
        {
            player.transform.position = new Vector3(0, 0, 0);
            mapManager.currentState = MapState.Cafe; // 맵 상태를 Cafe로 변경
            isWaitingForPlayer = true; // 대기 상태 해제
            player.SetActive(true); // 플레이어 활성화
            map.SetActive(true); // 맵 활성화
            EnablePlayerMovement(); // 플레이어 이동 가능하게 설정
            trainRoom.SetActive(false); // 객실 비활성화
            narration.SetActive(false); // 나레이션 비활성화
            dialogue.SetActive(false); // 대화창 비활성화
        }
        else if (index == 5) // "카페로 가자" 대사 이후
        {
            player.transform.position = new Vector3(0, 0, 0);
            mapManager.currentState = MapState.Cafe; // 맵 상태를 Cafe로 변경
            isWaitingForPlayer = true; // 대기 상태 해제
            player.SetActive(true); // 플레이어 활성화
            map.SetActive(true); // 맵 활성화
            EnablePlayerMovement(); // 플레이어 이동 가능하게 설정
            trainRoom.SetActive(false); // 객실 비활성화
            narration.SetActive(false); // 나레이션 비활성화
            dialogue.SetActive(false); // 대화창 비활성화
        }
        else if (index == 99 && mapManager.currentState == MapState.Cafe) // 인덱스 99 이후의 로직
        {
            isWaitingForPlayer = true; // 플레이어가 특정 위치에 도달할 때까지 대기
            EnablePlayerMovement();
            map.SetActive(true);
            player.SetActive(true);
            cafe.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        else if (index == 188) // 인덱스 188 이후의 로직
        {
            player.transform.position = new Vector3(0, 0, 0);
            mapManager.currentState = MapState.Cafe; // 맵 상태를 Cafe로 변경
            isWaitingForPlayer = true; // 플레이어가 특정 위치에 도달할 때까지 대기
            EnablePlayerMovement();
            map.SetActive(true);
            player.SetActive(true);
            cafe.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        else if (index == 23 && mapManager.currentState == MapState.Cafe) // 인덱스 23 이후의 로직
        {
            isWaitingForPlayer = true; // 플레이어가 특정 위치에 도달할 때까지 대기
            EnablePlayerMovement();
            map.SetActive(true);
            player.SetActive(true);
            cafe.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(false);
        }
        else if (index == 33 && mapManager.currentState == MapState.TrainRoom3)
        {
            isWaitingForPlayer = true; // 플레이어가 특정 위치에 도달할 때까지 대기
            EnablePlayerMovement();
            map.SetActive(true);
            player.SetActive(true);
            trainRoom.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(false);
            Npc_Rayviyak.SetActive(true);
        }
        else if (index == 36 && mapManager.currentState == MapState.Garden)
        {
            isWaitingForPlayer = true; // 플레이어가 특정 위치에 도달할 때까지 대기
            EnablePlayerMovement();
            map.SetActive(true);
            player.SetActive(true);
            garden.SetActive(false);
            narration.SetActive(false);
            dialogue.SetActive(false);
            Npc_Violet.SetActive(true);
        }
        else
        {
            CheckTalk(currentDialogue.location);
        }
    }

    public void OnDialogueButtonClicked(int index)
    {
        // 전달된 인덱스를 사용하여 대화 시작
        currentDialogueIndex = index;

        if (currentDialogueIndex == 33)
        {
            map.SetActive(false);
            player.SetActive(false);
            Npc_Rayviyak.SetActive(false);
            garden.SetActive(true);
            isWaitingForPlayer = false; // 대기 상태 해제
            PrintCh1ProDialogue(currentDialogueIndex);
        }
        else if (currentDialogueIndex == 36)
        {
            map.SetActive(false);
            player.SetActive(false);
            Npc_Violet.SetActive(false);
            jazzBar.SetActive(true);
            isWaitingForPlayer = false; // 대기 상태 해제
            PrintCh1ProDialogue(currentDialogueIndex);
        }
        else
        {
            // 다른 인덱스에 대해서도 기본적인 대화 진행을 수행하도록 처리
            PrintCh1ProDialogue(currentDialogueIndex);
        }
    }

    public void ActivateTalk(string locationName)
    {
        this.gameObject.SetActive(true);
        isActivated = true;

        // locationName에 따라 인덱스 조정하여 특정 대화를 시작할 수 있도록 수정
        currentDialogueIndex = ch1ProDialogue.FindIndex(dialogue => dialogue.location == locationName);

        if (currentDialogueIndex >= 0)
        {
            PrintCh1ProDialogue(currentDialogueIndex);
        }
    }

    public void DeactivateTalk()
    {
        this.gameObject.SetActive(false);
        isActivated = false;
    }

    public void CheckTalk(string location)
    {
        letter.SetActive(false);
        cafe.SetActive(false);
        trainRoom.SetActive(false);
        trainRoomHallway.SetActive(false);
        garden.SetActive(false);
        bakery.SetActive(false);
        medicalRoom.SetActive(false);
        letter.SetActive(false);
        jazzBar.SetActive(false);

        switch (location)
        {
            case locationTrainRoom:
                trainRoom.SetActive(true);
                if (currentDialogueIndex == 24)
                {
                    StartCoroutine(screenFader.FadeIn(letter));
                }
                else if (currentDialogueIndex >= 25 && currentDialogueIndex <= 29)
                {
                    letter.SetActive(true);
                    if (currentDialogueIndex >= 25 && currentDialogueIndex <= 28)
                    {
                        letterText.gameObject.SetActive(true);
                    }
                    else if (currentDialogueIndex >= 25)
                    {
                        letter.gameObject.SetActive(true);
                    }
                    if (currentDialogueIndex == 29)
                    {
                        StartCoroutine(screenFader.FadeOut(letter));
                    }
                }
                break;

            case locationMedicalRoom:
                medicalRoom.SetActive(true);
                break;

            case locationGarden:
                garden.SetActive(true);
                break;

            case locationBakery:
                bakery.SetActive(true);
                break;

            case locationJazzBar:
                jazzBar.SetActive(true);
                break;

            case locationCafe:
                cafe.SetActive(true);
                break;
        }

        if (currentDialogueIndex > ch1ProDialogue.Count)
        {
            DeactivateTalk();
        }
    }

    public void EnablePlayerMovement()
    {
        playerController.StartMove(); // 플레이어 이동 활성화
    }

    public void DisablePlayerMovement()
    {
        playerController.StopMove(); // 플레이어 이동 비활성화
    }

    private IEnumerator FadeOutAndDeactivateTalk(GameObject obj)
    {
        isFadingOut = true; // 페이드아웃 시작
        yield return StartCoroutine(screenFader.FadeOut(obj)); // FadeOut이 완료될 때까지 기다립니다.
        narration.SetActive(false);
        dialogue.SetActive(false);
        DeactivateTalk(); // FadeOut이 완료된 후 대화 비활성화
        isFadingOut = false; // 페이드아웃 종료
    }
}
