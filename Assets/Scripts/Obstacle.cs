using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Obstacle : MonoBehaviour
{
	public GameManager gm;

	public Image image;

	private int damageAmount = 10;
	
	void OnTriggerEnter2D( Collider2D col )
	{
		col.gameObject.GetComponent<Plane>().DoDamage(damageAmount);

		gm.om.DeactiveateObstacle(image);
	}

}
