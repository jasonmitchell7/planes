using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	public float gameSpeed = 1f;

	private bool _isPaused = false;

	private bool _isPlayable;
	private int countdownNum;

	// Plane Variables
	public Image PlanePrefab;

	public GameObject PanelMain;
	public GameObject PanelPlanes;
	public GameObject PanelClouds;
	public GameObject PanelCollectables;
	public GameObject PanelExplosions;
	public GameObject PanelObstacles;
	public GameObject PanelMissiles;
	public GameObject PanelGameOverMenu;
	public GameObject PanelCountdown;

	public Plane planeLeft;
	public Plane planeRight;

	public Image planeLeftImage;
	public Image planeRightImage;


	//Background Variables
	public Background bg;
	public Image CloudPrefab;
	public List<Sprite> CloudSprites;
	

	// Explosion Variables
	public ParticleSystem ExplosionPrefab;
	public List<ParticleSystem> Explosions;

	// Collectable Variables
	public CollectableManager cm;
	public Image HealthPackPrefab;
	public Image AmmoPackPrefab;

	// Obstacle Variables
	public ObstacleManager om;
	public List<Sprite> ObstacleSprites;
	public Image ObstaclePrefab;

	// Missle Variables
	public MissileManager mm;
	public Image MissilePrefab;

	// Stats and Score Variables
	public Stats stats;
	public Text score;


	void Start () 
	{
		PanelMain = this.transform.FindChild("PanelMain").gameObject;
		PanelPlanes = PanelMain.transform.FindChild("Planes").gameObject;
		PanelClouds = PanelMain.transform.FindChild("Clouds").gameObject;
		PanelCollectables = PanelMain.transform.FindChild("Collectables").gameObject;
		PanelObstacles = PanelMain.transform.FindChild("Obstacles").gameObject;
		PanelMissiles = PanelMain.transform.FindChild("Missiles").gameObject;
		PanelGameOverMenu = this.transform.FindChild("PanelGameOver").gameObject;
		PanelCountdown = this.transform.FindChild("PanelCountdown").gameObject;

		StartNewGame();

		bg = GetComponent<Background>();
		bg.gm = this;

		cm = GetComponent<CollectableManager>();
		cm.gm = this;

		om = GetComponent<ObstacleManager>();
		om.gm = this;

		mm = GetComponent<MissileManager>();
		mm.gm = this;

		Explosions = new List<ParticleSystem>();

		stats = new Stats();
		stats.gm = this;
		stats.ResetScore();


	}
	

	void Update () 
	{
		if (!_isPaused)
		{
			if (bg != null )
			{
				bg.MoveBG ();
			}

			if (cm != null )
			{
				cm.MoveCollectables();
			}

			if (om != null )
			{
				om.MoveObstacles();
			}

			if (mm != null )
			{
				mm.MoveMissiles();
			}
		}
	}

	void CreateNewPlanes()
	{
		float sideOffset = 200f;
		float bottomOffset = 40f;

		if (planeLeftImage == null)
		{
			planeLeftImage = Instantiate(PlanePrefab) as Image;
			planeLeftImage.gameObject.transform.SetParent( PanelPlanes.transform, true );
			planeLeftImage.gameObject.name = "Plane";
			planeLeft = planeLeftImage.gameObject.GetComponent<Plane>();
			planeLeft.gm = this;
			planeLeft.isLeft = true;
			planeLeft.infoBar = PanelMain.transform.FindChild("InfoBars").FindChild("Left").GetComponent<InfoBar>();
			planeLeft.infoBar.SetPlane(planeLeft);
			planeLeft.infoBar.UpdateBar();
			planeLeftImage.rectTransform.position = new Vector3(sideOffset - 568f, bottomOffset - 320f, 0f) ;
		}
		else
		{
			planeLeft.ResetSupplies();
			planeLeftImage.gameObject.SetActive( true );
			planeLeftImage.transform.position = new Vector3(sideOffset, bottomOffset, 0f);
			planeLeft.infoBar.gameObject.SetActive(true);
		}

		if (planeRightImage == null)
		{
			planeRightImage = Instantiate(PlanePrefab) as Image;
			planeRightImage.gameObject.transform.SetParent( PanelPlanes.transform, true );
			planeRightImage.gameObject.name = "Plane";
			planeRightImage.transform.position = new Vector3(Screen.width - sideOffset - 568f, bottomOffset - 320f, 0f);
			planeRight = planeRightImage.gameObject.GetComponent<Plane>();
			planeRight.gm = this;
			planeRight.isLeft = false;
			planeRight.infoBar = PanelMain.transform.FindChild("InfoBars").FindChild("Right").GetComponent<InfoBar>();
			planeRight.infoBar.SetPlane(planeRight);
			planeRight.infoBar.UpdateBar();
		}
		else
		{
			planeRight.ResetSupplies();
			planeRightImage.gameObject.SetActive( true );
			planeRightImage.transform.position = new Vector3(Screen.width - sideOffset, bottomOffset, 0f);
			planeRight.infoBar.gameObject.SetActive(true);
		}

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

	public void PauseGame()
	{
		_isPaused = true;
	}

	public void UnpauseGame()
	{
		_isPaused = false;
	}

	public void PauseButtonClicked()
	{
		if (_isPaused)
		{
			UnpauseGame();
		}
		else
		{
			PauseGame();
		}
	}

	public bool isPaused()
	{
		return _isPaused;
	}

	public void RampGameSpeed()
	{
		gameSpeed += 0.25f;
	}

	public bool isPlayable
	{
		get{ return _isPlayable;}
		set{_isPlayable = value;}
	}
	

	public void StartNewGame()
	{
		HidePanels();
		isPlayable = false;
		UnpauseGame();

		CreateNewPlanes();

		countdownNum = 3;
		Countdown();

	}

	void HidePanels()
	{
		PanelCountdown.gameObject.SetActive( false );
		PanelGameOverMenu.gameObject.SetActive( false );
	}

	void Countdown()
	{
		if ( countdownNum == 0 )
		{
			isPlayable = true;
			HidePanels();
			stats.ResetScore();
			score.gameObject.SetActive(true);
		}
		else
		{
			PanelCountdown.gameObject.SetActive( true );
			PanelCountdown.gameObject.transform.GetChild(0).GetComponent<Text>().text = countdownNum.ToString();
			countdownNum--;
			Invoke("Countdown", 1.0f);
		}
	}

	public void GameOver()
	{
		isPlayable = false;
		score.gameObject.SetActive(false);

		PanelGameOverMenu.transform.GetChild(2).GetComponent<Text>().text = stats.GetScore().ToString();
		if ( stats.CheckNewHighScore() )
		{
			Debug.Log ("Hi");
			PanelGameOverMenu.transform.GetChild(3).GetComponent<Text>().text = "New Highscore!!";
		}
		else
		{
			Debug.Log ("low");
			PanelGameOverMenu.transform.GetChild(3).GetComponent<Text>().text = "Highscore: " + stats.GetHighScore().ToString();
		}

		PanelGameOverMenu.SetActive(true);
	}
}
