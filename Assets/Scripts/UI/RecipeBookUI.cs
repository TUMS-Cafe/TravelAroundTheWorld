using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBookUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject recipeBookUI;
    public GameObject[] chapterPanels;
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool isRecipeBookOpen = false;
    private int currentChapterIndex = 0;

    private void Start()
    {
        recipeBookUI.SetActive(false);
        OpenChapter(0);
    }

    public void OpenRecipeBook()
    {
        if (isRecipeBookOpen) return;

        recipeBookUI.SetActive(true);
        audioSource.PlayOneShot(openSound);
        isRecipeBookOpen = true;

        OpenChapter(0);
    }

    public void CloseRecipeBook()
    {
        if (!isRecipeBookOpen) return;

        recipeBookUI.SetActive(false);
        audioSource.PlayOneShot(closeSound);
        isRecipeBookOpen = false;
    }

    public void OpenChapter(int chapterIndex)
    {
        foreach (GameObject panel in chapterPanels)
        {
            panel.SetActive(false);
        }

        if (chapterIndex >= 0 && chapterIndex < chapterPanels.Length)
        {
            chapterPanels[chapterIndex].SetActive(true);
            currentChapterIndex = chapterIndex;
        }
    }
}