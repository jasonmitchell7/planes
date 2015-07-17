using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CollectableManager : MonoBehaviour 
{
	public GameManager gm;

	private List<AmmoPack> ammoAvailable;
	private List<AmmoPack> ammoInUse;

	private List<HealthPack> healthAvailable;
	private List<HealthPack> healthInUse;

	public float healthSpawnChance = 0.002f;
	public float ammoSpawnChance = 0.005f;

	private int healthPackMax = 3;
	private int ammoPackMax = 3;

	void Start()
	{
		ammoAvailable = new List<AmmoPack>();
		ammoInUse = new List<AmmoPack>();

		healthAvailable = new List<HealthPack>();
		healthInUse = new List<HealthPack>();
	}
	
	public void MoveCollectables()
	{
		if (gm.isPlayable)
		{
			CheckSpawn();
		}

		MoveAmmoPacks();
		MoveHealthPacks();
	
	}


	void CheckSpawn()
	{
		float r = Random.Range (0.0f, 1.0f);

		if ( (r <= healthSpawnChance*gm.gameSpeed) & (healthInUse.Count < healthPackMax) )
		{
			HealthPack hp = GetNewHealthPack();
			healthInUse.Add( hp );
		}

		r = Random.Range (0.0f, 1.0f);
		
		if ( (r <= ammoSpawnChance*gm.gameSpeed) & (ammoInUse.Count < ammoPackMax) )
		{
			AmmoPack ap = GetNewAmmoPack();
			ammoInUse.Add( ap );
		}
		
	}


	void MoveHealthPacks()
	{
		List<HealthPack> HealthPacksToRemove = new List<HealthPack>();
		
		foreach (HealthPack hp in healthInUse)
		{
			Vector3 pos = hp.gameObject.transform.position;
			pos.y = pos.y - hp.speed*gm.gameSpeed;
			if ( pos.y >= 0 )
			{
				hp.gameObject.transform.position = pos;
			}
			else
			{
				HealthPacksToRemove.Add(hp);
			}
		}
		
		
		foreach (HealthPack hp in HealthPacksToRemove)
		{
			healthAvailable.Add(hp);
			healthInUse.Remove(hp);
			hp.gameObject.SetActive(false);
		}
	}
	
	
	
	HealthPack GetNewHealthPack()
	{
		HealthPack hp;
		
		if ( healthAvailable.Count > 0 )
		{
			hp = healthAvailable[0];
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + hp.image.rectTransform.rect.height, 0);
			hp.transform.position = pos;
			hp.gameObject.SetActive(true);
			hp.SetNewSpeed();
			healthAvailable.Remove(hp);
		}
		else
		{
			hp = Instantiate(gm.HealthPackPrefab).GetComponent<HealthPack>();;
			hp.transform.SetParent(gm.PanelCollectables.transform);
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + hp.image.rectTransform.rect.height, 0);
			hp.transform.position = pos;
			hp.gm = gm;
			hp.gameObject.name = "HealthPack";	
		}
		
		return hp;
	}

	void MoveAmmoPacks()
	{
		List<AmmoPack> AmmoPacksToRemove = new List<AmmoPack>();

		foreach (AmmoPack ap in ammoInUse)
		{
			Vector3 pos = ap.gameObject.transform.position;
			pos.y = pos.y - ap.speed*gm.gameSpeed;
			if ( pos.y >= 0 )
			{
				ap.gameObject.transform.position = pos;
			}
			else
			{
				AmmoPacksToRemove.Add(ap);
			}
		}

		
		foreach (AmmoPack ap in AmmoPacksToRemove)
		{
			ammoAvailable.Add(ap);
			ammoInUse.Remove(ap);
			ap.gameObject.SetActive(false);
		}
	}



	AmmoPack GetNewAmmoPack()
	{
		AmmoPack ap;
		
		if ( ammoAvailable.Count > 0 )
		{
			ap = ammoAvailable[0];
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + ap.image.rectTransform.rect.height, 0);
			ap.transform.position = pos;
			ap.gameObject.SetActive(true);
			ap.SetNewSpeed();
			ammoAvailable.Remove(ap);
		}
		else
		{
			ap = Instantiate(gm.AmmoPackPrefab).GetComponent<AmmoPack>();;
			ap.transform.SetParent(gm.PanelCollectables.transform);
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + ap.image.rectTransform.rect.height, 0);
			ap.transform.position = pos;
			ap.gm = gm;
			ap.gameObject.name = "AmmoPack";
		}
		
		return ap;
	}

	public void UseHealthPack(HealthPack hp)
	{
		healthAvailable.Add(hp);
		healthInUse.Remove(hp);
		hp.gameObject.SetActive(false);
	}

	
	public void UseAmmoPack(AmmoPack ap)
	{
		ammoAvailable.Add(ap);
		ammoInUse.Remove(ap);
		ap.gameObject.SetActive(false);
	}
}
