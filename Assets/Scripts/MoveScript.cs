using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour 
{

	public const float MIN_STRAFE_SPEED = 0.2f;

	public const float LERP_SPEED = 5.0f;

	public const float FLIP_DELTA = 10.0f;

	public float speed = 5.0f;

	bool flipped = false;

	Vector3 destination;

	public bool movingToDestination;

	public Rigidbody rb;
	
	public CharacterController cc;

	public void Update()
	{
	}

	public void Start()
	{
		rb = GetComponent<Rigidbody>();
		//rb.detectCollisions = true;
		//rb.isKinematic = true;	
		
		cc = GetComponent<CharacterController>();
	}

	public void FixedUpdate()
	{
		if (movingToDestination)
		{
            //((Rigidbody)GetComponent<Rigidbody>()).velocity = Vector3.Normalize(destination - transform.position) * speed;
            //rb.MovePosition( rb.position + Vector3.Normalize(destination - transform.position) * speed * Time.fixedDeltaTime);

            //cc.Move(Vector3.ProjectOnPlane( Vector3.Normalize(destination - transform.position), Vector3.up) * speed * Time.fixedDeltaTime);
            cc.Move((Vector3.ProjectOnPlane(Vector3.Normalize(destination - transform.position), Vector3.up) + new Vector3(0.0f, 1.08f - transform.position.y, 0.0f)) * speed * Time.fixedDeltaTime);

            //movingToDestination = false;

            if ((transform.position - destination).sqrMagnitude < 0.1f) movingToDestination = false;
		}


        NormalizeYPos();    
		
		UpdateRotation();

	}

    void NormalizeYPos(float preferedPos = 1.08f)
    {
        
        //cc.Move(new Vector3(0.0f, preferedPos - transform.position.y, 0.0f)* Time.fixedDeltaTime);
    }

	public void MoveTo(Vector3 dest)
	{
		destination = dest;
		movingToDestination = true;
	}

	//public void MoveToDirection()

	public void UpdateRotation()
	{
		Rigidbody body = GetComponent<Rigidbody>();
		//if (body.velocity.magnitude > MIN_STRAFE_SPEED)
		if (cc.velocity.magnitude > MIN_STRAFE_SPEED)
		{
			// поворачиваеим спрайт
			
			Quaternion spriteRotation = ((SpriteRenderer)(GetComponentInChildren<SpriteRenderer>())).gameObject.transform.localRotation;
			
			float zRotation = 0.0f;
			Vector3 up = Vector3.forward;
			spriteRotation.ToAngleAxis( out zRotation, out up);
			up = Vector3.forward;
			
			//float desiredAngle = Vector3.Angle(Vector3.right, Vector3.Normalize(body.velocity));
			
			float desiredAngle = Vector3.Angle(Vector3.right, Vector3.Normalize(cc.velocity));
			
			
			float newAngle =  Mathf.LerpAngle(zRotation, desiredAngle, LERP_SPEED * Time.fixedDeltaTime);
			
			((SpriteRenderer)(GetComponentInChildren<SpriteRenderer>())).gameObject.transform.localRotation = Quaternion.AngleAxis(newAngle, up) ;
			
			//проверяем, чтобы спрайт не ходил спиной вперед
			
			if (!flipped)
			{
				if (Mathf.Abs(Mathf.DeltaAngle(0.0f, newAngle)) > 90.0f + FLIP_DELTA)
				{
					Flip(false);
				}
				//if ((newAngle > 90.0f + FLIP_DELTA) || (newAngle < -90.0f - FLIP_DELTA))
			}
			else
			{
				if (Mathf.Abs(Mathf.DeltaAngle(180.0f, newAngle)) > 90.0f + FLIP_DELTA)
				{
					Flip(true);
				}
			}
		}
	}
	
	public void Flip(bool b)	
	{
		Vector3 vec = ((SpriteRenderer)(GetComponentInChildren<SpriteRenderer>())).gameObject.transform.localScale;
		if (b) vec.y = 1.0f;
		else vec.y = -1.0f;
		((SpriteRenderer)(GetComponentInChildren<SpriteRenderer>())).gameObject.transform.localScale = vec;
		
		flipped = !b;
	}
}
