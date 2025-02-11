using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsUIManager : MonoBehaviour
{
    [Header("Pages")]
    public GameObject soundPage;
    public GameObject controlPage2;
    public GameObject controlPage3;

    [Header("Sound Settings")]
    public Image sliderFill;
    public Slider soundSlider;

    [Header("Popups")]
    public GameObject mainMenuPopup;
    public GameObject quitGamePopup;

    private void Start()
    {
        soundSlider.onValueChanged.AddListener(UpdateSlider);
        ShowPage(1);
    }

    private void UpdateSlider(float value)
    {
        sliderFill.fillAmount = 1 - value;
        AudioListener.volume = value;
    }

    public void ShowPage(int pageNumber)
    {
        soundPage.SetActive(true);
        controlPage2.SetActive(pageNumber == 2);
        controlPage3.SetActive(pageNumber == 3);
    }

    public void ShowMainMenuPopup()
    {
        mainMenuPopup.SetActive(true);
    }

    public void HideMainMenuPopup()
    {
        mainMenuPopup.SetActive(false);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowQuitGamePopup()
    {
        quitGamePopup.SetActive(true);
    }

    public void HideQuitGamePopup()
    {
        quitGamePopup.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
