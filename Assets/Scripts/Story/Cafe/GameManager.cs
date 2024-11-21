using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Shot;
    public GameObject Extract;


    public GameObject Beverage;
    public GameObject CafeMap;
    public GameObject Delivery;
    public GameObject RecipeBook;

    public GameObject Milk;
    public GameObject TeaInventory;

    private bool buyMilk = PlayerManager.Instance.IsBoughtCafeItem("milk");
    private bool buyTeaSet = PlayerManager.Instance.IsBoughtCafeItem("teaSet");

    private int Day = PlayerManager.Instance.GetDay();


    public OrderController orderController;

    void Start()
    {
        SoundManager.Instance.PlayMusic("CAFE", true);

        if (buyMilk)
        {
            Milk.SetActive(true);
        }
        else if (buyTeaSet)
        {
            TeaInventory.SetActive(true);
        }

        int deliveryNum = SceneTransitionManager.Instance.GetDeliveryNum();

        if (deliveryNum != null && deliveryNum > 0)
        {
            Debug.Log("deliveryNum = " + deliveryNum);
            Beverage.SetActive(false);
            Delivery.SetActive(true);
        }
    }


    void Update()
    {
        Debug.Log("Day = " + Day);
        Debug.Log("buy Milk = " + buyMilk);
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            GameObject clickedObject = null;

            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);
            if (hit.collider != null)
            {
                clickedObject = hit.collider.gameObject;
                Debug.Log("Clicked object: " + clickedObject.name);
            }
            if (clickedObject != null && clickedObject.name == "CoffeePot")
            {
                Beverage.SetActive(true);
                CafeMap.SetActive(false);
                SoundManager.Instance.PlaySFX("click sound");
            }
            /*
            if (clickedObject !=   null && clickedObject.name == "RoomService")
            {
                Delivery.SetActive(true);
                CafeMap.SetActive(false);
            }
            if (clickedObject != null && (clickedObject.name == "RecipeBook" || clickedObject.name == "Recipe"))
            {
                RecipeBook.SetActive(true);
                CafeMap.SetActive(false);
                Beverage.SetActive(false);
            }
            */
            if (clickedObject != null && clickedObject.name == "Extract")
            {
                StartCoroutine(ActivateObjectAfterDelay(2f, Shot));
                SoundManager.Instance.PlaySFX("grinding coffee");
            }
            if (clickedObject != null && clickedObject.name == "TeaInventory")
            {
                Vector2 currentPosition = clickedObject.transform.position;

                Vector2 targetPosition1 = new Vector2(6f, 0.55f);
                Vector2 targetPosition2 = new Vector2(10.8f, 0.55f);

                // 현재 위치가 targetPosition1에 가까우면 targetPosition2로 이동
                if (Vector2.Distance(currentPosition, targetPosition1) < 0.1f)
                {
                    clickedObject.transform.position = targetPosition2;
                    Debug.Log("Moved to: " + targetPosition2);
                }
                // 현재 위치가 targetPosition2에 가까우면 targetPosition1으로 이동
                else if (Vector2.Distance(currentPosition, targetPosition2) < 0.1f)
                {
                    clickedObject.transform.position = targetPosition1;
                    Debug.Log("Moved to: " + targetPosition1);
                }
                else
                {
                    Debug.Log("No match found for current position.");
                }
            }


        }
    }
    IEnumerator ActivateObjectAfterDelay(float delay, GameObject obj)
    {
        SoundManager.Instance.PlaySFX("coffee machine (espresso)");
        yield return new WaitForSeconds(delay);
        obj.SetActive(true);
    }
}
