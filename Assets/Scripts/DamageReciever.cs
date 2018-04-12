using System;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;



[Serializable]
public class Hp
{
    Hp()
    {
        HP = MaxHP;
    }
    public float MaxHP = 5.0f;
    public float HP = 5.0f;
}

public class DamageReciever : MonoBehaviour
{
	public MeshRenderer HealthBar;

    public Hp hp;

    public Hp GetHP()
    {
        return hp;
    }

    

    void Start()
    {
        
    }

    void Update()
    {

    }

	public void GetDamage(float damage)
	{
		hp.HP = hp.HP - damage;
		UpdateHp();
	}



    void OnCollisionEnter(Collision col)
    {
        DamageDealer dd = (DamageDealer)(col.collider.gameObject.GetComponent<DamageDealer>());
        if (dd == null) return;
        //if (dd.CheckFriendlyFire()) return;

        hp.HP = hp.HP - dd.GetDamage();
        UpdateHp();
    }

    public void UpdateHp()
    {
        if (hp.HP < hp.MaxHP / 4.0f) SendMessage("OnLowHP", hp, SendMessageOptions.DontRequireReceiver);
        if (hp.HP < 0.0f) SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);

		if (HealthBar != null)
		{
			HealthBar.material.mainTextureOffset = new Vector2(0.5f - (hp.HP/hp.MaxHP), 0.0f);
		}
	}

	public void OnDeath()
	{
		((Animator)GetComponent<Animator> ()).SetBool ("dead", true);

		if ((MoveScript)GetComponent<MoveScript> ())
			((MoveScript)GetComponent<MoveScript> ()).enabled = false;
		

		if ((CharacterController)GetComponent<CharacterController> ())
			((CharacterController)GetComponent<CharacterController> ()).enabled = false;

        if (GetComponentInChildren<Camera>()) GetComponentInChildren<Camera>().gameObject.transform.parent = null;
		StartCoroutine (DestroyLater(2.0f));

	}

	IEnumerator DestroyLater(float timetodie)
	{	
		
		while (true)
		{

			yield return new WaitForSeconds(timetodie);
			Destroy (gameObject);
		}
	}
}

