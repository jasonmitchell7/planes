using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour 
{
	public GameManager gm;
	
	private List<Image> obstaclesAvailable;
	private List<Image> obstaclesInUse;
	private List<float> obstacleSpeeds;

	public float speedMin = 2f;
	public float speedMax = 6f;

	public int obstaclesMax = 20;
	public float spawnChance = 0.03f;

	void Start()
	{
		obstaclesAvailable = new List<Image>();
		obstaclesInUse = new List<Image>();
		obstacleSpeeds = new List<float>();
	}

	public void MoveObstacles()
	{
		
		CheckSpawn();
		
		MoveAll();
		
	}
	

	void CheckSpawn()
	{
		
		if ( (Random.Range (0.0f, 1.0f) <= spawnChance) & (obstaclesInUse.Count < obstaclesMax) )
		{
			Image obs = GetNewObstacle();
			ActivateObstacle( obs );
		}
	}

	void ActivateObstacle(Image obs )
	{
		obstaclesInUse.Add( obs );
		obstacleSpeeds.Add( Random.Range( speedMin, speedMax ) );
		obs.gameObject.SetActive(true);
	}

	public void DeactiveateObstacle( Image obs )
	{
		int i = obstaclesInUse.IndexOf(obs);
		obstaclesInUse.RemoveAt( i );
		obstacleSpeeds.RemoveAt( i );

		obs.gameObject.SetActive (false);
	}

	Image GetNewObstacle()
	{
		Image obs;
		
		if ( obstaclesAvailable.Count > 0 )
		{
			obs = obstaclesAvailable[0];
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + obs.rectTransform.rect.height, 0);
			obs.transform.position = pos;
			obstaclesAvailable.Remove(obs);
		}
		else
		{
			obs = Instantiate(gm.ObstaclePrefab);
			obs.transform.SetParent(gm.PanelObstacles.transform);
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + obs.rectTransform.rect.height, 0);
			obs.transform.position = pos;
			obs.gameObject.name = "Obstacle";	
			obs.sprite = gm.ObstacleSprites[Random.Range(0,gm.ObstacleSprites.Count)];
			Obstacle obsComp = obs.GetComponent<Obstacle>();
			obsComp.gm = gm;
			obsComp.image = obs;

		}
		
		return obs;
	}

	void MoveAll()
	{
		List<Image> ObstaclesToRemove = new List<Image>();

		int i = 0;

		foreach (Image obs in obstaclesInUse)
		{
			Vector3 pos = obs.gameObject.transform.position;
			pos.y = pos.y - obstacleSpeeds[i]*gm.gameSpeed;

			if ( pos.y >= 0 )
			{
				obs.gameObject.transform.position = pos;
			}
			else
			{
				ObstaclesToRemove.Add(obs);
			}

			i++;
		}
		
		
		foreach (Image obs in ObstaclesToRemove)
		{
			DeactiveateObstacle(obs);
		}
	}


}
