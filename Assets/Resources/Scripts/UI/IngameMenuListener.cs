using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IngameMenuListener : MonoBehaviour {  //TODO: Menu noch für andere levels und infinite

    public GameObject menuCanvas;  Panel und OptionsPanel abwechselnd und listener für options.
    public GameObject crosshairCanvas;
    public GameObject optionsPanel;

    private bool paused = false;
	
	void Update () {
        if(Input.GetButtonDown("Escape"))
        {
            ResumeOrStop();
        }
	}

    public void ResumeOrStop()
    {
        if (optionsPanel.activeSelf) return;
        paused = !paused;

        menuCanvas.SetActive(!menuCanvas.activeSelf);
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
        optionsPanel.SetActive(true);
    }

    public void GoToMainMenu()  //TODO: hier noch async und loading
    {
        if (optionsPanel.activeSelf) return;
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        if (optionsPanel.activeSelf) return;
        Application.Quit();
    }
}
