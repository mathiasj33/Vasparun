using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeInAndOutScript : MonoBehaviour {  //TODO: Destroy(this) überall

    public bool InfiniteMode { get; set; }
    public Canvas Canvas { get; set; }
    public Text Text { get; set; }

    private GameObject newTextGO;
    private Text newText;

	void Start () {
        InitNewText();
        StartCoroutine("FadeIn");
	}
	
    private void InitNewText()
    {
        string path = InfiniteMode ? "Prefabs/Minus" : "Prefabs/Plus";
        newTextGO = (GameObject)Instantiate(Resources.Load(path));
        newText = newTextGO.GetComponent<Text>();
        newText.transform.SetParent(Canvas.transform, false);
        newText.fontSize = Text.fontSize;
    }

	private IEnumerator FadeIn()
    {
        float a = 0;
        while(a < 1)
        {
            a += Time.deltaTime * 2;
            newText.color = new Color(1, 0, 0, a);
            yield return null;
        }
        StartCoroutine("FadeOut");
    }

    private IEnumerator FadeOut()
    {
        Vector3 position = new Vector3(newText.transform.position.x, newText.transform.position.y, newText.transform.position.z);
        float a = 1;
        while (a > 0)
        {
            a -= Time.deltaTime;
            newText.color = new Color(1, 0, 0, a);
            newText.transform.position -= new Vector3(0, 2, 0);
            yield return null;
        }
        newText.transform.position = position;
        Destroy(newTextGO);
        Destroy(this);
    }
}
