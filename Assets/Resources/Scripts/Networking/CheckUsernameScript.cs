using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CheckUsernameScript : MonoBehaviour
{
    public GameObject userPanel;
    public GameObject userField;
    public GameObject wrongCharsMsg;
    public GameObject alreadyExistsMsg;

    void Start()
    {
        if (!UsernameAlreadyChosen()) CheckConnection();
    }

    private bool UsernameAlreadyChosen()
    {
        return Settings.GetString("username", "") != "";
    }

    private void CheckConnection()
    {
        WWWForm form = new WWWForm();
        form.AddField("op", "CheckConnection");
        WWW www = new WWW(Globals.ServerURL, form);
        StartCoroutine(CheckConnection(www));
    }

    private IEnumerator CheckConnection(WWW www)
    {
        yield return www;
        if (www.text == "OK")
        {
            userPanel.SetActive(true);
        }
    }

    public void CheckAvailability()
    {
        string username = userField.GetComponent<InputField>().text;
        if(username.Contains("$") || username.Contains(","))
        {
            alreadyExistsMsg.SetActive(false);
            wrongCharsMsg.SetActive(true);
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("op", "ContainsUsername");
        form.AddField("username", username);
        WWW www = new WWW(Globals.ServerURL, form);
        StartCoroutine(CheckAvailability(www));
    }

    private IEnumerator CheckAvailability(WWW www)
    {
        yield return www;
        if (www.text == "true")
        {
            wrongCharsMsg.SetActive(false);
            alreadyExistsMsg.SetActive(true);
        }
        else
        {
            AddUser();
        }
    }

    private void AddUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("op", "AddUser");
        string username = userField.GetComponent<InputField>().text;
        form.AddField("username", username);
        userPanel.SetActive(false);
        new WWW(Globals.ServerURL, form);

        Settings.SetString("username", username);
    }
}
