using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour 
{
	public GameManager gm;

	private int touchIDLeft;
	private int touchIDRight;

	void Start () 
	{
		gm = this.transform.GetComponent<GameManager>();
	}
	

	void FixedUpdate () 
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			if (!gm.isPaused() & gm.isPlayable)
				HandleInputIOS();
		}
		else
		{
			if (!gm.isPaused() & gm.isPlayable)
				HandleInputPC();
		}
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

		if (Input.GetKey("w"))
		{
			gm.planeLeft.fireMissle();
		}

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			gm.planeRight.MoveLeft(false);
		}
		
		if (Input.GetKey(KeyCode.RightArrow))
		{
			gm.planeRight.MoveRight(false);
		}

		if (Input.GetKey (KeyCode.UpArrow))
		{
			gm.planeRight.fireMissle();
		}
	}

	void HandleInputIOS()
	{
		Touch[] myTouches = Input.touches;
		for (int i = 0; i < Input.touchCount; i++ )
		{
			if ( myTouches[i].fingerId == touchIDLeft )
			{
				DoTouchLeft(myTouches[i]);
				continue;
			}

			if ( myTouches[i].fingerId == touchIDRight )
			{
				DoTouchRight(myTouches[i]);
				continue;
			}

			if ( myTouches[i].position.x >= gm.planeLeft.transform.position.x - gm.planeLeft.planeWidth*0.5f &
			     myTouches[i].position.y >= gm.planeLeft.transform.position.y - gm.planeLeft.planeWidth*0.2f &
			     myTouches[i].position.x <= gm.planeLeft.transform.position.x + gm.planeLeft.planeWidth*0.5f &
			    myTouches[i].position.y <= gm.planeLeft.transform.position.y + gm.planeLeft.planeWidth*1.8f  &
			    gm.planeLeft.gameObject.activeSelf == true)
			{
				touchIDLeft = myTouches[i].fingerId;
				continue;
			}
			     
			if ( myTouches[i].position.x >= gm.planeRight.transform.position.x - gm.planeRight.planeWidth*0.5f &
			    myTouches[i].position.y >= gm.planeRight.transform.position.y - gm.planeRight.planeWidth*0.2f  &
			    myTouches[i].position.x <= gm.planeRight.transform.position.x + gm.planeRight.planeWidth*0.5f &
			    myTouches[i].position.y <= gm.planeRight.transform.position.y + gm.planeRight.planeWidth*1.8f &
			    gm.planeRight.gameObject.activeSelf == true)
			{
				touchIDRight = myTouches[i].fingerId;
				continue;
			}

		}

	}

	void DoTouchLeft( Touch t )
	{
		if (t.position.y >= gm.planeLeft.transform.position.y + gm.planeLeft.planeWidth*0.8f )
		{
			gm.planeLeft.fireMissle();
		}

		if (t.position.x < gm.planeLeft.transform.position.x)
		{
			gm.planeLeft.MoveLeft(false);
			return;
		}

		if (t.position.x > gm.planeLeft.transform.position.x)
		{
			gm.planeLeft.MoveRight(false);
			return;
		}
	}

	void DoTouchRight( Touch t )
	{
		if (t.position.y >= gm.planeRight.transform.position.y + gm.planeRight.planeWidth*0.8f )
		{
			gm.planeRight.fireMissle();
		}
		
		if (t.position.x < gm.planeRight.transform.position.x)
		{
			gm.planeRight.MoveLeft(false);
			return;
		}
		
		if (t.position.x > gm.planeRight.transform.position.x)
		{
			gm.planeRight.MoveRight(false);
			return;
		}
	}




}
