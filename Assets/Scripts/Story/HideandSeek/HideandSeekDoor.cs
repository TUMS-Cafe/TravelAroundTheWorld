using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideandSeekDoor : MonoBehaviour
{
    public string doorName; // 문 이름을 저장할 변수

    void Start()
    {
        // 오브젝트의 이름을 doorName 변수에 할당
        doorName = gameObject.name;
    }

    // 충돌 시 호출되는 메서드
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // DoorManager에게 충돌 정보를 전달
            HideandSeekDoorManager.Instance.HandleDoorCollision(doorName);
        }
    }
}
