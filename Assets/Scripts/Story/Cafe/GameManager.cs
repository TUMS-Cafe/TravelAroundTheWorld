using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Shot;
    public GameObject BtnExtract;
    public GameObject Steam;
    public GameObject BtnSteam;


    public GameObject Beverage;
    public GameObject CafeMap;
    public GameObject Delivery;
    public GameObject RecipeBook;

    public GameObject Milk;

    public Animator AniShot;
    public Animator AniSteam;
    public GameObject ShotAniObj;
    public GameObject SteamAniObj;

    //private bool buyMilk = PlayerManager.Instance.IsBoughtCafeItem("우유");
    //private bool buyTeaSet = PlayerManager.Instance.IsBoughtCafeItem("티 세트");

    //private int Day = PlayerManager.Instance.GetDay();

    public CafeMakeController cafeMake;
    public OrderController orderController;

    void Start()
    {

        UnlockNewIngredients();

        SoundManager.Instance.PlayMusic("CAFE", true);

        ShotAniObj.SetActive(false);
        AniShot.enabled = true;
        SteamAniObj.SetActive(false);
        AniSteam.enabled = true;

        Milk.SetActive(true);


        int deliveryNum = SceneTransitionManager.Instance.GetDeliveryNum();

        if (deliveryNum != null && deliveryNum > 0)
        {
            Debug.Log("deliveryNum = " + deliveryNum);
            Beverage.SetActive(false);
            Delivery.SetActive(true);
        }
    }

//addzzz
    void UnlockNewIngredients()
    {
        int chapter = 1;
        if (cafeMake != null){
        cafeMake.UnlockIngredients();
    }}


    void Update()
    {
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
                StartCoroutine(ShotActivateObjectAfterDelay(2f, Shot));
                SoundManager.Instance.PlaySFX("grinding coffee");
                Debug.Log("Extract button clicked.");
            }
            if (clickedObject != null && clickedObject.name == "Steam")
            {
                StartCoroutine(SteamActivateObjectAfterDelay(2f, Steam));
                //SoundManager.Instance.PlaySFX("grinding coffee");
            }
            /*if (clickedObject != null && clickedObject.name == "TeaInventory")
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
            }*/


        }
    }


    IEnumerator ShotActivateObjectAfterDelay(float delay, GameObject obj)
    {
        ShotAniObj.SetActive(true);
        AniShot.enabled = true;
        SoundManager.Instance.PlaySFX("coffee machine (espresso)");
        AniShot.Play("StartShotAnimation");
        yield return new WaitForSeconds(2f);
        ShotAniObj.SetActive(false);
        AniShot.enabled = false;
        obj.SetActive(true);

    }

    IEnumerator SteamActivateObjectAfterDelay(float delay, GameObject obj)
    {
        SteamAniObj.SetActive(true);
        AniSteam.enabled = true;
        SoundManager.Instance.PlaySFX("coffee machine (espresso)");
        AniSteam.Play("StartSteamAnimation");
        yield return new WaitForSeconds(2f);
        SteamAniObj.SetActive(false);
        AniSteam.enabled = false;
        obj.SetActive(true);

    }




    }
