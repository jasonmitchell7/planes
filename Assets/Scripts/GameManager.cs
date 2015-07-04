using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	// Plane Variables
	public Image PlanePrefab;

	public GameObject PanelMain;
	public GameObject PanelPlanes;
	public GameObject PanelClouds;
	public GameObject PanelExplosions;

	public Plane planeLeft;
	public Plane planeRight;

	public Image planeLeftImage;
	public Image planeRightImage;

	//Background Variables
	public Image CloudPrefab;
	public List<Sprite> CloudSprites;

	private List<Image> BGImages;
	private List<Image> Clouds;

	public float BGMoveSpeed = 5f;
	public int maxClouds = 7;
	public float cloudSpawnChance = 0.8f;
	private int numClouds = 0;

	// Explosion Variables
	public ParticleSystem ExplosionPrefab;
	public List<ParticleSystem> Explosions;

	void Start () 
	{
		PanelMain = this.transform.FindChild("PanelMain").gameObject;
		PanelPlanes = PanelMain.transform.FindChild("Planes").gameObject;
		PanelClouds = PanelMain.transform.FindChild("Clouds").gameObject;

		CreateNewPlanes();

		BGImages = new List<Image>();
		Clouds = new List<Image>();

		Explosions = new List<ParticleSystem>();

	}
	

	void Update () 
	{
		MoveBG ();
	}

	void CreateNewPlanes()
	{
		float sideOffset = 120f;
		float bottomOffset = 40f;

		if (planeLeftImage == null)
		{
			planeLeftImage = Instantiate(PlanePrefab) as Image;
			planeLeftImage.transform.SetParent( PanelPlanes.transform );
			planeLeftImage.gameObject.name = "PlaneLeft";
			planeLeftImage.transform.position = new Vector3(sideOffset - 568, bottomOffset - 320, 0f);
			planeLeft = planeLeftImage.gameObject.GetComponent<Plane>();
			planeLeft.gm = this;
			planeLeft.isLeft = true;
			planeLeft.infoBar = PanelMain.transform.FindChild("InfoBars").FindChild("Left").GetComponent<InfoBar>();
			planeLeft.infoBar.SetPlane(planeLeft);
			planeLeft.infoBar.Update();

		}
		else
		{
			planeLeftImage.gameObject.SetActive( true );
			planeLeftImage.transform.position = new Vector3(sideOffset - 568, bottomOffset - 320, 0f);
		}

		if (planeRightImage == null)
		{
			planeRightImage = Instantiate(PlanePrefab) as Image;
			planeRightImage.transform.SetParent( PanelPlanes.transform );
			planeRightImage.gameObject.name = "PlaneRight";
			planeRightImage.transform.position = new Vector3(Screen.width - sideOffset - planeRightImage.rectTransform.rect.width - 568, bottomOffset - 320, 0f);
			planeRight = planeRightImage.gameObject.GetComponent<Plane>();
			planeRight.gm = this;
			planeRight.isLeft = false;
			planeRight.infoBar = PanelMain.transform.FindChild("InfoBars").FindChild("Right").GetComponent<InfoBar>();
			planeRight.infoBar.SetPlane(planeRight);
			planeRight.infoBar.Update();
		}
		else
		{
			planeRightImage.gameObject.SetActive( true );
			planeRightImage.transform.position = new Vector3(Screen.width - sideOffset - planeRightImage.rectTransform.rect.width - 568, bottomOffset - 320, 0f);
		}

	}
	

	void MoveBG()
	{
		//( Random.Range(0.0f, 1.0f) <= cloudSpawnChance ) &&

		float r = Random.Range (0.0f, 1.0f);

		if ( (r <= cloudSpawnChance) & (numClouds < maxClouds) )
		{
			Image c = GetNewCloud();
			BGImages.Add( c );
			numClouds++;
		}

		List<Image> ImagesToRemove = new List<Image>();

		foreach (Image i in BGImages)
		{
			Vector3 pos = i.gameObject.transform.position;
			pos.y = pos.y - BGMoveSpeed;
			if (pos.y >= 0 )
			{
				i.gameObject.transform.position = pos;
			}
			else
			{
				ImagesToRemove.Add(i);
			}
		}

		foreach (Image i in ImagesToRemove)
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
			c = Instantiate(CloudPrefab);
			c.transform.SetParent(PanelClouds.transform);
			Vector3 pos = new Vector3(Random.Range (0f, Screen.width), Screen.height + c.rectTransform.rect.height, 0);
			c.transform.position = pos;
			c.gameObject.name = "Cloud";
			c.sprite = CloudSprites[Random.Range(0,CloudSprites.Count)];
			if (Random.Range(0,1) == 1)
			{
				Vector3 scale = c.rectTransform.localScale;
				scale.x = -scale.x;
				c.rectTransform.localScale = scale;
			}
		}

		return c;
	}

	public ParticleSystem GetExplosion(Vector3 pos)
	{

		foreach (ParticleSystem ps in Explosions)
		{
			if (!ps.isPlaying)
			{
				ps.transform.position = pos;
				return ps;
			}
		}

		ParticleSystem exp = Instantiate(ExplosionPrefab) as ParticleSystem;
		exp.transform.SetParent(PanelExplosions.transform);
		exp.transform.position = pos;
		exp.GetComponent<Renderer>().sortingLayerName = "Foreground";
		exp.GetComponent<Renderer>().sortingOrder = 999;
		Explosions.Add (exp);


		return exp;
	}

}
