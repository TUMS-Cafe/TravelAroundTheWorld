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

    private List<int> availableRooms = new List<int> { 101, 102, 201, 202, 301, 302 };
    private string drink;

 
    public void ShowRoomService()
    {
        roomService.SetActive(true);

        // 랜덤한 방 번호 선택
        int randomRoom = availableRooms[Random.Range(0, availableRooms.Count)];
        roomNumber.GetComponent<TextMeshProUGUI>().text = "-" + randomRoom.ToString() + "호-";
        drink = RandomDrinkSelector.Instance.GetRandomDrink(1);
        Debug.Log("랜덤 메뉴 선택"+  drink);

    // 음료 정보 설정
        drinkImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("CafeDrinks/"+drink);
        drinkName.GetComponent<TextMeshProUGUI>().text = drink;
    }

    public void ClickMakeDrink()
    {
        roomService.SetActive(false);
    }
}
