using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderController : MonoBehaviour
{
    //날짜, 우유 구매 여부를 PlayerManager에서 받아옴.
    //private int currentChapter = PlayerManager.Instance.GetDay();
    private int currentChapter = 1;

    //왼쪽 위 만들어야 할 주문을 띄워주기 위한 prefab
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

    public Transform orderSpawnPoint;
    public Vector3 offset = new Vector3(-1.7f, 0, 0);

    private List<string> generatedOrders = new List<string>();

    public DeliveryData deliveryData;
    public string deliveryOrder;

    //처리해야 할 랜덤 주문의 수
    private int randomNum = SceneTransitionManager.Instance.GetRandomMenuNum();
    private int deliveryNum = SceneTransitionManager.Instance.GetDeliveryNum();

    //addzzz
    private List<string> currentOrders = new List<string>();
    public GameObject orderPrefab;

    //addzzz
    void OnEnable()
    {
        //Debug.Log("Day = " + Day);
        //Debug.Log("randomNum = " + randomNum);
        deliveryOrder = deliveryData.deliveryOrder;
        //랜덤 주문을 하는 경우, randomNum을 이용하여 주문 생성
        if (randomNum > 0)
        {
            GenerateOrder(randomNum);
            DisplayRandomOrders();
        }
        //npc직접 주문 or delivery 주문 띄워지는 경우
        else
        {
            DisplayOrder();
        }
        

    }
    //날짜가 5일 이전이면 상점 구매 전이므로 에스프레소, 아메리카노 중에 주문 생성.
    //상점 구매 이후에는 우유 구매 여부(우유 구매X면 티백 구매함)에 따라 주문 생성)
    public void GenerateOrder(int randomNum)
    {
        List<string> availableKeys = new List<string>();

        // 조건에 따라 사용할 주문 목록 선택
        /*if (currentChapter < 5)
        {
            availableKeys.AddRange(new string[] { "preEspresso", "preHotAmericano", "preIceAmericano, " });
        }
        else if (buyMilk)
        {
            availableKeys.AddRange(new string[] { "milkEspresso", "milkHotAmericano", "milkIceAmericano", "HotLatte", "IceLatte" });
        }
        else if (!buyMilk)
        {
            availableKeys.AddRange(new string[] { "postEspresso", "postHotAmericano", "postIceAmericano", "GreenTea", "RooibosTea", "ChamomileTea", "HibiscusTea" });
        }*/
        if (currentChapter >= 0)
            availableKeys.AddRange(new string[] { "preEspresso", "preHotAmericano", "preIceAmericano,"});
        if (currentChapter >= 1)
            availableKeys.AddRange(new string[] { "HotLatte", "IceLatte", "HibiscusTea", "RooibosTea", "GreenTea", "ChamomileTea", "LunaTea" });
        if (currentChapter >= 2)
            availableKeys.AddRange(new string[] { "Affogato", "HotCaramelLatte", "IceCaramelLatte", "HotCinnamonLatte", "IceCinnamonLatte", "HotVanillaLatte", "IceVanillaLatte" });
        if (currentChapter >= 3)
            availableKeys.AddRange(new string[] { "StrawberryJuice", "MangoJuice", "BlueberryJuice", "StrawberryLatte", "MangoLatte", "BlueberryLatte", "MintLatte", "SweetPotatoLatte" });

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

    //가장 오른쪽 부분부터 음료 주문을 띄우기 시작 (start Position) 
    public void DisplayRandomOrders()
    {
        Vector3 currentPosition = orderSpawnPoint.position;

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
    List<CafeOrder> orders = new List<CafeOrder>();
   
   //addzzz 
    public void DisplayOrder()
    {
        string order = null;
        if (deliveryNum > 0)
        {
            order = deliveryOrder;
        }
        else
        {
            order = SceneTransitionManager.Instance.GetCafeOrders();
            if (string.IsNullOrEmpty(order))
            {
                Debug.LogError("Error: Order is null or empty in DisplayOrder!");
                return; // 예외 방지
            }
        }

        GameObject orderPrefab = GetOrderPrefab(order);
        if (orderPrefab != null)
        {
            GameObject newOrder = Instantiate(orderPrefab, orderListParent);
            newOrder.transform.position = orderSpawnPoint.position;
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
