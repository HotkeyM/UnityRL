using UnityEngine;
using System.Collections;

public class LadderScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        
        if (col.CompareTag("Player"))
         {
			//Debug.LogAssertion ("Player tag touched trigger");
			
            LevelManagerScript levelman = FindObjectOfType<LevelManagerScript>();
            levelman.StartNextLevel();
        }
    }
}
