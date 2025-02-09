using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeMakeController : MonoBehaviour
{
    public GameObject Espresso;
    public GameObject IceAmericano;
    public GameObject HotAmericano;
    public GameObject IceLatte;
    public GameObject HotLatte;
    public GameObject HibiscusTea;
    public GameObject ChamomileTea;
    public GameObject RooibosTea;
    public GameObject GreenTea;

    public GameObject Shot;

    public GameObject makeIceCup;
    public GameObject makeHotCup;

    public GameObject Delivery;
    public GameObject Beverage;
    public GameObject CafeMap;
    public List<string> currentIngredients = new List<string>();

    List<CafeOrder> updatedOrders = new List<CafeOrder>();

    public Transform orderListParent;

    private int newNum = 0;

    public DeliveryData deliveryData;
    public string deliveryOrder;

    private int randomNum = SceneTransitionManager.Instance.GetRandomMenuNum();
    private int deliveryNum = SceneTransitionManager.Instance.GetDeliveryNum();

    private SpriteRenderer iceCupSpriteRenderer;
    private SpriteRenderer hotCupSpriteRenderer;

    public Sprite IceCup;
    public Sprite IceCupWithIce;
    public Sprite IceCupWithWater;
    public Sprite IceCupWithWaterIce;
    public Sprite IceCupWithShot;
    public Sprite IceCupWithMilk;
    public Sprite IceCupWithMilkIce;

    public Sprite HotCup;
    public Sprite HotCupWithWater;
    public Sprite HotCupWithShot;
    public Sprite HotCupWithMilk;
    public Sprite HotCupWithGreen;
    public Sprite HotCupWithHibiscus;
    public Sprite HotCupWithChamomile;
    public Sprite HotCupWithRooibos;

    void Start()
    {
        iceCupSpriteRenderer = makeIceCup.GetComponent<SpriteRenderer>();
        hotCupSpriteRenderer = makeHotCup.GetComponent<SpriteRenderer>();

        IceCup = Resources.Load<Sprite>("CafeMaking/IceCup");
        IceCupWithIce = Resources.Load<Sprite>("CafeMaking/IceCup+Ice");
        IceCupWithWater = Resources.Load<Sprite>("CafeMaking/IceCup+Water");
        IceCupWithWaterIce = Resources.Load<Sprite>("CafeMaking/IceCup+Water+Ice");
        IceCupWithShot = Resources.Load<Sprite>("CafeMaking/IceCup+Shot");
        IceCupWithMilk = Resources.Load<Sprite>("CafeMaking/IceCup+Milk");
        IceCupWithMilkIce = Resources.Load<Sprite>("CafeMaking/IceCup+Milk+Ice");

        HotCup = Resources.Load<Sprite>("CafeMaking/HotCup");
        HotCupWithWater = Resources.Load<Sprite>("CafeMaking/HotCup+Water");
        HotCupWithShot = Resources.Load<Sprite>("CafeMaking/HotCup+Shot");
        HotCupWithMilk = Resources.Load<Sprite>("CafeMaking/IHotCup+Milk");
        HotCupWithGreen = Resources.Load<Sprite>("CafeMaking/HotCup+Green");
        HotCupWithHibiscus = Resources.Load<Sprite>("CafeMaking/HotCup+Hibiscus");
        HotCupWithChamomile = Resources.Load<Sprite>("CafeMaking/HotCup+Chamonile");
        HotCupWithRooibos = Resources.Load<Sprite>("CafeMaking/HotCup+Rooibos");
    }

    void OnEnable()
    {
        deliveryOrder = deliveryData.deliveryOrder;
        Debug.Log("deliveryNum = " + deliveryNum);
        Debug.Log("delivery menu = " + deliveryData.deliveryOrder);

    }

    public void HandleMakeArea(GameObject ingredient)
    {
        if (!currentIngredients.Contains(ingredient.name))
        {
            currentIngredients.Add(ingredient.name);
            Debug.Log("Ingredient added : " + ingredient.name);
            if (ingredient.name == "Shot")
            {
                SoundManager.Instance.PlaySFX("espresso");
                Shot.SetActive(false);
            }
            else if (ingredient.name == "Water" || ingredient.name =="Milk")
            {
                SoundManager.Instance.PlaySFX("pouring water");
            }
            else if (ingredient.name == "Ice")
            {
                SoundManager.Instance.PlaySFX("ice in a cup");
            }
            else if (ingredient.name == "IceCup" || ingredient.name == "HotCup")
            {
                SoundManager.Instance.PlaySFX("cupsetdown");
            }
            else if (ingredient.name == "GreenTeaLeaf" || ingredient.name == "HibiscusLeaf" ||
                ingredient.name == "RooibosLeaf"|| ingredient.name == "ChamomileLeaf")
            {
                SoundManager.Instance.PlaySFX("tea stir");
            }
        }
        CupDisPlay();
    }

    private void CupDisPlay()
    {
        if (currentIngredients.Contains("IceCup"))
        {
            iceCupSpriteRenderer.sprite = IceCup;
            if (currentIngredients.Contains("Shot") && !(currentIngredients.Contains("Water") || currentIngredients.Contains("Ice") || currentIngredients.Contains("Milk")))
            {
                iceCupSpriteRenderer.sprite = IceCupWithShot;
            }
            else if (currentIngredients.Contains("Ice") && !currentIngredients.Contains("Water") && !currentIngredients.Contains("Milk"))
                iceCupSpriteRenderer.sprite = IceCupWithIce;
            else if (currentIngredients.Contains("Water"))
            {
                if (currentIngredients.Contains("Ice"))
                    iceCupSpriteRenderer.sprite = IceCupWithWaterIce;
                else
                    iceCupSpriteRenderer.sprite = IceCupWithWater;
            }
            else if (currentIngredients.Contains("Milk"))
            {
                if (currentIngredients.Contains("Ice"))
                    iceCupSpriteRenderer.sprite = IceCupWithMilkIce;
                else
                    iceCupSpriteRenderer.sprite = IceCupWithMilk;
            }
        }
        else if (currentIngredients.Contains("HotCup"))
        {
            hotCupSpriteRenderer.sprite = HotCup;

            if (currentIngredients.Contains("Water"))
                hotCupSpriteRenderer.sprite = HotCupWithWater;
            else if (currentIngredients.Contains("Milk"))
                hotCupSpriteRenderer.sprite = HotCupWithMilk;
            else if (currentIngredients.Contains("Shot"))
                hotCupSpriteRenderer.sprite = HotCupWithShot;
            else if (currentIngredients.Contains("GreenTeaLeaf"))
                hotCupSpriteRenderer.sprite = HotCupWithGreen;
            else if (currentIngredients.Contains("HibiscusLeaf"))
                hotCupSpriteRenderer.sprite = HotCupWithHibiscus;
            else if (currentIngredients.Contains("ChamomileLeaf"))
                hotCupSpriteRenderer.sprite = HotCupWithChamomile;
            else if (currentIngredients.Contains("RooibosLeaf"))
                hotCupSpriteRenderer.sprite = HotCupWithRooibos;
        }
    }

        public void CheckRecipe()
    {

        Debug.Log("Current ingredients: " + string.Join(", ", currentIngredients)); // 리스트의 현재 상태를 출력

        if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Shot"))
        {
            if (currentIngredients.Contains("Water"))
            {
                HotAmericano.SetActive(true);
                Debug.Log("HotAmericano is maded");
                makeHotCup.SetActive(false);
                currentIngredients.Clear();
            }
            else if (currentIngredients.Contains("Milk"))
            {
                HotLatte.SetActive(true);
                Debug.Log("HotLatte is maded");
                makeHotCup.SetActive(false);
                currentIngredients.Clear();
            }
            else
            {
                Espresso.SetActive(true);
                Debug.Log("Espresso is maded");
                makeHotCup.SetActive(false);
                currentIngredients.Clear();
            }
        }
        else if ((currentIngredients.Contains("IceCup")|| currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Shot"))
        {
            IceAmericano.SetActive(true);
            Debug.Log("IceAmericano is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Milk") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Shot"))
        {
            IceLatte.SetActive(true);
            Debug.Log("IceLatte is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("HibiscusLeaf"))
        {
            HibiscusTea.SetActive(true);
            Debug.Log("HibiscusTea is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("RooibosLeaf"))
        {
            RooibosTea.SetActive(true);
            Debug.Log("RooibosTea is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("GreenTeaLeaf"))
        {
            GreenTea.SetActive(true);
            Debug.Log("GreenTea is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("ChamomileLeaf"))
        {
            ChamomileTea.SetActive(true);
            Debug.Log("Chamomile is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        Invoke("CheckOrder", 0.2f);
    }
    public void CheckOrder()
    {
        if (randomNum > 0)
        {
            ProcessOrder(Espresso, "Espresso", 50, false);
            ProcessOrder(HotAmericano, "HotAmericano", 150, false);
            ProcessOrder(IceAmericano, "IceAmericano", 150, false);
            ProcessOrder(HotLatte, "HotLatte", 180, false);
            ProcessOrder(IceLatte, "IceLatte", 180, false);
            ProcessOrder(GreenTea, "GreenTea", 110, false);
            ProcessOrder(HibiscusTea, "HibiscusTea", 150, false);
            ProcessOrder(RooibosTea, "RooibosTea", 160, false);
            ProcessOrder(ChamomileTea, "ChamomileTea", 120, false);
        }
        else if (deliveryNum > 0)
        {
            ProcessOrder(Espresso, "Espresso", 50, true);
            ProcessOrder(HotAmericano, "HotAmericano", 150, true);
            ProcessOrder(IceAmericano, "IceAmericano", 150, true);
            ProcessOrder(HotLatte, "HotLatte", 180, true);
            ProcessOrder(IceLatte, "IceLatte", 180, true);
            ProcessOrder(GreenTea, "GreenTea", 110, true);
            ProcessOrder(HibiscusTea, "HibiscusTea", 150, true);
            ProcessOrder(RooibosTea, "RooibosTea", 160, true);
            ProcessOrder(ChamomileTea, "ChamomileTea", 120, true);
        }
        else
        {
            ProcessDirectOrder(Espresso, "Espresso", 50);
            ProcessDirectOrder(HotAmericano, "HotAmericano", 150);
            ProcessDirectOrder(IceAmericano, "IceAmericano", 150);
            ProcessDirectOrder(HotLatte, "HotLatte", 180);
            ProcessDirectOrder(IceLatte, "IceLatte", 180);
            ProcessDirectOrder(GreenTea, "GreenTea", 110);
            ProcessDirectOrder(HibiscusTea, "HibiscusTea", 150);
            ProcessDirectOrder(RooibosTea, "RooibosTea", 160);
            ProcessDirectOrder(ChamomileTea, "ChamomileTea", 120);
        }

        SceneTransitionManager.Instance.UpdateCafeOrders(updatedOrders);
    }

    private void ProcessOrder(GameObject drink, string drinkName, int earnings, bool isDeliveryOrder)
    {
        if (drink.activeSelf)
        {
            if (isDeliveryOrder)
            {
                if (deliveryOrder == drinkName)
                {
                    if (orderListParent.childCount > 0)
                    {
                        Destroy(orderListParent.GetChild(0).gameObject);
                    }
                    PlayerManager.Instance.EarnMoney(earnings);
                    drink.SetActive(false);
                    ProcessOrderCompletion();
                    BackToDelivery();
                }
            }
            else
            {
                foreach (Transform order in orderListParent)
                {
                    if (order.gameObject.activeInHierarchy && order.name.Contains(drinkName))
                    {
                        PlayerManager.Instance.EarnMoney(earnings);
                        Destroy(order.gameObject);
                        drink.SetActive(false);
                        ProcessOrderCompletion();
                        break;
                    }
                }
            }
        }
    }

    private void ProcessDirectOrder(GameObject drink, string drinkName, int earnings)
    {
        if (drink.activeSelf)
        {
            updatedOrders.Add(new CafeOrder(drinkName));
            if (SceneTransitionManager.Instance.GetCafeOrders() == drinkName)
            {
                if (orderListParent.childCount > 0)
                {
                    Destroy(orderListParent.GetChild(0).gameObject);
                }
                PlayerManager.Instance.EarnMoney(earnings);
                drink.SetActive(false);
            }
        }
    }

    public void ProcessOrderCompletion()
    {
        currentIngredients.Clear();
        newNum++;
        Debug.Log("주문 제작 완료 수 = "+ newNum);
        if (randomNum > 0)
        {
            SceneTransitionManager.Instance.UpdateRandomMenuDelivery(newNum);

            if (orderListParent.childCount == 5)
            {
                for (int i = 0; i < orderListParent.childCount; i++)
                {
                    Transform order = orderListParent.GetChild(i);
                    if (order.gameObject.activeInHierarchy)
                    {
                        // 각 주문의 위치를 앞으로 이동
                        order.localPosition = new Vector3(
                            order.localPosition.x + 1.35f, // 이동할 x 축의 거리
                            order.localPosition.y,
                            order.localPosition.z
                        );
                    }
                }
            }
        }
        else
            SceneTransitionManager.Instance.UpdateCafeDelivery(newNum);
    }

    public void BackToDelivery()
    {
        Beverage.SetActive(false);
        Delivery.SetActive(true);
    }
}