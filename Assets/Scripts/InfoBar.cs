using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoBar : MonoBehaviour
{
	public Image Health;
	public Image Ammo;

	private Plane plane;



	public void SetPlane(Plane p)
	{
		plane = p;
	}

	public void Update()
	{
		Health.fillAmount = (float)plane.health/(float)plane.maxHealth;
		Ammo.fillAmount = (float)plane.ammo/(float)plane.maxAmmo;
	}
}
