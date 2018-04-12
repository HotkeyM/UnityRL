using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class LootChance
{
	public GameObject loot;
	public float chance;
}

public class LootScript : MonoBehaviour
{

    bool dropped = false;

	public LootChance[] chances;

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
        if (dropped) return;
		foreach (var v in chances) 
		{
			if (UnityEngine.Random.Range (0.0f, 1.0f / LevelManagerScript.global.mobLootK) < v.chance)
			{
				GameObject l = (GameObject) Instantiate (v.loot, new Vector3 (UnityEngine.Random.Range (-0.25f, 0.25f), 0.0f, UnityEngine.Random.Range (-0.25f, 0.25f)) + transform.position, Quaternion.identity);
			}
		}

        dropped = true;
	}
}

