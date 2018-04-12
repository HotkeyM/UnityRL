using System;
using System.Collections.Generic;

using UnityEngine;

class DamageDealer : MonoBehaviour
{
	
	public float MinDamage = 0.0f;
	public float MaxDamage = 0.0f;
	
	public GameObject source;

    public float attackDelta = 0.0f;

    float previousTime = 0.0f;

    List<DamageReciever> touchingRecievers;

    void Start()
    {
        touchingRecievers = new List<DamageReciever>();
    }

	public bool CheckFriendlyFire(GameObject another)
	{

        if (gameObject.CompareTag(another.tag)) return true;

        if (source) if (source.CompareTag(another.tag)) return true;


        return false;
	}

	
	public float GetDamage()
	{
		UnityEngine.Random r = new UnityEngine.Random();
		return UnityEngine.Random.value * (MaxDamage - MinDamage) + MinDamage;
	}
	
      void OnTriggerEnter(Collider col)
    {

        //ContactPoint contact = collision.contacts[0];

        //previousTime = Time.time;

        if (col.gameObject != source)
        {
			DamageReciever dr = (DamageReciever)(col.gameObject.GetComponent<DamageReciever>());
			if (dr == null) return;
            /*
            if ((Time.time - previousTime) < attackDelta)
            {
                Debug.Log("cooldown");
                return;
            }
            */
            if (CheckFriendlyFire(dr.gameObject)) return;

            /*
            previousTime = Time.time;
            touchingRecievers.Add(dr);
            */

			dr.hp.HP = dr.hp.HP - GetDamage();
			dr.UpdateHp();

            Debug.Log("BULLET HIT");
            //Destroy(gameObject);

        }
    }

    void OnTriggerExit(Collider col)
    {
        DamageReciever dr = (DamageReciever)(col.gameObject.GetComponent<DamageReciever>());
        if (dr == null) return;

        if (touchingRecievers.IndexOf(dr) != -1) touchingRecievers.Remove (dr);
    }

    /*
    void OnTriggerStay(Collider col)
    {

        if (col.gameObject.layer == 8) return;

        previousTime = Time.time;

        if (col.gameObject != source)
        {
            DamageReciever dr = (DamageReciever)(col.gameObject.GetComponent<DamageReciever>());
            if (dr == null) return;
            if ((Time.time - previousTime) < attackDelta)
            {
                Debug.Log("cooldown");
                return;
            }
            if (CheckFriendlyFire(dr.gameObject)) return;

            previousTime = Time.time;

            dr.hp.HP = dr.hp.HP - GetDamage();
            dr.UpdateHp();

            Debug.Log("BULLET HIT");
            //Destroy(gameObject);

        }
    }

    */
    void Update()
    {
        /*
        if ((Time.time - previousTime) > attackDelta)
        {
            previousTime = Time.time;
            foreach (var dr in touchingRecievers)
            {
                dr.hp.HP = dr.hp.HP - GetDamage();
                dr.UpdateHp();
            }
        }
        */
	}
	
	
}