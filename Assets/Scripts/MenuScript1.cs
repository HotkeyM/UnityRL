using UnityEngine;
using System.Collections;

public class MenuScript1 : MonoBehaviour {

    public void StartGame()
    {
        LevelManagerScript.currentLevel = 0;

        Application.LoadLevel("generated");
    }

    public void Exit()
    {
        Application.Quit();
    }

	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(GameObject.Find("Music"));

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
