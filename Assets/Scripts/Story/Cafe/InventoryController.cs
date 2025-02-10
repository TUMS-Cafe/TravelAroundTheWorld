using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    
    public List<GameObject> inventories;  // 인벤토리 리스트 (1~4)
    public Transform leftPosition;  // 왼쪽 인벤토리 위치
    public Transform rightPosition; // 오른쪽 인벤토리 위치
    public Button leftArrowButton;
    public Button rightArrowButton;

    private int leftIndex = 0;  // 현재 왼쪽에 위치한 인벤토리 인덱스
    private int rightIndex = 1; // 현재 오른쪽에 위치한 인벤토리 인덱스
    private int chapter = 3;

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        leftArrowButton.onClick.AddListener(() => MoveInventory(-1));
        rightArrowButton.onClick.AddListener(() => MoveInventory(1));

        // 처음에는 1번, 2번 인벤토리만 보이도록 설정
        UpdateInventoryDisplay();
    }

    // 인벤토리 이동 함수
    public void MoveInventory(int direction)
    {
        Debug.Log($"Button Clicked! Direction: {(direction == 1 ? "Right ▶" : "Left ◀")}");

        int maxIndex = GetMaxInventoryIndex();
        if (direction == 1) // ▶ 오른쪽 버튼 클릭
        {
            
            leftIndex = (leftIndex + 1) % inventories.Count;
            rightIndex = (rightIndex + 1) % inventories.Count;
        }
        else if (direction == -1) // ◀ 왼쪽 버튼 클릭
        {
            leftIndex = (leftIndex - 1 + inventories.Count) % inventories.Count;
            rightIndex = (rightIndex - 1 + inventories.Count) % inventories.Count;
        }

        Debug.Log($"New LeftIndex: {leftIndex}, RightIndex: {rightIndex}");
        UpdateInventoryDisplay();
    }

    // 인벤토리 상태 업데이트
    void UpdateInventoryDisplay()
    {
        int maxIndex = GetMaxInventoryIndex();

        // 모든 인벤토리를 비활성화
        for (int i = 0; i < inventories.Count; i++)
        {
            inventories[i].SetActive(false);
        }

        if (leftIndex <= maxIndex && rightIndex <= maxIndex)
        {
            inventories[leftIndex].SetActive(true);
            inventories[leftIndex].transform.position = leftPosition.position;

            inventories[rightIndex].SetActive(true);
            inventories[rightIndex].transform.position = rightPosition.position;
        }
    }
    int GetMaxInventoryIndex()
    {
        switch (chapter)
        {
            case 1: return 1; // 인벤토리 1, 2만 표시
            case 2: return 2; // 인벤토리 1, 2, 3 표시
            case 3: return 3; // 인벤토리 1, 2, 3, 4 표시
            default: return 1; // 기본값: 1, 2 표시
        }
    }
}