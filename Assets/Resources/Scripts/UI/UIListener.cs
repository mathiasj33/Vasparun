using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIListener : MonoBehaviour {  //TODO: Pause menu

    public GameObject optionsPanel;
    public GameObject creditsPanel;

    public void LoadLevel1()   //TODO: level selection menu
    {
        if (IsModalWindowDisplayed()) return;
        SceneManager.LoadScene(1);
    }

    public void LoadInfinite()
    {
        if (IsModalWindowDisplayed()) return;
        SceneManager.LoadScene(4);
    }

    public void ShowOptions()
    {
        if (IsModalWindowDisplayed()) return;
        optionsPanel.SetActive(true);
    }

    public void HideAndApplyOptions() //TODO: options weiter implementieren
    {
        GameObject.Find("Main").GetComponent<OptionsListener>().ApplyOptions();
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

    public void LeaveGame()
    {
        Application.Quit();
    }

    private bool IsModalWindowDisplayed()
    {
        return optionsPanel.activeSelf || creditsPanel.activeSelf;
    }
}
