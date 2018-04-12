using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShooterEnemyScript : MonoBehaviour {
    enum State { CHASE, RUNAWAY };

    State state;

    List<Vector3> waypoints;

    List<Vector3> waypointToPlayer;

    Map m;

    GameObject player;

    DamageReciever recv;

    public float regenerateRate = 1.0f;

    IEnumerator RecountPathToPlayer()
    {
        while (true)
        {

            if (player != null) waypointToPlayer = m.GetAStarPath(transform.position, player.transform.position);
            yield return new WaitForSeconds(1.0f);
        }
    }

    // Use this for initialization
    void Start()
    {

        recv = (DamageReciever)GetComponent<DamageReciever>();
        player = GameObject.FindGameObjectWithTag("Player");
        m = (Map)GameObject.Find("Map").GetComponent<Map>();

        StartCoroutine(RecountPathToPlayer());

        state = State.CHASE;
        //UpdateState();

    }

    void Chase()
    {
        
        SendMessage("MoveTo", player.transform.position);
        

        if (Vector3.Magnitude(Vector3.ProjectOnPlane(transform.position - player.transform.position, Vector3.up)) < 6.0f)
        {
            SendMessage("ShootToPoint", player.transform.position + new Vector3 (Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)), SendMessageOptions.DontRequireReceiver);
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
        if (Vector3.Magnitude(Vector3.ProjectOnPlane(transform.position - waypoints[0], Vector3.up)) < 0.2f)
        {
            waypoints.RemoveAt(0);
        }

        SendMessage("MoveTo", waypoints[0]);
    }

    void UpdateState()
    {
        DamageReciever dr = (DamageReciever)gameObject.GetComponent<DamageReciever>();

        if (dr != null)
        {
            if (Vector3.Magnitude(Vector3.ProjectOnPlane(transform.position - player.transform.position, Vector3.up)) < 8.0f)
            {
                state = State.CHASE;
            }
            else
            {
                state = State.RUNAWAY;
            }
        }
    }

    // Update is called once per frame
    void Update()
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

       

    }

}
