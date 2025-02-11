using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeMakeController : MonoBehaviour
{
    public GameObject Water, Ice, Milk;
    public GameObject Hibiscus, Chamomile, Rooibos, Green, Luna;
    public GameObject IceCream, CaramelSyrup, VanillaSyrup, Cinnamon;
    public GameObject Strawberry, Mango, Blueberry, Mint, SweetPotato;

    public GameObject Espresso, HotAmericano, IceAmericano, HotLatte, IceLatte;
    public GameObject HibiscusTea, ChamomileTea, RooibosTea, GreenTea, LunaTea;
    public GameObject Affogato, HotCaramelLatte, IceCaramelLatte, HotCinnamonLatte, IceCinnamonLatte, HotVanillaLatte, IceVanillaLatte, MintLatte;
    public GameObject StrawberryJuice, MangoJuice, BlueberryJuice, StrawberryLatte, MangoLatte, BlueberryLatte, SweetPotatoLatte;

    public GameObject Shot;
    public GameObject MilkSteam;

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

    //private int randomNum = SceneTransitionManager.Instance.GetRandomMenuNum();
    //private int deliveryNum = SceneTransitionManager.Instance.GetDeliveryNum();

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

    //임시 챕터, 해금 식재료 리스트
    public string currentChapter;
    private List<string> unlockedIngredients = new List<string>();
    private Dictionary<string, List<GameObject>> ingredientObjects = new Dictionary<string, List<GameObject>>();


    //ani
    public GameObject StrawberryJuiceAnimation; 
    public Animator strawberryJuiceAnimator; 
    public GameObject MangoJuiceAnimation;
    public Animator mangoJuiceAnimator;
    public GameObject BlueberryJuiceAnimation;
    public Animator blueberryJuiceAnimator;
    public GameObject MintLatteAnimator;
    public Animator mintLatteAnimator;
    public GameObject SweetPotatoLatteAnimator;
    public Animator sweetPotatoLatteAnimator;


    void Start()
    {
        currentChapter = PlayerManager.Instance.GetSceneName(); 
        //currentChapter = "Ch3";
        Debug.Log($" currentChapter: {currentChapter}");
        InitializeIngredientUnlockMap();
        UnlockIngredientsByChapter();

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
        HotCupWithChamomile = Resources.Load<Sprite>("CafeMaking/HotCup+Chamomile");
        HotCupWithRooibos = Resources.Load<Sprite>("CafeMaking/HotCup+Rooibos");

        //ani
        StrawberryJuiceAnimation.SetActive(false);
        MangoJuiceAnimation.SetActive(false);
        BlueberryJuiceAnimation.SetActive(false);
        MintLatteAnimator.SetActive(false);
        SweetPotatoLatteAnimator.SetActive(false);

    }

    private void InitializeIngredientUnlockMap()
    {
        if (ingredientObjects == null)
        {
            ingredientObjects = new Dictionary<string, List<GameObject>>(); // 초기화
        }

        ingredientObjects["Ch0"] = new List<GameObject> { Water, Ice, Milk };
        ingredientObjects["Ch1"] = new List<GameObject> { Hibiscus, Rooibos, Green, Chamomile, Luna};
        ingredientObjects["Ch2"] = new List<GameObject> { IceCream, CaramelSyrup, VanillaSyrup, Cinnamon };
        ingredientObjects["Ch3"] = new List<GameObject> { Strawberry, Mango, Blueberry, Mint, SweetPotato };
       // Debug.Log("Ingredient unlock map initialized.");
        
    }

    public void UnlockIngredientsByChapter()
    {
        foreach (var chapterEntry in ingredientObjects)
        {
            string chapter = chapterEntry.Key;
            bool isUnlocked = string.Compare(chapter, currentChapter) <= 0;

            foreach (GameObject ingredient in chapterEntry.Value)
            {
                if (ingredient != null)
                {
                    ingredient.SetActive(isUnlocked);  // 🔹 GameObject 직접 활성화
                }
            }
        }

        //Debug.Log($"Updated ingredient visibility based on chapter: {currentChapter}");
    }

    public bool CanUseIngredient(string ingredient)
    {
        foreach (var chapterEntry in ingredientObjects)
        {
            if (chapterEntry.Value.Exists(obj => obj.name == ingredient))
            {
                return string.Compare(chapterEntry.Key, currentChapter) <= 0;
            }
        }
        return false;
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
            if (ingredient.name == "MilkSteam")
            {
                SoundManager.Instance.PlaySFX("espresso");
                MilkSteam.SetActive(false);
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
                ingredient.name == "RooibosLeaf"|| ingredient.name == "ChamomileLeaf" || ingredient.name == "LunaLeaf")
            {
                SoundManager.Instance.PlaySFX("tea stir");
            }
            else if (ingredient.name == "IceCream" || ingredient.name == "CaramelSyrup" ||
                ingredient.name == "VanillaSyrup" || ingredient.name == "Cinnamon" || 
                ingredient.name == "Strawberry" || ingredient.name == "Mango" || ingredient.name == "Blueberry" || 
                ingredient.name == "Mint" || ingredient.name == "SweetPotato")
            {
                SoundManager.Instance.PlaySFX("click sound");
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
            else if (currentIngredients.Contains("IceCream"))
            {
                Affogato.SetActive(true);
                Debug.Log("Affogato is maded");
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
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("LunaLeaf"))
        {
            LunaTea.SetActive(true);
            Debug.Log("LunaTea is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        //------------------------------
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Shot") && currentIngredients.Contains("MilkSteam") && currentIngredients.Contains("CaramelSyrup"))
        {
            HotCaramelLatte.SetActive(true);
            Debug.Log("HotCaramelLatte is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Shot") && currentIngredients.Contains("MilkSteam") && currentIngredients.Contains("Cinnamon"))
        {
            HotCinnamonLatte.SetActive(true);
            Debug.Log("HotCinnamonLatte is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("Shot") && currentIngredients.Contains("MilkSteam") && currentIngredients.Contains("VanillaSyrup"))
        {
            HotVanillaLatte.SetActive(true);
            Debug.Log("HotVanillaLatte is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Shot") && currentIngredients.Contains("CaramelSyrup"))
        {
            IceCaramelLatte.SetActive(true);
            Debug.Log("IceCaramelLatte is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Shot") && currentIngredients.Contains("Cinnamon"))
        {
            IceCinnamonLatte.SetActive(true);
            Debug.Log("IceCinnamonLatte is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Shot") && currentIngredients.Contains("VanillaSyrup"))
        {
            IceVanillaLatte.SetActive(true);
            Debug.Log("IceVanillaLatte is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
        }
        
        //--------------------
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Strawberry"))
        {
            StrawberryJuice.SetActive(true);
            Debug.Log("StrawberryJuice is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();

            //애니 
            StartCoroutine(PlayStrawberryJuiceAnimation());
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Mango"))
        {
            MangoJuice.SetActive(true);
            Debug.Log("MangoJuice is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
            StartCoroutine(PlayMangoJuiceAnimation());
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Water") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Blueberry"))
        {
            BlueberryJuice.SetActive(true);
            Debug.Log("BlueberryJuice is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
            StartCoroutine(PlayBlueberryJuiceAnimation());
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Milk") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Strawberry"))
        {
            StrawberryLatte.SetActive(true);
            Debug.Log("StrawberryLatte is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
            StartCoroutine(PlayStrawberryJuiceAnimation());
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Milk") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Mango"))
        {
            MangoLatte.SetActive(true);
            Debug.Log("MangoLatte is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
            StartCoroutine(PlayMangoJuiceAnimation());
        }
        else if ((currentIngredients.Contains("IceCup") || currentIngredients.Contains("MakeIceCup")) && currentIngredients.Contains("Milk") && currentIngredients.Contains("Ice") && currentIngredients.Contains("Blueberry"))
        {
            BlueberryLatte.SetActive(true);
            Debug.Log("BlueberryLatte is maded");
            makeIceCup.SetActive(false);
            currentIngredients.Clear();
            StartCoroutine(PlayBlueberryJuiceAnimation());
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("MilkSteam") && currentIngredients.Contains("Mint"))
        {
            MintLatte.SetActive(true);
            Debug.Log("MintLatte is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
            StartCoroutine(PlayMintLatteAnimation());
        }
        else if ((currentIngredients.Contains("HotCup") || currentIngredients.Contains("MakeHotCup")) && currentIngredients.Contains("MilkSteam") && currentIngredients.Contains("SweetPotato"))
        {
            SweetPotatoLatte.SetActive(true);
            Debug.Log("SweetPotatoLatte is maded");
            makeHotCup.SetActive(false);
            currentIngredients.Clear();
            StartCoroutine(PlaySweetPotatoLatteAnimation());
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
        //}

        SceneTransitionManager.Instance.UpdateCafeOrders(updatedOrders);
    }}

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
        /*if (randomNum > 0)
        {
            SceneTransitionManager.Instance.UpdateRandomMenuDelivery(newNum);
        */
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
        /*}
        else
            SceneTransitionManager.Instance.UpdateCafeDelivery(newNum);*/
    }

    public void BackToDelivery()
    {
        Beverage.SetActive(false);
        Delivery.SetActive(true);
    }

    IEnumerator PlayStrawberryJuiceAnimation(){
        StrawberryJuiceAnimation.SetActive(true); 
        strawberryJuiceAnimator.Play("StrawberryJuiceAnimation");
        SoundManager.Instance.PlaySFX("blender");

        yield return new WaitForSeconds(strawberryJuiceAnimator.GetCurrentAnimatorStateInfo(0).length); 

        StrawberryJuiceAnimation.SetActive(false);
    }
    IEnumerator PlayMangoJuiceAnimation()
    {
        MangoJuiceAnimation.SetActive(true);
        mangoJuiceAnimator.Play("MangoJuiceAnimation");
        SoundManager.Instance.PlaySFX("blender");

        yield return new WaitForSeconds(mangoJuiceAnimator.GetCurrentAnimatorStateInfo(0).length);

        MangoJuiceAnimation.SetActive(false);
    }
    IEnumerator PlayBlueberryJuiceAnimation()
    {
        BlueberryJuiceAnimation.SetActive(true);
        blueberryJuiceAnimator.Play("BlueberryJuiceAnimation");
        SoundManager.Instance.PlaySFX("blender");

        yield return new WaitForSeconds(blueberryJuiceAnimator.GetCurrentAnimatorStateInfo(0).length);

        BlueberryJuiceAnimation.SetActive(false);
    }
    IEnumerator PlayMintLatteAnimation()
    {
        MintLatteAnimator.SetActive(true);
        mintLatteAnimator.Play("MintLatteAnimation");
        SoundManager.Instance.PlaySFX("blender");

        yield return new WaitForSeconds(mintLatteAnimator.GetCurrentAnimatorStateInfo(0).length);

        MintLatteAnimator.SetActive(false);
    }
    IEnumerator PlaySweetPotatoLatteAnimation()
    {
        SweetPotatoLatteAnimator.SetActive(true);
        sweetPotatoLatteAnimator.Play("SweetPotatoLatteAnimation");
        SoundManager.Instance.PlaySFX("blender");

        yield return new WaitForSeconds(sweetPotatoLatteAnimator.GetCurrentAnimatorStateInfo(0).length);

        SweetPotatoLatteAnimator.SetActive(false);
    }
}