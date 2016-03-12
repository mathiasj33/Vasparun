using UnityEngine;
using UnityEngine.SceneManagement;

public class UIListener : MonoBehaviour {

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
        return creditsPanel.activeSelf || surveyPanel.activeSelf;
    }
}
