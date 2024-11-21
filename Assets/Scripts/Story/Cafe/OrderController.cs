using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    private int Day = PlayerManager.Instance.GetDay();
    private bool buyMilk = PlayerManager.Instance.IsBoughtCafeItem("milk");

    public GameObject orderEspressoPrefab;
    public GameObject orderHotAmericanoPrefab;
    public GameObject orderIceAmericanoPrefab;
    public GameObject orderHotLattePrefab;
    public GameObject orderIceLattePrefab;
    public GameObject orderGreenTeaPrefab;
    public GameObject orderRooibosTeaPrefab;
    public GameObject orderHibiscusTeaPrefab;
    public GameObject orderChamomileTeaPrefab;

    public Transform orderListParent;

    public Vector3 startPosition = new Vector3(2f, 0f, -2f);
    public Vector3 offset = new Vector3(-1.35f, 0, 0);

    private List<string> generatedOrders = new List<string>();

    public DeliveryData deliveryData;
    public string deliveryOrder;

    private int randomNum = SceneTransitionManager.Instance.GetRandomMenuNum();
    private int deliveryNum = SceneTransitionManager.Instance.GetDeliveryNum();

    void OnEnable()
    {
        Debug.Log("Day = " + Day);
        Debug.Log("randomNum = " + randomNum);
        deliveryOrder = deliveryData.deliveryOrder;
        if (randomNum > 0)
        {
            GenerateOrder(randomNum);
            DisplayRandomOrders();
        }
        else
        {
            DisplayOrder();
        }

    }

    public void GenerateOrder(int randomNum)
    {
        List<string> availableKeys = new List<string>();

        // 조건에 따라 사용할 주문 목록 선택
        if (Day < 5)
        {
            availableKeys.AddRange(new string[] { "preEspresso", "preHotAmericano", "preIceAmericano" });
        }
        else if (buyMilk)
        {
            availableKeys.AddRange(new string[] { "milkEspresso", "milkHotAmericano", "milkIceAmericano", "HotLatte", "IceLatte" });
        }
        else if (!buyMilk)
        {
            availableKeys.AddRange(new string[] { "postEspresso", "postHotAmericano", "postIceAmericano", "GreenTea", "RooibosTea", "ChamomileTea", "HibiscusTea" });
        }

        // 랜덤으로 주문 생성
        for (int i = 0; i < randomNum; i++)
        {
            if (availableKeys.Count == 0)
            {
                Debug.LogWarning("No available orders to generate!");
                return;
            }

            string randomOrderKey = availableKeys[Random.Range(0, availableKeys.Count)];
            if (CafeOrderManager.Instance.TryCreateOrder(randomOrderKey))
            {
                generatedOrders.Add(randomOrderKey);
            }
            else
            {
                // 주문이 불가능한 경우(수량이 부족한 경우), 해당 키를 목록에서 제거
                availableKeys.Remove(randomOrderKey);
                randomNum++;
                // 더 이상 생성할 수 있는 주문이 없으면 종료
                if (availableKeys.Count == 0)
                {
                    Debug.LogWarning("No more available orders to generate.");
                    break;
                }
            }
        }
    }

    public void DisplayRandomOrders()
    {
        Vector3 currentPosition = startPosition;

        foreach (var order in generatedOrders)
        {
            GameObject orderPrefab = GetOrderPrefab(order);

            if (orderPrefab != null)
            {
                GameObject newOrder = Instantiate(orderPrefab, orderListParent);
                newOrder.transform.localPosition = currentPosition;
                Debug.Log($"Order {order} placed at {currentPosition}");

                // 다음 주문 아이템을 위한 위치 계산
                currentPosition += offset;
            }
            else
            {
                Debug.LogError("No prefab found for order: " + order);
            }
        }
    }

    public void DisplayOrder()
    {
        string order = null;
        if (deliveryNum > 0)
            order = deliveryOrder;
        else
            order = SceneTransitionManager.Instance.GetCafeOrders();
        GameObject orderPrefab = GetOrderPrefab(order);
        if (orderPrefab != null)
        {
            GameObject newOrder = Instantiate(orderPrefab, orderListParent);
            newOrder.transform.localPosition = startPosition;
        }

    }

    public GameObject GetOrderPrefab(string order)
    {
        switch (order)
        {
            case "Espresso":
            case "preEspresso":
            case "milkEspresso":
            case "postEspresso":
                return orderEspressoPrefab;

            case "HotAmericano":
            case "preHotAmericano":
            case "milkHotAmericano":
            case "postHotAmericano":
                return orderHotAmericanoPrefab;

            case "IceAmericano":           
            case "preIceAmericano":
            case "milkIceAmericano":
            case "postIceAmericano":
                return orderIceAmericanoPrefab;

            case "HotLatte":
                return orderHotLattePrefab;

            case "IceLatte":
                return orderIceLattePrefab;

            case "GreenTea":
                return orderGreenTeaPrefab;

            case "RooibosTea":
                return orderRooibosTeaPrefab;

            case "ChamomileTea":
                return orderChamomileTeaPrefab;

            case "HibiscusTea":
                return orderHibiscusTeaPrefab;

            default:
                return null;
        }
    }
}
