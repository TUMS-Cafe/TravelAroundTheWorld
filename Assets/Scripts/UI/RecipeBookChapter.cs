using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookChapter : MonoBehaviour
{
    [Header("Page Components")]
    public GameObject[] pages;
    public Button nextButton;
    public Button prevButton;

    private int currentPageIndex = 0;

    private void Start()
    {
        UpdatePage();
    }

    public void NextPage()
    {
        currentPageIndex = (currentPageIndex + 1) % pages.Length;
        UpdatePage();
    }

    public void PreviousPage()
    {
        currentPageIndex = (currentPageIndex - 1 + pages.Length) % pages.Length;
        UpdatePage();
    }

    private void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPageIndex);
        }
    }
}
