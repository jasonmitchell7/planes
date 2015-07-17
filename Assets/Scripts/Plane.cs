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
	public int planesCollideDamage = 25;
	public float rotationSpeed = 1;

	private bool isMoving = false;
	private bool hasControl = true;
	private bool isDying = false;

	private bool _isReloading = false;

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
				RotateLeft(10f/gm.gameSpeed);
			}
			else
			{
				RotateRight (10f/gm.gameSpeed);
			}
		}
	}

	void RotateMiddle(float speedMod)
	{
		Quaternion q = Quaternion.Slerp (this.transform.rotation,Quaternion.Euler(0,0,0), Time.deltaTime*rotationSpeed*speedMod*gm.gameSpeed);
		q = Quaternion.Euler (new Vector3(0f,0f,q.eulerAngles.z));
		this.transform.rotation = q;
	}

	void RotateLeft(float speedMod)
	{
		Quaternion q = Quaternion.Slerp (this.transform.rotation,Quaternion.Euler(0,0,45), Time.deltaTime*rotationSpeed*speedMod*gm.gameSpeed);
		q = Quaternion.Euler (new Vector3(0f,0f,q.eulerAngles.z));
		this.transform.rotation = q;

	}

	void RotateRight(float speedMod)
	{
		Quaternion q = Quaternion.Slerp (this.transform.rotation,Quaternion.Euler(0,0,-45), Time.deltaTime*rotationSpeed*speedMod*gm.gameSpeed);
		q = Quaternion.Euler (new Vector3(0f,0f,q.eulerAngles.z));
		this.transform.rotation = q;
;
	}

	public void MoveLeft(bool forceMove)
	{
		Vector3 pos = this.transform.position;
		pos.x = pos.x - moveSpeed*gm.gameSpeed;

		if (forceMove)
		{
			this.transform.position = pos;
			this.isMoving = true;
			this.RotateLeft(1);

			return;
		}
		
		if (hasControl & !isDying)
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
		pos.x = pos.x + moveSpeed*gm.gameSpeed;
		
		if (forceMove)
		{
			this.transform.position = pos;
			this.isMoving = true;
			this.RotateRight(1);

			return;
		}

		if (hasControl & !isDying)
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
		if (!gm.planeLeft.gameObject.activeSelf || !gm.planeRight.gameObject.activeSelf)
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

	public void DoDamage( int damage )
	{
		health = health - damage;
		infoBar.UpdateBar();

		Vector3 pos = this.transform.position;
		pos.y += planeWidth*0.8f;

		ParticleSystem exp = gm.GetExplosion(pos);
		exp.Play();

		LoseControl();

		CheckDead();


	}

	void DoDamageBoth( int damage )
	{
		gm.planeLeft.health = gm.planeLeft.health - damage;
		gm.planeLeft.LoseControl();
		gm.planeLeft.infoBar.UpdateBar();
		gm.planeRight.health = gm.planeRight.health - damage;
		gm.planeRight.LoseControl();
		gm.planeRight.infoBar.UpdateBar();

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
		if ( health <= 0  & !isDying )
		{
			isDying = true;
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
		infoBar.gameObject.SetActive(false);

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
		isDying = false;
		this.gameObject.SetActive(false);

		if (!gm.planeLeft.gameObject.activeSelf & !gm.planeRight.gameObject.activeSelf )
			gm.GameOver();
	}

	public void HealPlane(int amount)
	{
		this.health += amount;

		if (this.health > this.maxHealth)
			this.health = this.maxHealth;

		infoBar.ShowHealthPickup();

		this.infoBar.UpdateBar();
	}

	public void GiveMoreAmmo(int amount)
	{
		this.ammo += amount;
		
		if (this.ammo > this.maxAmmo)
			this.ammo = this.maxAmmo;

		infoBar.ShowAmmoPickup();
		EvaluateRedButton();
		
		this.infoBar.UpdateBar();
	}

	public void fireMissle()
	{
		if ( ammo > 0 & !_isReloading )
		{
			Vector3 posLeft = this.transform.position;
			posLeft.y += planeWidth*0.6f;
			Vector3 posRight = posLeft;
			posLeft.x -= planeWidth*0.25f;
			posRight.x += planeWidth*0.25f;

			gm.mm.NewMissile(posLeft);
			gm.mm.NewMissile(posRight);

			ammo--;
			infoBar.UpdateBar();

			_isReloading = true;
			EvaluateRedButton();
			Invoke("Reload", 0.5f);

		}
	}

	void EvaluateRedButton()
	{
		if (_isReloading == false & ammo > 0 )
		{
			infoBar.ShowRedButton();
		}
		else
		{
			infoBar.HideRedButton();
		}
	}

	void Reload()
	{
		_isReloading = false;
		EvaluateRedButton();
	}

	public void ResetSupplies()
	{
		health = maxHealth;
		ammo = maxAmmo;
		infoBar.UpdateBar();
	}

}
