﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AmmoPack : Collectable 
{

	private int amountMin = 1;
	private int amountMax = 3;
	
	void OnTriggerEnter2D( Collider2D col )
	{
		if ( col.gameObject.name == "Plane" & gm.isPlayable ) 
		{
			col.gameObject.GetComponent<Plane>().GiveMoreAmmo(Random.Range(amountMin,amountMax));

			gm.stats.AddBonusPoints(1);
			gm.stats.CalcScore();
			
			gm.cm.UseAmmoPack(this);
		}

		if( col.gameObject.name == "Missile" & gm.isPlayable )
		{
			gm.mm.DeactivateMissile( col.gameObject.GetComponent<Image>() );
			gm.cm.UseAmmoPack(this);

			Vector3 pos = image.rectTransform.position;
			pos.y -= image.rectTransform.rect.height/2;
			gm.GetExplosion(pos).Play();
		}
	}

}
