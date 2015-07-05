﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InfoBar : MonoBehaviour
{
	public Image Health;
	public Image Ammo;
	public Image RedButton;
	public Image HealthPickup;
	public Image AmmoPickup;

	private Plane plane;

	public void SetPlane(Plane p)
	{
		plane = p;
	}

	public void UpdateBar()
	{
		Health.fillAmount = (float)plane.health/(float)plane.maxHealth;
		Ammo.fillAmount = (float)plane.ammo/(float)plane.maxAmmo;
	}

	void HidePickups()
	{
		HealthPickup.gameObject.SetActive(false);
		AmmoPickup.gameObject.SetActive(false);
	}

	public void ShowAmmoPickup()
	{
		HidePickups();
		AmmoPickup.gameObject.SetActive(true);

		Invoke("HidePickups",0.5f);
	}

	public void ShowHealthPickup()
	{
		HidePickups();
		HealthPickup.gameObject.SetActive(true);
		
		Invoke("HidePickups",0.5f);
	}

	public void ShowRedButton()
	{
		RedButton.gameObject.SetActive(true);
	}

	public void HideRedButton()
	{
		RedButton.gameObject.SetActive(false);
	}
}
