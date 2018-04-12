using UnityEngine;
using System.Collections;

public class GeneralEnemyScript : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnDeath()
	{
		LevelManagerScript.global.numEnemies--;
	}

}

