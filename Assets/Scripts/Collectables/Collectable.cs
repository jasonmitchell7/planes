using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collectable : MonoBehaviour 
{
	public Image image;
	public GameManager gm;

	private float moveSpeedMin = 4f;
	private float moveSpeedMax = 7f;

	public float speed;

	void Awake()
	{
		this.image = this.GetComponent<Image>();
		this.SetNewSpeed();
	}

	public void SetNewSpeed()
	{
		this.speed = Random.Range(moveSpeedMin, moveSpeedMax);
	}

}
