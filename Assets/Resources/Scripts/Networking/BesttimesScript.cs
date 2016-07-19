using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class BesttimesScript : MonoBehaviour {  TODO: Pausieren, main menu button, try again, zum typen scrollen der es auch ist, andere maps und infinite

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

        users.text = userString.ToString();
        times.text = timesString.ToString();

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
        contentTrans.offsetMin = new Vector2(contentTrans.offsetMin.x, 531 - users.preferredHeight);
    }
}
