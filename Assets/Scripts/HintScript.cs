using UnityEngine;
using System.Collections;

public class HintScript : MonoBehaviour {

	public string hint = "Тестовая подсказка";

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter(Collider col)
	{

		SendMessage ("Comment", hint);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
