using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CafeInventoryController : MonoBehaviour
{
    public List<GameObject> inventories;  // 인벤토리 패널 (1~4번)
    public button leftArrowButton;
    public button rightArrowButton;

    private int currentInventoryIndex = 0;  // 현재 보이는 인벤토리의 인덱스 (0~3)

    void Start()
    {
        UpdateInventoryDisplay();

        leftArrowButton.onClick.AddListener(() => MoveInventory(-1));
        rightArrowButton.onClick.AddListener(() => MoveInventory(1));
    }

    // 인벤토리를 이동하는 함수
    public void MoveInventory(int direction)
    {
        int newIndex = (currentInventoryIndex + direction + inventories.Count) % inventories.Count;

        StartCoroutine(AnimateInventoryChange(currentInventoryIndex, newIndex));
        currentInventoryIndex = newIndex;
    }

    // 인벤토리 이동 애니메이션 처리
    IEnumerator AnimateInventoryChange(int oldIndex, int newIndex)
    {
        GameObject oldInventory = inventories[oldIndex];
        GameObject newInventory = inventories[newIndex];

        float duration = 0.3f; // 애니메이션 지속 시간
        float elapsedTime = 0f;
        Vector3 oldPos = oldInventory.transform.position;
        Vector3 newPos = newInventory.transform.position;

        // 이동 방향 결정 (오른쪽 or 왼쪽)
        Vector3 targetOldPos = oldPos + new Vector3(direction * -500, 0, 0);
        Vector3 targetNewPos = newPos + new Vector3(direction * -500, 0, 0);

        newInventory.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            oldInventory.transform.position = Vector3.Lerp(oldPos, targetOldPos, t);
            newInventory.transform.position = Vector3.Lerp(newPos, targetNewPos, t);

            yield return null;
        }

        oldInventory.SetActive(false);
        UpdateInventoryDisplay();
    }

    // 현재 인벤토리 상태를 업데이트
    void UpdateInventoryDisplay()
    {
        for (int i = 0; i < inventories.Count; i++)
        {
            inventories[i].SetActive(i == currentInventoryIndex);
        }
    }
}
