using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideandSeekMapManager : MonoBehaviour
{
    public MapState currentState; // 맵의 현재 상태
    public Vector2 playerPosition; // 플레이어의 현재 위치
    public Transform playerTransform; // 플레이어의 Transform 참조

    private Dictionary<string, Bounds> cafeSubZones; // 카페바 구역 지정
    public bool isInCafeBarZone = false; // 카페바 존에 있는지 여부

    //public Ch1TalkManager talkManager;
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
        Vector3 cafeBarSize = new Vector3(6f, 3f, 1f); // cafebar의 스케일

        cafeSubZones["CafeBar"] = new Bounds(cafeBarPosition, cafeBarSize);
    }

    public void UpdateMapState()
    {
        if (playerPosition.x >= -88 && playerPosition.x <= -68 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.MechanicalRoom;
        }
        else if (playerPosition.x >= -68 && playerPosition.x <= -48 && playerPosition.y >= -5f && playerPosition.y <= 5f)
        {
            currentState = MapState.EngineRoom;
        }
        else if (playerPosition.x >= -48 && playerPosition.x <= -38.8 && playerPosition.y >= 1.8f && playerPosition.y <= 9.8f)
        {
            currentState = MapState.TrainRoom3;
        }
        else if (playerPosition.x >= -48 && playerPosition.x <= -28.8 && playerPosition.y >= -5f && playerPosition.y <= 1.8f)
        {
            currentState = MapState.Hallway;
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

        //Debug.Log("Current State: " + currentState);
    }

    public void PlayMapMusic()
    {
        string newMusic = "";

        switch (currentState)
        {
            case MapState.MechanicalRoom:
                break;
            case MapState.EngineRoom:
                break;
            case MapState.TrainRoom3:
                newMusic = "a room";
                break;
            case MapState.Hallway:
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
                break;
        }
    }
}
