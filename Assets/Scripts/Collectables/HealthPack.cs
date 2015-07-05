using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthPack : Collectable
{
	private int healAmount = 25;

	void OnTriggerEnter2D( Collider2D col )
	{
		if ( col.gameObject.name == "Plane")
		{
			col.gameObject.GetComponent<Plane>().HealPlane(healAmount);

			gm.stats.AddBonusPoints(2);
			gm.stats.CalcScore();

			gm.cm.UseHealthPack(this);
		}

		if( col.gameObject.name == "Missile" )
		{
			gm.mm.DeactivateMissile( col.gameObject.GetComponent<Image>() );
			gm.cm.UseHealthPack(this);
			
			Vector3 pos = image.rectTransform.position;
			pos.y -= image.rectTransform.rect.height/2;
			gm.GetExplosion(pos).Play();
		}
	}

}
