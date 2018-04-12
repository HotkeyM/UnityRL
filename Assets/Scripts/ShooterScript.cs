using UnityEngine;
using System.Collections;

public class ShooterScript : MonoBehaviour {

	public GameObject bulletPrefab;
    public float Cooldown = 0.5f;
    public float BulletSpeed = 10.0f;
    public float bulletLifetime = 2.0f;
    

    float lastTimeShoot;

    CharacterController cc;

	// Use this for initialization
	void Start () {

        cc = (CharacterController)GetComponent<CharacterController>();
        lastTimeShoot = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	
		
	}

    public void Shoot()
    {
        if (Time.time - lastTimeShoot > Cooldown)
        {
            lastTimeShoot = Time.time;
            // Узнаем вектор, в котором надо выпустить снаряд.
            Ray r = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
                Vector3 direction = new Vector3(hit.point.x - transform.position.x, 0.0f, hit.point.z - transform.position.z);
                direction = Vector3.Normalize(direction);

                //Узнаем размер чарактер контроллера
                float radius = cc.radius;
                //GameObject b = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet"), transform.position + direction * radius, Quaternion.identity);
                GameObject b = (GameObject)Instantiate(bulletPrefab, transform.position + direction * radius + new Vector3(0.0f, 0.50f, 0.0f), Quaternion.identity);

                BulletScript bullet = (BulletScript)b.GetComponent<BulletScript>();


                bullet.direction = direction;
                bullet.velocity = BulletSpeed;
                bullet.creator = gameObject;
                bullet.lifetime = bulletLifetime;

                DamageDealer dd = (DamageDealer)b.GetComponent<DamageDealer>();
                dd.source = gameObject;

                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

                foreach (var v in GameObject.FindGameObjectsWithTag("Bullet"))
                {
                    Physics.IgnoreCollision(bullet.GetComponent<CapsuleCollider>(), v.GetComponent<CapsuleCollider>());
                }
            }


        }
    }

    public void ShootToPoint(Vector3 target)
    {
        if (Time.time - lastTimeShoot > Cooldown)
        {
            lastTimeShoot = Time.time;
            // Узнаем вектор, в котором надо выпустить снаряд.
            
                Vector3 direction = new Vector3(target.x - transform.position.x, 0.0f, target.z - transform.position.z);
                direction = Vector3.Normalize(direction);

                //Узнаем размер чарактер контроллера
                float radius = cc.radius;
                //GameObject b = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet"), transform.position + direction * radius, Quaternion.identity);
                GameObject b = (GameObject)Instantiate(bulletPrefab, transform.position + direction * radius + new Vector3(0.0f, 0.50f, 0.0f), Quaternion.identity);

                BulletScript bullet = (BulletScript)b.GetComponent<BulletScript>();


                bullet.direction = direction;
                bullet.velocity = BulletSpeed;
                bullet.creator = gameObject;
                bullet.lifetime = bulletLifetime;

                DamageDealer dd = (DamageDealer)b.GetComponent<DamageDealer>();
                dd.source = gameObject;

                Physics.IgnoreCollision(bullet.GetComponent<Collider>(), GetComponent<Collider>());

                foreach (var v in GameObject.FindGameObjectsWithTag("Bullet"))
                {
                    Physics.IgnoreCollision(bullet.GetComponent<CapsuleCollider>(), v.GetComponent<CapsuleCollider>());
                }
            


        }
    }

    public void ShootFromCenter()
    {
        if (Time.time - lastTimeShoot > Cooldown)
        {
            lastTimeShoot = Time.time;
            // Узнаем вектор, в котором надо выпустить снаряд.
            Ray r = Camera.allCameras[0].ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit))
            {
               GameObject b = (GameObject)Instantiate(bulletPrefab, transform.position + new Vector3(0.0f, 0.50f, 0.0f), Quaternion.identity);

                BulletScript bullet = (BulletScript)b.GetComponent<BulletScript>();

                bullet.velocity = BulletSpeed;
                bullet.creator = gameObject;
                bullet.lifetime = bulletLifetime;

                DamageDealer dd = (DamageDealer)b.GetComponent<DamageDealer>();
                dd.source = gameObject;

                Physics.IgnoreCollision(bullet.GetComponent<CapsuleCollider>(), GetComponent<CharacterController>());

                foreach (var v in GameObject.FindGameObjectsWithTag("Bullet"))
                {
                    Physics.IgnoreCollision(bullet.GetComponent<CapsuleCollider>(), v.GetComponent<CapsuleCollider>());
                }
            }


        }
    }

  
}
