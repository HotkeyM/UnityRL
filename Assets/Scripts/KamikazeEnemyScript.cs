using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class KamikazeEnemyScript : MonoBehaviour {

    

    State state = State.IDLE;

    List<GameObject> mobsPool;

    List<GameObject> playersPool;

    public float period = 2.0f;

    public float amp = 3.0f;

    CharacterController cont;
    SpriteRenderer rend;

    enum State { IDLE, CHASE };
    void InitObjectsPools()
    {
        if (mobsPool == null) mobsPool = new List<GameObject>();
        else mobsPool.Clear();
        if (playersPool == null) playersPool = new List<GameObject>();
        else playersPool.Clear();

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Mob"))
        {
            mobsPool.Add(g);
        }

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            playersPool.Add(g);
        }
    }

    List<GameObject> GetObjectsInSight(List<GameObject> objects, float distance = 20.0f)
    {
        List<GameObject> res = new List<GameObject>();

        //Debug.LogWarning(objects.Count.ToString());

        foreach (GameObject g in objects)
        {
            if (!g)
            {
                InitObjectsPools();
                break;
            }
        }

        foreach (GameObject g in objects)
        {
            if (!g)
            {
                InitObjectsPools();
            }
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(new Ray(transform.position, g.transform.position - transform.position), out hit, distance))
            {
                if (hit.collider.gameObject == g)
                {
                    res.Add(g);
                }
            }
            //if (RaycastHit)
        }

        return res;
    }

    void Idle()
    {
        Vector3 sum = new Vector3(0.0f, 0.0f, 0.0f);
        int counter = 0;
        foreach (var m in GetObjectsInSight(mobsPool))
        {
            sum += m.transform.position;
            counter++;
        }
        sum = sum / (counter * 1.0f);
        gameObject.SendMessage("MoveTo", sum);
    }

    void Chase()
    {
        gameObject.SendMessage("MoveTo", //playersPool[0].transform.position +
            transform.position + Vector3.Normalize(playersPool[0].transform.position - transform.position) + Quaternion.Euler(0.0f,90.0f,0.0f) * Vector3.Normalize(playersPool[0].transform.position - transform.position) * Mathf.Sin(Time.time/period) * Mathf.Sin(Time.time / period) * amp);
        //Vector3.Normalize(Vector3.Cross(Vector3.ProjectOnPlane(playersPool[0].transform.position, Vector3.up), Vector3.up)) * Mathf.Sin(Time.time / period) * amp * Vector3.Magnitude(playersPool[0].transform.position- transform.position));
        //gameObject.SendMessage("ShootFromCenter");
        if (Vector3.Magnitude(playersPool[0].transform.position - transform.position) < 1.3f)
        {
            SendMessage("ShootFromCenter");

            Destroy(gameObject);

        }

    }

    void Start()
    {
        rend = (SpriteRenderer)GetComponentInChildren<SpriteRenderer>();
        cont = (CharacterController)GetComponent<CharacterController>();
        //InitObjectsPools();
        StartCoroutine(ListsUpdater());
        state = State.IDLE;

    }

    void UpdateState()
    {
        if (GetObjectsInSight(playersPool).Count > 0) state = State.CHASE;
        else state = State.IDLE;
    }


    IEnumerator ListsUpdater()
    {

        while (true)
        {
            InitObjectsPools();
            yield return new WaitForSeconds(3.0f);
        }
    }

    void Update()
    {

        UpdateState();

        switch (state)
        {
            case State.IDLE:
                Idle();
                break;
            case State.CHASE:
                Chase();
                break;
            
        }
    }
}
