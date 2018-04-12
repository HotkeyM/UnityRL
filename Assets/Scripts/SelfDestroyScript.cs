using UnityEngine;
using System.Collections;

public class SelfDestroyScript : MonoBehaviour {

    public float time = 1.5f;

	// Use this for initialization
	void Start () {
        StartCoroutine(SelfDestruction(time));
    }


    IEnumerator SelfDestruction(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
