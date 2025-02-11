using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapState
{
    Null,
    MechanicalRoom,
    EngineRoom,
    Hallway,
    TrainRoom3,
    TrainRoom101,
    TrainRoom102,
    Hallway1,
    TrainRoom201,
    TrainRoom202,
    Hallway2,
    TrainRoom301,
    TrainRoom302,
    Hallway3,
    Garden,
    Cafe,
    Bakery,
    MedicalRoom,
    Balcony
}

public class Ch0MapManager : MonoBehaviour
{
    public MapState currentState; // 맵의 현재 상태
    public Vector2 playerPosition; // 플레이어의 현재 위치
    public Transform playerTransform; // 플레이어의 Transform 참조

    private Dictionary<string, Bounds> cafeSubZones; // 카페바 구역 지정
    public bool isInCafeBarZone = false; // 카페바 존에 있는지 여부

    public TalkManager talkManager;
    //private string currentMusic = ""; // 현재 재생 중인 음악 이름

    void Start()
    {
        currentState = MapState.Null; // 초기 상태 설정
        if (playerTransform != null)
        {
            playerPosition = playerTransform.position; // 초기 플레이어 위치 설정
        }
        else
        {
            Debug.LogError("Player Transform is not assigned!");
        }

        InitializeCafeSubZones(); // 카페 내 존 초기화
    }

    void Update()
    {
        // 플레이어 위치 업데이트
        UpdatePlayerPosition();

        // 플레이어 위치에 따른 맵 상태 업데이트
        UpdateMapState();

        PlayMapMusic();
    }

    void UpdatePlayerPosition()
    {
        if (playerTransform != null)
        {
            playerPosition = playerTransform.position;
        }
        else
        {
            Debug.LogError("Player Transform is not assigned!");
        }
    }

    void InitializeCafeSubZones()
    {
        cafeSubZones = new Dictionary<string, Bounds>();

        Vector3 cafeBarPosition = new Vector3(6.2f, -4f, 0f); // cafebar의 위치
        Vector3 cafeBarSize = new Vector3(3f, 1.7f, 1f); // cafebar의 스케일

        cafeSubZones["CafeBar"] = new Bounds(cafeBarPosition, cafeBarSize);
    }

    void UpdateMapState()
    {
        if (playerPosition.x >= -126.1 && playerPosition.x <= -106.1 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.MechanicalRoom;
        }
        else if (playerPosition.x >= -106.1 && playerPosition.x <= -86.1 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.EngineRoom;
        }
        else if (playerPosition.x >= -85.5 && playerPosition.x <= -77.5 && playerPosition.y >= 1.8f && playerPosition.y <= 9.8f)
        {
            currentState = MapState.TrainRoom101;
        }
        else if (playerPosition.x >= -76.2 && playerPosition.x <= -68.2 && playerPosition.y >= 1.8f && playerPosition.y <= 9.8f)
        {
            currentState = MapState.TrainRoom102;
        }
        else if (playerPosition.x >= -66.4 && playerPosition.x <= -58.4 && playerPosition.y >= 1.8f && playerPosition.y <= 9.8f)
        {
            currentState = MapState.TrainRoom201;
        }
        else if (playerPosition.x >= -56.9 && playerPosition.x <= -48.9 && playerPosition.y >= 1.8f && playerPosition.y <= 9.8f)
        {
            currentState = MapState.TrainRoom202;
        }
        else if (playerPosition.x >= -47.2 && playerPosition.x <= -39.2 && playerPosition.y >= 1.8f && playerPosition.y <= 9.8f)
        {
            currentState = MapState.TrainRoom301;
        }
        else if (playerPosition.x >= -37.7 && playerPosition.x <= -29.7 && playerPosition.y >= 1.8f && playerPosition.y <= 9.8f)
        {
            currentState = MapState.TrainRoom302;
        }
        else if (playerPosition.x >= -86.1 && playerPosition.x <= -67.2 && playerPosition.y >= -5f && playerPosition.y <= 1.8f)
        {
            currentState = MapState.Hallway1;
        }
        else if (playerPosition.x >= -67.2 && playerPosition.x <= -48 && playerPosition.y >= -5f && playerPosition.y <= 1.8f)
        {
            currentState = MapState.Hallway2;
        }
        else if (playerPosition.x >= -48 && playerPosition.x <= -28.8 && playerPosition.y >= -5f && playerPosition.y <= 1.8f)
        {
            currentState = MapState.Hallway3;
        }
        else if (playerPosition.x >= -28.8 && playerPosition.x <= -9.6 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.Garden;
        }
        else if (playerPosition.x >= -9.6 && playerPosition.x <= 9.6 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.Cafe;

            // 카페바 존에 있는지 확인
            Bounds cafeBarBounds = cafeSubZones["CafeBar"];
            if (cafeBarBounds.Contains(playerPosition))
            {
                isInCafeBarZone = true;
            }
            else { isInCafeBarZone = false; }
        }
        else if (playerPosition.x >= 9.6 && playerPosition.x <= 28.8 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.Bakery;
        }
        else if (playerPosition.x >= 28.8 && playerPosition.x <= 48 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.MedicalRoom;
        }
        else if (playerPosition.x >= 48 && playerPosition.x <= 67.2 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.Balcony;
        }
        else
        {
            currentState = MapState.Null;
        }
    }

    public void PlayMapMusic()
    {
        string newMusic = "";

        switch (currentState)
        {
            case MapState.MechanicalRoom:
                newMusic = "";
                break;
            case MapState.EngineRoom:
                newMusic = "";
                break;
            case MapState.TrainRoom101:
                newMusic = "a room";
                break;
            case MapState.TrainRoom102:
                newMusic = "a room";
                break;
            case MapState.TrainRoom201:
                newMusic = "a room";
                break;
            case MapState.TrainRoom202:
                newMusic = "a room";
                break;
            case MapState.TrainRoom301:
                newMusic = "a room";
                break;
            case MapState.TrainRoom302:
                newMusic = "a room";
                break;
            case MapState.Hallway1:
                newMusic = "a room";
                break;
            case MapState.Hallway2:
                newMusic = "a room";
                break;
            case MapState.Hallway3:
                newMusic = "a room";
                break;
            case MapState.Garden:
                newMusic = "GARDEN";
                break;
            case MapState.Cafe:
                newMusic = "CAFE";
                break;
            case MapState.Bakery:
                newMusic = "BAKERY";
                break;
            case MapState.MedicalRoom:
                newMusic = "amedicaloffice_001";
                break;
            case MapState.Balcony:
                newMusic = "";
                break;
        }

        // 새로운 음악이 없는 경우 이전 음악을 멈춤
        if (string.IsNullOrEmpty(newMusic))
        {
            if (!string.IsNullOrEmpty(talkManager.currentMusic))
            {
                SoundManager.Instance.StopMusic();
                talkManager.currentMusic = ""; // 현재 재생 중인 음악 이름을 초기화
            }
        }
        else if (talkManager.currentMusic != newMusic)
        {
            SoundManager.Instance.PlayMusic(newMusic, loop: true);
            talkManager.currentMusic = newMusic; // 현재 재생 중인 음악 이름을 업데이트
        }
    }
}