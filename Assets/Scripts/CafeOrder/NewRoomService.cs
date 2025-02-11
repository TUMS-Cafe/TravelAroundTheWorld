using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewRoomService : MonoBehaviour
{

    public GameObject roomService;
    public GameObject roomNumber;
    public GameObject drinkImg;
    public GameObject drinkName;
    public Ch1TalkManager talkManager1;
    public Ch3TalkManager talkManager3;


    private List<int> availableRooms = new List<int> { 101, 102, 201, 202, 301 };
    private string drink = "Espresso";
    private string korName = "Espresso";


    public void ShowRoomService()
    {
        roomService.SetActive(true);

        // 랜덤한 방 번호 선택
        int randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
        roomNumber.GetComponent<TextMeshProUGUI>().text = "-" + randomRoom.ToString() + "호-";
        drink = RandomDrinkSelector.Instance.GetRandomDrink(1);
        korName = RandomDrinkSelector.Instance.ChangeDrinkName(drink);
        Debug.Log("랜덤 메뉴 선택"+  korName);

        // 음료 정보 설정
        drinkImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("CafeDrinks/"+ korName);
        drinkName.GetComponent<TextMeshProUGUI>().text = korName;
    }

    public void ClickMakeDrink()
    {
        //Transform drinkNameTransform = transform.Find("RoomService 1/OrderSection/DrinkName");
        Debug.Log(transform.name);
        drink = RandomDrinkSelector.Instance.ChangeDrinkNameToEnglish(transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text); 
        List<CafeOrder> orders = new List<CafeOrder>();
        orders.Add(new CafeOrder(drink));
        //Debug.Log("현재 씬 이름 확인 " + PlayerManager.Instance.GetSceneName());
        //Debug.Log("Ch1이랑 같은지 확인 "+ (PlayerManager.Instance.GetSceneName() == "Ch1"));
        //Debug.Log("음료 이름 확인 " + drink);
        if (PlayerManager.Instance.GetSceneName() == "Ch1")
        {
            SceneTransitionManager.Instance.HandleDialogueTransition("ch1Scene", "CafeScene", talkManager1.currentDialogueIndex, orders);
        }
        else if (PlayerManager.Instance.GetSceneName() == "Ch2") 
        {
            SceneTransitionManager.Instance.HandleDialogueTransition("Ch2Scene", "CafeScene", talkManager1.currentDialogueIndex, orders); // 수정 필요
        }
        else if (PlayerManager.Instance.GetSceneName() == "Ch3")
        {
            SceneTransitionManager.Instance.HandleDialogueTransition("Ch3Scene", "CafeScene", talkManager3.currentDialogueIndex+1, orders);
        }

    }
}
