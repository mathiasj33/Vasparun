using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class BesttimesScript : MonoBehaviour {   //TODO: main menu button, try again, zum typen scrollen der es auch ist, eigenen score hochladen und comparen, new highscore message wenn besser, andere maps und infinite

    public int level;

    public GameObject canvas;
    public GameObject error;
    public GameObject scrollView;
    public GameObject content;

    public Text users;
    public Text times;

    private IDictionary<string, float> dict = new Dictionary<string, float>();
	
	public void ShowBesttimes()
    {
        canvas.SetActive(true);
        GameObject.Find("Main").GetComponent<PauseScript>().ResumeOrStop(false);

        WWWForm form = new WWWForm();
        form.AddField("op", "SelectAllUserTimes");
        form.AddField("level", level);
        WWW www = new WWW(Globals.ServerURL, form);
        StartCoroutine(ShowBesttimes(www));
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
        }
    }

    private void PopulateDict(string text)
    {
        string[] pairs = text.Split('$');
        foreach(string pair in pairs)
        {
            string[] arr = pair.Split(':');
            dict.Add(arr[0], float.Parse(arr[1]));
        }
    }

    private void PopulateUI(string text)
    {
        PopulateDict(text);

        StringBuilder userString = new StringBuilder();
        StringBuilder timesString = new StringBuilder();

        foreach (string user in dict.Keys)
        {
            userString.Append(user).AppendLine();
            string timeString = TimeFormatter.Format(dict[user]);
            timesString.Append(timeString).AppendLine();
        }

        //users.text = userString.ToString();
        //times.text = timesString.ToString();
        users.text = "";
        times.text = "";
        for(int i = 0; i < 100; i++)
        {
            users.text += "User" + i + "\n";
            times.text += "Time" + i + "\n";
        }

        scrollView.SetActive(true);
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
}
