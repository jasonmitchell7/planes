﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Obstacle : MonoBehaviour
{
	public GameManager gm;

	public Image image;

	private int damageAmount = 10;
	
	void OnTriggerEnter2D( Collider2D col )
	{
		if ( col.gameObject.name == "Plane" & gm.isPlayable)
		{
			if (gm.planeLeft.gameObject.activeSelf & gm.planeRight.gameObject.activeSelf)
			{
				col.gameObject.GetComponent<Plane>().DoDamage(damageAmount);
			}
			else
			{
				col.gameObject.GetComponent<Plane>().DoDamage(damageAmount*2);
			}

			gm.om.DeactiveateObstacle(image);
		}

		if( col.gameObject.name == "Missile" & gm.isPlayable)
		{
			gm.mm.DeactivateMissile( col.gameObject.GetComponent<Image>() );
			gm.om.DeactiveateObstacle(image);
			
			Vector3 pos = image.rectTransform.position;
			pos.y -= image.rectTransform.rect.height/2;
			gm.GetExplosion(pos).Play();

			gm.stats.obstaclesDestroyed++;
			gm.stats.AddBonusPoints(2);
			gm.stats.CalcScore();
		}
	}

}
