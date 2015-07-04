using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour 
{
	public GameManager gm;

	void Start () 
	{
		gm = this.transform.GetComponent<GameManager>();
	}
	

	void FixedUpdate () 
	{
		HandleInputPC();
	}

	void HandleInputPC()
	{
		if (Input.GetKey("a"))
		{
			gm.planeLeft.MoveLeft(false);
		}

		if (Input.GetKey("d"))
		{
			gm.planeLeft.MoveRight(false);
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			gm.planeRight.MoveLeft(false);
		}
		
		if (Input.GetKey(KeyCode.RightArrow))
		{
			gm.planeRight.MoveRight(false);
		}
	}
}
