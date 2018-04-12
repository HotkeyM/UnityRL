using UnityEngine;
using System.Collections;

public class MouseAndKeyboardControl : MonoBehaviour {



	public Rigidbody rb;

	public CharacterController cc;

	public MoveScript ms;

    public MeshRenderer ManaBar;

    public float maxMana = 10.0f;
    public float Mana = 10.0f;

    float manaRate = 1.0f;

    bool overHeat = false;

    float smoothedSpeed = 0.0f;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
	

		ms = GetComponent<MoveScript>();

		rb = GetComponent<Rigidbody>();
		//rb.detectCollisions = true;
		//rb.isKinematic = true;	

		cc = GetComponent<CharacterController>();


        
	}

	void TryShoot()
    {
        

        

            if (!overHeat && Mana > 0.0f)
            {
                SendMessage("Shoot", SendMessageOptions.DontRequireReceiver);
                Mana = Mana - 1.0f;
            }

            if (Mana < 0.0f) overHeat = true;


            //Instantiate(Resources.Load("Prefabs/Bullet"),
        

        
    }

	// Update is called once per frame
	void Update () {
	
		if (Input.GetButtonDown("Jump"))
		{
			gameObject.SendMessage("GetDamage", 1.0f, SendMessageOptions.DontRequireReceiver);	
		}

		if (Input.GetMouseButtonDown(0))
		{
			Ray r = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(r, out hit))
			{
				//if (hit.collider.gameObject.layer == 8)
				//{
					gameObject.SendMessage("MoveTo", hit.point);

					//((NavMeshAgent)GetComponent<NavMeshAgent>()).SetDestination(hit.transform.position);
				//	}
			}
		}

        if (Input.GetMouseButtonDown(2))
        {
            LevelManagerScript levelman = FindObjectOfType<LevelManagerScript>();
            levelman.StartNextLevel();
        }

            smoothedSpeed = Mathf.Lerp(smoothedSpeed, Vector3.Magnitude(cc.velocity), Time.deltaTime);

        Vector3 v = new Vector3(Camera.allCameras[0].transform.position.x, 7.0f + smoothedSpeed * 1.0f, Camera.allCameras[0].transform.position.z);
        //v.y = 5.0f + (CharacterController)(GetComponent<CharacterController>()).velocity * 1.0f;
        Camera.allCameras[0].transform.position = v;


        if (Input.GetKeyDown(KeyCode.X))
        {
            SendMessage("RandomJoke",SendMessageOptions.DontRequireReceiver);
        }

		if (Input.GetMouseButtonDown (1)) 
		{
			TryShoot ();
		}

        UpdateMana();
        
    }

    void UpdateMana()
    {
        Mana = Mana + manaRate * Time.deltaTime;
        if (Mana > (maxMana / 2.0f)) overHeat = false;
        if (Mana > maxMana) Mana = maxMana;

        float Voffset = 0.5f;
        if (overHeat) Voffset = 0.0f;
        if (ManaBar)
        {
            ManaBar.material.mainTextureOffset = new Vector2(0.5f - ((Mana / maxMana)*0.5f), Voffset);
        }
    }

	void OnLevelWasLoaded(int level) 
	{
		DamageReciever recv = (DamageReciever)GetComponent<DamageReciever> ();
		recv.enabled = false;

		StartCoroutine(EnableDamageReciever(1.5f));

		Animator ani = (Animator)GetComponent<Animator> ();
		ani.SetBool ("initLevelAnimation", true);
	}

	IEnumerator EnableDamageReciever(float time)
	{
		yield return new WaitForSeconds (time);
		DamageReciever recv = (DamageReciever)GetComponent<DamageReciever> ();
		recv.enabled = true;

		Animator ani = (Animator)GetComponent<Animator> ();
		ani.SetBool("initLevelAnimation", false);
	}

	void FixedUpdate()
	{
		if ( Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f)
		{
			ms.movingToDestination = false;
			
			//((Rigidbody)GetComponent<Rigidbody>()).velocity = Vector3.Normalize(Input.GetAxis("Horizontal") * Vector3.forward +
			//                                                                  Input.GetAxis("Vertical") * Vector3.right) * speed;
			
			//rb.MovePosition( rb.position + Vector3.Normalize(Input.GetAxis("Vertical") * Vector3.forward +
			//                                        Input.GetAxis("Horizontal") * Vector3.right) * speed * Time.fixedDeltaTime);
			ms.cc.Move(Vector3.Normalize(Input.GetAxis("Vertical") * Vector3.forward +
			                          Input.GetAxis("Horizontal") * Vector3.right) * ms.speed * Time.fixedDeltaTime);
		}
	}




}
