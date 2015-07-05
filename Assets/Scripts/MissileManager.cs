using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MissileManager : MonoBehaviour 
{
	public GameManager gm;

	private List<Image> missilesAvailable;
	private List<Image> missilesInUse;
	
	public float speed = 4f;

	
	void Start()
	{
		missilesAvailable = new List<Image>();
		missilesInUse = new List<Image>();
	}
	
	public void MoveMissiles()
	{
		List<Image> MissilesToRemove = new List<Image>();

		
		foreach (Image mis in missilesInUse)
		{
			Vector3 pos = mis.gameObject.transform.position;
			pos.y = pos.y + speed*gm.gameSpeed;
			
			if ( pos.y <= Screen.height )
			{
				mis.gameObject.transform.position = pos;
			}
			else
			{
				MissilesToRemove.Add(mis);
			}

		}
		
		
		foreach (Image mis in MissilesToRemove)
		{
			DeactivateMissile(mis);
		}
		
	}


	public void NewMissile(Vector3 pos)
	{
		Image mis;
		
		if ( missilesAvailable.Count > 0 )
		{
			mis = missilesAvailable[0];
			mis.transform.position = pos;
			missilesAvailable.Remove( mis );
			mis.gameObject.SetActive(true);
		}
		else
		{
			mis = Instantiate(gm.MissilePrefab);
			mis.transform.SetParent(gm.PanelMissiles.transform);
			mis.transform.position = pos;
			mis.gameObject.name = "Missile";		
		}

		missilesInUse.Add(mis);
	}

	public void DeactivateMissile( Image mis )
	{
		missilesAvailable.Add(mis);
		missilesInUse.Remove(mis);
		mis.gameObject.SetActive(false);
	}
}
