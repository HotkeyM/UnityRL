using UnityEngine;
using System.Collections;

public class Anima : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		Animator an = GetComponent<Animator>();
		an.CrossFade("WalkFwdLoop",0.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
