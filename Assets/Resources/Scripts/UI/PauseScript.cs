using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {

    public GameObject mainPanel;
    public GameObject crosshairCanvas;

    private bool paused;
	
	public void ResumeOrStop(bool showMenu)
    {
        paused = !paused;

        if(showMenu) mainPanel.SetActive(!mainPanel.activeSelf);
        crosshairCanvas.SetActive(!crosshairCanvas.activeSelf);

        GameObject.Find("Main Camera").GetComponent<FirstPersonCamera>().enabled = !paused;
        GameObject.Find("Gun").GetComponent<GunMovementControl>().enabled = !paused;
        GameObject.Find("Player").GetComponent<ShootControl>().enabled = !paused;

        Cursor.visible = paused;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;

        Time.timeScale = paused ? 0 : 1;
    }
}
