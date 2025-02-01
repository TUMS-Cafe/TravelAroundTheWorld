using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStopPlayer : MonoBehaviour
{
    private string playerTag = "Player"; // 플레이어의 태그 (기본값: "Player")
    private Ch2TalkManager talkManager; // Ch2TalkManager 참조

    private void Start()
    {
        // 씬 내에서 Ch2TalkManager 찾기
        talkManager = FindObjectOfType<Ch2TalkManager>();

        if (talkManager == null)
        {
            Debug.LogError("[오류] Ch2TalkManager를 찾을 수 없습니다!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[디버그] {other.gameObject.name} (플레이어)가 {gameObject.name} (NPC)과 충돌 → 이동 정지");

            // 플레이어의 이동 차단
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false; // 플레이어 이동 불가
            }

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero; // 즉시 정지
                rb.constraints = RigidbodyConstraints2D.FreezePosition; // 이동 완전 차단
            }
            talkManager.isInputDisabled = false;

            //Ch2TalkManager의 isInputDisabled 해제
            if (talkManager != null)
            {
                talkManager.isInputDisabled = false;
                Debug.Log("[디버그] 플레이어가 NPC와 접촉 → isInputDisabled = false");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log($"[디버그] {other.gameObject.name} (플레이어)가 {gameObject.name} (NPC)에서 벗어남 → 이동 가능");

            // 플레이어의 이동 다시 가능하게 설정
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = true; // 다시 이동 가능
            }

            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation; // 이동 가능 (회전만 고정)
            }
        }
    }
}
