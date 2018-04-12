using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {



	// Use this for initialization
	void Start () {
	


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnReplayLevelClicked()
	{
		//Current
		//if (LevelManagerScript.global.numContinues > 0)
	}

	public static GameObject InstantiateMenu()
	{
		GameObject menu = (GameObject)Instantiate (Resources.Load ("Prefabs/Menu"));

		return menu;
	}
}
