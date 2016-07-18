using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenuListener : MonoBehaviour {

    public GameObject mainPanel;
    public GameObject crosshairCanvas;
    public GameObject optionsPanel;
    public GameObject loadingPanel;

    public GameObject bestTimeCanvas;

    private bool paused = false;
	
	void Update () {
        if(Input.GetButtonDown("Escape")) ResumeOrStop();
        if (Input.GetKeyDown("t")) ShowBestTimeCanvas();
    }

    public void ShowBestTimeCanvas()
    {

    }

    public void ResumeOrStop()
    {
        if (optionsPanel.activeSelf) return;
        paused = !paused;

        mainPanel.SetActive(!mainPanel.activeSelf);
        crosshairCanvas.SetActive(!crosshairCanvas.activeSelf);
        GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>().enabled = !paused;
        GameObject.Find("Gun").GetComponent<GunMovementControl>().enabled = !paused;
        GameObject.Find("Player").GetComponent<ShootControl>().enabled = !paused;
        Cursor.visible = paused;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = paused ? 0 : 1;
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
