using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class EnemyScript: MonoBehaviour
    {
        //public bool idleIcon;
        public Texture2D idleTexture;
        public Texture2D chaseTexture;
        public Texture2D exploreTexture;
        public Texture2D runawayTexture;

        public GameObject StateIcon;
        State prevState = State.IDLE;

        List <GameObject> mobsPool;

        List <GameObject> playersPool;

        enum State {IDLE, CHASE, EXPLORE, RUNAWAY};



        State state;

        void InitObjectsPools()
        {
			if (mobsPool == null) mobsPool = new List<GameObject>();
			else mobsPool.Clear();
			if (playersPool == null)playersPool = new List<GameObject>();
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

        List <GameObject> GetObjectsInSight(List <GameObject> objects, float distance = 20.0f)
        {
		List<GameObject> res = new List<GameObject> ();

			//Debug.LogWarning(objects.Count.ToString());

			foreach (GameObject g in objects)
			{
				if (!g)
					{InitObjectsPools();
					break;}
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



        void UpdateStateIcon()
        {
            if (state != prevState)
            {
                if (StateIcon == null) return;

                MeshRenderer renderer = (MeshRenderer)StateIcon.GetComponent<MeshRenderer>();
                
                if (state == State.IDLE) renderer.material.SetTexture("_MainTex", idleTexture);
                if (state == State.CHASE) renderer.material.SetTexture("_MainTex", chaseTexture);
                if (state == State.EXPLORE) renderer.material.SetTexture("_MainTex", exploreTexture);
                if (state == State.RUNAWAY) renderer.material.SetTexture("_MainTex", runawayTexture);

            }
            
            
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
            sum = sum / (counter*1.0f);
            gameObject.SendMessage("MoveTo", sum);
        }

		void Chase()
		{
			gameObject.SendMessage("MoveTo", playersPool[0].transform.position);
        gameObject.SendMessage("ShootFromCenter");

		}

        public void OnLowHP(Hp hp)
        {

        }

        public void OnMediumHp(Hp hp)
        {
        }

        public void OnHighHp(Hp hp)
        {
        }

       

        void Explore()
        {
        }

        /*void InitObjectsPools()
        {
            foreach (var g in GameObject.FindObjectsOfType<MobScript>())
            {
                mobsPool.Add(g.gameObject);
            }
        }*/

        void Start()
        {
            //InitObjectsPools();
			StartCoroutine (ListsUpdater());
            state = State.IDLE;

        }

        void UpdateState()
        {
			if (GetObjectsInSight (playersPool).Count > 0) state = State.CHASE;
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
                case State.EXPLORE:
                    Explore();
                    break;
            }
        }
    }

