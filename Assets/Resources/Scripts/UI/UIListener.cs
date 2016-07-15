using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIListener : MonoBehaviour {  //TODO: Pause menu, Highscores etc. und anmelden und so

    public GameObject selectionsPanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;
    public GameObject loadingPanel;

    private OptionsListener optionsListener;

    public void Start()
    {
        optionsListener = GameObject.Find("Main").GetComponent<OptionsListener>();
    }

    public void ShowLevelSelection()  //TODO: credits aktualisieren
    {
        if (IsModalWindowDisplayed()) return;
        selectionsPanel.SetActive(true);
    }

    public void CloseLevelSelection()
    {
        selectionsPanel.SetActive(false);
    }

    public void LoadLevel(int level)
    {
        ShowLoadingPanel();
        SceneManager.LoadSceneAsync(level);
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
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

    public void ApplyMusicOptions()
    {
        optionsListener.ApplyMusicOptions();
    }

    public void HideAndApplyOptions()
    {
        optionsListener.ApplyOptions();
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
        return selectionsPanel.activeSelf || optionsPanel.activeSelf || creditsPanel.activeSelf || loadingPanel.activeSelf;
    }
}
