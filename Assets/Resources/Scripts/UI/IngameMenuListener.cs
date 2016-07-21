using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenuListener : MonoBehaviour {

    public GameObject mainPanel;
    public GameObject crosshairCanvas;
    public GameObject optionsPanel;
    public GameObject loadingPanel;

    public GameObject bestTimeCanvas;
	
	void Update () {
        if(Input.GetButtonDown("Escape")) ResumeOrStop();
    }

    public void ResumeOrStop()
    {
        if (optionsPanel.activeSelf) return;
        GameObject.Find("Main").GetComponent<PauseScript>().ResumeOrStop(true);
    }

    public void RestartLevel()
    {
        if (optionsPanel.activeSelf) return;
        loadingPanel.SetActive(true);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowOptions()
    {
        mainPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void HideAndApplyOptions()
    {
        GameObject.Find("Main").GetComponent<OptionsListener>().ApplyOptions();
        optionsPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void GoToMainMenu()
    {
        if (optionsPanel.activeSelf) return;
        Time.timeScale = 1;
        loadingPanel.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        if (optionsPanel.activeSelf) return;
        Application.Quit();
    }
}
