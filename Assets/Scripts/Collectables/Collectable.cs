using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collectable : MonoBehaviour 
{
	public Image image;
	public GameManager gm;

	private float moveSpeedMin = 2f;
	private float moveSpeedMax = 4f;

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
