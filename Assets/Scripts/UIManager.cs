using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour 
{
	public float targetAspect = 16f/9f;

	void Start () 
	{
		Camera cam = GetComponent<Camera>();
		cam.aspect = targetAspect;
	
	}

	void Update () 
	{
	
	}
}
