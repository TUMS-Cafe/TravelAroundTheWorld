using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ClickOrderList : MonoBehaviour
{
    private bool test = false;
    // 이동 될 애들
    public GameObject RoomService;
    public GameObject Bat;

    public GameObject CafeMap;

    private float speed = 2300; // 이동 속도


    private Vector3 RSHide = new Vector3 (-2000, 0, 0);
    private Vector3 RSShow = new Vector3(-300, 0, 0);
    private Vector3 BatHide = new Vector3(749, -2100, 0);
    private Vector3 BatShow = new Vector3(749, -237.5f, 0);


    private Vector3 targetPositionRoomService;
    
    private bool shouldMoveRoomService = false; // 이동을 시작할지 여부
    private Vector3 startPositionRoomService; // 시작 위치
    private float startTimeRoomService; // 이동 시작 시간
    private float journeyLengthRoomService; // 시작 위치와 목표 위치 간의 거리

    private Vector3 targetPositionBat;

    private bool shouldMoveBat = false; // 이동을 시작할지 여부
    private Vector3 startPositionBat; // 시작 위치
    private float startTimeBat; // 이동 시작 시간
    private float journeyLengthBat; // 시작 위치와 목표 위치 간의 거리

    public DeliveryData deliveryData;

    // Start is called before the first frame update
    void Start()
    {
        CafeMap = GameObject.Find("CafeMap");
        deliveryData = FindObjectOfType<DeliveryData>();
        if (deliveryData == null)
        {
            Debug.LogError("deliveryData is null!");
        }
        else
        {
            Debug.Log("deliveryData is properly assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMoveRoomService)
        {
            // 경과된 시간 계산
            float distCovered = (Time.time - startTimeRoomService) * speed;

            // Lerp를 사용하여 오브젝트를 부드럽게 이동
            

            // 목표 위치에 도달했는지 확인
            if (Vector3.Distance(RoomService.transform.localPosition, targetPositionRoomService) < 0.01f)
            {
                RoomService.transform.localPosition = targetPositionRoomService; // 목표 위치에 도달 시 위치 고정
                shouldMoveRoomService = false; // 이동 완료
            }
            else
            {
                float fractionOfJourney = distCovered / journeyLengthRoomService;
                RoomService.transform.localPosition = Vector3.Lerp(startPositionRoomService, targetPositionRoomService, fractionOfJourney);
            }

        }
        if(shouldMoveBat)
        {
            float distCovered1 = (Time.time - startTimeBat) * speed;

            

            if(Vector3.Distance(Bat.transform.localPosition, targetPositionBat) < 0.01f)
            {
                Bat.transform.localPosition = targetPositionBat;
                shouldMoveBat = false;
            }
            else
            {
                float fractionOfJourney1 = distCovered1 / journeyLengthBat;
                Bat.transform.localPosition = Vector3.Lerp(startPositionBat, targetPositionBat, fractionOfJourney1);
            }
        }
    }

    public void ClickCrafting() // 제작하기 버튼 눌렀을때
    {
        
        SoundManager.Instance.PlaySFX("click sound");
        // 한글 메뉴
        string menu = transform.parent.parent.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        
        // 영어 메뉴 찾는 과정
        foreach (menu i in OrderStruct.info)
        {
            if (i.menu_kr == menu)
            {
                menu = i.menu_name;
                break;
            }
        }
        deliveryData.deliveryOrder = menu;
        Debug.Log("current order = " + deliveryData.deliveryOrder);
        //Button button = GameObject.FindWithTag("X").GetComponent<Button>();
        //button.onClick.Invoke();
        Invoke("DestroyOrder", 1f);
       
        GameObject.Find("BeverageP").transform.Find("Beverage").transform.gameObject.SetActive(true);
        GameObject.Find("DeliveryP").transform.Find("Delivery").transform.gameObject.SetActive(false);
        CafeMap.SetActive(false);

        //menu에 음료 이름 들어가있고 한글 혹은 영어 중 편하신걸로 하시면 될 것 같아요

    }
    public void DestroyOrder()
    {
        Destroy(transform.parent.parent.gameObject);
    }
    public void ClickOrder()
    {
        SoundManager.Instance.PlaySFX("click sound");
        string roomNum = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        string totalPrice = transform.GetChild(transform.childCount-1).GetChild(0).GetComponent<TextMeshProUGUI>().text;
        Debug.Log(roomNum); Debug.Log(totalPrice);
        Debug.Log("주문서 클릭");
        GameObject.FindWithTag("RS").GetComponent<TextMeshProUGUI>().text = roomNum + "이야~ 정확히 " + totalPrice + "지불하셨어~";


    }
    public void Show()
    {
        SoundManager.Instance.PlaySFX("click sound");
        targetPositionRoomService = RSShow;
        targetPositionBat = BatShow;

        shouldMoveRoomService = true; // 이동 시작
        startPositionRoomService = RoomService.transform.localPosition; // 시작 위치 초기화
        startTimeRoomService = Time.time; // 이동 시작 시간 초기화
        journeyLengthRoomService = Vector3.Distance(startPositionRoomService, targetPositionRoomService); // 거리 계산

        shouldMoveBat = true;
        startPositionBat = Bat.transform.localPosition;
        startTimeBat = Time.time;
        journeyLengthBat = Vector3.Distance(startPositionBat, targetPositionBat);


    }

    public void Hide()
    {
        Debug.Log("Hide");
        SoundManager.Instance.PlaySFX("click sound");
        targetPositionRoomService = RSHide;
        targetPositionBat = BatHide;

        shouldMoveRoomService = true; // 이동 시작
        startPositionRoomService = RoomService.transform.localPosition; // 시작 위치 초기화
        startTimeRoomService = Time.time; // 이동 시작 시간 초기화
        journeyLengthRoomService = Vector3.Distance(startPositionRoomService, targetPositionRoomService); // 거리 계산

        shouldMoveBat = true;
        startPositionBat = Bat.transform.localPosition;
        startTimeBat = Time.time;
        journeyLengthBat = Vector3.Distance(startPositionBat, targetPositionBat);
    }
}
