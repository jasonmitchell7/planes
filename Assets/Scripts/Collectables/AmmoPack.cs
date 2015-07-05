using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoPack : Collectable 
{

	private int amountMin = 1;
	private int amountMax = 3;
	
	void OnTriggerEnter2D( Collider2D col )
	{
		col.gameObject.GetComponent<Plane>().GiveMoreAmmo(Random.Range(amountMin,amountMax));
		
		gm.cm.UseAmmoPack(this);
	}

}
