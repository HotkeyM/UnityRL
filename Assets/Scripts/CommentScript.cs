using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommentScript : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Comment(string str)
    {
        GameObject mb = (GameObject)Instantiate(Resources.Load("Prefabs/MessageBox"));

        Vector3 screenPos = Camera.allCameras[0].WorldToViewportPoint(transform.position);

        RectTransform rt = (RectTransform)(mb.GetComponentInChildren<Image>().gameObject.GetComponent<RectTransform>());
		rt.anchorMax = screenPos + new Vector3(0.0f, 0.1f,0.0f);//.position = new Vector2(screenPos.x * 800.0f, (screenPos.y) * 800.0f / Camera.allCameras[0].aspect);
		rt.anchorMin = screenPos + new Vector3(0.0f, 0.1f,0.0f);//.position = new Vector2(screenPos.x * 800.0f, (screenPos.y) * 800.0f / Camera.allCameras[0].aspect);

        Text t = (Text)mb.GetComponentInChildren<Text>();
        t.text = str;


    }

    public void RandomJoke()
    {
        string[] Jokes = {
            "Попку припекло!",
            "Да у тебя же БАТТХЕРТ!",
            "Азаза, затролил лалку!"
        };

        Comment( Jokes[ Random.Range(0, Jokes.Length)]);

    }   
}
