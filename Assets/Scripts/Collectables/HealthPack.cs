using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthPack : Collectable
{
	private int healAmount = 10;

	void OnTriggerEnter2D( Collider2D col )
	{
		col.gameObject.GetComponent<Plane>().HealPlane(healAmount);

		gm.cm.UseHealthPack(this);
	}

}
