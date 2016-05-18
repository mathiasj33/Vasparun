using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIListener : MonoBehaviour {

    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject surveyPanel;

    public void LoadLevel1()
    {
        if (IsModalWindowDisplayed()) return;
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2()
    {
        if (IsModalWindowDisplayed()) return;
        SceneManager.LoadScene(2);
    }

    public void LoadInfinite()
    {
        if (IsModalWindowDisplayed()) return;
        SceneManager.LoadScene(3);
    }

    public void ShowOptions()
    {
        if (IsModalWindowDisplayed()) return;
        optionsPanel.SetActive(true);
    }

    public void HideOptions()
    {
        Slider slider = GameObject.Find("SensitivitySlider").GetComponent<Slider>();
        Globals.Sensitivity = slider.value;
        optionsPanel.SetActive(false);
    }

    public void ShowCredits()
    {
        if (IsModalWindowDisplayed()) return;
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void ShowSurvey()
    {
        if (IsModalWindowDisplayed()) return;
        surveyPanel.SetActive(true);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }

    private bool IsModalWindowDisplayed()
    {
        return optionsPanel.activeSelf || creditsPanel.activeSelf || surveyPanel.activeSelf;
    }
}
