using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	Map map;

	float t;

	Animator ani;

	bool useAnimation = true;

	enum State {Idle, Moving, Attacking, Using, Other};

	State state = State.Idle;

	int currentRotation = 0;

	int X, Y;

	// Use this for initialization
	void Start () {
	
		map = (Map) GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
		ani = GetComponentInChildren<Animator>();


	}

	public void SetCoordinates(int x, int y, int rot = 0)
	{
		X =x;
		Y = y;

		currentRotation = rot;

		transform.position = new Vector3(x*1.0f,0.0f,y*1.0f);
		Quaternion r = new Quaternion();

		r.SetLookRotation(new Vector3(0.0f,0.0f,1.0f));


		//for (int i = 0; i < currentRotation; ++i)
		//{
			r.eulerAngles = new Vector3 (0, 90 * currentRotation, 0);
		//}
		transform.rotation = r;

	}

	bool IsPlayersTurn()
	{
		return true;
	}

	bool IsPlayerInputReady()
	{
		if (state == State.Idle)
		return true;
		else return false;
	}

	void Move()
	{
		float time = (Time.time - t);
	
		float tileMoveSpeed = 0.3f;
		float tileLength = 1.0f;

		if (time >= tileMoveSpeed)
		{
			state = State.Idle;

			if (currentRotation == 0) Y++;
			if (currentRotation == 1) X++;
			if (currentRotation == 2) Y--;
			if (currentRotation == 3) X--;

			SetCoordinates(X,Y, currentRotation);


			if (useAnimation)
			{
				// ЧЕКИНГ КЛАВИАТУРЫ, ГРЯЗНЫЙ ХАК
				if ( (System.Math.Abs(Input.GetAxis("Vertical")) < 0.1f) && (System.Math.Abs(Input.GetAxis("Horizontal")) < 0.1f))

				ani.SetBool("moving", false);
			//ani.CrossFade("idle",0.1f);
			}
		}

		Vector3 deltaPos = new Vector3();

		if (currentRotation == 0) deltaPos = new Vector3(0.0f, 0.0f, 1.0f);
		if (currentRotation == 1) deltaPos = new Vector3(1.0f, 0.0f, 0.0f);
		if (currentRotation == 2) deltaPos = new Vector3(0.0f, 0.0f, -1.0f);
		if (currentRotation == 3) deltaPos = new Vector3(-1.0f, 0.0f, 0.0f);


		transform.position +=  deltaPos * ((Time.deltaTime / tileMoveSpeed) * tileLength);


	}

	bool TestMovement(int direction)
	{
		switch (direction)
		{
		case 0:
			if (map.m_data[X,Y+1] != Map.Tile.Floor) return false;
			else return true;
			break;
		case 1:
		
			if (map.m_data[X +1,Y] != Map.Tile.Floor) return false;
			else return true;
			break;
		case 2:
			if (map.m_data[X,Y-1] != Map.Tile.Floor) return false;
			else return true;
			break;
		case 3:
			if (map.m_data[X-1,Y] != Map.Tile.Floor) return false;
			else return true;
			break;
		default:
			return false;
		}
	}

	void StartMove(int direction)
	{
		if (!TestMovement(direction)) return;
		//проверить на стены надо
		state = State.Moving;
		currentRotation = direction;

		Quaternion r = new Quaternion();
		
		r.SetLookRotation(new Vector3(0.0f,0.0f,1.0f));
		

		r.eulerAngles = new Vector3 (0, 90 * currentRotation, 0);
		transform.rotation = r;

		t = Time.time;

		if (useAnimation)
		{
			ani.SetBool("moving",true);
			//ani.CrossFade("walk",0.1f);
		}
	}
	

	void CheckAnimation()
	{

	}

	// Update is called once per frame
	void Update () {


		if (IsPlayersTurn())
		{
		if (Input.GetKeyDown("space"))
		{
			useAnimation = !useAnimation;
			if (!useAnimation)
			{
				ani.StopPlayback();
			}
		}

		switch (state)
		{
		case State.Idle:



			if (Input.GetAxis("Vertical") > 0.1f) 
			{
				StartMove (0);
				break;
			}
			if (Input.GetAxis("Vertical") < -0.1f) 
			{
				StartMove (2);
				break;
			}
			if (Input.GetAxis("Horizontal") > 0.1f) StartMove (1);
			if (Input.GetAxis("Horizontal") < -0.1f) StartMove (3);
			
				if ( (System.Math.Abs(Input.GetAxis("Vertical")) < 0.1f) && (System.Math.Abs(Input.GetAxis("Horizontal")) < 0.1f))
					
					ani.SetBool("moving", false);

			break;
		case State.Moving:
			Move();
			break;
		default:
			break;
		}
		}


	}
}
