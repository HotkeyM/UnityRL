using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextSceneScript : MonoBehaviour {

	public float timeout = 5.0f;

    public Image im; 

	// Use this for initialization
	void Start () {
	
	}

	public void NextLevel()
	{
		Application.LoadLevel ("Menu");
	}


	// Update is called once per frame
	void Update () {


	
		if (Time.timeSinceLevelLoad > timeout)
			NextLevel ();

	}
}
