using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkerEnemyScript : MonoBehaviour
{

	enum State {CHASE, RUNAWAY};

	State state;

	List <Vector3> waypoints;

	List <Vector3> waypointToPlayer;

	Map m;

	GameObject player;

    DamageReciever recv;

    public float regenerateRate = 1.0f;

	IEnumerator RecountPathToPlayer()
	{
		while (true)
		{

            if (player != null) waypointToPlayer = m.GetAStarPath (transform.position, player.transform.position); 
			yield return new WaitForSeconds(1.0f);
		}
	}

	// Use this for initialization
	void Start ()
	{

        recv = (DamageReciever)GetComponent<DamageReciever>();
		player = GameObject.FindGameObjectWithTag ("Player");
		m = (Map) GameObject.Find ("Map").GetComponent<Map> ();

		StartCoroutine (RecountPathToPlayer ());

		state = State.CHASE;
		//UpdateState();

	}

    void Chase()
    {
        if (Vector3.Magnitude(Vector3.ProjectOnPlane(transform.position - player.transform.position, Vector3.up)) < 2.0f)
        {
            SendMessage("MoveTo", player.transform.position);
        }

        else
        {

            if ((waypointToPlayer == null) || (waypointToPlayer.Count < 2))
            {
                waypointToPlayer = m.GetAStarPath(transform.position, player.transform.position);
            }

            if (Vector3.Magnitude(transform.position - waypointToPlayer[0]) < 0.2f)
            {
                waypointToPlayer.RemoveAt(0);
            }

            SendMessage("MoveTo", waypointToPlayer[0]);
        }
	}

	void Runaway()
	{

        recv.hp.HP = recv.hp.HP + Time.deltaTime * regenerateRate;
        if (recv.hp.HP > recv.hp.MaxHP) recv.hp.HP = recv.hp.MaxHP;
        recv.UpdateHp();


        if ((waypoints == null) || (waypoints.Count < 2))
		{
            Debug.Log("No waytoints, generating waypoints");
            Vector3 randomTarget = m.GetRandomFloorTileVector();
            

            waypoints = m.GetAStarPath(transform.position, randomTarget);
		}
		if (Vector3.Magnitude ( Vector3.ProjectOnPlane(transform.position - waypoints [0],Vector3.up)) < 0.2f)
		{
			waypoints.RemoveAt (0);
		}

		SendMessage ("MoveTo", waypoints [0]);
	}

	void UpdateState()
	{
		DamageReciever dr = (DamageReciever)gameObject.GetComponent<DamageReciever> ();

		if (dr != null) 
		{
			if (state == State.CHASE)
			{
				if (dr.hp.HP > dr.hp.MaxHP / 2.0f)
					state = State.CHASE;
				else 
				{
					state = State.RUNAWAY;
					waypoints = m.GetAStarPath(transform.position, m.GetRandomFloorTileVector());
				}
			}

			if (state == State.RUNAWAY) 
			{
                if (dr.hp.HP > dr.hp.MaxHP * 0.9f)
                {
                    state = State.CHASE;
                    waypointToPlayer = m.GetAStarPath(transform.position, player.transform.position);
                }
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{

        if (player == null) return;

        UpdateState();

			switch (state)
			{
			case State.CHASE:
				Chase();
				break;
		case State.RUNAWAY:
				Runaway();
				break;
			}

        if (Vector3.Magnitude(Vector3.ProjectOnPlane(transform.position - player.transform.position, Vector3.up)) < 6.0f)
        {
            SendMessage("ShootToPoint", player.transform.position, SendMessageOptions.DontRequireReceiver);
        }

    }

    

}

