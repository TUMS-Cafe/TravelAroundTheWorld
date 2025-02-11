using System;
using System.Collections.Generic;
using UnityEngine;

public class RandomDrinkSelector : MonoBehaviour
{
    public static RandomDrinkSelector Instance { get; private set; }
    private Dictionary<int, List<string>> drinkNamesByChapter;
    private Dictionary<string, string> drinkNameMap;

    private void Awake()
    {
        // 싱글턴 패턴 적용
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 이동해도 유지되게 설정
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeDrinkData();
        InitializeDrinkNameMap();
    }


    private void InitializeDrinkData()
    {
        // 챕터별 음료 이름 초기화
        drinkNamesByChapter = new Dictionary<int, List<string>>
        {
            { 1, new List<string> { "Espresso", "HotAmericano", "IceAmericano", "HotLatte", "IceLatte", "HibiscusTea", "RooibosTea", "GreenTea", "ChamomileTea" } },
            { 2, new List<string> { "Affogato", "HotCaramelLatte", "IceCaramelLatte", "HotCinnamonLatte", "IceCinnamonLatte", "HotVanillaLatte", "IceVanillaLatte" } },
            { 3, new List<string> { "StrawberryJuice", "MangoJuice", "BlueberryJuice", "StrawberryLatte", "MangoLatte", "BlueberryLatte", "MintLatte", "SweetPotatoLatte" } }
        };
        /*
        {
            { 1, new List<string> { "Espresso", "HotAmericano", "IceAmericano", "HotLatte", "IceLatte", "HibiscusTea", "RooibosTea", "GreenTea", "ChamomileTea"} },
            { 2, new List<string> { "Affogato", "HotCaramelLatte", "IceCaramelLatte", "HotCinnamonLatte", "IceCinnamonLatte", "HotVanillaLatte", "IceVanillaLatte" } },
            { 3, new List<string> { "StrawberryJuice", "MangoJuice", "BlueberryJuice", "StrawberryLatte", "MangoLatte", "BlueberryLatte", "MintLatte", "SweetPotatoLatte" } }
        };
        /*
        {
            { 1, new List<string> { "아메리카노", "에스프레소","아이스 아메리카노", "라떼", "아이스 라떼", "히비스커스 티","루이보스 티", "녹차","캐모마일 티" } },
            { 2, new List<string> { "아포가토", "카라멜 라떼","아이스 카라멜 라떼","시나몬 라떼","아이스 시나몬 라떼","바닐라 라떼","아이스 바닐라 라떼" } },
            { 3, new List<string> { "딸기 라떼", "딸기 주스", "블루베리 주스","블루베리 라떼","망고 주스","망고 라떼","민트 라떼","고구마 라떼" } }
        };
        */
    }

    public string GetRandomDrink(int chapter)
    {
        if (chapter < 1 || chapter > 3)
        {
            Debug.LogError("잘못된 챕터 번호입니다.");
            return null;
        }

        List<string> possibleDrinks = new List<string>();
        List<float> probabilities = new List<float>();

        if (chapter == 2)
        {
            possibleDrinks.AddRange(drinkNamesByChapter[1]);
            possibleDrinks.AddRange(drinkNamesByChapter[2]);

            // ch2 확률: ch1 30%, ch2 70%
            foreach (var drink in drinkNamesByChapter[1]) probabilities.Add(0.3f / drinkNamesByChapter[1].Count);
            foreach (var drink in drinkNamesByChapter[2]) probabilities.Add(0.7f / drinkNamesByChapter[2].Count);
        }
        else if (chapter == 3)
        {
            possibleDrinks.AddRange(drinkNamesByChapter[1]);
            possibleDrinks.AddRange(drinkNamesByChapter[2]);
            possibleDrinks.AddRange(drinkNamesByChapter[3]);

            // ch3 확률: ch1 10%, ch2 30%, ch3 60%
            foreach (var drink in drinkNamesByChapter[1]) probabilities.Add(0.1f / drinkNamesByChapter[1].Count);
            foreach (var drink in drinkNamesByChapter[2]) probabilities.Add(0.3f / drinkNamesByChapter[2].Count);
            foreach (var drink in drinkNamesByChapter[3]) probabilities.Add(0.6f / drinkNamesByChapter[3].Count);
        }
        else
        {
            possibleDrinks = drinkNamesByChapter[1];
            probabilities = new List<float>(new float[possibleDrinks.Count]); // ch1은 모든 음료 동일 확률
            for (int i = 0; i < probabilities.Count; i++) probabilities[i] = 1.0f / probabilities.Count;
        }

        return SelectDrinkByProbability(possibleDrinks, probabilities);
    }

    private string SelectDrinkByProbability(List<string> drinks, List<float> probabilities)
    {
        float randomValue = UnityEngine.Random.value;
        float cumulative = 0f;

        for (int i = 0; i < drinks.Count; i++)
        {
            cumulative += probabilities[i];
            if (randomValue <= cumulative)
            {
                return drinks[i];
            }
        }
        return drinks[drinks.Count - 1]; // 확률이 오차로 인해 초과되지 않을 경우 마지막 음료 반환
    }
    private void InitializeDrinkNameMap()
    {
        // 영어 <-> 한글 변환 매핑
        drinkNameMap = new Dictionary<string, string>
        {
            { "Espresso", "에스프레소" },
            { "HotAmericano", "아메리카노" },
            { "IceAmericano", "아이스 아메리카노" },

            { "HotLatte", "라떼" },
            { "IceLatte", "아이스 라떼" },

            { "HibiscusTea", "히비스커스 티" },
            { "RooibosTea", "루이보스 티" },
            { "GreenTea", "녹차" },
            { "ChamomileTea", "캐모마일 티" },

            { "Affogato", "아포가토" },
            { "HotCaramelLatte", "카라멜 라떼" },
            { "IceCaramelLatte", "아이스 카라멜 라떼" },
            { "HotCinnamonLatte", "시나몬 라떼" },
            { "IceCinnamonLatte", "아이스 시나몬 라떼" },
            { "HotVanillaLatte", "바닐라 라떼" },
            { "IceVanillaLatte", "아이스 바닐라 라떼" },

            { "StrawberryJuice", "딸기 주스" },
            { "MangoJuice", "망고 주스" },
            { "BlueberryJuice", "블루베리 주스" },

            { "StrawberryLatte", "딸기 라떼" },
            { "MangoLatte", "망고 라떼" },
            { "BlueberryLatte", "블루베리 라떼" },

            { "MintLatte", "민트 라떼" },
            { "SweetPotatoLatte", "고구마 라떼" }
        };
    }


    public string ChangeDrinkName(string englishName)
    {
        return drinkNameMap.ContainsKey(englishName) ? drinkNameMap[englishName] : englishName;
    }

    public string ChangeDrinkNameToEnglish(string koreanName)
    {
        foreach (var pair in drinkNameMap)
        {
            if (pair.Value == koreanName)
                return pair.Key;
        }
        return koreanName; // 매칭되는 값이 없으면 원래 값 반환
    }

}
