using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public Vector3 direction;

	public float velocity = 10.0f;

	public float lifetime = 2.0f;

	public GameObject creator;

	CharacterController col;

	Rigidbody rig;

	// Use this for initialization
	void Start () {

		StartCoroutine (SelfDestruction ());
		col = (CharacterController) (GetComponent<CharacterController> ());
		rig = (Rigidbody) (GetComponent<Rigidbody> ());
	}

	IEnumerator SelfDestruction () {
		yield return new WaitForSeconds (lifetime);
		Destroy (gameObject);
	}

	IEnumerator SelfDestruction (float time) {
		yield return new WaitForSeconds (time);
		Destroy (gameObject);
	}

	// Update is called once per frame

	void Update () { }

	void OnTriggerEnter (Collider c) {
		velocity = 0.0f;
		//StartCoroutine(SelfDestruction(0.05f));
		Destroy (gameObject);

	}

	void FixedUpdate () {

		//Debug.Log ("bullet moving" + direction.ToString () + " " + velocity.ToString ());
		if (col) col.Move (Vector3.Normalize (direction) * velocity * Time.fixedDeltaTime);
		if (rig) {
			rig.MovePosition (rig.position + Vector3.Normalize (direction) * velocity * Time.fixedDeltaTime);
		}
	}

}