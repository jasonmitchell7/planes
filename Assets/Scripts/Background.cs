using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Background : MonoBehaviour
{
	public GameManager gm;

	private List<Image> BGImages;
	public List<Image> Clouds;
	
	public float BGMoveSpeed = 2f;
	public int maxClouds = 20;
	public float cloudSpawnChance = 0.05f;
	private int numClouds = 0;

	void Start()
	{
		BGImages = new List<Image>();
		Clouds = new List<Image>();
	}
	
	public void MoveBG()
	{
		
		float r = Random.Range (0.0f, 1.0f);
		
		if ( (r <= cloudSpawnChance) & (numClouds < maxClouds) )
		{
			Image c = GetNewCloud();
			BGImages.Add( c );
			numClouds++;
		}
		
		List<Image> ImagesToRemove = new List<Image>();
		if (BGImages != null)
		{
			foreach (Image i in BGImages)
			{
				if (i != null)
				{
					Vector3 pos = i.gameObject.transform.position;
					pos.y = pos.y - BGMoveSpeed*gm.gameSpeed;
					if (pos.y >= 0 )
					{
						i.gameObject.transform.position = pos;
					}
					else
					{
						ImagesToRemove.Add(i);
					}
				}
			}
		}

		foreach (Image i in ImagesToRemove)
		{
			if (i != null)
			{
				if (i.gameObject.name == "Cloud")
				{
					Clouds.Add(i);
					BGImages.Remove(i);
					i.gameObject.SetActive( false );
					
					numClouds--;
				}
			}
		}
	}

	Image GetNewCloud()
	{
		Image c;

		if ( Clouds.Count > 0 )
		{
			c = Clouds[0];
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + c.rectTransform.rect.height, 0);
			c.transform.position = pos;
			c.gameObject.SetActive(true);
			Clouds.RemoveAt(0);
		}
		else
		{
			c = Instantiate(gm.CloudPrefab);
			c.transform.SetParent(gm.PanelClouds.transform);
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + c.rectTransform.rect.height, 0);
			c.transform.position = pos;
			c.gameObject.name = "Cloud";
			c.sprite = gm.CloudSprites[Random.Range(0,gm.CloudSprites.Count)];
			if (Random.Range(0,1) == 1)
			{
				Vector3 scale = c.rectTransform.localScale;
				scale.x = -scale.x;
				c.rectTransform.localScale = scale;
			}
		}
		
		return c;
	}


}
