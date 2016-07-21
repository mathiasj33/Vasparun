using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class BesttimesScript : MonoBehaviour {   //TODO: eigenen score hochladen und comparen, new highscore message wenn besser (New Highscore! You were xs faster! / You ran xm more!), andere maps und infinite

    public int level;

    public GameObject canvas;
    public GameObject error;
    public GameObject usernameError;
    public GameObject scrollView;
    public GameObject content;

    public Text users;
    public Text times;

    private SortedDictionary<string, float> dict = new SortedDictionary<string, float>();
    private string username;
	
	public void ShowBesttimes()
    {
        canvas.SetActive(true);
        GameObject.Find("Main").GetComponent<PauseScript>().ResumeOrStop(false);

        if (!IsUsernameValid())
        {
            usernameError.SetActive(true);
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("op", "SelectAllUserTimes");
        form.AddField("level", level);
        WWW www = new WWW(Globals.ServerURL, form);
        StartCoroutine(ShowBesttimes(www));
    }

    private bool IsUsernameValid()
    {
        username = Settings.GetString("username", "");
        if (username == "") return false;
        return true;
    }

    private IEnumerator ShowBesttimes(WWW www)
    {
        yield return www;
        if(www.error != null || www.text == "Error")
        {
            error.SetActive(true);
        }
        else
        {
            PopulateUI(www.text);
            SetScrollHeight();
            SetScrollBarPosition();
        }
    }

    private void PopulateUI(string text)
    {
        PopulateDict(text);
        //for (int i = 0; i < 100; i++)
        //{
        //    dict.Add("User" + i, i);
        //}
        //username = "User67";

        StringBuilder userString = new StringBuilder();
        StringBuilder timesString = new StringBuilder();

        foreach (string user in dict)
        {
            string userAppend = user == username ? "<i>" + user + "</i>" : user;
            userString.Append(userAppend).AppendLine();

            string timeString = TimeFormatter.Format(dict.Get(user));
            string timeAppend = user == username ? "<i>" + timeString + "</i>" : timeString;
            timesString.Append(timeAppend).AppendLine();
        }

        users.text = userString.ToString();
        times.text = timesString.ToString();

        scrollView.SetActive(true);
    }

    private void PopulateDict(string text)
    {
        string[] pairs = text.Split('$');
        foreach (string pair in pairs)
        {
            string[] arr = pair.Split(':');
            dict.Add(arr[0], float.Parse(arr[1]));
        }
    }

    private void SetScrollHeight()
    {
        RectTransform usersTrans = users.GetComponent<RectTransform>();
        usersTrans.sizeDelta = new Vector2(usersTrans.sizeDelta.x, users.preferredHeight);
        usersTrans.anchoredPosition = new Vector2(usersTrans.anchoredPosition.x, -users.preferredHeight / 2);

        RectTransform timesTrans = times.GetComponent<RectTransform>();
        timesTrans.sizeDelta = usersTrans.sizeDelta;
        timesTrans.anchoredPosition = usersTrans.anchoredPosition;

        RectTransform contentTrans = content.GetComponent<RectTransform>();
        contentTrans.offsetMin = new Vector2(contentTrans.offsetMin.x, 441.5f - users.preferredHeight);
    }

    private void SetScrollBarPosition()
    {
        float userPos = dict.IndexOf(username);
        float total = dict.Count;
        scrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 1 - (userPos / total);
    }
}
