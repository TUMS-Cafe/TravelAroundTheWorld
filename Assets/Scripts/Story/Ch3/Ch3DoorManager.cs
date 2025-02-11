using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch3DoorManager : MonoBehaviour
{
    // 플레이어 오브젝트를 저장할 변수
    public GameObject player;

    // MapManager를 참조할 변수
    public Ch3MapManager mapManager;

    // 싱글턴 인스턴스
    public static Ch3DoorManager Instance { get; private set; }

    void Awake()
    {
        // 싱글턴 패턴 구현
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 충돌 처리 메서드
    public void HandleDoorCollision(string doorName)
    {
        // 플레이어의 현재 MapState를 가져옵니다.
        MapState playerState = mapManager.currentState;

        // 문 이름과 MapState에 따라 플레이어의 위치를 이동시킵니다.
        if (doorName == "Door302")
        {
            if (playerState == MapState.Hallway3)
            {
                // 플레이어의 y 좌표를 위쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3.7f, player.transform.position.z);
            }
            else if (playerState == MapState.TrainRoom302)
            {
                // 플레이어의 y 좌표를 아래쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 3.7f, player.transform.position.z);
            }
        }
        else if (doorName == "Door301")
        {
            if (playerState == MapState.Hallway3)
            {
                // 플레이어의 y 좌표를 위쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3.7f, player.transform.position.z);
            }
            else if (playerState == MapState.TrainRoom301)
            {
                // 플레이어의 y 좌표를 아래쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 3.7f, player.transform.position.z);
            }
        }
        else if (doorName == "Door202")
        {
            if (playerState == MapState.Hallway2)
            {
                // 플레이어의 y 좌표를 위쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3.7f, player.transform.position.z);
            }
            else if (playerState == MapState.TrainRoom202)
            {
                // 플레이어의 y 좌표를 아래쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 3.7f, player.transform.position.z);
            }
        }
        else if (doorName == "Door201")
        {
            if (playerState == MapState.Hallway2)
            {
                // 플레이어의 y 좌표를 위쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3.7f, player.transform.position.z);
            }
            else if (playerState == MapState.TrainRoom201)
            {
                // 플레이어의 y 좌표를 아래쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 3.7f, player.transform.position.z);
            }
        }
        else if (doorName == "Door102")
        {
            if (playerState == MapState.Hallway1)
            {
                // 플레이어의 y 좌표를 위쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3.7f, player.transform.position.z);
            }
            else if (playerState == MapState.TrainRoom102)
            {
                // 플레이어의 y 좌표를 아래쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 3.7f, player.transform.position.z);
            }
        }
        else if (doorName == "Door101")
        {
            if (playerState == MapState.Hallway1)
            {
                // 플레이어의 y 좌표를 위쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 3.7f, player.transform.position.z);
            }
            else if (playerState == MapState.TrainRoom101)
            {
                // 플레이어의 y 좌표를 아래쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 3.7f, player.transform.position.z);
            }
        }
        else if (doorName == "Door2")
        {
            if (playerState == MapState.Hallway3)
            {
                // 플레이어의 x 좌표를 오른쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x + 1.7f, player.transform.position.y, player.transform.position.z);
            }
            else if (playerState == MapState.Garden)
            {
                // 플레이어의 x 좌표를 왼쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x - 1.7f, player.transform.position.y, player.transform.position.z);
            }
        }
        else if (doorName == "Door7")
        {
            if (playerState == MapState.Hallway2)
            {
                // 플레이어의 x 좌표를 오른쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x + 1.7f, player.transform.position.y, player.transform.position.z);
            }
            else if (playerState == MapState.Hallway3)
            {
                // 플레이어의 x 좌표를 왼쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x - 1.7f, player.transform.position.y, player.transform.position.z);
            }
        }
        else if (doorName == "Door8")
        {
            if (playerState == MapState.Hallway1)
            {
                // 플레이어의 x 좌표를 오른쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x + 1.7f, player.transform.position.y, player.transform.position.z);
            }
            else if (playerState == MapState.Hallway2)
            {
                // 플레이어의 x 좌표를 왼쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x - 1.7f, player.transform.position.y, player.transform.position.z);
            }
        }
        else if (doorName == "Door3")
        {
            if (playerState == MapState.Garden)
            {
                // 플레이어의 x 좌표를 오른쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x + 1.7f, player.transform.position.y, player.transform.position.z);
            }
            else if (playerState == MapState.Cafe)
            {
                // 플레이어의 x 좌표를 왼쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x - 1.7f, player.transform.position.y, player.transform.position.z);
            }
        }
        else if (doorName == "Door4")
        {
            if (playerState == MapState.Cafe)
            {
                // 플레이어의 x 좌표를 오른쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x + 1.2f, player.transform.position.y, player.transform.position.z);
            }
            else if (playerState == MapState.Bakery)
            {
                // 플레이어의 x 좌표를 왼쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x - 1.2f, player.transform.position.y, player.transform.position.z);
            }
        }
        else if (doorName == "Door5")
        {
            if (playerState == MapState.Bakery)
            {
                // 플레이어의 x 좌표를 오른쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x + 1.2f, player.transform.position.y, player.transform.position.z);
            }
            else if (playerState == MapState.MedicalRoom)
            {
                // 플레이어의 x 좌표를 왼쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x - 1.2f, player.transform.position.y, player.transform.position.z);
            }
        }
        else if (doorName == "Door6")
        {
            if (playerState == MapState.MedicalRoom)
            {
                // 플레이어의 x 좌표를 오른쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x + 1.2f, player.transform.position.y, player.transform.position.z);
            }
            else if (playerState == MapState.Balcony)
            {
                // 플레이어의 x 좌표를 왼쪽으로 이동
                player.transform.position = new Vector3(player.transform.position.x - 1.2f, player.transform.position.y, player.transform.position.z);
            }
        }
    }
}