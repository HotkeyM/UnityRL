using UnityEngine;
using System.Collections;

public class PowerUpScript : MonoBehaviour {

	public float manaBoost = 0.0f;
	public float maxManaBoost = 0.0f;

	public float hpBoost = 0.0f;
	public float maxHpBoost = 0.0f;

	public float speedBoost = 0.0f;
	//public float manaBoost = 0.0f;

	// Use this for initialization
	void Start () {
	
		if (GetComponent<SphereCollider> ())
			GetComponent<SphereCollider> ().enabled = false;

		StartCoroutine (ActivateCollider ());
	}

	IEnumerator ActivateCollider()
	{
		yield return new WaitForSeconds (0.5f);

		if (GetComponent<SphereCollider> ())
			GetComponent<SphereCollider> ().enabled = true;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.CompareTag ("Player")) 
		{
			MouseAndKeyboardControl cont = (MouseAndKeyboardControl)col.gameObject.GetComponent<MouseAndKeyboardControl> ();
			if (cont) {
				cont.Mana += manaBoost;
				cont.maxMana += maxManaBoost;
			}

			DamageReciever recv = (DamageReciever)col.gameObject.GetComponent<DamageReciever> ();
			if (recv) {
				recv.hp.HP += hpBoost;
				recv.hp.MaxHP += maxHpBoost;

                if (recv.hp.HP > recv.hp.MaxHP) recv.hp.HP = recv.hp.MaxHP;


                recv.UpdateHp ();
			}

			Destroy (gameObject);
		}

	}

	bool TryLuck()
	{
		return true;
	}

	// Update is called once per frame
	void Update () {
		
	
	}
}
