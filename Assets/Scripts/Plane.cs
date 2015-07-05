using UnityEngine;
using System.Collections;

public class Plane : MonoBehaviour 
{

	public float moveSpeed = 3;
	public float planeWidth = 100f;
	public GameManager gm;
	public bool isLeft;
	public int maxHealth;
	public int health;
	public int maxAmmo;
	public int ammo;
	public int planesCollideDamage = 5;
	public float rotationSpeed = 1;

	private bool isMoving = false;
	private bool hasControl = true;

	public InfoBar infoBar;

	private int explosionCount;

	void Awake()
	{
		maxHealth = 100;
		health = maxHealth;
		maxAmmo = 10;
		ammo = maxAmmo;
		hasControl = true;
	}
	void Update()
	{
		if (!isMoving)
		{
			RotateMiddle(4);
		}
		else
		{
			isMoving = false;
		}

		if (!hasControl)
		{
			if (Random.Range (0f,1f) < 0.5 )
			{
				RotateLeft(10);
			}
			else
			{
				RotateRight (10);
			}
		}
	}

	void RotateMiddle(float speedMod)
	{
		Quaternion q = Quaternion.Slerp (this.transform.rotation,Quaternion.Euler(0,0,0), Time.deltaTime*rotationSpeed*speedMod);
		q = Quaternion.Euler (new Vector3(0f,0f,q.eulerAngles.z));
		this.transform.rotation = q;
	}

	void RotateLeft(float speedMod)
	{
		Quaternion q = Quaternion.Slerp (this.transform.rotation,Quaternion.Euler(0,0,45), Time.deltaTime*rotationSpeed*speedMod);
		q = Quaternion.Euler (new Vector3(0f,0f,q.eulerAngles.z));
		this.transform.rotation = q;

	}

	void RotateRight(float speedMod)
	{
		Quaternion q = Quaternion.Slerp (this.transform.rotation,Quaternion.Euler(0,0,-45), Time.deltaTime*rotationSpeed*speedMod);
		q = Quaternion.Euler (new Vector3(0f,0f,q.eulerAngles.z));
		this.transform.rotation = q;
;
	}

	public void MoveLeft(bool forceMove)
	{
		Vector3 pos = this.transform.position;
		pos.x = pos.x - moveSpeed;

		if (forceMove)
		{
			this.transform.position = pos;
			this.isMoving = true;
			this.RotateLeft(1);

			return;
		}
		
		if (hasControl)
		{
			if ( CheckPos(pos) )
			{
				this.transform.position = pos;
				this.isMoving = true;
				this.RotateLeft(1);
			}
		}
	}

	public void MoveRight(bool forceMove)
	{
		Vector3 pos = this.transform.position;
		pos.x = pos.x + moveSpeed;
		
		if (forceMove)
		{
			this.transform.position = pos;
			this.isMoving = true;
			this.RotateRight(1);

			return;
		}

		if (hasControl)
		{
			if ( CheckPos(pos) )
			{
				this.transform.position = pos;
				this.isMoving = true;
				this.RotateRight(1);
			}
		}
	}
	
	bool CheckPos(Vector3 pos)
	{
		// Check off screen left.
		if (pos.x - planeWidth/2 < 0)
		{
			return false;
		}

		// Check off screen right.
		if (pos.x > Screen.width - planeWidth/2 )
		{
			return false;
		}

		// Check if planes collide with each other.
		if (!gm.planeLeft.gameObject.activeSelf || !gm.planeLeft.gameObject.activeSelf)
			return true;

		if (isLeft)
		{
			if (gm.planeRight.transform.position.x <= pos.x + planeWidth )
			{
				DoDamageBoth(planesCollideDamage);
				return false;
			}
		}
		else
		{
			if (gm.planeLeft.transform.position.x + planeWidth >= pos.x )
			{
				DoDamageBoth(planesCollideDamage);
				return false;
			}
		}

		return true;
	}

	void DoDamage( int damage )
	{
		health = health - damage;

	}

	void DoDamageBoth( int damage )
	{
		gm.planeLeft.health = gm.planeLeft.health - damage;
		gm.planeLeft.LoseControl();
		gm.planeLeft.infoBar.Update();
		gm.planeRight.health = gm.planeRight.health - damage;
		gm.planeRight.LoseControl();
		gm.planeRight.infoBar.Update();

		Vector3 pos = gm.planeRight.transform.position;
		pos.y += planeWidth/2 + 5f;
		pos.x -= planeWidth/2;

		ParticleSystem exp = gm.GetExplosion(pos);
		exp.Play();

		gm.planeLeft.MoveLeft(true);
		gm.planeRight.MoveRight(true);

		gm.planeLeft.CheckDead();
		gm.planeRight.CheckDead();

	}

	void CheckDead()
	{
		if (health <= 0 )
		{
			BeginDestroy();
		}
	}

	void LoseControl()
	{
		hasControl = false;

		Invoke("RegainControl", 0.5f);
	}

	void RegainControl()
	{
		hasControl = true;
	}

	void BeginDestroy()
	{
		hasControl = false;

		explosionCount = Random.Range(4,6);
		DestroyExplosions();

	}

	void DestroyExplosions()
	{
		Vector3 pos = this.transform.position;
		pos.x += Random.Range(-50f, 100f);
		pos.y += Random.Range(-80f, 80f);

		ParticleSystem exp = gm.GetExplosion(pos);
		exp.Play();

		explosionCount--;

		if (explosionCount <= 0)
		{
			EndDestroy();
		}
		else
		{
			Invoke("DestroyExplosions",Random.Range(0.2f, 0.3f));
		}
	}

	void EndDestroy()
	{
		this.gameObject.SetActive(false);
	}

	public void HealPlane(int amount)
	{
		this.health += amount;

		if (this.health > this.maxHealth)
			this.health = this.maxHealth;

		this.infoBar.Update();
	}

	public void GiveMoreAmmo(int amount)
	{
		this.health += amount;
		
		if (this.ammo > this.maxAmmo)
			this.ammo = this.maxAmmo;
		
		this.infoBar.Update();
	}
}
