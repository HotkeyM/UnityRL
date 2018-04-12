using UnityEngine;
using System.Collections;

public class KeyScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            foreach (var v in GameObject.FindGameObjectsWithTag("LockedDoor"))
            {
                Instantiate(Resources.Load("Prefabs/ladder"), v.transform.position, v.transform.rotation);
                Destroy(v);

            }

        }

        Destroy(gameObject);
    }
}
