using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour
{
    private CafeMakeController cafeMakeController;
    private IngredientController ingredientController;
    private SpriteRenderer spriteRenderer;

    private Sprite defaultSprite;
    private Sprite collisionSprite;
    public float spriteChangeDuration = 0.1f;


    void Start()
    {
        ingredientController = FindObjectOfType<IngredientController>();
        cafeMakeController = FindObjectOfType<CafeMakeController>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = Resources.Load<Sprite>("CafeImage/cafemake_Trashcan0");
        collisionSprite = Resources.Load<Sprite>("CafeImage/cafemake_Trashcan2");
        if (defaultSprite != null)
        {
            spriteRenderer.sprite = defaultSprite;
        }
        else
        {
            Debug.LogError("Default sprite could not be loaded.");
        }

    }

    public void HandleTrashCan(GameObject trash)
    {
        if (collisionSprite != null)
        {
            spriteRenderer.sprite = collisionSprite;
            Invoke("ResetSprite", spriteChangeDuration); // 일정 시간 후 원래 스프라이트로 복귀
        }
        if (trash.name == "MakeIceCup")
        {
            trash.SetActive(false);
            cafeMakeController.currentIngredients.Clear();
            Debug.Log("Current ingredients: " + string.Join(", ", cafeMakeController.currentIngredients)); // 리스트의 현재 상태를 출력
        }
        else if(trash.name == "MakeHotCup"){
            trash.SetActive(false);
            cafeMakeController.currentIngredients.Clear();
            Debug.Log("Current ingredients: " + string.Join(", ", cafeMakeController.currentIngredients)); // 리스트의 현재 상태를 출력
        }
        else if (trash.name == "DoneEsp" || trash.name == "DoneIceAm" || trash.name == "DoneHotAm" ||
            trash.name == "DoneIceLt" || trash.name == "DoneHotLt" || trash.name == "DoneHb"||
            trash.name == "DoneRoo" || trash.name == "DoneGt" || trash.name == "DoneCm")
        {
            trash.SetActive(true);
        }
        else if (trash.name == "Shot")
            trash.SetActive(false);

        trash.gameObject.SetActive(false);

        if (cafeMakeController != null)
        {
            cafeMakeController.currentIngredients.Clear();
            Debug.Log("All ingredients cleared and object destroyed.");
        }
    }

    private void ResetSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }
}